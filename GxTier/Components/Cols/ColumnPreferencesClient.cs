using System.Net.Http.Json;

using GxShared.Cols.Models;

namespace GxTie.Components.Cols
{
    // Client/Services/ColumnPreferencesClient.cs
    public class ColumnPreferencesClient
    {
        private readonly HttpClient _dftClient;
        public ColumnPreferencesClient(IHttpClientFactory httpClientFactory)
        {
            _dftClient = httpClientFactory.CreateClient("AuthClient");
        }

        private static string Qs(ColPrefContext ctx) => $"?Totyp={ctx.Totyp}&Efrm={ctx.Efrm}&Styp={ctx.Styp}";

        public async Task<List<LSelCol>> LoadAsync(int idorg, ColPrefContext ctx)
            => await _dftClient.GetFromJsonAsync<List<LSelCol>>($"Colprefs/{idorg}{Qs(ctx)}")
               ?? new List<LSelCol>();

        public async Task<bool> SaveAsync(int idorg, ColPrefContext ctx, List<LSelCol> selection)
        {
            var resp = await _dftClient.PostAsJsonAsync($"Colprefs/{idorg}{Qs(ctx)}", selection);
            return resp.IsSuccessStatusCode;
        }

        // SeedAsync removed — org rows are override-only now; no per-org seeding action exists server-side.

        public async Task<bool> ResetAsync(int idorg, ColPrefContext ctx)
        {
            var resp = await _dftClient.PostAsync($"Colprefs/{idorg}/reset{Qs(ctx)}", null);
            return resp.IsSuccessStatusCode;
        }

        public async Task<int> SeedCatalogAsync()
        {
            var resp = await _dftClient.PostAsync("Colprefs/catalog/seed", null);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<int>();
        }
    }
}