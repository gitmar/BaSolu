using System.Net.Http.Json;

using GxShared.Cols.Models;

namespace GxTie.Components.Cols
{
    public sealed class PublicColumnPreferencesClient : IColumnPreferencesReader
    {
        private readonly HttpClient _dftClient;
        public PublicColumnPreferencesClient(IHttpClientFactory factory) => _dftClient = factory.CreateClient("LOCALClient");

        public async Task<List<LSelCol>> LoadAsync(int idorg, ColPrefContext c, CancellationToken ct = default)
            => await _dftClient.GetFromJsonAsync<List<LSelCol>>(
                   $"Olines/{idorg}/columns?Totyp={c.Totyp}&Efrm={c.Efrm}&Styp={c.Styp}", ct) ?? [];
    }
}
