
using GxShared.GlobModels;
using Newtonsoft.Json;
using GxWapi.DaModels;

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

    }
}
