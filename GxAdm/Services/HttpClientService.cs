using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using Blazored.LocalStorage;

using Newtonsoft.Json.Linq;
namespace GxAdm.Services
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
            _clients["AUTHClient"] = _httpClientFactory.CreateClient("AUTHClient");
            _clients["OFFLClient"] = _httpClientFactory.CreateClient("OFFLClient");
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
            } else
            {
                Console.WriteLine($"Setting Token is null or empty : {token}");
            }
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
        //public async Task<string> SendRequestAsync(string clientname, HttpMethod method, 
        //    string requestUri, object? content = null)
        //{
        //    if (!_clients.ContainsKey(clientname))
        //    {
        //        throw new InvalidOperationException($"HttpClient {clientname} not found. ");
        //    }
        //    var request = new HttpRequestMessage(method, requestUri);

        //    if (method == HttpMethod.Post && content != null)
        //    {
        //        request.Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
        //    }
        //    var response = await _clients[clientname].SendAsync(request);
        //    response.EnsureSuccessStatusCode();
        //    return await response.Content.ReadAsStringAsync();
        //}
        //public async Task<HttpResponseMessage> SendOdareqAsync(string clientname, HttpMethod method,
        //    string requestUri, object? content = null)
        //{
        //    if (!_clients.ContainsKey(clientname))
        //    {
        //        throw new InvalidOperationException($"HttpClient {clientname} not found. ");
        //    }
        //    var request = new HttpRequestMessage(method, requestUri);

        //    if (method == HttpMethod.Post && content != null)
        //    {
        //        request.Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
        //    }
        //    var response = await _clients[clientname].SendAsync(request);
        //    response.EnsureSuccessStatusCode();
        //    return response;
        //}
        //public async Task<byte[]> GetRptBytesAsync(string requestUri)
        //{
        //    var response = await _clients["AUTHClient"].GetByteArrayAsync(requestUri);
        //    return response;
        //}
        //public async Task SetAuthorizationHeaderAsync(string clientname)
        //{
        //    if (!_clients.ContainsKey(clientname))
        //    {
        //        throw new InvalidOperationException($"HttpClient {clientname} not found.");
        //    }

        //    var token = await _localStorage.GetItemAsync<string>("blazToken");
        //    if (!string.IsNullOrWhiteSpace(token))
        //    {
        //        _clients[clientname].DefaultRequestHeaders.Authorization =
        //            new AuthenticationHeaderValue("Bearer", token);
        //    }
        //}
//    }
//}
