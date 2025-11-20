using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using GxShared.GlobModels;
using Microsoft.AspNetCore.Components.Authorization;

using Newtonsoft.Json.Linq;

namespace GxAdm.Services
{
    //Authentication Account Service
    public class MyAuthStateProvider : AuthenticationStateProvider
    {
  
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientFactory _httpClientFactory;
        private LoginResult _logResu;
        //private IAuthenticationService _authService;
        public MyAuthStateProvider(ILocalStorageService localStorage,
            IHttpClientFactory httpClientFactory)
        {
            _localStorage = localStorage;
            _httpClientFactory = httpClientFactory;
            //_currentUser = currentUser;
            _logResu = new LoginResult();
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("blazToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("Token is missing or empty.");
                var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
                return new AuthenticationState(anonymous);
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                var claims = jwt.Claims;
                var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

                var roles = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
                await _localStorage.SetItemAsync("blazRoles", roles);
                await _localStorage.SetItemAsync("blazExp", jwt.ValidTo);

                NotifyUserAuthentication(user);
                return new AuthenticationState(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token parsing failed: {ex.Message}");
                var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
                return new AuthenticationState(anonymous);
            }
        }
        public IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            if (!string.IsNullOrEmpty(jwt))
            {
                var payload = jwt.Split('.')[1];
                var jsonBytes = ParseBase64WithoutPadding(payload);
                var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
                //return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
                foreach (var kvp in keyValuePairs)
                {
                    claims.Add(new Claim(kvp.Key, kvp.Value.ToString()));
                    //Console.WriteLine($"kvp/ {kvp.Key} value/ { kvp.Value.ToString() }");
                }
            }
            return claims;
        }
        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            //Console.WriteLine($"base64: {base64}");
            return Convert.FromBase64String(base64);
        }
        public void NotifyUserAuthentication(ClaimsPrincipal authenticatedUser)
        {
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }
        public async Task NotifyUserLogout()
        {
            //Blazor side
            await _localStorage.RemoveItemAsync("blazToken");
            await _localStorage.RemoveItemAsync("blazAuthen");
            await _localStorage.RemoveItemAsync("blazUserid");
            await _localStorage.RemoveItemAsync("blazOrgid");
            await _localStorage.RemoveItemAsync("blazRoles");
            await _localStorage.RemoveItemAsync("blazExp");
            //Aurelia side
            await _localStorage.RemoveItemAsync("clieAuthen");
            await _localStorage.RemoveItemAsync("clieToken");
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var anoauthstate = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(anoauthstate);
        }
        public class JwtConfig
        {
            public string Secret { get; set; } = string.Empty;
            public string Issuer { get; set; } = string.Empty;
            public string Audience { get; set; } = string.Empty;
            public string Akeyid { get; set; } = string.Empty;
        }
    }
}