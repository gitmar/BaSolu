using Microsoft.JSInterop;

using System.Threading.Tasks;

namespace GxAdm.Services
{
    public class PouchDbService
    {
        private readonly IJSRuntime _jsRun;
        public PouchDbService(IJSRuntime jsRun)
        {
            _jsRun = jsRun;
        }
        public async Task SaveAsync(string id, string name, string city)
        {
            var doc = new
            {
                _id = id,
                name = name,
                city = city
            };
            await _jsRun.InvokeVoidAsync("SaveToPouchDB", doc);
        }
        public async Task<string> GetAsync(string id)
        {
            return await _jsRun.InvokeAsync<string>("getFromPouchDB", id); 
        }
    }
}
