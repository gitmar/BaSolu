using System.Reflection;

using GxWapi.DaModels;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using Microsoft.OData.Client;

namespace GxAdm.Services
{
    public sealed class PendingChangesGuard : IAsyncDisposable
    {
        private readonly IODataContextFactory _contextFactory;
        private readonly NavigationManager _navManager;
        private readonly IJSRuntime _jsRuntime;
        private readonly IHttpClientFactory _httpClientFactory;
        private MyODataContext? _ctx;
        private bool _disposed;
        private HttpClient? _httpClient;
        private bool _isUpdating = false;
        public event Action? OnChanges;
        // 🔥 GENERALIZED - Replaces Plngen-specific methods
        private readonly Dictionary<Type, Func<object, bool>> _unconfirmedPredicates = new();
        private readonly HashSet<Guid> _unconfirmedRowguids = new();
        private readonly HashSet<object> _unconfirmedAdds = new();
        private int _childPendingCount = 0;        
        public PendingChangesGuard(
            IODataContextFactory contextFactory,
            NavigationManager navManager,
            IJSRuntime jsRuntime,
            IHttpClientFactory httpClientFactory)
        {
            _contextFactory = contextFactory;
            _navManager = navManager;
            _jsRuntime = jsRuntime;
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClientFactory.CreateClient("ODataClient");
        }

        public void AttachContext(MyODataContext ctx) => _ctx = ctx;

        public bool IsUnconfirmedAdd<T>(T entity) where T : class
        {
            return _unconfirmedPredicates.TryGetValue(typeof(T), out var predicate) && predicate(entity);
        }
        // 🔥 NEW: Generic version for Rubvar + others
        public void MarkUnconfirmedAdd<T>(T entity) where T : class
        {
            _unconfirmedAdds.Add(entity);
            Console.WriteLine($"🔍 Marked unconfirmed {typeof(T).Name}: {entity?.GetHashCode()}");
            //NotifyChanges();
        }
        // 🔥 NEW GENERIC METHODS - Child entities use these
        //public void RegisterUnconfirmedPredicate<T>(Func<T, bool> isUnconfirmed) where T : class
        //{
        //    _unconfirmedPredicates[typeof(T)] = obj => isUnconfirmed((T)obj);
        //    Console.WriteLine($"✅ Registered {typeof(T).Name}: Xxxx == -1");
        //}
        public void DiscardAllUnconfirmedAdds()
        {
            if (_ctx?.Context?.Entities == null) return;

            // 🔥 UNIVERSAL: ALL registered predicates (Plngen/Rubvar/Rubfmt/...)
            foreach (var kvp in _unconfirmedPredicates)
            {
                var entityType = kvp.Key;
                var predicate = kvp.Value;

                var unconfirmed = _ctx.Context.Entities
                    .Where(e => e.Entity != null &&
                               entityType.IsInstanceOfType(e.Entity) &&
                               predicate(e.Entity))
                    .ToList();

                foreach (var entityDesc in unconfirmed)
                {
                    _ctx.Context.Detach(entityDesc);
                    Console.WriteLine($"🚫 Discarded unconfirmed {entityType.Name}");
                }
            }

            NotifyChanges();
        }
        // 🔥 Your existing methods - UNCHANGED
        public static bool IsPendingState(EntityStates state) =>
            state == EntityStates.Added || state == EntityStates.Modified || state == EntityStates.Deleted;

        public bool HasPendingChanges() =>
            (_ctx?.Context?.Entities.Any(e => IsPendingState(e.State)) ?? false) || _childPendingCount > 0;

        public int GetPendingChangesCount() =>
            (_ctx?.Context?.Entities.Count(e => IsPendingState(e.State)) ?? 0) + _childPendingCount;

        //public bool HasConfirmedPendingChanges()
        //{
        //    if (_ctx?.Context == null) return _childPendingCount > 0;
        //    //return _ctx.Context.Entities.Any(e => IsPendingState(e.State) &&
        //    //       !(e.Entity is Plngen p && (p.Xadd1 == -1 || p.Xedt1 == -1)));
        //    // 🔥 GENERALIZED: Use predicates instead of Plngen-specific check
        //    return _ctx.Context.Entities.Any(e => IsPendingState(e.State) &&
        //           !_unconfirmedPredicates.Values.Any(pred => pred(e.Entity)));
        //}
        //public bool HasConfirmedPendingChanges()
        //{
        //    if (_ctx?.Context == null) return _childPendingCount > 0;

