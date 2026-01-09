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
        private IDisposable? _registration;
        private bool _hasPendingChanges;
        private MyODataContext? _ctx;

        public PendingChangesGuard(IODataContextFactory contextFactory,
                                   NavigationManager navManager,
                                   IJSRuntime jsRuntime)
        {
            _contextFactory = contextFactory;
            _navManager = navManager;
            _jsRuntime = jsRuntime;

            _registration = _navManager.RegisterLocationChangingHandler(OnLocationChangingAsync);
        }

        public void MarkDirty() => _hasPendingChanges = true;
        public void ClearDirty() => _hasPendingChanges = false;
        // In guard
        public void AttachContext(MyODataContext ctx) => _ctx = ctx;

        public async Task FlushAsync()
        {
            if (_ctx == null) return;

            if (!_ctx.Context.Entities.Any(e =>
                e.State == EntityStates.Added ||
                e.State == EntityStates.Modified ||
                e.State == EntityStates.Deleted))
            {
                return;
            }

            await _ctx.Context.SaveChangesAsync();
            _hasPendingChanges = false;
        }

        private async ValueTask OnLocationChangingAsync(LocationChangingContext context)
        {
            if (!_hasPendingChanges)
                return;

            context.PreventNavigation();

            var confirmed = await _jsRuntime.InvokeAsync<bool>("confirm",
                "There are pending changes. Do you want to save them?");

            if (confirmed)
            {
                await FlushAsync();
                ClearDirty(); // ensure no loop
            }
            else
            {
                // user declined → discard changes
                ClearDirty();
            }

            // Resume navigation safely
            _navManager.NavigateTo(context.TargetLocation, forceLoad: false);
        }

        public async ValueTask DisposeAsync()
        {
            _registration?.Dispose();
            if (_hasPendingChanges)
            {
                await FlushAsync();
            }
        }
    }
}