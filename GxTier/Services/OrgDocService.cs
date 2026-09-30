using GxShared.GxDtos;

using Simple.OData.Client;

namespace GxTie.Services
{
    public class OrgDocService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrgDocService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private IODataClient SoluClient =>
            new ODataClient(_httpClientFactory.CreateClient("ODataClient").BaseAddress!.ToString());

        private IODataClient DocuClient =>
            new ODataClient(_httpClientFactory.CreateClient("ODataDocuClient").BaseAddress!.ToString());

        public async Task<(GxorgaDto Orga, List<GastoreDto> Stores, List<GadocDto> Docs, List<GaimgDto> Images)>
            GetOrgWithDocsAsync(int key)
        {
            // Load parent Gxorga from DxsoluContext (/odata)
            var orga = await SoluClient
                .For<GxorgaDto>("Gxorgas")
                .Expand("Plngens($expand=Rubvars)")
                .Key(key)
                .FindEntryAsync();

            // Load children from DxdocuContext (/odata3)
            var storesTask = DocuClient
                .For<GastoreDto>("Gastores")
                .Filter(f => f.Idorg == key)
                .FindEntriesAsync();

            var docsTask = DocuClient
                .For<GadocDto>("Gadocs")
                .Filter(f => f.Idorg == key)
                .FindEntriesAsync();

            var imgsTask = DocuClient
                .For<GaimgDto>("Gaimgs")
                .Filter(f => f.Idorg == key)
                .FindEntriesAsync();

            await Task.WhenAll(storesTask, docsTask, imgsTask);

            return (
                Orga: orga,
                Stores: (await storesTask).ToList(),
                Docs: (await docsTask).ToList(),
                Images: (await imgsTask).ToList()
            );
        }

        public async Task<GxorgaDto> CreateOrgWithInitialDocsAsync(GxorgaDto newOrga, GastoreDto? store = null, GadocDto? doc = null, GaimgDto? img = null)
        {
            // 1) Insert parent Gxorga in DxsoluContext (/odata)
            var createdOrga = await SoluClient
                .For<GxorgaDto>("Gxorgas")
                .Set(newOrga)
                .InsertEntryAsync();

            int key = createdOrga.Idorg;

            // 2) Optionally create initial child rows in DxdocuContext (/odata3)
            var tasks = new List<Task>();

            if (store != null)
            {
                store.Idorg = key;
                tasks.Add(DocuClient
                    .For<GastoreDto>("Gastores")
                    .Set(store)
                    .InsertEntryAsync());
            }

            if (doc != null)
            {
                doc.Idorg = key;
                tasks.Add(DocuClient
                    .For<GadocDto>("Gadocs")
                    .Set(doc)
                    .InsertEntryAsync());
            }

            if (img != null)
            {
                img.Idorg = key;
                tasks.Add(DocuClient
                    .For<GaimgDto>("Gaimgs")
                    .Set(img)
                    .InsertEntryAsync());
            }

            if (tasks.Count > 0)
                await Task.WhenAll(tasks);

            return createdOrga;
        }
    }
}
