using System.Net.Http.Json;

using GxShared.Cols.Models;

namespace GxPilo.Services.Cols
{
    // Client/Services/ColumnPreferencesClient.cs
    public class ColumnPreferencesClient
    {
        private readonly HttpClient _http;

        public ColumnPreferencesClient(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient("AuthClient");
        }

        private static string Qs(ColPrefContext ctx) => $"?Totyp={ctx.Totyp}&Efrm={ctx.Efrm}&Styp={ctx.Styp}";

        public async Task<List<LSelCol>> LoadAsync(int idorg, ColPrefContext ctx)
            => await _http.GetFromJsonAsync<List<LSelCol>>($"Colprefs/{idorg}{Qs(ctx)}")
               ?? new List<LSelCol>();

        public async Task<bool> SaveAsync(int idorg, ColPrefContext ctx, List<LSelCol> selection)
        {
            var resp = await _http.PostAsJsonAsync($"Colprefs/{idorg}{Qs(ctx)}", selection);
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> SeedAsync(int idorg)
        {
            var resp = await _http.PostAsync($"Colprefs/{idorg}/seed", null);
            return resp.IsSuccessStatusCode;
        }
        public async Task<bool> ResetAsync(int idorg, ColPrefContext ctx)
        {
            var resp = await _http.PostAsync($"Colprefs/{idorg}/reset{Qs(ctx)}", null);
            return resp.IsSuccessStatusCode;
        }

        public async Task<int> SeedCatalogAsync()
        {
            var resp = await _http.PostAsync("Colprefs/catalog/seed", null);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<int>();
        }
    }
}