        //    // 🔥 GENERALIZED: Check ALL registered predicates
        //    return _ctx.Context.Entities.Any(e => IsPendingState(e.State) &&
        //           !_unconfirmedPredicates.Values.Any(pred => pred(e.Entity)));
        //}
        public void RegisterUnconfirmedPredicate<T>(Func<T, bool> isUnconfirmed) where T : class
        {
            _unconfirmedPredicates[typeof(T)] = obj =>
            {
                if (obj is T entity)  // 🔥 SAFE CAST FIRST
                    return isUnconfirmed(entity);
                return false;  // Not this type → not unconfirmed
            };
            Console.WriteLine($"✅ Registered {typeof(T).Name}: Xxxx == -1");
        }
        public bool HasConfirmedPendingChanges()
        {
            if (_ctx?.Context == null) return _childPendingCount > 0;

            return _ctx.Context.Entities.Any(e =>
            {
                if (!IsPendingState(e.State)) return false;

                // 🔥 Check ALL predicates SAFELY
                foreach (var kvp in _unconfirmedPredicates)
                {
                    if (kvp.Value(e.Entity)) return false;  // Unconfirmed!
                }
                return true;  // Confirmed pending change
            });
        }
        public void NotifyChanges() => OnChanges?.Invoke();

        // 🔥 Your existing FlushAsync - ENHANCED
        public async Task FlushAsync()
        {
            DiscardAllUnconfirmedAdds();  // All entity types

            if (!HasConfirmedPendingChanges())
            {
                Console.WriteLine("✅ No confirmed changes to flush");
                return;
            }

            await _ctx.Context.SaveChangesAsync();
        }

        // 🔥 Navigation guard - Uses HasConfirmedPendingChanges

        public async Task CancelChangesAsync()
        {
            Console.WriteLine("🗑️ CancelChangesAsync START");

            // 1. Detach ALL current entities
            DiscardAllUnconfirmedAdds();

            if (_ctx?.Context != null)
            {
                var allPending = _ctx.Context.Entities
                    .Where(e => IsPendingState(e.State))
                    .ToList();

                foreach (var entityDesc in allPending)
                {
                    _ctx.Context.Detach(entityDesc);
                }
            }

            _childPendingCount = 0;

            // 🔥 2. REPLACE CONTEXT - Forces fresh tracking
            _ctx = await _contextFactory.CreateAsync();
            AttachContext(_ctx);

            NotifyChanges();

            Console.WriteLine($"✅ CancelChangesAsync END - Fresh context");
        }

        //private void NotifyChanges() => OnChanges?.Invoke();
        public event Action<int>? OnChildPendingChanged;
        public ValueTask DisposeAsync()  // ❌ Remove 'async'
        {
            Console.WriteLine("🧹 PendingChangesGuard DisposeAsync");

            // Clear internal state only
            _ctx = null;
            _unconfirmedPredicates.Clear();

            // Remove _trackedEntities.Clear() - doesn't exist

            // ✅ CORRECT ValueTask return
            return ValueTask.CompletedTask;  // NOT 'default'
        }
        public void DiscardUnconfirmedAdds<T>() where T : class
        {
            if (_ctx?.Context?.Entities == null) return;

            var unconfirmed = _ctx.Context.Entities
                .Where(e => e.Entity is T t
                           && _unconfirmedPredicates.TryGetValue(typeof(T), out var pred)
                           && pred(t)  // ← Xadd1 == -1 ONLY
                           && _unconfirmedAdds.Contains(t))  // ← ALSO check MarkUnconfirmedAdd tracking
                .ToList();

            foreach (var entityDesc in unconfirmed)
            {
                _ctx.Context.Detach(entityDesc);
                Console.WriteLine($"✅ DETACHED unconfirmed {typeof(T).Name}");
                _unconfirmedAdds.Remove((T)entityDesc.Entity);
            }
        }
    }

    //public void DiscardAllUnconfirmedAdds()
    //{
    //    if (_ctx?.Context == null) return;

    //    // 🔥 ALL unconfirmed via predicate dictionary
    //    foreach (var kvp in _unconfirmedPredicates)
    //    {
    //        var entityType = kvp.Key;
    //        var predicate = kvp.Value;

    //        var unconfirmed = _ctx.Context.Entities
    //            .Where(e => e.Entity != null &&
    //                       entityType.IsInstanceOfType(e.Entity) &&
    //                       predicate(e.Entity))
    //            .ToList();

    //        foreach (var entityDesc in unconfirmed)
    //        {
    //            _ctx.Context.Detach(entityDesc);
    //            Console.WriteLine($"🚫 Discarded unconfirmed {entityType.Name}");
    //        }
    //    }

    //    // Plus Plngen fallback (backward compatible)
    //    var plngenUnconfirmed = _ctx.Context.Entities
    //        .Where(e => e.Entity is Plngen p && (p.Xadd1 == -1 || p.Xedt1 == -1))
    //        .ToList();

    //    foreach (var entityDesc in plngenUnconfirmed)
    //        _ctx.Context.Detach(entityDesc);

    //    NotifyChanges();
    //}
}
