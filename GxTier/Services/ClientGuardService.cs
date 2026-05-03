using GxShared.GxDtos;
using GxShared.Interfaces;
using TG.Blazor.IndexedDB;

namespace GxTie.Services
{
    public sealed class IndexedDbOpsStore : IPendingOpsStore
    {
        private readonly IndexedDBManager _db;

        public IndexedDbOpsStore(IndexedDBManager db) => _db = db;

        public async Task AddRecordAsync(PendingOpRecord record)
        {
            var storeRecord = new StoreRecord<PendingOpRecord>
            {
                Storename = "PendingOps",
                Data = record
            };
            await _db.AddRecord(storeRecord);
        }

        public async Task<List<PendingOpRecord>> GetRecordsAsync()
        {
            return await _db.GetRecords<PendingOpRecord>("PendingOps");
        }

        public async Task ClearStoreAsync()
        {
            await _db.ClearStore("PendingOps");
        }
    }

}
