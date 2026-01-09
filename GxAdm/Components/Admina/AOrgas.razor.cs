using Microsoft.OData.Client;

namespace GxAdm.Components.Admina
{
    public partial class AOrgas : IAsyncDisposable
    {
        private async Task SaveAllAsync()
        {
            bool hasChanges = _gODContext.Context.Entities.Any(e =>
                e.State == EntityStates.Added ||
                e.State == EntityStates.Modified ||
                e.State == EntityStates.Deleted);

            if (!hasChanges)
                return;

            await _gODContext.Context.SaveChangesAsync();
            allEnable = false;
        }

        public async ValueTask DisposeAsync()
        {
            // Called when navigating away
            await SavePendingChangesAsync();
        }

        private async Task SavePendingChangesAsync()
        {
            // Flush dirty state here
            await SaveAllAsync();
        }
    }
}
