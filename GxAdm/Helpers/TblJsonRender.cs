using System;
using System.Net.Http.Json;

using GxShared.Others;

using static System.Net.WebRequestMethods;

namespace GxAdm.Helpers
{
    public class TblJsonRender
    {
        private readonly HttpClient _httpClie;

        public TblJsonRender(IHttpClientFactory htClieFactory)
        {
            _httpClie = htClieFactory.CreateClient("AUTHClient"); // ✅ Use the named client
        }
        public async Task<TableSet?> rendTables()
        {
            try
            {
                var tables = await _httpClie.GetFromJsonAsync<TableSet>("tables");
                foreach (var lang in tables?.Langues ?? Enumerable.Empty<TableItem>())
                {
                    Console.WriteLine($"{lang.Elea}: {lang.Liba} ({lang.Abg})");
                }
                return tables;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error fetching tables: {ex.Message}");
                return null;
            }
        }
    }
}
