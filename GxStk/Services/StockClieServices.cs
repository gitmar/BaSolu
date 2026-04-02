using System.Net.Http.Json;
using GxShared.StkDtos;
using GxWapi.DaModels;

namespace GxStk.Services
{
    public class ItemClientService
    {
        private readonly IMyODataContext _context;

        public ItemClientService(IMyODataContext context)
        {
            _context = context;
        }

        public async Task<List<Stkitum>> GetItemsAsync()
        {
            var query = _context.Query<Stkitum>("Items")
                .AddQueryOption("$filter", "isactive eq true");

            return await _context.ExecuteQueryAsync<Stkitum>(query);
        }
    }
    public class StockClientService
    {
        private readonly MyODataContext _context;
        public StockClientService(MyODataContext context)
        {
            _context = context;
        }
        public async Task AddStockAsync(int itemId, int warehouseId, decimal qty, decimal cost)
        {
            var uri = new Uri($"Items({itemId})/AddStock", UriKind.Relative);

            var parameters = new Dictionary<string, object>
        {
            { "WarehouseId", warehouseId },
            { "Quantity", qty },
            { "UnitCost", cost },
            { "BatchId", null }
        };

            //await _context.Context.ExecuteAsync(uri, "POST", parameters);
        }

        public async Task RemoveStockAsync(int itemId, int warehouseId, decimal qty, decimal price)
        {
            var uri = new Uri($"Items({itemId})/RemoveStock", UriKind.Relative);

            var parameters = new Dictionary<string, object>
        {
            { "WarehouseId", warehouseId },
            { "Quantity", qty },
            { "UnitCost", price },
            { "BatchId", null }
        };

            //await _context.ExecuteQueryAsync(uri, "POST", parameters);
        }
    }
    public class PurchaseClientService
    {
        private readonly HttpClient _stkClient;

        public PurchaseClientService(HttpClient stkClient)
        {
            _stkClient = stkClient;
        }

        public async Task<int> CreatePurchaseAsync(CreatePurchaseDto dto)
        {
            var response = await _stkClient.PostAsJsonAsync("odata/Operations/CreatePurchase", dto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<int>();
        }
    }
    public class SaleClientService
    {
        private readonly HttpClient _http;

        public SaleClientService(HttpClient http)
        {
            _http = http;
        }

        public async Task<int> CreateSaleAsync(CreateSaleDto dto)
        {
            var response = await _http.PostAsJsonAsync("odata/Operations/CreateSale", dto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<int>();
        }
    }
}
