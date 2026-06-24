namespace GxPilo.Components.Plans
{
    using GxShared.GxDtos;
    using GxShared.Interfaces;

    using Simple.OData.Client;
 
    public class PlanRepository : ICrudRepository<PlngenDto>
    {
        private readonly IODataClient _client;
        public string EntitySet => "Plngens";

        public PlanRepository(IODataClient client)
        {
            _client = client;
        }

        public Task<PlngenDto> CreateAsync(PlngenDto entity) => _client.AddAsync<PlngenDto>(EntitySet, entity);
        public Task<PlngenDto> UpdateAsync(PlngenDto entity) => _client.UpdateAsync(EntitySet, entity);
        public Task DeleteAsync(PlngenDto entity) => _client.DeleteAsync(EntitySet, entity.Id);
        public bool Validate(PlngenDto entity) => !string.IsNullOrWhiteSpace(entity.Liba); // Example
    }
}
