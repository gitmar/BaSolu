
using GxShared.Sess;
using GxShared.Others;

using Newtonsoft.Json;

namespace GxTie.Services
{
    public class RendAOlne
    {
        private readonly HttpClientService _clieManager;
        public RendAOlne(HttpClientService clieManager)
        {
            _clieManager = clieManager;
        }

        public async Task<List<Gpdivh>> LoadOrgaPostes(int inIdorg)
        {
            //FmtPsts
            var json = await _clieManager.SendRequestAsync(
                "AUTHClient", 
                HttpMethod.Get, 
                $"lgauth/olnepostes/{inIdorg}");
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
 
        public async Task<List<Gtabl>> LoadOrgaTables(int inIdorg)
        {
            var json = await _clieManager.SendRequestAsync(
                "AUTHClient",
                HttpMethod.Get,
                $"lgauth/olnetables/{inIdorg}");

            var result = JsonConvert.DeserializeObject<List<Gtabl>>(json);

            return result ?? new List<Gtabl>();
        }
        public async Task<List<Gpatr>> LoadOrgaAtribs(int inIdorg)
        {
            var json = await _clieManager.SendRequestAsync(
                "AUTHClient",
                HttpMethod.Get,
                $"lgauth/olneatribs/{inIdorg}");

            var result = JsonConvert.DeserializeObject<List<Gpatr>>(json);

            return result ?? new List<Gpatr>();
        }
        public async Task<List<Gpcat>> LoadOrgaCategs(int inIdorg)
        {
            var json = await _clieManager.SendRequestAsync(
                "AUTHClient",
                HttpMethod.Get,
                $"lgauth/olnecategs/{inIdorg}");

            var result = JsonConvert.DeserializeObject<List<Gpcat>>(json);

            return result ?? new List<Gpcat>();
        }
        public async Task<List<Gpvar>> LoadOrgaVariabs(int inIdorg)
        {
            var json = await _clieManager.SendRequestAsync(
                "AUTHClient",
                HttpMethod.Get,
                $"lgauth/olnevars/{inIdorg}");

            var result = JsonConvert.DeserializeObject<List<Gpvar>>(json);

            return result ?? new List<Gpvar>();
        }
    }
}