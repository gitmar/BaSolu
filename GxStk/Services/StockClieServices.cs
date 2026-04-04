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
        private readonly HttpClient _http;

        public StockClientService(HttpClient http)
        {
            _http = http;
        }
        public async Task AddStock(int itemId, int warehouseId, decimal quantity, decimal unitCost, int? batchId)
        {
            var response = await _http.PostAsJsonAsync(
                $"odata/Stkita({itemId})/AddStock",
                new { warehouseId, quantity, unitCost, batchId });

            response.EnsureSuccessStatusCode();
        }

        public async Task AdjustStockBatch(AdjustmentRequestDto request)
        {
            var response = await _http.PostAsJsonAsync(
                "odata/Stkita/AdjustStockBatch",
                new { request });

            response.EnsureSuccessStatusCode();
        }

        public async Task CreateSale(int clientId, List<CreateSaleLineDto> lines)
        {
            var response = await _http.PostAsJsonAsync(
                "odata/Stkita/CreateSale",
                new { clientId, lines });

            response.EnsureSuccessStatusCode();
        }

        public async Task AddItemEntry(ItemEntryLineDto line)
        {
            await _http.PostAsJsonAsync(
                $"odata/Stkita({line.ItemId})/AddStock",
                new
                {
                    warehouseId = line.WarehouseId,
                    quantity = line.Quantity,
                    unitCost = line.UnitCost,
                    batchId = line.BatchId
                });
        }
        public async Task CreatePurchase(ItemPurchaseRequestDto request)
        {
            await _http.PostAsJsonAsync(
                "odata/Stkita/CreatePurchase", // optional endpoint if you implement purchase
                request);
        }
        public async Task TransferStock(ItemTransferLineDto line)
        {
            await _http.PostAsJsonAsync(
                $"odata/Stkita({line.ItemId})/TransferStock",
                new
                {
                    fromWarehouseId = line.FromWarehouseId,
                    toWarehouseId = line.ToWarehouseId,
                    quantity = line.Quantity,
                    batchId = line.BatchId,
                    transferType = line.TransferType
                });
        }
        public async Task<List<Stkitum>> GetItemsAsync()
            => await _http.GetFromJsonAsync<List<Stkitum>>("odata/Stkita");

        public async Task<List<Gsloca>> GetWarehousesAsync()
            => await _http.GetFromJsonAsync<List<Gsloca>>("odata/Gslocas");

        public async Task<List<Tiersp>> GetSuppliersAsync()
            => await _http.GetFromJsonAsync<List<Tiersp>>("odata/Tiersps");
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
//public class StockClientService
//{
//    private readonly MyODataContext _context;
//    public StockClientService(MyODataContext context)
//    {
//        _context = context;
//    }
//    public async Task AddStockAsync(int itemId, int warehouseId, decimal qty, decimal cost)
//    {
//        var uri = new Uri($"Items({itemId})/AddStock", UriKind.Relative);

//        var parameters = new Dictionary<string, object>
//    {
//        { "WarehouseId", warehouseId },
//        { "Quantity", qty },
//        { "UnitCost", cost },
//        { "BatchId", null }
//    };

//        //await _context.Context.ExecuteAsync(uri, "POST", parameters);
//    }

//    public async Task RemoveStockAsync(int itemId, int warehouseId, decimal qty, decimal price)
//    {
//        var uri = new Uri($"Items({itemId})/RemoveStock", UriKind.Relative);

//        var parameters = new Dictionary<string, object>
//    {
//        { "WarehouseId", warehouseId },
//        { "Quantity", qty },
//        { "UnitCost", price },
//        { "BatchId", null }
//    };

//        //await _context.ExecuteQueryAsync(uri, "POST", parameters);
//    }
//}
