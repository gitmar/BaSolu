using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Text.Json;

namespace GxStk.Services
{
    public class AdoStartup
    {
    }
    public class HybridStorageService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public HybridStorageService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        public async Task SaveData<T>(string key, T data)
        {
            try
            {
                // First try server
                await _http.PostAsJsonAsync("/api/sync", data);

                // Fallback to cache
                await _js.InvokeVoidAsync("cacheData", key, JsonSerializer.Serialize(data));
            }
            catch
            {
                // Ultimate fallback
                await _js.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(data));
            }
        }
    }
}
