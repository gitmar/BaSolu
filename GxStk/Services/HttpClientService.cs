using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using Blazored.LocalStorage;

using Newtonsoft.Json.Linq;
namespace GxStk.Services
{
    public class HttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalStorageService _localStorage;
        private readonly Dictionary<string, HttpClient> _clients = new();

        public HttpClientService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage)
        {
            _httpClientFactory = httpClientFactory;
            _localStorage = localStorage;
            _clients["AuthClient"] = _httpClientFactory.CreateClient("AuthClient");
            _clients["LocalClient"] = _httpClientFactory.CreateClient("LocalClient");
            _clients["ODataClient"] = _httpClientFactory.CreateClient("ODataClient");
            // Add others as needed

        }

        public async Task<string> SendRequestAsync(string clientName, HttpMethod method, string requestUri, object? content = null)
        {
            var token = await _localStorage.GetItemAsync<string>("blazToken");
            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("Token not available yet — skipping request.");
                return null; // or throw, or return empty string
            }
            var client = _httpClientFactory.CreateClient(clientName);
            var request = new HttpRequestMessage(method, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (method == HttpMethod.Post && content != null)
            {
                var json = JsonSerializer.Serialize(content);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task SetAuthorizationHeaderAsync(string clientname)
        {
            if (!_clients.ContainsKey(clientname))
            {
                throw new InvalidOperationException($"HttpClient {clientname} not found.");
            }

            var token = await _localStorage.GetItemAsync<string>("blazToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                _clients[clientname].DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                Console.WriteLine($"Setting Token is null or empty : {token}");
            }
        }
        // LoadOrgaAsync - Bypass ODataClient for single DTOs
        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,  // Match server casing
            PropertyNameCaseInsensitive = true
        };
        public async Task<TDto?> GetDtoWithExpandAsync<TDto>(string entitySet, int key, string expand)
        where TDto : class
        {
            var client = _httpClientFactory.CreateClient("ODataClient");
            var response = await client.GetAsync($"{entitySet}({key})?$expand={expand}");

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TDto>(json, jsonOptions);
        }

    }
    public class TokenAwareClientManager
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientFactory _httpClientFactory;

        public TokenAwareClientManager(ILocalStorageService localStorage, IHttpClientFactory httpClientFactory)
        {
            _localStorage = localStorage;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpClient> GetHttpClientAsync(string clientName)
        {
            var token = await _localStorage.GetItemAsync<string>("blazToken");
            var client = _httpClientFactory.CreateClient(clientName);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }
    }
}