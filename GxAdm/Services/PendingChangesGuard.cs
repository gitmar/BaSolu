using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        private IDisposable? _registration;
        private MyODataContext? _ctx;
        private bool _disposed;
        private HttpClient? _httpClient;

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
            _registration = _navManager.RegisterLocationChangingHandler(OnLocationChangingAsync);
        }

        public void AttachContext(MyODataContext ctx) => _ctx = ctx;

        /// <summary>
        /// ✅ FIXED: Lambda instead of method group
        /// </summary>
        public bool HasPendingChanges()
        {
            return _ctx?.Context?.Entities.Any(e => IsPendingState(e.State)) ?? false;
        }

        /// <summary>
        /// ✅ FIXED: .Count() → .Where().Count()
        /// </summary>
        public int GetPendingChangesCount()
        {
            return _ctx?.Context?.Entities.Where(e => IsPendingState(e.State)).Count() ?? 0;
        }

        private static bool IsPendingState(EntityStates state) =>
            state == EntityStates.Added ||
            state == EntityStates.Modified ||
            state == EntityStates.Deleted;

        private static bool IsUnchanged(EntityStates state) =>
            state == EntityStates.Unchanged;

        /// <summary>
        /// 🔥 FIXED: Uses EntityDescriptor directly (no EntityEntry needed)
        /// </summary>
        private bool HasInvalidEntities()
        {
            if (_ctx?.Context == null) return false;

            foreach (var entityDesc in _ctx.Context.Entities)
            {
                if (!IsPendingState(entityDesc.State)) continue;
                if (IsInvalidKey(entityDesc)) return true;
            }
            return false;
        }

        private bool IsInvalidKey(EntityDescriptor entityDesc)
        {
            var identityStr = entityDesc.Identity.ToString() ?? "unknown";
            return entityDesc.Entity switch
            {
                Gxorga gxorga when gxorga.Idorg == 0 => FailLog("Gxorga", identityStr, "Idorg"),
                Gsgfix gsgfix when gsgfix.Xseq == 0 => FailLog("Gsgfix", identityStr, "Xseq"),
                Gsglne gsglne when gsglne.Id == 0 => FailLog("Gsglne", identityStr, "Id"),
                Tiewel tiewel when tiewel.Id == 0 => FailLog("Tiewel", identityStr, "Id"),
                Tiersp tiersp when tiersp.Id == 0 => FailLog("Tiersp", identityStr, "Id"),
                Plngen plngen when plngen.Id == 0 => FailLog("Plngen", identityStr, "Id"),
                _ => false
            };
        }

        private bool FailLog(string entityType, string identity, string pkName)
        {
            Console.WriteLine($"🚫 INVALID {entityType}: {identity} - {pkName}=0");
            return true;
        }

        /// <summary>
        /// 🔥 FIXED: Explicit lambda type
        /// </summary>
        public async Task FlushAsync()
        {
            if (_ctx == null || !HasPendingChanges() || HasInvalidEntities())
            {
                Console.WriteLine("No valid changes to flush");
                return;
            }

            var pendingChanges = _ctx.GetPendingChanges().ToList();
            Console.WriteLine($"🚀 Flushing {pendingChanges.Count} changes...");

            try
            {
                // ✅ FIXED: Explicit async lambda
                var saveTasks = pendingChanges.Select(async entityEntry =>
                {
                    var success = await SaveEntityAsync(entityEntry.Entity, GetEntityKey(entityEntry.Entity));
                    return (entityEntry, success);
                }).ToArray();

                var results = await Task.WhenAll(saveTasks);

                if (results.All(r => r.success))
                {
                    foreach (var entityEntry in pendingChanges)
                    {
                        _ctx.Detach(entityEntry.Entity);
                    }
                    Console.WriteLine($"🎉 Saved {pendingChanges.Count} entities");
                }
                else
                {
                    throw new InvalidOperationException("Save failed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 FlushAsync failed: {ex.Message}");
                throw;
            }
        }
        private async Task<bool> SaveEntityAsync(object entity, object key)
        {
            try
            {
                var entityTypeName = entity.GetType().Name;
                var entitySetName = entity switch
                {
                    Gxorga _ => "Gxorgas",
                    Gsgfix _ => "Gsgfixes",
                    Gsglne _ => "Gsglnes",
                    Tiewel _ => "Tiewels",
                    Tiersp _ => "Tiersps",
                    Plngen _ => "Plngens",

                    _ => throw new ArgumentException($"Unknown entity: {entityTypeName}")
                };

                var url = $"odata/{entitySetName}({key})";
                Console.WriteLine($"💾 PUT {url}");

                var response = await _httpClient!.PutAsJsonAsync(
                    url,
                    entity,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    });

                Console.WriteLine($"📡 Response: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅ {entityTypeName}({key}): OK");
                    return true;
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ {entityTypeName}({key}): {response.StatusCode}");
                Console.WriteLine($"   Error: {errorContent}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Exception: {ex.Message}");
                return false;
            }
        }
        public void CancelChanges()
        {
            if (_ctx?.Context == null) return;

            var pending = _ctx.Context.Entities
                .Where(e => !IsUnchanged(e.State))
                .ToList();

            foreach (var entityDesc in pending)
            {
                _ctx.Detach(entityDesc.Entity);
            }

            Console.WriteLine($"✅ Cancelled {pending.Count} changes");
        }

        private object GetEntityKey(object entity)
        {
            return entity switch
            {
                Gxorga gxorga => gxorga.Idorg,
                Gsgfix gsgfix => gsgfix.Xseq,
                Gsglne gsglne => gsglne.Id,
                Tiewel tiewel => tiewel.Id,
                Tiersp tiersp => tiersp.Id,
                Plngen plngen => plngen.Id,
                _ => throw new ArgumentException($"Unknown entity type: {entity.GetType().Name}")
            };
        }

        private async ValueTask OnLocationChangingAsync(LocationChangingContext context)
        {
            if (!HasPendingChanges()) return;

            context.PreventNavigation();

            var confirmed = await _jsRuntime.InvokeAsync<bool>("confirm",
                $"There are {GetPendingChangesCount()} unsaved changes. Save before leaving?");

            if (confirmed)
            {
                try { await FlushAsync(); }
                catch { CancelChanges(); }
            }
            else
            {
                CancelChanges();
            }

            _navManager.NavigateTo(context.TargetLocation, forceLoad: false);
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed) return;

            try
            {
                _registration?.Dispose();
                if (HasPendingChanges()) await FlushAsync();
            }
            catch
            {
                CancelChanges();
            }
            finally
            {
                _disposed = true;
            }
        }
    }
}