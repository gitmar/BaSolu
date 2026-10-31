using System.Net.Http.Json;

using GxShared.Cols.Models;

namespace GxTie.Components.Cols
{
    public sealed class PublicColumnPreferencesClient : IColumnPreferencesReader
    {
        private readonly HttpClient _dftClient;

        public PublicColumnPreferencesClient(IHttpClientFactory httpClientFactory)
        {
            _dftClient = httpClientFactory.CreateClient("LOCALClient");
        }

        public async Task<List<LSelCol>?> LoadAsync(int idorg, ColPrefContext ctx)
        {
            using var resp = await _dftClient.GetAsync(
                $"olines/publcolprefs/{idorg}{Qs(ctx)}");

            if (!resp.IsSuccessStatusCode)
            {
                var error = await resp.Content.ReadAsStringAsync();
                Console.WriteLine(
                    $"Public column prefs failed: {(int)resp.StatusCode} {resp.ReasonPhrase} {error}");

                return null;
            }

            return await resp.Content.ReadFromJsonAsync<List<LSelCol>?>();
        }
        private static string Qs(ColPrefContext ctx)
            => $"?Totyp={ctx.Totyp}&Efrm={ctx.Efrm}&Styp={ctx.Styp}";
    }
}
