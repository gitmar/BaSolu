
using System.Net.Http.Json;

using GxShared.Others;
using GxShared.Sess;

using Newtonsoft.Json;

namespace GxTie.Services
{
    public class RendAOlne
    {
        private readonly HttpClientService _clieManager;
        private readonly HttpClient _pbapiClient;
        public RendAOlne(HttpClientService clieManager, IHttpClientFactory cliefactory)
        {
            _clieManager = clieManager;
            _pbapiClient = cliefactory.CreateClient("LOCALClient");
        }

        public async Task<List<Gpdivh>> LoadOrgaPostes(int inIdorg)
        {
            var response = await _pbapiClient.GetAsync(
                $"olines/olnepostes/{inIdorg}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<Gpdivh>>();

            if (result?.Any() == true)
            {
                Console.WriteLine($"Nb Postes : {result.Count}");
                return result;
            }

            Console.WriteLine("Nb Psts : null");
            return new();
        }

        public async Task<List<Gtabl>> LoadOrgaTables(int inIdorg)
        {
            //var json = await _clieManager.SendRequestAsync(
            //    "LOCALClient",
            //    HttpMethod.Get,
            //    $"olines/olnetables/{inIdorg}");

            var response = await _pbapiClient.GetAsync(
                $"olines/olnetables/{inIdorg}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<Gtabl>>();

            if (result?.Any() == true)
            {
                Console.WriteLine($"Nb Tables : {result.Count}");
                return result;
            }

            Console.WriteLine("Nb Tbles : null");
            return new();
        }
        public async Task<List<Gpatr>> LoadOrgaAtribs(int inIdorg)
        {
            //var json = await _clieManager.SendRequestAsync(
            //    "LOCALClient",
            //    HttpMethod.Get,
            //    $"olines/olneatribs/{inIdorg}");

            var response = await _pbapiClient.GetAsync(
                $"olines/olneatribs/{inIdorg}");
            
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<Gpatr>>();

            if (result?.Any() == true)
            {
                Console.WriteLine($"Nb Attributs : {result.Count}");
                return result;
            }

            Console.WriteLine("Nb Attribs : null");
            return new();
        }
        public async Task<List<Gpcat>> LoadOrgaCategs(int inIdorg)
        {
            //var json = await _clieManager.SendRequestAsync(
            //    "LOCALClient",
            //    HttpMethod.Get,
            //    $"olines/olnecategs/{inIdorg}");

            var response = await _pbapiClient.GetAsync(
                $"olines/olnecategs/{inIdorg}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<Gpcat>>();

            if (result?.Any() == true)
            {
                Console.WriteLine($"Nb Categories : {result.Count}");
                return result;
            }

            Console.WriteLine("Nb Categs : null");
            return new();
        }
        public async Task<List<Gpvar>> LoadOrgaVariabs(int inIdorg)
        {
            //var json = await _clieManager.SendRequestAsync(
            //    "LOCALClient",
            //    HttpMethod.Get,
            //    $"olines/olnevars/{inIdorg}");
            var response = await _pbapiClient.GetAsync(
                $"olines/olnevarias/{inIdorg}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<Gpvar>>();

            if (result?.Any() == true)
            {
                Console.WriteLine($"Nb Variables : {result.Count}");
                return result;
            }

            Console.WriteLine("Nb Varias : null");
            return new();
        }
    }
}