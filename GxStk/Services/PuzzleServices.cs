using System.Net.Http.Json;
using System.Security.Claims;

using Blazored.LocalStorage;

using GxStk.ClieModels;

using GxShared.GlobModels;

using Microsoft.AspNetCore.Components.Authorization;

using static GxStk.Services.PuzzleSyncService;

namespace GxStk.Services
{
    public interface IPuzzleSyncService
    {
        Task<bool> EnsureSessionIsFreshAsync();
        Task<SessionSyncResult> SyncPuzzleStateAsync();
        Task ClearPuzzleStateAsync();
        Task ForceLogoutAsync();
    }

    public class PuzzleSyncService : IPuzzleSyncService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MyAuthStateProvider _authProvider;
        private readonly Userbag _userbag;
        private readonly MyShareVars _shareVars;
        
        public PuzzleSyncService(ILocalStorageService localStorage, IHttpClientFactory httpClientFactory, MyAuthStateProvider authProvider, Userbag userbag, MyShareVars shareVars)
        {
            _localStorage = localStorage;
            _httpClientFactory = httpClientFactory;
            _authProvider = authProvider;
            _userbag = userbag;
            _shareVars = shareVars;
        }

        public async Task<bool> EnsureSessionIsFreshAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("blazToken");
            var exp = await _localStorage.GetItemAsync<DateTime>("blazExp");

            if (string.IsNullOrEmpty(token) || exp < DateTime.UtcNow.AddMinutes(5))
            {
                var refreshToken = await _localStorage.GetItemAsync<string>("blazRtoken");
                if (string.IsNullOrEmpty(refreshToken))
                {
                    await _authProvider.NotifyUserLogout();
                    return false;
                }

                var client = _httpClientFactory.CreateClient("TOKENClient");
                var response = await client.PostAsJsonAsync("lgauth/refresh", new { RefreshToken = refreshToken });

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Refresh failed: {response.StatusCode}");
                    await _authProvider.NotifyUserLogout();
                    return false;
                }

                var loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();
                if (loginResult == null || string.IsNullOrEmpty(loginResult.Atoken) || loginResult.Adexp < DateTime.UtcNow)
                {
                    await _authProvider.NotifyUserLogout();
                    return false;
                }

                await _localStorage.SetItemAsync("blazToken", loginResult.Atoken);
                await _localStorage.SetItemAsync("blazExp", loginResult.Adexp);
                await _localStorage.SetItemAsync("blazRtoken", loginResult.Rtoken);
                await _localStorage.SetItemAsync("blazUserid", loginResult.Userid);
                await _localStorage.SetItemAsync("blazOrgid", loginResult.Uorgid);

                var claims = _authProvider.ParseClaimsFromJwt(loginResult.Atoken);
                var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
                _authProvider.NotifyUserAuthentication(user);

                return true;
            }
            Console.WriteLine($"Token is valid");
            // Token is still valid — no refresh needed
            return true;
        }
        public async Task<SessionSyncResult> SyncPuzzleStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("blazToken");
            var userid = await _localStorage.GetItemAsync<string>("blazUserid");
            var orgid = await _localStorage.GetItemAsync<string>("blazOrgid");
            var roles = await _localStorage.GetItemAsync<List<string>>("blazRoles");
            var exp = await _localStorage.GetItemAsync<DateTime>("blazExp");

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userid))
            {
                return new SessionSyncResult
                {
                    Success = false,
                    Message = "Missing token or userid.",
                    Context = null
                };
            }

            var context = new SessionContext
            {
                UserId = userid,
                OrgId = orgid,
                Roles = roles ?? new List<string>(),
                Token = token,
                ExpiryUtc = exp
            };

            // Hydrate Userbag
            _userbag.Userid = context.UserId;
            _userbag.Idorg = short.TryParse(context.OrgId, out var parsedOrg) ? parsedOrg : (short)0;
            _userbag.SiOwner = context.Roles.Contains("Admin");
            if (_userbag.GetType().GetProperty("IsSupport") != null)
                _userbag.GetType().GetProperty("IsSupport")?.SetValue(_userbag, context.Roles.Contains("Support"));

            // Hydrate MyShareVars
            if (_shareVars.GetType().GetProperty("SessionToken") != null)
                _shareVars.GetType().GetProperty("SessionToken")?.SetValue(_shareVars, context.Token);
            if (_shareVars.GetType().GetProperty("IsPuzzleReady") != null)
                _shareVars.GetType().GetProperty("IsPuzzleReady")?.SetValue(_shareVars, true);
            if (_shareVars.GetType().GetProperty("LastSyncUtc") != null)
                _shareVars.GetType().GetProperty("LastSyncUtc")?.SetValue(_shareVars, DateTime.UtcNow);

            return new SessionSyncResult
            {
                Success = true,
                Message = "Puzzle sync completed.",
                Context = context
            };
        }
        public async Task ForceLogoutAsync()
        {
            if (_authProvider is MyAuthStateProvider myAuth)
                await myAuth.NotifyUserLogout();

            _shareVars.isAuthenticated = false;
            //>>>_userbag.Clear();
        }
        public async Task ClearPuzzleStateAsync()
        {
            //_puzzleState = null;
            //_sessionMetadata = null;
            await _localStorage.RemoveItemAsync("puzzleState");
        }
        public class SessionContext
        {
            public string UserId { get; set; }
            public string OrgId { get; set; }
            public List<string> Roles { get; set; } = new();
            public string Token { get; set; }
            public DateTime ExpiryUtc { get; set; }
        }
        public class SessionSyncResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public SessionContext Context { get; set; }
        }
    }
}
