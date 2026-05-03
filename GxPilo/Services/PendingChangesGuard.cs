using AutoMapper;
using GxShared.GxDtos;
using Newtonsoft.Json;
using Simple.OData.Client;
namespace GxPilo.Services
{
    public sealed class PendingChangesGuard : IAsyncDisposable
    {
        private readonly ODataClient _client;
        private readonly IMapper _mapper;
        private readonly List<Func<ODataClient, IMapper, Task>> _pendingOps = new();

        private int _insertCount;
        private int _updateCount;
        private int _deleteCount;

        public event Action? OnChanges;

        public PendingChangesGuard(ODataClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        // ✅ Track Query
        public void TrackQuery<TDto>(string entitySet, Func<IBoundClient<TDto>, IBoundClient<TDto>>? queryBuilder = null)
    where TDto : class
        {
            _pendingOps.Add(async (client, mapper) =>
            {
                var bound = client.For<TDto>(entitySet);

                // Apply optional conditions (filter, expand, orderby, etc.)
                if (queryBuilder != null)
                {
                    bound = queryBuilder(bound);
                }

                var results = await bound.FindEntriesAsync();

                // You can now map results or store them
                foreach (var dto in results)
                {
                    Console.WriteLine($"Fetched entity: {JsonConvert.SerializeObject(dto)}");
                }
            });

            NotifyChanges();
        }
        // ✅ Direct Query
        public async Task<IEnumerable<TDto>> ExecuteQueryAsync<TDto>(
    string entitySet,
    Func<IBoundClient<TDto>, IBoundClient<TDto>>? queryBuilder = null)
    where TDto : class
        {
            var bound = _client.For<TDto>(entitySet);

            if (queryBuilder != null)
            {
                bound = queryBuilder(bound);
            }

            var results = await bound.FindEntriesAsync();
            return results;
        }

        // ✅ Track Insert
        public void TrackInsert<TDto>(string entitySet, TDto dto) where TDto : class
        {
            _pendingOps.Add(async (client, mapper) =>
            {
                var entity = mapper.Map<object>(dto);
                await client.For<object>(entitySet).Set(entity).InsertEntryAsync();
            });
            _insertCount++;
            NotifyChanges();
        }
        // ✅ Track Update
        public void TrackUpdate<TDto>(string entitySet, object key, TDto dto) where TDto : class
        {
            _pendingOps.Add(async (client, mapper) =>
            {
                // Serialize DTO → Dictionary
                string json = JsonConvert.SerializeObject(dto);
                var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                // Filter persisted fields (exclude keys, UI-only, etc.)
                var filtered = PersistedFieldRegistry.Filter<TDto>(dict);

                // Remove nulls (Delta<T> expects only changed fields)
                var nonNulls = filtered
                    .Where(kvp => kvp.Value != null)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                // Log payload for debugging
                //Console.WriteLine(JsonConvert.SerializeObject(nonNulls, Formatting.Indented));

                // Send PATCH with only changed fields
                await client.For(entitySet)
                            .Key(key)
                            .Set(nonNulls)
                            .UpdateEntryAsync();
            });
            _updateCount++;
            NotifyChanges();
        }
        // ✅ Track Delete
        public void TrackDelete(string entitySet, object key)
        {
            _pendingOps.Add(async (client, mapper) =>
            {
                await client.For(entitySet)
                            .Key(key)
                            .DeleteEntryAsync();
            });

            _deleteCount++;
            NotifyChanges();
        }
        // ✅ Counts
        public bool HasPendingChanges() => _pendingOps.Count > 0;
        public int GetPendingChangesCount() => _pendingOps.Count;
        public int GetInsertCount() => _insertCount;
        public int GetUpdateCount() => _updateCount;
        public int GetDeleteCount() => _deleteCount;

        // ✅ Commit all tracked operations
        public async Task FlushAsync()
        {
            if (!HasPendingChanges())
            {
                Console.WriteLine("No pending changes to flush");
                return;
            }

            Console.WriteLine($"Flushing {GetPendingChangesCount()} changes...");
            foreach (var op in _pendingOps)
            {
                await op(_client, _mapper);
            }

            _pendingOps.Clear();
            _insertCount = _updateCount = _deleteCount = 0;
            NotifyChanges();
        }

        // ✅ Cancel all tracked operations
        public async Task CancelChanges()
        {
            Console.WriteLine("Cancelling all pending changes");
            _pendingOps.Clear();
            _insertCount = _updateCount = _deleteCount = 0;
            NotifyChanges();
        }

        private void NotifyChanges() => OnChanges?.Invoke();

        public ValueTask DisposeAsync()
        {
            Console.WriteLine("Disposing PendingChangesGuard");
            _pendingOps.Clear();
            _insertCount = _updateCount = _deleteCount = 0;
            return ValueTask.CompletedTask;
        }
    }
}
