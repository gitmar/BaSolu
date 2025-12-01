using System.Net.Http.Json;
using System.Security.Claims;

using Blazored.LocalStorage;

using GxStk.Helpers;
using GxStk.Profile;

using GxShared.Sess;
using GxShared.Identity;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

using Newtonsoft.Json;

using static GxStk.Services.PuzzleSyncService;
namespace GxStk.Services
{
    public class SessionContextService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authProvider;
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClientService _apiClient;
        public LoginResult? LoginResponse { get; private set; }
        public Userbag? Userbag { get; private set; }
        public SessionDto? CurrentSession { get; private set; }

        private bool _isInitialized = false;
        private string errorMessage = "";
        public bool IsAuthenticated => LoginResponse?.IsAuthenticated == true;
        public bool IsSessionValid => IsAuthenticated && !string.IsNullOrWhiteSpace(LoginResponse?.Userid);

        public event Action? OnSessionInvalidated;

        public SessionContextService(ILocalStorageService localStorage, AuthenticationStateProvider authProvider, HttpClientService apiClient)
        {
            _localStorage = localStorage;
            _authProvider = authProvider;
            _apiClient = apiClient;
        }
        public async Task InitializeAsync()
        {
            LoginResponse = await _localStorage.GetItemAsync<LoginResult>("blazLoginResponse");
            Userbag = await _localStorage.GetItemAsync<Userbag>("blazUserbag");
            CurrentSession = await _localStorage.GetItemAsync<SessionDto>("blazSessionDto");

            Console.WriteLine("SESSION HERE");

            if (!IsSessionValid)
            {
                OnSessionInvalidated?.Invoke();
                return;
            }

            Console.WriteLine("SESSION HAS BEEN INITIALIZED");

            // Optional: validate token online only if connected
            if (await IsOnlineAsync())
            {
                try
                {
                    var response = await _apiClient.SendRequestAsync("AUTHClient", HttpMethod.Get, $"usercontext/session");
                    var sessionDto = JsonConvert.DeserializeObject<SessionDto>(response);
                    if (sessionDto != null)
                    {
                        CurrentSession = sessionDto;
                        await _localStorage.SetItemAsync("blazSessionDto", sessionDto);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Session validation failed: {ex.Message}");
                    // Optional: invalidate session if token is rejected
                    // OnSessionInvalidated?.Invoke();
                }
            }
        }
        public bool HasRole(string role) =>
            CurrentSession?.Roles?.Contains(role) == true;

        public string? OrgName => CurrentSession?.OrgName;
        public async Task<bool> IsOnlineAsync()
        {
            try
            {
                return await _jsRuntime.InvokeAsync<bool>("navigator.onLine");
            }
            catch
            {
                return false;
            }
        }
        //public async Task InitializeAsync()
        //{
        //    if (_isInitialized) return;
        //    _isInitialized = true;
        //    LoginResponse = await _localStorage.GetItemAsync<LoginResponse>("blazLoginResponse");
        //    Userbag = await _localStorage.GetItemAsync<Userbag>("blazUserbag");
        //    Console.WriteLine("SESSION HERE");
        //    if (!IsSessionValid)
        //    {
        //        OnSessionInvalidated?.Invoke();
        //    }
        //    // Optional: validate token online
        //    CurrentSession = await _localStorage.GetItemAsync<SessionDto>("blazSessionDto");
        //    Console.WriteLine("SESSION HAS BEEN INITIALIZED");
        //    var response = await _apiClient.SendRequestAsync("AUTHClient", HttpMethod.Get, $"usercontext/session");
        //    var sessionDto = JsonConvert.DeserializeObject<SessionDto>(response);
        //    if (sessionDto != null)
        //    {
        //        await _localStorage.SetItemAsync("blazSessionDto", sessionDto);
        //    }
        //}

        public async Task PersistSessionAsync(LoginResult login, Userbag userbag)
        {
            LoginResponse = login;
            await SetUserbagAsync(userbag); // ✅ centralizes Userbag persistence

            await _localStorage.SetItemAsync("blazToken", login.Atoken);
            await _localStorage.SetItemAsync("blazRtoken", login.Rtoken);
            await _localStorage.SetItemAsync("blazExp", login.Adexp);
            await _localStorage.SetItemAsync("blazUserid", login.Userid);
        }

        public async Task ClearSessionAsync()
        {
            LoginResponse = null;
            Userbag = null;

            await _localStorage.RemoveItemAsync("blazLoginResponse");
            await _localStorage.RemoveItemAsync("blazUserbag");

            OnSessionInvalidated?.Invoke();
        }
        public async Task<bool> HydrateUserbagAsync()
        {
            if (LoginResponse == null || string.IsNullOrWhiteSpace(LoginResponse.Userid))
                return false;

            var orgid = await _localStorage.GetItemAsync<int>("blazOrgid");

            var response = await _apiClient.SendRequestAsync("AUTHClient", HttpMethod.Get, $"usercontext/userbag");
            var result = JsonConvert.DeserializeObject<Userbag>(response);
            if (Userbag == null || !result.Succeeded)
            {
                OnSessionInvalidated?.Invoke();
                errorMessage = "Failed to load user context.";
                return false;
            }
            //var userbag = await http.GetFromJsonAsync<Userbag>($"api/");

            //Userbag = result;
            await _localStorage.SetItemAsync("blazUserbag", Userbag);
            await PersistSessionAsync(LoginResponse, Userbag);
            return true;
        }
        public async Task<bool> TryRefreshTokenAsync()
        {
            var rtoken = await _localStorage.GetItemAsync<string>("blazRtoken");
            var response = await _apiClient.SendRequestAsync("AUTHClient", HttpMethod.Post, "lgauth/refresh", new { Rtoken = rtoken });
            var newTokens = JsonConvert.DeserializeObject<LoginResult>(response);

            if (newTokens?.Atoken == null) return false;

            await _localStorage.SetItemAsync("blazToken", newTokens.Atoken);
            await _localStorage.SetItemAsync("blazRtoken", newTokens.Rtoken);
            return true;
        }
        public async Task SetUserbagAsync(Userbag bag)
        {
            Userbag = bag;
            Console.WriteLine($"USERBAG is set {Userbag.Idorg}");
            await _localStorage.SetItemAsync("blazUserBag", JsonConvert.SerializeObject(bag));
        }

        public async Task LoadUserbagAsync()
        {
            var json = await _localStorage.GetItemAsync<string>("blazUserBag");
            if (!string.IsNullOrWhiteSpace(json))
            {
                Console.WriteLine($"Bag prise {json}");
                Userbag = JsonConvert.DeserializeObject<Userbag>(json);
            }
        }
        public async Task<Userbag?> GetUserbagAsync()
        {
            if (Userbag != null)
                return Userbag;

            var json = await _localStorage.GetItemAsync<string>("blazUserBag");
            if (!string.IsNullOrWhiteSpace(json))
            {
                try
                {
                    Userbag = JsonConvert.DeserializeObject<Userbag>(json);
                    Console.Write($"Userbag got {json}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Userbag deserialization failed: {ex.Message}");
                }
            }

            return Userbag;
        }
    }
    public class SessionContextClient
    {
        private readonly HttpClientService _apiClient;
        public SessionContextClient(HttpClientService apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<Userbag?> GetUserbagAsync()
        {
            var response = await _apiClient.SendRequestAsync("AUTHClient", HttpMethod.Get, $"usercontext/userbag");
            return JsonConvert.DeserializeObject<Userbag>(response);
        }  
    }
}