using System.Net.Http.Json;

using GxShared.Sess;
using GxShared.StkDtos;

using GxWapi.DaModels;

namespace GxStk.Services
{
    public class PersonClientService
    {
        private readonly IODataContextFactory _contextFactory;

        public PersonClientService(IODataContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Tiersp>> GetTiersAsync()
        {
            var context = await _contextFactory.CreateAsync();

            var query = context.Query<Tiersp>("Tiersp")
                .AddQueryOption("$filter", "IsActive eq true");

            var result = await context.ExecuteQueryAsync<Tiersp>(query);

            return result.ToList();
        }
    }
    public class TiersClientService
    {
        private readonly HttpClient _http;

        public TiersClientService(HttpClient http)
        {
            _http = http;
        }

        public async Task AddStock(int itemId, int warehouseId, decimal quantity, decimal unitCost, int? batchId)
        {
            var response = await _http.PostAsJsonAsync(
                $"odata/Stkitus({itemId})/AddStock",
                new { warehouseId, quantity, unitCost, batchId });

            response.EnsureSuccessStatusCode();
        }

        public async Task AdjustStockBatch(AdjustmentRequestDto request)
        {
            var response = await _http.PostAsJsonAsync(
                "odata/Stkitus/AdjustStockBatch",
                new { request });

            response.EnsureSuccessStatusCode();
        }

        public async Task CreateSale(int clientId, List<CreateSaleLineDto> lines)
        {
            var response = await _http.PostAsJsonAsync(
                "odata/Stkitus/CreateSale",
                new { clientId, lines });

            response.EnsureSuccessStatusCode();
        }

        public async Task AddItemEntry(ItemEntryLineDto line)
        {
            await _http.PostAsJsonAsync(
                $"odata/Stkitus({line.ItemId})/AddStock",
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
                "odata/Stkitus/CreatePurchase", // optional endpoint if you implement purchase
                request);
        }
        public async Task TransferStock(ItemTransferLineDto line)
        {
            await _http.PostAsJsonAsync(
                $"odata/Stkitus({line.ItemId})/TransferStock",
                new
                {
                    fromWarehouseId = line.FromWarehouseId,
                    toWarehouseId = line.ToWarehouseId,
                    quantity = line.Quantity,
                    batchId = line.BatchId,
                    transferType = line.TransferType
                });
        }
        //public async Task<List<Stkitu>> GetItemsAsync()
        //    => await _http.GetFromJsonAsync<List<Stkitu>>("odata/Stkitus");

        public async Task<List<Tieloca>> GetWarehousesAsync()
            => await _http.GetFromJsonAsync<List<Tieloca>>("odata/Gslocas");

        //public async Task<List<Tiersp>> GetSuppliersAsync()
        //    => await _http.GetFromJsonAsync<List<Tiersp>>("odata/Tiersps");
    }
    //public class TiersClientService
    //{
    //    private readonly DxsoluContext _context;
    //    private readonly MovementService _movement;

    //    public TiersClientService(DxsoluContext context, MovementService movement)
    //    {
    //        _context = context;
    //        _movement = movement;
    //    }

    //    public async Task MoveTiersAsync(int tierspId, int newHie, int newPost)
    //    {
    //        var tiers = await _context.Tiersps.FindAsync(tierspId);
    //        if (tiers == null) throw new Exception("Tiers not found");

    //        var oldHie = tiers.Ihie;
    //        var oldPost = tiers.Ipst;

    //        // Update state
    //        tiers.Ihie = newHie;
    //        tiers.Ipst = newPost;

    //        await _context.SaveChangesAsync();

    //        // Log movement
    //        await _movement.LogPersonAsync(
    //            orgId: tiers.Idorg,
    //            tierspId: tiers.Id,
    //            fromHie: oldHie,
    //            toHie: newHie,
    //            fromPost: oldPost,
    //            toPost: newPost,
    //            type: MoveType.PersonMove,
    //            userId: "system"
    //        );
    //    }
    //}
}
