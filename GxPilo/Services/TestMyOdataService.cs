using System.Text.Json.Nodes;

using Microsoft.OData.Client;

namespace GxPilo.Services
{
    public class MyODataClient
    {
        private readonly Container _context;

        public MyODataClient(IHttpClientFactory httpClientFactory)
        {
            var serviceUri = new Uri("https://localhost:7095/odata/");
            _context = new Container(serviceUri);
        }

        //public async Task<List<Gsglne>> GetGsglnesAsync()
        //{
        //    DataServiceQuery<Gsglne> query =
        //        (DataServiceQuery<Gsglne>)_context.Gsglnes.AddQueryOption("$top", "100");

        //    var result = await query.ExecuteAsync();
        //    Console.WriteLine($"nb. gsglne : {result.FirstOrDefault()?.Liba}");
        //    return result.ToList();
        //}
    }
// Services/GsglneHttpService.cs

        public class GsglneDto
        {
            public int Id { get; set; }
            public int Igsg { get; set; }
            public int Otyp { get; set; }
            public int Etyp { get; set; }
            public int Iele { get; set; }
            public int Totyp { get; set; }
            public int Pxcod { get; set; }
            public int Xcod { get; set; }
            public int Idorg { get; set; }
            public int Pvars { get; set; }
            public int Pitb { get; set; }
            public int Pele { get; set; }
            public string? Liba { get; set; }
            public string? Abg { get; set; }
            public string? Scdrub { get; set; }
            public string? Fgsrc { get; set; }
            public string? Fgexe { get; set; }
            public int Efrm { get; set; }
            public Guid Rowguid { get; set; }
            public bool EleaAuto { get; set; }
            public int Elea { get; set; }
            public string? Obsv { get; set; }
            // add any extra fields you want to display
        }

        public class GsglneHttpService
        {
            private readonly HttpClient _httpClient;

            public GsglneHttpService(IHttpClientFactory httpClientFactory)
            {
                _httpClient = httpClientFactory.CreateClient("OTESTClient");
            }

            public async Task<List<GsglneDto>> GetGsglnesAsync()
            {
                // Use the same auth that AUTHClient already provides
                var json = await _httpClient.GetStringAsync("odata/Gsglnes?$top=100");

                var doc = JsonNode.Parse(json);
                var values = doc!["value"]!.AsArray();

                var result = new List<GsglneDto>();

                foreach (var item in values)
                {
                    result.Add(new GsglneDto
                    {
                        Id = item["Id"]!.GetValue<int>(),
                        Igsg = item["Igsg"]!.GetValue<int>(),
                        Otyp = item["Otyp"]!.GetValue<int>(),
                        Etyp = item["Etyp"]!.GetValue<int>(),
                        Iele = item["Iele"]!.GetValue<int>(),
                        Totyp = item["Totyp"]!.GetValue<int>(),
                        Pxcod = item["Pxcod"]!.GetValue<int>(),
                        Xcod = item["Xcod"]!.GetValue<int>(),
                        Idorg = item["Idorg"]!.GetValue<int>(),
                        Pvars = item["Pvars"]!.GetValue<int>(),
                        Pitb = item["Pitb"]!.GetValue<int>(),
                        Pele = item["Pele"]!.GetValue<int>(),
                        Liba = item["Liba"]?.GetValue<string>(),
                        Abg = item["Abg"]?.GetValue<string>(),
                        Scdrub = item["Scdrub"]?.GetValue<string>(),
                        Fgsrc = item["Fgsrc"]?.GetValue<string>(),
                        Fgexe = item["Fgexe"]?.GetValue<string>(),
                        Efrm = item["Efrm"]?.GetValue<int>() ?? 0,
                        Rowguid = Guid.Parse(item["Rowguid"]!.ToString()),
                        EleaAuto = item["EleaAuto"]?.GetValue<bool>() ?? false,
                        Elea = item["Elea"]?.GetValue<int>() ?? 0,
                        Obsv = item["Obsv"]?.GetValue<string>(),
                        // add more if needed
                    });
                }

                return result;
            }
        }
    }

