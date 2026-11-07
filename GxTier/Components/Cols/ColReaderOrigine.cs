using GxShared.Cols.Models;

namespace GxTie.Components.Cols
{
    public interface IColumnPreferencesReader
    {
        Task<List<LSelCol>> LoadAsync(int idorg, ColPrefContext context, CancellationToken cancellationToken = default);
        //Task<List<LSelCol>?> LoadAsync(int idorg, ColPrefContext ctx);
    }
}
