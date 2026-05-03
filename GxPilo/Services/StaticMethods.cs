using AutoMapper;

using Simple.OData.Client;

namespace GxPilo.Services
{
    public static class ODataClientExtensions
    {
        // ✅ Explicit Update
        public static async Task MarkModifiedAsync<TDto>(
            this ODataClient client,
            string entitySet,
            object key,
            TDto dto,
            IMapper mapper)
            where TDto : class
        {
            var entity = mapper.Map<object>(dto);
            await client.For<object>(entitySet).Key(key).Set(entity).UpdateEntryAsync();
        }

        // ✅ Check if entity is new
        public static async Task<bool> IsNewAsync<TDto>(
            this ODataClient client,
            string entitySet,
            object key)
            where TDto : class
        {
            // Try to load by key
            var existing = await client.For<TDto>(entitySet).Key(key).FindEntryAsync();
            return existing == null;
        }
    }
}
