
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Text.Json.Serialization;

using GxShared.Others;
using GxShared.Sess;

using GxWapi.DaModels;

//using Newtonsoft.Json;

namespace GxStk.Services
{
    public class RendAgres
    {
        private readonly HttpClientService _clieManager;
        public RendAgres(HttpClientService clieManager)
        {
            _clieManager = clieManager;
        }
        public async Task<List<Gpdivh>> LoadOrgaPostes()
        {
            //FmtPsts
            var json = await _clieManager.SendRequestAsync("AUTHClient", HttpMethod.Get, "lgauth/allpostes");
            //var result = JsonConvert.DeserializeObject<List<Gpdivh>>(json);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = System.Text.Json.JsonSerializer.Deserialize<List<Gpdivh>>(json, options);
            if (result.Any())
            {
                Console.WriteLine($"Nb Divs : {result.Count}");
                return result.ToList();
            }
            else
            {
                Console.WriteLine("Nb Psts : null");
                return new();
            }
            //Pdom 6tiers Patr1baseadm
            //LsDivs = LsDivs.Where(ud => ud.Pdom == 6 && ud.Patr == 1).ToList();
        }
        public async Task<List<Grole>> LoadOrgaRoles()
        {
            //FmtPsts
            var json = await _clieManager.SendRequestAsync("AUTHClient", HttpMethod.Get, "lgauth/allroles");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = System.Text.Json.JsonSerializer.Deserialize<List<Grole>>(json, options);
            //var result = JsonConvert.DeserializeObject<List<Grole>>(json);
            if (result.Any())
            {
                Console.WriteLine($"Nb Roles : {result.Count}");
                return result.ToList();
            }
            else
            {
                Console.WriteLine("Nb Roles : null");
                return new();
            }
        }
        //public async Task<List<Gptbl>> LoadOrgaTables()
        //{
        //    //FmtPsts
        //    var json = await _clieManager.SendRequestAsync("AUTHClient", HttpMethod.Get, "lgauth/alltables");
        //    Console.WriteLine($"JSON length: {json?.Length ?? 0} chars");
        //    //var result = JsonConvert.DeserializeObject<List<Gptbl>>(json);
        //    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        //    var result = System.Text.Json.JsonSerializer.Deserialize<List<Gptbl>>(json, options);
        //    if (result.Any())
        //    {
        //        Console.WriteLine($"Nb tables : {result.Count}");
        //        return result.ToList();
        //    }
        //    else
        //    {
        //        Console.WriteLine("Nb tables : null");
        //        return new();
        //    }
        //}
        public async Task<List<Gptbl>> LoadOrgaTables()
        {
            var json = await _clieManager.SendRequestAsync("AUTHClient", HttpMethod.Get, "lgauth/alltables");
            Console.WriteLine($"JSON length: {json?.Length ?? 0} chars");

            if (string.IsNullOrEmpty(json)) return new();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,  // Matches your API's null policy
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull  // Optional, for symmetry
            };
            var result = System.Text.Json.JsonSerializer.Deserialize<List<Gptbl>>(json, options);

            if (result?.Any() == true)
            {
                Console.WriteLine($"Nb tables: {result.Count}");
                return result.ToList();
            }
            Console.WriteLine("Nb tables: null/empty");
            return new();
        }
        public async Task<List<Gpcol>> LoadOrgaColons()
        {
            //FmtPsts
            var json = await _clieManager.SendRequestAsync("AUTHClient", HttpMethod.Get, "lgauth/allcolons");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = System.Text.Json.JsonSerializer.Deserialize<List<Gpcol>>(json, options);
            //var result = JsonConvert.DeserializeObject<List<Gpcol>>(json);
            if (result.Any())
            {
                Console.WriteLine($"Nb colonnes : {result.Count}");
                return result.ToList();
            }
            else
            {
                Console.WriteLine("Nb colonnes : null");
                return new();
            }
        }
    }
}
