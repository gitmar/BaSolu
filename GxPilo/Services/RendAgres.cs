
using GxShared.Sess;
using GxShared.Others;
using Newtonsoft.Json;
using GxWapi.DaModels;

namespace GxPilo.Services
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
            var result = JsonConvert.DeserializeObject<List<Gpdivh>>(json);
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
            var result = JsonConvert.DeserializeObject<List<Grole>>(json);
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
        public async Task<List<Gptbl>> LoadOrgaTables()
        {
            //FmtPsts
            var json = await _clieManager.SendRequestAsync("AUTHClient", HttpMethod.Get, "lgauth/alltables");
            var result = JsonConvert.DeserializeObject<List<Gptbl>>(json);
            if (result.Any())
            {
                Console.WriteLine($"Nb tables : {result.Count}");
                return result.ToList();
            }
            else
            {
                Console.WriteLine("Nb tables : null");
                return new();
            }
        }
        public async Task<List<Gpcol>> LoadOrgaColons()
        {
            //FmtPsts
            var json = await _clieManager.SendRequestAsync("AUTHClient", HttpMethod.Get, "lgauth/allcolons");
            var result = JsonConvert.DeserializeObject<List<Gpcol>>(json);
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
