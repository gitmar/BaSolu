using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

using Blazored.LocalStorage;

using GxShared.Sess;
using GxShared.Identity;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json.Linq;

namespace GxTie.Services
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
        //public void NotifyUserAuthentication(ClaimsPrincipal authenticatedUser)
        //{
        //    var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        //    NotifyAuthenticationStateChanged(authState); // ✅ internal trigger
        //}
        //public async Task<bool> ValidateToken(string token, JsonWebKey[] usrkeys)
        //{
        //    int bida = 1;
        //    try
        //    {
        //        //var keys = await GetJsonWebKeys(username);
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var jwToken = tokenHandler.ReadJwtToken(token);
        //        bida = 2;
        //        var kid = jwToken.Header["kid"].ToString();
        //        ////Console.WriteLine($"Valeur KID : {kid}");
        //        bida = 3;
        //        ////Console.WriteLine($"Valeur SignKEY : {jwToken}");
        //        var signkey = jwToken.SigningKey.ToString();
        //        bida = 4;
        //        var signingKey = usrkeys.FirstOrDefault(k => k.Kid == kid);
        //        bida = 5;
        //        if (signingKey == null)
        //        {
        //            bida = 6;
        //            Console.WriteLine($"vue kid : {kid}");
        //            //Console.WriteLine($"vue signingKey : {signingKey}");
        //            throw new SecurityTokenInvalidSignatureException("Invalid kid.");
        //        }
        //        bida = 7;
        //        byte[] nwsigningKey = Encoding.UTF8.GetBytes(signkey);
        //        ////Console.WriteLine($"after kid : {kid}");
        //        ////Console.WriteLine($"after signingKey : {signingKey}");
        //        //decoding jsonweb key
        //        bida = 8;
        //        byte[] verifyingKey = Convert.FromBase64String(signingKey.K);
        //        var validationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(nwsigningKey), // Adjust if using asymmetric keys
        //            ValidateIssuer = true,
        //            ValidIssuer = "https://www.a24soft.com",
        //            ValidateAudience = true,
        //            ValidAudience = "https://www.gxclie.a24soft.com",
        //            ClockSkew = TimeSpan.Zero,
        //        };
        //        //bida = 9;
        //        //tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
        //        //var jwtSecurityToken = validatedToken as JwtSecurityToken;
        //        //if (jwtSecurityToken != null)
        //        //{
        //        //    bida = 10;
        //        //    var roles = jwtSecurityToken.Claims.Where(c => c.Type == ClaimTypes.Role)
        //        //            .Select(c => c.Value).ToList();
        //        //    //Use roles for further authorization logic
        //        //}
        //        //////Console.WriteLine("V---TOKEN VERIFIE");
        //        //    return true;
        //        //}
        //        //catch (Exception ex)
        //        //{
        //        //    Console.WriteLine($"V---TOKEN NON VERIFIE : {bida} erreur : { ex.Message }");
        //        //    return false;
        //        //}
        //        //}
        //        //private async Task<JsonWebKey[]> GetJsonWebKeys(String username)
        //        //{
        //        //    var gwtoken = await _localStorage.GetItemAsync<string>("blazToken");
        //        //    var _apiClient = _httpClientFactory.CreateClient("ASClient");
        //        //    var response = await _apiClient.GetAsync("lgauth/getkeytok?username=" + username);
        //        //    response.EnsureSuccessStatusCode();
        //        //    var jwks = await response.Content.ReadFromJsonAsync<Userjwt>();
        //        //    return jwks.JwKeys.ToArray();
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //    }
        //}

        public class JwtConfig
        {
            public string Secret { get; set; } = string.Empty;
            public string Issuer { get; set; } = string.Empty;
            public string Audience { get; set; } = string.Empty;
            public string Akeyid { get; set; } = string.Empty;
        }

    }
}