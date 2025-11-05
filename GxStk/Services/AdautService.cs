using System.Net.Http.Json;

using GxShared.GlobModels;

namespace GxStk.Services
{
    public class AdautService
    {
        private readonly HttpClient _apiClient;
        private Secubag _secbag { get; set; }
        public AdautService(Secubag secubag, IHttpClientFactory httpCliefact)
        {
            _secbag = secubag;
            _apiClient = httpCliefact.CreateClient("BZEClient");
        }
        public async Task<bool> SetMySecuAsync()
        {
            bool yabag = false;
            Secubag nwsecbag = new Secubag();
            try
            {
                var response = await _apiClient.GetAsync("lgauth/orgusers");
                if (response.IsSuccessStatusCode)
                {
                    //Console.WriteLine("LECTURE LoadApiRecord");
                    _secbag = await response.Content.ReadFromJsonAsync<Secubag>();
                    //Console.WriteLine($"nb auth : {_secbag.Fxauts.Count()}");
                    //Console.WriteLine($"nb roles : {_secbag.Fxrols.Count()}");
                    //Console.WriteLine($"nb usrirol : {_secbag.FxURAs.Count()}");
                    yabag = true;
                }
            } catch(Exception ex)
            {
                Console.WriteLine($"ERREUR {ex.Message}");
            }
            return yabag;
        }
        public async Task<Secubag> GetMySecuAsync()
        {
            if (_secbag != null)
            {
                return _secbag;
            } else
            {
                await SetMySecuAsync();
                if (_secbag == null) _secbag = new Secubag();
                return _secbag;
            }
        }
    }
}