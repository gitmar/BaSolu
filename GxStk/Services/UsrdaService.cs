using GxShared.GlobModels;
using GxSaie.Services;
using System.Net.Http.Json;
using Newtonsoft.Json;
using BlazorBootstrap.Components;
using System.Runtime.CompilerServices;

namespace GxSaie.Services
{
    public interface IUsrdaService
    {
        Task<Userbag> GetMyDataAsync(string UserId, int UorgId);
        Task<bool> SetMyDataAsync(string UserId, int UorgId);
    }
    public class UsrdaService : IUsrdaService
    {
        //private readonly HttpClient _apiClient;
        private readonly HttpClientService _httpClie;
        private Userbag _usrbag { get; set; }

        public UsrdaService(Userbag usrbag, IHttpClientFactory httpCliefact, HttpClientService httpClie)
        {
            _usrbag = usrbag;
            _httpClie = httpClie;
            //_apiClient = httpCliefact.CreateClient("BZEClient");
        }
        public async Task<bool> SetMyDataAsync(string UserId, int UorgId)
        {
            //Console.WriteLine("DEPART loadApiRecord->..IndexedDB");
            bool yabag = false;
            _usrbag = null;

            //var ModResu = await _apiClient.GetAsync("udonbag?vuserid=" + UserId + "&vorgid=" + UorgId);
            //if (ModResu.IsSuccessStatusCode)
            //{
            //Console.WriteLine("LECTURE LoadApiRecord");
            var strcall = await _httpClie.SendRequestAsync("AUTHClient", HttpMethod.Get, "lgauth/udonbag?vuserid=" + UserId + "&vorgid=" + UorgId);
            var uinitBag = JsonConvert.DeserializeObject<Userbag>(strcall);
            if (uinitBag != null)
            {//re-chargement des donnees
             //Console.WriteLine("FIN LECTURE DEPART");
             //utilisateur courant/idorg courant
                Console.WriteLine($"RETOUR<-trouveRecord IndexedDB... fixes : {uinitBag.Ufixes.Count}");
                _usrbag = uinitBag;
                yabag = true;
            }
            //}
            return yabag;
        }
        public async Task<Userbag> GetMyDataAsync(string UserId, int UorgId)
        {
            if (_usrbag == null)
            {
                Console.WriteLine($"RECREATING11 USRBAG user: {UserId} orga: {UorgId}");
                await SetMyDataAsync(UserId, UorgId);
            }
            else
            {
                if ((_usrbag.Ufixes.Count == 0) && (!string.IsNullOrEmpty(UserId)) && (UorgId >= 1000))
                {
                    await SetMyDataAsync(UserId, UorgId);
                    Console.WriteLine($"RECREATING22 USRBAG user: {UserId} orga: {UorgId}");
                }
                Console.WriteLine($"REPRISE USRBAG bag: {_usrbag.Ufixes.Count} user: {UserId} orga: {UorgId}");
            }
            return _usrbag;
        }
    }
}
