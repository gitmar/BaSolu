using System.Net.Http.Headers;

using Blazored.LocalStorage;

namespace GxPilo.Services
{
    public class AuthDelegatingHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;
        public AuthDelegatingHandler(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _localStorage.GetItemAsync<string>("blazToken");
            Console.WriteLine($"[AuthHandler] Request: {request.Method} {request.RequestUri}");
            Console.WriteLine($"[AuthHandler] Token exists: {!string.IsNullOrWhiteSpace(token)}");
            Console.WriteLine($"[AuthHandler] Auth before: {request.Headers.Authorization}");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
