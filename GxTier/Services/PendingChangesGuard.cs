using System.Reflection;

using GxWapi.DaModels;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using Microsoft.OData.Client;

namespace GxTie.Services
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
        public void DiscardAllUnconfirmedAdds()
        {
            if (_ctx?.Context?.Entities == null) return;

            foreach (var kvp in _unconfirmedPredicates)
            {
                var entityType = kvp.Key;
                var predicate = kvp.Value;

                var unconfirmed = _ctx.Context.Entities
                    .Where(e => e.Entity != null &&
                               entityType.IsInstanceOfType(e.Entity) &&
                               predicate(e.Entity) &&
                               _unconfirmedAdds.Contains(e.Entity))  // 🔥 FIXED!
                    .ToList();
                foreach (var entityDesc in unconfirmed)
                {
                    _ctx.Context.Detach(entityDesc);  // 🔥 Primary action
                    Console.WriteLine($"🚫 Discarded unconfirmed {entityType.Name}");

                    // Optional: Clean tracking (safe remove)
                    try
                    {
                        _unconfirmedAdds.Remove((dynamic)entityDesc.Entity);
                    }
                    catch
                    {
                        // Ignore - already detached
                    }
                }
            }

            NotifyChanges();
        }

        //public void DiscardAllUnconfirmedAdds()
        //{
        //    if (_ctx?.Context?.Entities == null) return;

        //    // 🔥 UNIVERSAL: ALL registered predicates (Plngen/Rubvar/Rubfmt/...)
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

        //    NotifyChanges();
        //}
        // 🔥 Your existing methods - UNCHANGED
        public static bool IsPendingState(EntityStates state) =>
            state == EntityStates.Added || state == EntityStates.Modified || state == EntityStates.Deleted;

        public bool HasPendingChanges() =>
            (_ctx?.Context?.Entities.Any(e => IsPendingState(e.State)) ?? false) || _childPendingCount > 0;

        public int GetPendingChangesCount() =>
            (_ctx?.Context?.Entities.Count(e => IsPendingState(e.State)) ?? 0) + _childPendingCount;

        public async Task ClearPendingChangesAsync()
        {
            Console.WriteLine("🔄 ClearPendingChangesAsync START");

            try
            {
                // 🔥 1. Get ALL pending entities (Added/Modified/Deleted)
                var pendingEntities = _ctx?.Context.Entities
                    .Where(e => e.State != EntityStates.Unchanged)
                    .ToList();

                Console.WriteLine($"📊 Found {pendingEntities.Count} pending entities");

                // 🔥 2. Detach EACH entity async-safe
                foreach (var entityEntry in pendingEntities)
                {
                    try
                    {
                        _ctx?.Detach(entityEntry.Entity);
                        Console.WriteLine($"✅ Detached: {entityEntry.Entity.GetType().Name}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Failed to detach {entityEntry.Entity}: {ex.Message}");
                    }
                }

                // 🔥 3. Force context refresh (async-safe)
                await Task.Yield();  // Yield to UI thread

                Console.WriteLine($"✅ ClearPendingChangesAsync COMPLETE - Cleared {pendingEntities.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ClearPendingChangesAsync FAILED: {ex.Message}");
            }
        }
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
            //Console.WriteLine($"✅ Registered {typeof(T).Name}: Xxxx == -1");
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
        public async Task CancelChangesAsync()
        {
            Console.WriteLine("🗑️ CancelChangesAsync START");

            // 1. Detach ALL current entities
            DiscardAllUnconfirmedAdds();
            Console.WriteLine($"{GetPendingChangesCount}");
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
        public async Task FlushAsync(bool skipUnconfirmedDiscard = false)
        {
            Console.WriteLine($"1- Pending: {GetPendingChangesCount()}");

            if (!skipUnconfirmedDiscard)
                DiscardAllUnconfirmedAdds();

            if (!HasConfirmedPendingChanges())
            {
                Console.WriteLine("✅ No confirmed changes to flush");
                return;
            }

            Console.WriteLine($"2- Saving {GetPendingChangesCount()} confirmed changes");
            await _ctx.Context.SaveChangesAsync();
            Console.WriteLine($"3- After save: {GetPendingChangesCount()}");
        }

        // 🔥 Your existing FlushAsync - ENHANCED
        //public async Task FlushAsync()
        //{
        //    Console.WriteLine($"1- {GetPendingChangesCount()}");
        //    DiscardAllUnconfirmedAdds();  // All entity types twice verification

        //    if (!HasConfirmedPendingChanges())
        //    {
        //        Console.WriteLine("✅ No confirmed changes to flush");
        //        return;
        //    }
        //    Console.WriteLine($"2- {GetPendingChangesCount()}");
        //    await _ctx.Context.SaveChangesAsync();
        //    Console.WriteLine($"3- {GetPendingChangesCount()}");
        //}
        public async Task FlushConfirmedChangesOnlyAsync()
        {
            ////Console.WriteLine($"Saving ONLY confirmed changes: {GetConfirmedPendingChangesCount()}");

            // 🔥 NO DiscardAllUnconfirmedAdds() here
            if (!HasConfirmedPendingChanges())
            {
                Console.WriteLine("✅ No confirmed changes to flush");
                return;
            }

            await _ctx.Context.SaveChangesAsync();
            ////ClearConfirmedChanges();  // Reset flags only
        }
        // 🔥 Navigation guard - Uses HasConfirmedPendingChanges

        

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
}
