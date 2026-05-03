    using System.Net.Http.Json;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using GxShared.StkDtos;

    using GxWapi.DaModels;

    using static System.Net.WebRequestMethods;
    using System.Text.Json.Serialization;
    using Microsoft.EntityFrameworkCore;

    namespace GxTie.Services
    {
        public class ItemClientService
        {
            private readonly IODataContextFactory _contextFactory;

            public ItemClientService(IODataContextFactory contextFactory)
            {
                _contextFactory = contextFactory;
            }

            public async Task<List<Stkitu>> GetItemsAsync()
            {
                var context = await _contextFactory.CreateAsync();

                var query = context.Query<Stkitu>("Stkita")
                    .AddQueryOption("$filter", "IsActive eq true");

                var result = await context.ExecuteQueryAsync<Stkitu>(query);

                return result.ToList();
            }
        }
        public class StockClientService
        {
            private readonly HttpClient _http;
            private readonly TokenAwareClientManager _tokenAwareClie;

            public StockClientService(IHttpClientFactory httpClientFactory, TokenAwareClientManager tokenAwareClie)
            {
                _tokenAwareClie = tokenAwareClie;
                _http = httpClientFactory.CreateClient("AUTHClient");

            }
            public async Task AddStock(int itemId, int warehouseId, decimal quantity, decimal unitCost, int? batchId)
            {
                var response = await _http.PostAsJsonAsync(
                    $"Stkita({itemId})/AddStock",
                    new { warehouseId, quantity, unitCost, batchId });

                response.EnsureSuccessStatusCode();
            }

            public async Task AdjustStockBatch(AdjustmentRequestDto request)
            {
                var response = await _http.PostAsJsonAsync(
                    "Stkita/AdjustStockBatch",
                    new { request });

                response.EnsureSuccessStatusCode();
            }

            public async Task CreateSale(int clientId, List<CreateSaleLineDto> lines)
            {
                var response = await _http.PostAsJsonAsync(
                    "Stkita/CreateSale",
                    new { clientId, lines });

                response.EnsureSuccessStatusCode();
            }

            public async Task AddItemEntry(ItemEntryLineDto line)
            {
                await _http.PostAsJsonAsync(
                    $"Stkita({line.ItemId})/AddStock",
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
                    "Stkita/CreatePurchase", // optional endpoint if you implement purchase
                    request);
            }
            public async Task TransfStock(ItemTransferLineDto line)
            {
                await _http.PostAsJsonAsync(
                    $"Stkita({line.ItemId})/TransfStock",
                    new
                    {
                        fromWarehouseId = line.FromWarehouseId,
                        toWarehouseId = line.ToWarehouseId,
                        quantity = line.Quantity,
                        batchId = line.BatchId,
                        transferType = line.TransferType
                    });
            }
            public async Task<List<Rubhie>> GetDepartmentsAsync()
            {
                var client = await _tokenAwareClie.GetHttpClientAsync("ODataClient");
                var response = await client.GetFromJsonAsync<ODataResponse<Rubhie>>("Rubhies");
                return response?.Value ?? new();
            }
            public async Task<List<Stkitu>> GetItaAsync()
            {
                var client = await _tokenAwareClie.GetHttpClientAsync("ODataClient");
                var response = await client.GetFromJsonAsync<ODataResponse<Stkitu>>("Stkita");
                return response?.Value ?? new List<Stkitu>();
            }

            //public async Task<List<Rubpst>> GetWarehousesAsync()
            //{
            //    var client = await _tokenAwareClie.GetHttpClientAsync("ODataClient");
            //    var response = await client.GetFromJsonAsync<ODataResponse<Rubpst>>("Rubpsts");
            //    return response?.Value ?? new List<Rubpst>();
            //}

            //public async Task<List<Stkitum>> GetItaAsync()
            //    => await _http.GetFromJsonAsync<List<Stkitum>>("Stkita");

            //public async Task<List<Rubpst>> GetWarehousesAsync()
            //    => await _http.GetFromJsonAsync<List<Rubpst>>("Rubpsts");

            public async Task<List<Tiersp>> GetSuppliersAsync()
                => await _http.GetFromJsonAsync<List<Tiersp>>("Tiersps");
        }

        public class PurchaseClientService
        {
            private readonly HttpClient _http;
            private readonly TokenAwareClientManager _tokenAwareClie;

            public PurchaseClientService(IHttpClientFactory httpClientFactory, TokenAwareClientManager tokenAwareClie)
            {
                _tokenAwareClie = tokenAwareClie;
                _http = httpClientFactory.CreateClient("AUTHClient");
            }

            public async Task<int> CreatePurchaseAsync(CreatePurchaseDto dto)
            {
                var response = await _http.PostAsJsonAsync("odata/Operations/CreatePurchase", dto);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<int>();
            }

            public async Task<List<CreatePurchaseDto>> GetPurchasesAsync()
            {
                var client = await _tokenAwareClie.GetHttpClientAsync("ODataClient");
                var response = await client.GetFromJsonAsync<ODataResponse<CreatePurchaseDto>>("Purchases");
                return response?.Value ?? new List<CreatePurchaseDto>();
            }
        }

        public class SaleClientService
        {
            private readonly HttpClient _http;
            private readonly TokenAwareClientManager _tokenAwareClie;

            public SaleClientService(IHttpClientFactory httpClientFactory, TokenAwareClientManager tokenAwareClie)
            {
                _http = httpClientFactory.CreateClient("AUTHClient");
                _tokenAwareClie = tokenAwareClie;
            }

            public async Task<int> CreateSaleAsync(CreateSaleDto dto)
            {
                var response = await _http.PostAsJsonAsync("odata/Operations/CreateSale", dto);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<int>();
            }

            public async Task<List<CreateSaleDto>> GetSalesAsync()
            {
                var client = await _tokenAwareClie.GetHttpClientAsync("ODataClient");
                var response = await client.GetFromJsonAsync<ODataResponse<CreateSaleDto>>("Sales");
                return response?.Value ?? new List<CreateSaleDto>();
            }
        }

        public class MasterDataClientService
        {
            private readonly MyODataContext _ctx;

            public MasterDataClientService(MyODataContext ctx)
            {
                _ctx = ctx;
            }

            // ---------------- Categories ----------------
            public async Task<List<Stkcat>> GetCategories(int idorg)
            {
                return (await _ctx.Context.Stkcats
                    .Where(c => c.Idorg == idorg)
                    .ToListAsync());
            }

            public async Task CreateCategory(Stkcat cat)
            {
                _ctx.Context.AddToStkcats(cat);
                await _ctx.Context.SaveChangesAsync();
            }

            // ---------------- Units ----------------
            public async Task<List<Stkun>> GetUnits(int idorg)
            {
                return (await _ctx.Context.Stkuns
                    .Where(u => u.Idorg == idorg)
                    .ToListAsync());
            }

            public async Task CreateUnit(Stkun unit)
            {
                _ctx.Context.AddToStkuns(unit);
                await _ctx.Context.SaveChangesAsync();
            }

            // ---------------- Attributes ----------------
            public async Task<List<Stkatr>> GetAttributes(int idorg)
            {
                return (await _ctx.Context.Stkatrs
                    .Where(a => a.Idorg == idorg)
                    .ToListAsync());
            }

            public async Task CreateAttribute(Stkatr attr)
            {
                _ctx.Context.AddToStkatrs(attr);
                await _ctx.Context.SaveChangesAsync();
            }
        }
        //public class SaleClientService
        //{
        //    private readonly HttpClient _http;
        //    private readonly TokenAwareClientManager _tokenAwareClie;

        //    public SaleClientService(IHttpClientFactory httpClientFactory, TokenAwareClientManager tokenAwareClie)
        //    {
        //        _http = httpClientFactory.CreateClient("AUTHClient");
        //        _tokenAwareClie = tokenAwareClie;
        //    }

        //    public async Task<int> CreateSaleAsync(CreateSaleDto dto)
        //    {

        //        var response = await _http.PostAsJsonAsync("odata/Operations/CreateSale", dto);
        //        response.EnsureSuccessStatusCode();

        //        return await response.Content.ReadFromJsonAsync<int>();
        //    }
        //}
        public class ODataResponse<T>
        {
            [JsonPropertyName("value")]
            public List<T> Value { get; set; }
        }
    }
