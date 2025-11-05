//using GxSaie.Formulas.Engine.Parsing;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using Microsoft.IdentityModel.Tokens;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Reflection.Metadata;
//using System.Runtime.InteropServices;
//using System.Security.Claims;
//using System.Text;
//using System.Text.Json;
//using System.Threading;
//using System.Threading.Tasks;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GxShared
{
    internal class Broucode
    {
    }
}

//// Uafls = new List<Upara>();
//// var wufl = new Upara();
//// wufl.Tfl = 2;
//// wufl.Title = "CONJOINT(ES)";
//// wufl.Uabg = "cnj";
//// wufl.Uya = _usrBag.Usorga.Sicnjoint;
//// wufl.Umax = _usrBag.Usorga.Cnjmax;
//// Uafls.Add(wufl);
//// wufl = new Upara();
//// wufl.Tfl = 3;
//// wufl.Title = "PARENT(S)";
//// wufl.Uabg = "par";
//// wufl.Uya = _usrBag.Usorga.Siparent;
//// wufl.Umax = _usrBag.Usorga.Parmax;
//// Uafls.Add(wufl);
//// wufl = new Upara();
//// wufl.Tfl = 4;
//// wufl.Title = "ENFANT(S)";
//// wufl.Uabg = "enf";
//// wufl.Uya = _usrBag.Usorga.Sienfant;
//// wufl.Umax = _usrBag.Usorga.Enfmax;
//// Uafls.Add(wufl);
//// wufl = new Upara();
//// wufl.Tfl = 5;
//// wufl.Title = "PARAIN(S)";
//// wufl.Uabg = "pai";
//// wufl.Uya = _usrBag.Usorga.Siparain;
//// wufl.Umax = _usrBag.Usorga.Paimax;
//// Uafls.Add(wufl);
//// wufl = new Upara();
//// wufl.Tfl = 6;
//// wufl.Title = "FILHEUL(S)";
//// wufl.Uabg = "flh";
//// wufl.Uya = _usrBag.Usorga.Sifiheul;
//// wufl.Umax = _usrBag.Usorga.Flhmax;
//// Uafls.Add(wufl);//CRUD
//<@* div class="row align-items-center">
//                                                @if (allEnable == false && curOrga != null)
//                                                {
//                                                    <button class="col-md-1 padding-0 btn btn-primary btn-sm my-0" @onclick="() => ShowAddAnx(2)">Creer</button>
//                                                    if (sinwPanx)
//                                                    {
//                                                    <InputNumber class="my-input col-md-1" @bind-Value="newAnx.Elea" placeholder="No"
//                                                                 style="font-weight:bold">
//                                                    </InputNumber>
//                                                    <InputText class="my-input col-md-4" @bind-Value="newAnx.Liba" placeholder="Designation"
//                                                               style="font-weight:bold">
//                                                    </InputText>
//                                                    <InputText class="my-input col-md-2" @bind-Value="newAnx.Abg" placeholder="Abrege"
//                                                               style="font-weight:bold">
//                                                    </InputText>
//                                                    <InputSelect id="ntcnt" @bind-Value="newAnx.Totyp">
//                                                        @foreach (var tob in LsFixes.Where(ut => ut.Gvars == 53 && ut.Itb == 2 && ut.Elea != 0))
//                                                        {
//                                                            <option value="@tob.Elea">@tob.Liba</option>
//                                                        }
//                                                    </InputSelect>
//                                                    <InputNumber class="my-input col-md-1" @bind-Value="newAnx.Ivala" placeholder="Valeur" />
//                                                    <div class="col-md-1 btn-group">
//                                                        <button class="btn btn-success btn-sm" @onclick="() => ConfirmAddAnx(newAnx)">Ajout</button>
//                                                        &nbsp;&nbsp;
//                                                        <button class="btn btn-secondary btn-sm" @onclick="CancelAddAnx">Annul</button>
//                                                    </div>
//                                                    }
//                                                }
//                                            </div> *@
// private async Task LoadTiaplAsync()
// {
//     var expand = "Tiersps($expand=Tieafls),Tiewels($expand=Tiwafls)";
//     var token = await _localStorage.GetItemAsync<string>("blazToken");
//     curODContext = new MyODataContext(new Uri("https://localhost:7095/odata"), token);
//     var dxorgas = await curODContext.LoadFilteredAsync<Gxorga>(expand: expand);
//     //pisa = 3;
//     curOrga = dxorgas.FirstOrDefault();
//     if (curOrga == null)
//     {
//         Console.WriteLine("No organization found for the current user.");
//         return;
//     }
//     Console.WriteLine($"orga recu : {curOrga.Raison}");
//     Console.WriteLine($"nb tiers recu : {curOrga.Tiersps.Count()}");
//     Console.WriteLine($"nb tiwls recu : {curOrga.Tiewels.Count()}");
// }
//public class BzEntityManager
//{
//    private readonly EntityManager _entityManager;
//    //private readonly MyDataService _myDataService;
//    private readonly HttpClient _httpClient;
//    private readonly ITokenService _tokenService;
//    public BzEntityManager(EntityManager entityManager,
//        ITokenService tokenService
//       ) //, MyDataService myDataService, HttpClient httpClient)
//    {
//        _entityManager = entityManager;
//        //_myDataService = myDataService;
//        _httpClient = ((DataService)_entityManager.DataService).HttpClient; // Access existing HttpClient 
//        _tokenService = tokenService;
//    }
//    //_bcontext.Database.UseTransaction(transaction.GetDbTransaction());
//    var inUser = await _userManager.FindByIdAsync(userId);
//    if (inUser == null)
//    {
//        return -1; //user null
//    }
//    var curcde = await _bcontext.Ocmdes.FindAsync(cdeid);
//    if (curcde == null)
//    {
//        return -2;
//    }
//    Tiewel untiew = new Tiewel();
//    try
//    {
//    }
//    catch (Exception ex)
//    {
//        return -4;
//        //throw new NotSupportedException($"Echec creation tiewel {ex.Message}.");
//    }
//}
//    //public static async Task<BzEntityManager> CreateAsync(string serviceName, IHttpClientFactory httpClientFactory, ITokenService tokenService)
//    //{
//    //    Console.WriteLine("12BZ INITIALIZATION");
//    //    var httpClient = httpClientFactory.CreateClient(serviceName);
//    //    var baseAddress = httpClient.BaseAddress.ToString() + "lgbreeze";
//    //    Console.WriteLine("13BZ INITIALIZATION");
//    //    // Create MyDataService with the configured HttpClient
//    //    var dataService = new DataService(baseAddress); // MyDataService(httpClient, baseAddress, null, tokenService);
//    //    //dataService.HttpClient = httpClient;
//    //    var entityManager = new EntityManager(dataService);
//    //    // Access the existing HttpClient used by DataService
//    //    Console.WriteLine("14BZ INITIALIZATION");
//    //    var existingHttpClient = ((DataService)entityManager.DataService).HttpClient;
//    //    Console.WriteLine("15BZ INITIALIZATION");

//    //    var accessToken = await tokenService.GetATokenAsync();
//    //    Console.WriteLine("16BZ INITIALIZATION");
//    //    if (!string.IsNullOrEmpty(accessToken))
//    //    {
//    //        existingHttpClient.DefaultRequestHeaders.Authorization =
//    //            new AuthenticationHeaderValue("Bearer", accessToken);
//    //        Console.WriteLine($"1Authorization Header Manually Set: Bearer {accessToken}");
//    //    }
//    //    Console.WriteLine("17BZ INITIALIZATION");
//    //    // Set the correct EntityManager in MyDataService
//    //    //dataService.SetEntityManager(entityManager);

//    //    // Perform dummy metadata call to preload metadata
//    //    await entityManager.FetchMetadata();

//    //    Console.WriteLine("18BZ INITIALIZATION");
//    //    var enreg = entityManager.MetadataStore.EntityTypes.Count();
//    //    Console.WriteLine($"19BZ INITIALIZATION : { enreg }");
//    //    return new BzEntityManager(entityManager, tokenService);
//    //}        //[HttpGet("Metadata")]
// private async Task LoadTiwsAsync()
// {
//     //int dep = 300;
//     try
//     {
//         Console.WriteLine("ENTREE LOADING");
//         //dep = 311;
//         var query = new EntityQuery<Tiewel>("tieenrol"); //.Expand("Tiwatrs").Expand("Tiwafils");
//         //dep = 312;
//         var result = await _entityManager.ExecuteQuery(query);
//         var untie = result.AsQueryable().FirstOrDefault();

//         // if (Curtie != null)
//         // {
//         //     //Console.WriteLine($"INTIE {result.Count()}");
//         //     var tlscnjs = Curtie.Tiwafils.Where(tia => tia.Ttyp == 2).ToList();
//         //     var tlspars = Curtie.Tiwafils.Where(tia => tia.Ttyp == 3).ToList();
//         //     var tlsenfs = Curtie.Tiwafils.Where(tia => tia.Ttyp == 4).ToList();
//         //     var tlspais = Curtie.Tiwafils.Where(tia => tia.Ttyp == 5).ToList();
//         //     var tlsflhs = Curtie.Tiwafils.Where(tia => tia.Ttyp == 6).ToList();
//         // }
//     }
//     catch (Exception ex)
//     {
//         strmeta = _entityManager.strmeta;
//         Console.WriteLine($"Probleme EntityManagment : {ex.Message}");
//     }
//     Console.WriteLine("SORTIE");
// }
//public async Task<BzEntityManager> GetBzEntityManagerAsync99()
//{
//    if (_bzEntityManager == null)
//    {
//        Console.WriteLine("SUITE1 CONFIGURE METADATA");

//        // 1. Create EntityManager with httpClient and serviceName
//        var httpClient = _httpClientFactory.CreateClient("BZEClient");
//        var fullServiceUrl = $"{_backendUrl.TrimEnd('/')}/api/lgbreeze";
//        _bzEntityManager = new BzEntityManager(fullServiceUrl, httpClient, _tokenService);
//        _dftClient = _httpClientFactory.CreateClient("DefaultClient");

//        // 2. Configure Namespace Mapping
//        // Corrected namespace mapping direction
//        var namingConvention = Breeze.Sharp.NamingConvention.Default
//            .WithClientServerNamespaceMapping(
//                new Dictionary<string, string> {
//                    { // Server → Client 
//                        "GxShared.DaModels", "GxWapi.DaModels"
//                    }
//                }
//            );
//        _bzEntityManager.MetadataStore.NamingConvention = namingConvention;

//        // 3. Configure allowed mismatch types
//        _bzEntityManager.MetadataStore.AllowedMetadataMismatchTypes = MetadataMismatchTypes.AllAllowable;
//        _bzEntityManager.MetadataStore.MetadataMismatch += (s, e) =>
//        {
//            Console.WriteLine($"[Metadata] {e.StructuralTypeName}: {e.MetadataMismatchType}");
//            e.Allow = e.MetadataMismatchType switch
//            {
//                MetadataMismatchTypes.MissingCLREntityType => true, // Temporary allowance
//                MetadataMismatchTypes.MissingCLRDataProperty => false, // Strict enforcement
//                MetadataMismatchTypes.MissingCLRNavigationProperty => false, // Block missing navigation properties
//                _ => e.Allow
//            };
//            Console.WriteLine($"[MISMATCH] {e.StructuralTypeName}: {e.MetadataMismatchType}");
//        };

//        // 3.2 Metadata by call
//        _bzEntityManager.DataService.HasServerMetadata = false;

//        // Step 1: Fetch metadata from the Web API
//        //var metadata = await httpClient.GetStringAsync("metadata");
//        var response = await httpClient.GetAsync("metadata");
//        var result = response.Content.ReadFromJsonAsync<string>();
//        var metadata = result.Result;
//        Console.WriteLine($"PAS 1 Parsed:\n {metadata}");

//        //var metadata = await _bzEntityManager.MetadataStore.FetchMetadata(_bzEntityManager.DataService);
//        // Parse the metadata correctly
//        //var serialer = new JsonSerializer();
//        //
//        //
//        var metadataJson = metadata; // JsonConvert.DeserializeObject(metadata);// . //mJSON... .Parse(metadata);
//        Console.WriteLine($"PAS 2 Parsed:\n {metadataJson}");
//        //// Filter the entities using LINQ-to-JSON
//        //var desiredEntities = new HashSet<string> { "Tiewel", "Tiwatr", "Tiwafil", "Tiersp", "Tieafil", "Gxorga" };
//        //Console.WriteLine($"PAS 3 : {desiredEntities.ToString()}");
//        //// Safely access structuralTypes
//        //var structuralTypesToken = metadata["structuralTypes"];
//        //Console.WriteLine($"PQS 31 : {structuralTypesToken.Type}");
//        //if (structuralTypesToken is JArray structuralTypesArray)
//        //{
//        //    var filteredMetadata = new JObject(
//        //        new JProperty("metadataVersion", metadata["metadataVersion"]),
//        //        new JProperty("structuralTypes", new JArray(
//        //            structuralTypesArray.Children<JObject>()
//        //                .Where(entity => entity is Object && desiredEntities.Contains(entity["shortName"]?.ToString()))
//        //                .Select(entity => entity.DeepClone())
//        //        ))
//        //    );
//        //    Console.WriteLine($"PAS 4 : {filteredMetadata.ToString()}");
//        //    // Add naming convention
//        //    filteredMetadata["namingConvention"] = new JObject(
//        //        new JProperty("name", "none"),
//        //        new JProperty("serverToClient", new JObject()),
//        //        new JProperty("clientToServer", new JObject())
//        //    );

//        //    // Convert to properly formatted JSON string
//        //    var filteredJson = JsonConvert.SerializeObject(filteredMetadata, Formatting.Indented);

//        //    // Debug output
//        //    Console.WriteLine($"Filtered Metadata:\n{filteredJson}");
//        //    //System.IO.File.WriteAllText("wwwroot/gdata/filtmetadata.json", filteredJson);

//        //    // Import metadata
//        //    _bzEntityManager.MetadataStore.ImportMetadata(filteredJson);
//        //}
//        //else
//        //{
//        //    Console.WriteLine("structuralTypes is missing or invalid");
//        //    // Consider falling back to full metadata import
//        //    _bzEntityManager.MetadataStore.ImportMetadata(metadataJson);
//        //}

//        // 4. Authorization (Not needed for metadata fetch)
//        // Removed unnecessary authorization setup
//        // Import metadata
//        _bzEntityManager.MetadataStore.ImportMetadata(metadata);
//        return _bzEntityManager;
//    }
//    else
//    {
//        return _bzEntityManager;
//    }
//}

////public async Task<BzEntityManager> GetBzEntityManagerAsync()
////{
////    if (_bzEntityManager == null)
////    {
////        Console.WriteLine("SUITE1 CONFIGURE METADATA");
////        // 1. Create EntityManager with httpClient and serviceName
////        var httpClient = _httpClientFactory.CreateClient("BZEClient");
////        var fullServiceUrl = $"{_backendUrl.TrimEnd('/')}/api/lgbreeze";
////        _bzEntityManager = new BzEntityManager(fullServiceUrl, httpClient, _tokenService);
////        _dftClient = _httpClientFactory.CreateClient("DefaultClient");

////        // 2. Configure Namespace Mapping
////        var namingConvention = Breeze.Sharp.NamingConvention.Default
////            .WithClientServerNamespaceMapping(
////                new Dictionary<string, string> {
////                    { "GxShared.DaModels", "GxWapi.DaModels" }
////                }
////            );
////        _bzEntityManager.MetadataStore.NamingConvention = namingConvention;
////        // 3.Configure allowed mismatch types
////        _bzEntityManager.MetadataStore.AllowedMetadataMismatchTypes = MetadataMismatchTypes.AllAllowable;
////        _bzEntityManager.MetadataStore.MetadataMismatch += (s, e) =>
////        {
////            Console.WriteLine($"[Metadata] {e.StructuralTypeName}: {e.MetadataMismatchType}");
////            e.Allow = e.MetadataMismatchType switch
////            {
////                MetadataMismatchTypes.MissingCLREntityType => true, // Temporary allowance
////                MetadataMismatchTypes.MissingCLRDataProperty => false, // Strict enforcement
////                MetadataMismatchTypes.MissingCLRNavigationProperty => false, // Block missing navigation properties
////                _ => e.Allow
////            };
////            //File.WriteAllText("metadata.json", e.Detail);
////            Console.WriteLine($"[MISMATCH] {e.StructuralTypeName}: {e.MetadataMismatchType}");
////        };
////        //bzEntityManager.MetadataStore.GetDataService().HasServerMetadata = false;
////        //_bzEntityManager.DataService.HasServerMetadata = false;
////        //_bzEntityManager.DataService.Adapter.JsonResultsAdapter()
////        // 3.1 Metadata by file
////        //string _stmeta = await _dftClient.GetStringAsync("gdata/metadata.json");
////        // 3.2 Metadata by call
////        _bzEntityManager.DataService.HasServerMetadata = false;
////        //var _apmeta = await httpClient.GetFromJsonAsync<string>("metadata");

////        // Step 1: Fetch metadata from the Web API
////        var metadataJson = await httpClient.GetStringAsync("metadata");

////        // Step 2: Parse the metadata
////        ////var metadata = JObject.Parse(metadataJson);

////        // Parse the metadata correctly
////        var metadata = JToken.Parse(metadataJson);

////        // Filter the entities using LINQ-to-JSON
////        var desiredEntities = new HashSet<string> { "Tiewel", "Tiwatr", "Tiwafil", "Tiersp", "Tieafil", "Gxorga" };

////        var filteredMetadata = new JObject(
////            new JProperty("metadataVersion", metadata["metadataVersion"]),
////            new JProperty("structuralTypes", new JArray(
////                metadata["structuralTypes"]
////                    .Where(t => desiredEntities.Contains(t["shortName"]?.ToString()))
////                    .Select(t => t.DeepClone())
////            ))
////        );

////        // Add naming convention CLIENT-SIDE instead of modifying server metadata
////        filteredMetadata["namingConvention"] = new JObject(
////            new JProperty("name", "none"),
////            new JProperty("serverToClient", new JObject()),
////            new JProperty("clientToServer", new JObject())
////        );

////        // Convert to properly formatted JSON string
////        var filteredJson = JsonConvert.SerializeObject(filteredMetadata, Formatting.Indented);

////        // Debug output
////        Console.WriteLine($"Filtered Metadata:\n{filteredJson}");
////        System.IO.File.WriteAllText("filtered-metadata.json", filteredJson);
////        // Import metadata
////        _bzEntityManager.MetadataStore.ImportMetadata(filteredJson);


////        //// Step 3: Filter the entities
////        //var desiredEntities = new List<string> { "Tiewel", "Tiwatr", "Tiwafil", "Tiersp", "Tieafil", "Gxorga" };
////        //var filteredMetadata = new JObject
////        //{
////        //    ["metadataVersion"] = metadata["metadataVersion"],
////        //    ["namingConvention"] = metadata["namingConvention"],
////        //    ["structuralTypes"] = new JArray() // Initialize as new JArray
////        //};
////        //var structuralTypes = filteredMetadata["structuralTypes"] as JArray;
////        //if (structuralTypes == null)
////        //{
////        //    structuralTypes = new JArray();
////        //    filteredMetadata["structuralTypes"] = structuralTypes;
////        //}
////        //foreach (var entity in metadata["structuralTypes"])
////        //{
////        //    var entityName = entity["shortName"].ToString();
////        //    if (desiredEntities.Contains(entityName))
////        //    {
////        //        structuralTypes.Add(entity); // Add directly if it is a JOnject
////        //        // Add the entity as a JToken
////        //        //filteredMetadata["structuralTypes"].Add(entity);
////        //    }
////        //}

////        //// Step 4: Import filtered metadata into the MetadataStore
////        //_bzEntityManager.MetadataStore.ImportMetadata(filteredMetadata.ToString());


////        //////var _jsonobj = JsonConvert.SerializeObject(_apmeta);
////        ////Console.WriteLine($" apMeta : {_apmeta}");
////        ////_bzEntityManager.MetadataStore.ImportMetadata(_apmeta);
////        ////////Console.WriteLine($" meta fichier charge : {_stmeta.ToString()}");
////        ////// 3.3 Test
////        ////Console.WriteLine($" nb entities : {_bzEntityManager.MetadataStore.EntityTypes.Count()}");
////        ////var entityType = _bzEntityManager.MetadataStore.GetEntityType("Tiewel:#GxShared.DaModels");
////        ////if (entityType != null)
////        ////{
////        ////    Console.WriteLine($"Key Properties: {string.Join(", ", entityType.KeyProperties.Select(p => p.Name))}");
////        ////}

////        // 4. Authorization
////        var accessToken = await _tokenService.GetATokenAsync();
////        var request = new HttpRequestMessage(HttpMethod.Get, $"{_backendUrl.TrimEnd('/')}/api/metadata");
////        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

////        // 5. Fetch Metadata
////        //SERVER DIRECT
////        //// dataservi = _bzEntityManager.DataService;
////        ////dataservi.HasServerMetadata = false;
////        ////await _bzEntityManager.MetadataStore.FetchMetadata(dataservi);
////        //IMPORT DIRECT
////        //_bzEntityManager.MetadataStore.ImportMetadata(_apmeta);
////        // PROGRAMMATICALLY FIX KEY PROPERTIES
////        //AddKeyProperties(_bzEntityManager.MetadataStore);


////        return _bzEntityManager;
////    }
////    else
////    {
////        return _bzEntityManager;
////    }
////}
//public class TransfNameConvention : NamingConvention
//{
//    public TransfNameConvention(Dictionary<string, string> namespaceMapping)
//    : base(
//          serverName => serverName,
//          clientName => clientName
//        ) 
//    {
//        // Add namespace mapping
//        this.WithClientServerNamespaceMapping(namespaceMapping);
//    }
//}
//public static void AddKeyProperties(MetadataStore metadataStore)
// {
//     Console.WriteLine($"nb entities type/// : {metadataStore.EntityTypes.Count()}");
//     // Iterate over all entity types in the MetadataStoree
//     foreach (var entityType in metadataStore.EntityTypes)
//     {
//         Console.WriteLine($"Entity Nom : {entityType.Name}");
//         foreach (var dataProperty in entityType.DataProperties)
//         {
//             if (dataProperty.IsPartOfKey && !entityType.KeyProperties.Contains(dataProperty))
//             {
//                 // Directly set IsPartOfKey to ensure recognition as a key
//                 dataProperty.IsPartOfKey = true;
//                 Console.WriteLine($"Marked '{dataProperty.Name}' as key property for '{entityType.ShortName}'");
//             }
//         }
//     }
// }
//var metav = metan["metadataVersion"]?.ToString();
//Console.WriteLine($"RETROUVE : {metav} // {metan}");
//var metadataStore = _bzEntityManager.MetadataStore;
//checking
//Console.WriteLine($"TEST : {_bzEntityManager.GetEntities(EntityState.Detached).Count()}");
//var metadatastr = metadataStore.
//var metadatastr = metadataStore.GetEntityType("GxShared.DaModels.Gxorga");
//Console.WriteLine($"Suite {metadatastr.ToString()}");
//File.WriteAllText("metadata.json", metadatastr.ToString());

//var entityType = _bzEntityManager.MetadataStore.GetEntityType("Gxorga:#GxShared.DaModels", true);
//if (entityType != null)
//{
//    Console.WriteLine($"EntityType found: {entityType.Name}");
//    // Now you can access key properties or other metadata
//    var keyProperties = entityType.KeyProperties;
//    foreach (var keyProperty in keyProperties)
//    {
//        Console.WriteLine($"Key Property: {keyProperty.Name}");
//    }
//}
//else
//{
//    Console.WriteLine("EntityType not found.");
//}
//}


//_bzEntityManager.MetadataStore.MetadataMismatch += (s, e) =>
//{
//    File.WriteAllText("metadata.json", e.Detail);
//};

//var entityType = metadataStore.GetStructuralType(("Gxorga:#GxShared.DaModels") as EntityType);

//if (entityType != null)
//{
//    entityType.AddKeyProperty("Id");
//}
//else
//{
//    Console.WriteLine("EntityType for 'Gxorga' not found.");
//}

//var metadata = _bzEntityManager.MetadataStore.ExportMetadata();
//Console.WriteLine($"Meta integre length { metadata.Length } /str : {metadata}");
////File.WriteAllText("metadata.json", metadata);
//if (entityType != null && !entityType.KeyProperties.Any())
//{
//    //entityType.AddKeyProperty("Idorg");
//    Console.WriteLine("Manually added Idorg as key property.");
//}
//var entityType = metadataStore.GetStructuralType("Gxorga:#GxShared.DaModels") as EntityType;
//if (entityType != null)
//{
//    //entityType.AddKeyProperty("Id");
//}
//else
//{
//    Console.WriteLine("EntityType for 'Gxorga' not found.");
//}
//foreach (var entityType in metadataStore.EntityTypes)
//{
//    // Skip if key properties are already defined
//    if (entityType.KeyProperties.Any()) continue;

//    foreach (var dataProperty in entityType.DataProperties)
//    {
//        //Check if the property is marked as part of the key
//        if (dataProperty.IsPartOfKey)
//        {

//            //// Programmatically add key properties with explicit type
//            //entityType.KeyProperties.Add<DataProperty>(dataProperty);
//            //// Cast to IDataProperty if necessary
//            //entityType.KeyProperties.Add((DataProperty)dataProperty);
//            //// Explicitly add to KeyProperties
//            //Breeze.Sharp.MetadataStore.EntityType.KeyProperties.Add(dataProperty);
//            //entityType.KeyProperties.Add(dataProperty);
//            Console.WriteLine($"Added key property '{dataProperty.Name}' to entity '{entityType.ShortName}'");
//        }

//    }

//    // If no key properties found, log a warning
//    if (!entityType.KeyProperties.Any())
//    {
//        Console.WriteLine($"WARNING: No key properties found for entity '{entityType.ShortName}'");
//    }
//}

//}
//public async Task<BzEntityManager> GetBzEntityManagerAsync()
//{
//    if (_bzEntityManager == null)
//    {
//        var httpClient = _httpClientFactory.CreateClient("BZEClient");
//        var fullServiceUrl = $"{_backendUrl.TrimEnd('/')}/api/lgbreeze";
//        _bzEntityManager = new BzEntityManager(fullServiceUrl, httpClient, _tokenService);
//        Console.WriteLine($"METADATA URL {_backendUrl.TrimEnd('/')}/api/metadata");
//        var accessToken = await _tokenService.GetATokenAsync();
//        var request = new HttpRequestMessage(HttpMethod.Get, $"{_backendUrl.TrimEnd('/')}/api/metadata");
//        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
//        await _bzEntityManager.FetchMetadata();

//        // Initialize entities after fetching metadata
//        //await _bzEntityManager.InitializeEntitiesAsync();
//    }

//    return _bzEntityManager;
//}
//}

//        public class MyBzEntityManagerProvider
//    {
//        private readonly IHttpClientFactory _httpClientFactory;
//        private readonly string _backendUrl;
//        private readonly ITokenService _tokenService;
//        private MyBzEntityManager _bzEntityManager;

//        public MyBzEntityManagerProvider(IHttpClientFactory httpClientFactory, IConfiguration configuration, ITokenService tokenService)
//        {
//            _httpClientFactory = httpClientFactory;
//            _backendUrl = configuration["BackendUrl"]; // Read BackendUrl from configuration
//            _tokenService = tokenService;
//        }

//        public async Task<MyBzEntityManager> GetBzEntityManagerAsync()
//        {
//            if (_bzEntityManager == null)
//            {
//                var httpClient = _httpClientFactory.CreateClient("BZEClient");
//                var fullServiceUrl = $"{_backendUrl.TrimEnd('/')}/api/lgbreeze";
//                Console.WriteLine($"FIN URL: {fullServiceUrl}");
//                var accessToken = await _tokenService.GetATokenAsync();
//                _bzEntityManager = new MyBzEntityManager(fullServiceUrl, httpClient, _tokenService);

//                // Use custom metadata fetch method
//                await _bzEntityManager.FetchMyMetadata(accessToken);

//                // initialize entities after fetching metadata
//                await _bzEntityManager.InitializeEntitiesAsync(); // Call to initialize entities
//            }

//            return _bzEntityManager;
//        }
//    }
//public class MyBzEntityManager : BzEntityManager
//{
//    public MyBzEntityManager(string serviceName, HttpClient httpClient, ITokenService tokenService)
//        : base(serviceName, httpClient, tokenService)
//    {
//    }

//}
////public class BzEntityManager : EntityManager
////{
////    // Store HttpClient as a private field
////    private readonly HttpClient _httpClient;
////    private readonly string _serviceName;
////    // Expose HttpClient as a public property (optional)
////    public HttpClient HttpClient => _httpClient;
////    public string ServiceName => _serviceName;
////    public IEntity EtOrga { get; protected set; }
////    public IEntity EtTiewel { get; protected set; }
////    public IEntity EtTiwafil { get; protected set; }

////    public BzEntityManager(string serviceName, HttpClient httpClient)
////    : base(new DataService(serviceName, httpClient)) // Pass serviceName and HttpClient correctly
////    {
////        _httpClient = httpClient;
////        _serviceName = serviceName;
////    }
////    public async Task FetchMyMetadata(string accessToken)
////    {

////        var request = new HttpRequestMessage(HttpMethod.Get, $"{this.ServiceName}/metadata");
////        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

////        var response = await _httpClient.SendAsync(request);

////        if (response.IsSuccessStatusCode)
////        {
////            var metadataStr = await response.Content.ReadAsStringAsync();
////            Console.WriteLine($"RETOUR DE METADATA {metadataStr}");
////            //var meta2 = JsonConvert.SerializeObject(metadataJson);
////            this.MetadataStore.ImportMetadata(meta2);
////        }
////        else
////        {
////            throw new Exception($"Failed to fetch metadata: {response.ReasonPhrase}");
////        }
////    }

////    // You can add additional methods or properties here if needed
////    //public override async Task Breeze.Sharp.FetchMetadata()
////    //{
////    //    // Override to do nothing or custom implementation
////    //    await Task.Delay(3000);
////    //}
////}
////    public class BzEntityManager : EntityManager
////    {
////        private readonly HttpClient _httpClient;
////        private readonly string _serviceName;
////        private readonly ITokenService _tokenService;

////        public BzEntityManager(string serviceName, HttpClient httpClient, ITokenService tokenService)
////            : base(new DataService(serviceName, httpClient))
////        {
////            _httpClient = httpClient;
////            _serviceName = serviceName;
////            _tokenService = tokenService;
////        }

////        public override async Task FetchMetadata()
////        {
////            var accessToken = await _tokenService.GetATokenAsync();
////            var request = new HttpRequestMessage(HttpMethod.Get, $"{_serviceName}/metadata");
////            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

////            var response = await _httpClient.SendAsync(request);
////            if (response.IsSuccessStatusCode)
////            {
////                var metadataJson = await response.Content.ReadAsStringAsync();
////                this.MetadataStore.ImportMetadata(metadataJson);
////            }
////            else
////            {
////                throw new Exception($"Failed to fetch metadata: {response.ReasonPhrase}");
////            }
////        }
////    }
////}
////    public class MyBzEntityManager : BzEntityManager
////    {
////        private readonly HttpClient _httpClient;

////        public MyBzEntityManager(string serviceName, HttpClient httpClient, ITokenService tokenService)
////            : base(serviceName, httpClient, tokenService)
////        {
////            _httpClient = httpClient;
////        }


////        public async Task InitializeEntitiesAsync()
////        {
////            //Ensure metadata is fetched first
////            //var accessToken = _tokenService;
////            DataService.this.FetchMetadata();
////            //..MetadataStore.Instance.RegisterEntityType(typeof(Gxorga));
////            //MetadataStore.Instance.RegisterEntityType(typeof(Tiewel));
////            //MetadataStore.Instance.RegisterEntityType(typeof(Tiwafil));
////            //await FetchMyMetadata(_tokenService.GetATokenAsync());
////            //Initialize entities based on the fetched metadata
////            EtOrga = CreateEntity<Gxorga>();
////            if (EtOrga == null) throw new InvalidOperationException("Gxorga entity creation failed");
////            EtTiewel = CreateEntity<Tiewel>();
////            EtTiwafil = CreateEntity<Tiwafil>();

////        }
////        public class MyBzEntityManagerProvider
////    {
////        private readonly IHttpClientFactory _httpClientFactory;
////        private readonly string _backendUrl;
////        private readonly ITokenService _tokenService;
////        private MyBzEntityManager _bzEntityManager;

////        public MyBzEntityManagerProvider(IHttpClientFactory httpClientFactory, IConfiguration configuration, ITokenService tokenService)
////        {
////            _httpClientFactory = httpClientFactory;
////            _backendUrl = configuration["BackendUrl"]; // Read BackendUrl from configuration
////            _tokenService = tokenService;
////        }

////        public async Task<MyBzEntityManager> GetBzEntityManagerAsync()
////        {
////            if (_bzEntityManager == null)
////            {
////                var httpClient = _httpClientFactory.CreateClient("BZEClient");
////                var fullServiceUrl = $"{_backendUrl.TrimEnd('/')}/api/lgbreeze";
////                Console.WriteLine($"FIN URL: {fullServiceUrl}");
////                var accessToken = await _tokenService.GetATokenAsync();
////                _bzEntityManager = new MyBzEntityManager(fullServiceUrl, httpClient, _tokenService);

////                // Use custom metadata fetch method
////                await _bzEntityManager.FetchMyMetadata(accessToken);

////                // initialize entities after fetching metadata
////                await _bzEntityManager.InitializeEntitiesAsync(); // Call to initialize entities
////            }

////            return _bzEntityManager;
////        }
////    }
/// <summary>
/// /public class BzEntityManagerProvider
/// </summary>
////{
////    private readonly IHttpClientFactory _httpClientFactory;
////    private readonly string _backendUrl; // Store BackendUrl
////    private BzEntityManager _bzEntityManager;

////    public BzEntityManagerProvider(IHttpClientFactory httpClientFactory, IConfiguration configuration)
////    {
////        _httpClientFactory = httpClientFactory;
////        _backendUrl = configuration["BackendUrl"]; // Read BackendUrl from configuration
////    }
////    public async Task<BzEntityManager> GetBzEntityManagerAsync(string accessToken)
////    {
////        if (_bzEntityManager == null)
////        {
////            // Create HttpClient using the factory and configured handler
////            var httpClient = _httpClientFactory.CreateClient("BZEClient");

////            // Configure Breeze EntityManager
////            var fullServiceUrl = $"{_backendUrl.TrimEnd('/')}/api/lgbreeze";
////            Console.WriteLine($"FIN URL: {fullServiceUrl}");
////            _bzEntityManager = new BzEntityManager(httpClient, fullServiceUrl);

////            // Fetch metadata explicitly
////            await _bzEntityManager.FetchMetadata();
////        }
////        return _bzEntityManager;
////    }
////}
///
//    using System.Collections.Generic;
//using System;
//using System.Text;
//using System.Net.Http;
//using Microsoft.Extensions.Configuration; // Make sure to include this namespace
//using System.Threading.Tasks;

//using System.Text.Json;
//using System.Web;
//using System.Data;
//using static System.Net.Mime.MediaTypeNames;
//using Microsoft.EntityFrameworkCore.Migrations.Operations;
//using Blazored.LocalStorage;
//using GxClie.ClieServices;
//using Newtonsoft.Json;
//using System.Text.Json.Serialization;
//using Breeze.Sharp; // Ensure you include the necessary Breeze namespace


//using Microsoft.EntityFrameworkCore.Metadata;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;

//using Newtonsoft.Json.Linq;

//using System.Data.Services.Client;
//using System.IdentityModel.Tokens.Jwt;
//using System.Net;
//using System.Net.Http;

//using System.Runtime.CompilerServices;
//using Breeze.Sharp.Core;
//using System.Linq;
//[AllowAnonymous]
//public IHttpActionResult GetMetadata()
//{
//    try
//    {
//        // Generate and return metadata
//        var metadata = _contextProvider.Metadata();
//        return Ok(metadata);
//    }
//    catch (Exception ex)
//    {
//        // Handle exceptions gracefully
//        return InternalServerError(new Exception("Failed to retrieve metadata.", ex));
//    }
//}
//public ActionResult GetMyMetadata()
//{

//    // Build the metadata from your DbContext
//    var myMeta = Breeze.Persistence.EFCore.MetadataBuilder.BuildFrom(_bcontext);
//    // add necessary variables
//    myMeta.MetadataVersion = "3.5";
//    // serialize and return metadata
//    var metadata = JsonConvert.SerializeObject(myMeta, new JsonSerializerSettings(
//    {
//        ContractResolver = new DefaultContractResolver() //preserve property names
//    });  
//    //myMeta.NamingConvention = "none";
//    // Convert to JSON
//    var jsonMeta = JsonConvert.SerializeObject(myMeta);
//    Console.WriteLine($"meta API 11 : {jsonMeta}");
//    return Content(jsonMeta, "application/json");
//}

//    public static async Task<BzEntityManager> CreateAsync(string serviceName, IHttpClientFactory httpClientFactory, ITokenService tokenService)
//    {
//        Console.WriteLine("12BZ INITIALIZATION");
//        var httpClient = httpClientFactory.CreateClient(serviceName);
//        var baseAddress = httpClient.BaseAddress.ToString() + "lgbreeze";
//        Console.WriteLine("13BZ INITIALIZATION");

//        var dataService = new DataService(baseAddress);
//        var entityManager = new EntityManager(dataService);

//        var existingHttpClient = ((DataService)entityManager.DataService).HttpClient;
//        Console.WriteLine("15BZ INITIALIZATION");
//        // Ensure metadata is imported
//        //await EnsureAuthorizationHeaderAsync(existingHttpClient, tokenService);
//        //string accessToken = null;
//        //try
//        //{
//        //    Console.WriteLine("161BZ INITIALIZATION");
//        //    accessToken = await tokenService.GetATokenAsync();
//        //    await Task.Delay(1); // Allow context switching
//        //    Console.WriteLine("162BZ INITIALIZATION");
//        //}
//        //catch (Exception ex)
//        //{
//        //    Console.WriteLine($"TOKEN RETRIEVAL ERROR: {ex.Message}");
//        //    // Handle accordingly (e.g., throw, return null, etc.)
//        //    return null; // or throw new Exception("Token retrieval failed.");
//        //}
//        Console.WriteLine("171BZ INITIALIZATION");
//        //if (!string.IsNullOrEmpty(accessToken))
//        //{
//        //    existingHttpClient.DefaultRequestHeaders.Authorization =
//        //        new AuthenticationHeaderValue("Bearer", accessToken);
//        //    Console.WriteLine($"1Authorization Header Manually Set: Bearer {accessToken}");
//        //} else
//        //{
//        //    Console.WriteLine($"Token IS NOT STRING {accessToken}");
//        //}
//        Console.WriteLine("172BZ INITIALIZATION");
//        await entityManager.FetchMetadata();

//        Console.WriteLine("18BZ INITIALIZATION");
//        var enreg = entityManager.MetadataStore.EntityTypes.Count();
//        Console.WriteLine($"19BZ INITIALIZATION : {enreg}");
//        //var bzEntityManager = new BzEntityManager(entityManager, existingHttpClient, tokenService);

//        return new BzEntityManager(entityManager, tokenService);
//    }
//@page "/inscription"

//@layout EnrolMenu

//@using System
//@using System.Reflection
//@using System.Web;
//@using System.ComponentModel.DataAnnotations
//@using System.Security.Claims
//@using System.Text
//@using System.Text.Json
//@using System.Dynamic
//@using BlazorBootstrap

//@using System.Net.Http.Headers
//@using Microsoft.AspNetCore.WebUtilities
//@using Microsoft.AspNetCore.Components.Forms
//@using GxShared.GlobModels
//@using GxShared.DaModels

//@using GxClie.ClieServices
//@using GxClie.DaEntities
//@using Blazored.LocalStorage
//@using GxClie.Account

//@using GxClie.Components.Mform

//@inject MyAuthStateProvider _authprovider
//@inject IUsrdaService _dbusrManager
//@inject IHttpClientFactory _httpfacClient
//@inject ILocalStorageService _localStorage
//@inject NavigationManager _navmanager
//@inject ITokenService _tokenService
//@inject BzEntityManagerProvider _entityManagerPROV
//@inject AdautService _adautService

//<PageTitle>Reception</PageTitle>
//<h2>Reception</h2>
//@if (isLoading)
//{
//    <Progress class= "progress mb-3" >
//        < ProgressBar Width = "100" Label = "Loading..." Type = "ProgressType.StripedAndAnimated" Color = "ProgressColor.Primary" />
//    </ Progress >
//}
//@if(_invRoles.Count > 0)
//{
//    < label for= "Tiegroupe" > Groupe:</ label >
//    < select id = "Tiegroupe" @bind = "regModel.Tqirole" >
//        @foreach(var gpe in _lsFixes.Where(iv => iv.Gvars == 53 && iv.Itb == 2 && iv.Elea != 0).ToList())
//        {
//            < option value = "@gpe.Elea" > @gpe.Liba </ option >
//        }
//    </ select >
//}
//< !--downmodal-- >
//< EnrolModal IsVisible = "@isModalVisible"
//           IsVisibleChanged = "@OnModalVisibilityChanged"
//           Title = "Chargement des documents"
//           Entity = "@curTiwel"
//           LsmVars = "@_lsVars"
//           LsmFixes = "@_lsFixes"
//           OnSave = "@HandleSave" >
//    @* <input type="text" @bind="curTiwel.Nom" placeholder="Name" />
//    <input type="text" @bind="curTiwel.Pnom" placeholder="Description" /> *@
//</ EnrolModal >
//< div >
//    @if(success)
//    {
//        < div class= "alert alert-success" > @successmsg </ div >
//    }
//    @if(errors)
//    {
//    foreach (var error in errorList)
//    {
//            < div class= "alert alert-danger" > @error </ div >
//        }
//    }
//</ div >
//< AuthorizeView >
//    < Authorized >
//        < EditForm Context = "invform" Model = "@curTiwel" >
//        @if(!isLoading){
//            < DataAnnotationsValidator />
//            < ValidationSummary />
//                < div class= "container row" >
//                    < div class= "d-flex flex-row" >
//                        < div class= "p-2" >
//                            < div >
//                                < div class= "d-flex flex-column" >
//                                    < div >
//                                        < label >< b >< u > INSCRIT </ u ></ b ></ label >
//                                        < span > &nbsp; &nbsp; &nbsp; &nbsp;</ span >
//                                        @if(curTiwel.Idtie > 0){
//    @if(!tiwEdding)
//                                            {
//                                                < span > &nbsp; &nbsp; &nbsp; &nbsp;</ span >
//                                                < button type = "button" class= "btn btn-sm btn-success bg-opacity-50" @onclick = "DelTiwAsync" > Del </ button >
//                                                < span > &nbsp; &nbsp;</ span >
//                                                < button type = "button" class= "btn btn-sm btn-success bg-opacity-50" @onclick = "DelTiwAsync" > Edit </ button >
//                                                < span > &nbsp; &nbsp;</ span >
//                                                < button type = "button" class= "btn btn-lnk btn-sm btn-info bg-opacity-50" @onclick = "ShowModal" > piece(s) </ button >
//                                            } else
//{
//                                                < span > &nbsp; &nbsp;</ span >
//                                                < button type = "button" class= "btn btn-sm btn-success bg-opacity-50" @onclick = "AddTiwConfAsync" > Confirmer < i class= "bi bi-arrow-bardown" ></ i ></ button >
//                                                < span > &nbsp; &nbsp;</ span >
//                                                < button type = "button" class= "btn btn-sm btn-success bg-opacity-50" @onclick = "MajTiwAnulAsync" > Anuller < i class= "bi bi-arrow-barup" ></ i ></ button >
//                                            }
//                                        } else
//{
//    @if(!tiwAdding)
//                                            {
//                                                < span > &nbsp; &nbsp;</ span >
//                                                < button type = "button" class= "btn btn-sm btn-success bg-opacity-50" @onclick = "AddTiwAsync" >< i class= "bi bi-alarm" ></ i > Ajouter </ button >
//                                            }
//                                            else
//{
//                                                < span > &nbsp; &nbsp;</ span >
//                                                < button type = "button" class= "btn btn-sm btn-success bg-opacity-50" @onclick = "AddTiwConfAsync" > Confirmer < i class= "bi bi-arrow-bardown" ></ i ></ button >
//                                                < span > &nbsp; &nbsp;</ span >
//                                                < button type = "button" class= "btn btn-sm btn-success bg-opacity-50" @onclick = "MajTiwAnulAsync" > Anuller < i class= "bi bi-arrow-barup" ></ i ></ button >
//                                                < span > &nbsp; &nbsp;</ span >
//                                                < button type = "button" class= "btn btn-lnk btn-sm btn-info bg-opacity-50" @onclick = "ShowModal" > piece(s) </ button >
//                                            }
//                                        }
//                                    </ div >

//                                @foreach(var ucol in _lsVars.Where(uv => uv.Gvars == 12 && uv.Itb == 1 && uv.Elea != 0).ToList())
//                                {
//    @if(ucol.Jtyp == 1) //si col enrol present
//                                    {
//                                        < div >
//                                            < label >< b > @ucol.Liba :</ b ></ label >
//                                            @if(ucol.Zgpe == 1) //table-numerique
//                                            {
//                                                < TabselComponent Obj = @selflh PropertyName = @ucol.Scdrub Zfro = @ucol.Zfro tbElems = "@_tbElems" />
//                                            }
//                                            else
//        { //autre
//            @switch(ucol.Ztyp)
//                                                    {
//                                                        case 1: //string
//                                                            < StringComponent Obj = @selflh PropertyName = @ucol.Scdrub Placeholder = "@($"Enter { ucol.Liba}
//                    ")" />
//                                                            break;
//                case 2: //int
//                case 3: //long
//                case 4: //real
//                case 5: //decimal
//                                                            < NumberComponent Obj = @selflh PropertyName = @ucol.Scdrub Placeholder = "@($"Enter { ucol.Liba}
//                    ")" />
//                                                            break;
//                case 6: //boolean
//                                                            < BoolComponent Obj = @selflh PropertyName = @ucol.Scdrub Placeholder = "@($"Enter { ucol.Liba}
//                    ")" />
//                                                            break;
//                case 7: //date
//                                                            < DateComponent Obj = @selflh PropertyName = @ucol.Scdrub Placeholder = "@($"Enter { ucol.Liba}
//                    ")" />
//                                                            break;
//                default:
//                                                            < StringComponent Obj = @selflh PropertyName = @ucol.Scdrub Placeholder = "@($"Enter { ucol.Liba}
//                    ")" />
//                                                            break;
//                }
//            }
//                                        </ div >
//                                    }
//    }


//                                    < div >
//                                        < label for= "tiemat" > No dossier:</ label >
//                                        < InputText id = "tiemat" name = "tiematInput" @bind - Value = "curTiwel.Xmatri" required
//                                                   placeholder = "Enter no dossier" />
//                                    </ div >
//                                    < div >
//                                        < label for= "tienom" > Nom:</ label >
//                                        < InputText id = "tienom" name = "tienomInput" @bind - Value = "curTiwel.Nom" required
//                                                   placeholder = "Enter his-her nom" />
//                                    </ div >
//                                    < div >
//                                        < label for= "tiepnom" > Prenom(s):</ label >
//                                        < InputText id = "tiepnom" name = "tiepnomInput" @bind - Value = "curTiwel.Pnom" required
//                                                   placeholder = "Enter his-her prenom(s)" />
//                                    </ div >
//                                    < div >
//                                        < label for= "Tiesexe" > Sexe:</ label >
//                                        < select id = "Tiesexe" @bind = "curTiwel.Sexe" >
//                                            @foreach(var isex in _lsVars.Where(iv => iv.Gvars == 5 && iv.Itb == 12).ToList())
//                                            {
//                                                < option value = "@isex.Elea" > @isex.Liba </ option >
//                                            }
//                                        </ select >
//                                    </ div >
//                                    < div >
//                                        < label for= "Tiesitmat" > Situat.matrimon.:</ label >
//                                        < select id = "Tiesitmat" @bind = "curTiwel.Sitmat" >
//                                            @foreach(var isex in _lsVars.Where(iv => iv.Gvars == 5 && iv.Itb == 13).ToList())
//                                            {
//                                                < option value = "@isex.Elea" > @isex.Liba </ option >
//                                            }
//                                        </ select >
//                                    </ div >
//                                    < div >
//                                        < label for= "parlien" > Responsabilite:</ label >
//                                        < select id = "parlien" @bind = "curTiwel.Respon" >
//                                            @foreach(var resp in _lsFixes.Where(iv => iv.Gvars == 53 && iv.Itb == 7 && iv.Elea != 0).ToList())
//                                            {
//                                                < option value = "@resp.Elea" > @resp.Liba </ option >
//                                            }
//                                        </ select >
//                                    </ div >
//                                    < div >
//                                        < label for= "tiemail" > Email:</ label >
//                                        < InputText type = "email" id = "tiemail" name = "tiemailInput" @bind - Value = "curTiwel.Usremail" required
//                                                   placeholder = "Enter his-her email" />
//                                    </ div >
//                                    < div >
//                                        < label for= "tiephone" > Telephone:</ label >
//                                        < InputText id = "tiephone" name = "tiephoneInput" @bind - Value = "curTiwel.Usrphone" required
//                                                   placeholder = "Enter his-her phone" />
//                                    </ div >
//                                    < div >
//                                        < label for= "tieadr" > Adresse:</ label >
//                                        < InputText id = "tieadr" name = "tieadrInput" @bind - Value = "curTiwel.Adr" required
//                                                   placeholder = "Enter his-her address" />
//                                    </ div >
//                                    < div >
//                                        < label for= "TypDiv" > Services:</ label >
//                                        @if(_lsDivs.Count > 0)
//                                        {
//                                            < select id = "TypDiv" @bind = "curTiwel.Idhie" >
//                                                < option value = "0" > Aucun </ option >
//                                                @foreach(var divo in _lsDivs)
//                                                {
//                                                    < option value = "@divo.Idhie" > @divo.Liba </ option >
//                                                }
//                                            </ select >
//                                        }
//                                        else
//    {
//                                            < p > Loading / ou absent </ p >
//                                        }
//                                    </ div >
//                                </ div >
//                            </ div >
//                            @if(Sicnjoint == 1)
//                            {
//                                < div >
//                                    < div >
//                                        < label > -----</ label >
//                                    </ div >
//                                    < div >
//                                        < label >< b >< u > CONJOINT(S) </ u ></ b ></ label >
//                                        < button type = "button" class= "btn btn-sx btn-success bg-opacity-50" @onclick = "() => AddAflAsync(selcnj, 1)" > Ajouter </ button >
//                                        @if(selcnj.Idafil > 0)
//                                        {
//                                            < button type = "button" class= "btn btn-sx btn-success bg-opacity-50" @onclick = "() => DelAflAsync(selcnj)" > Del </ button >
//                                        }
//                                    </ div >
//                                    < div >
//                                        < label for= "cnjlist" > Liste:</ label >
//                                        < select id = "cnjlist" @bind = "icnj" >
//                                            @foreach(var tiea in tiwAfils.ToList())
//                                            {
//                                                < option value = "@tiea.Idafil" >@($"{tiea.Nom} {tiea.Pnom}") </ option >
//                                            }
//                                        </ select >
//                                    </ div >
//                                    < div >
//                                        < label for= "cnjrang" > Rang:</ label >
//                                        < InputNumber id = "cnjrang" name = "cnjrangInput" @bind - Value = "selcnj.Trang" required
//                                                   placeholder = "Enter his-her rang" />
//                                    </ div >
//                                    < div >
//                                        < label for= "cnjlien" > Responsabilite:</ label >
//                                        < select id = "cnjlien" @bind = "selcnj.Respon" >
//                                            @foreach(var resp in _lsFixes.Where(iv => iv.Gvars == 53 && iv.Itb == 7 && iv.Elea != 0).ToList())
//                                            {
//                                                < option value = "@resp.Elea" > @resp.Liba </ option >
//                                            }
//                                        </ select >
//                                    </ div >
//                                    < div >
//                                        < label for= "cnjnom" > Nom:</ label >
//                                        < InputText id = "parnom" name = "cnjnomInput" @bind - Value = "selcnj.Nom" required
//                                                   placeholder = "Enter his-her nom" />
//                                    </ div >
//                                    < div >
//                                        < label for= "cnjpnom" > Prenom(s):</ label >
//                                        < InputText id = "cnjpnom" name = "cnjpnomInput" @bind - Value = "selcnj.Pnom" required
//                                                   placeholder = "Enter his-her prenom(s)" />
//                                    </ div >
//                                    < div >
//                                        < label for= "cnjsexe" > Sexe:</ label >
//                                        < select id = "cnjsexe" @bind = "selcnj.Sexe" >
//                                            @foreach(var isex in _lsVars.Where(iv => iv.Gvars == 5 && iv.Itb == 12).ToList())
//                                            {
//                                                < option value = "@isex.Elea" > @isex.Liba </ option >
//                                            }
//                                        </ select >
//                                    </ div >
//                                    < div >
//                                        < label for= "cnjtel" > Telephone:</ label >
//                                        < InputText id = "cnjtel" name = "cnjtelInput" @bind - Value = "selcnj.Usrphone" required
//                                                   placeholder = "Enter his-her phone" />
//                                    </ div >
//                                    < div >
//                                        < label for= "cnjema" > Email:</ label >
//                                        < InputText id = "cnjema" name = "cnjemaInput" @bind - Value = "selcnj.Usremail" required
//                                                   placeholder = "Enter his-her email" />
//                                    </ div >
//                                    < div >
//                                        < label for= "cnjadr" > Adresse:</ label >
//                                        < InputText id = "cnjadr" name = "cnjadrInput" @bind - Value = "selcnj.Adr" required
//                                                   placeholder = "Enter his-her address" />
//                                    </ div >
//                                </ div >
//                            }
//                            @if(Siparent == 1)
//                            {
//                                < div >
//                                    < div >
//                                        < label > -----</ label >
//                                    </ div >
//                                    < div >
//                                        < label >< b >< u > PARENT(S) </ u ></ b ></ label >
//                                        < button type = "button" class= "btn btn-sx btn-success bg-opacity-50" @onclick = "() => AddAflAsync(selpar, 2)" > Ajouter </ button >
//                                        @if(selpar.Idafil > 0)
//                                        {
//                                            < button type = "button" class= "btn btn-sx btn-success bg-opacity-50" @onclick = "() => DelAflAsync(selpar)" > Del </ button >
//                                        }
//                                    </ div >
//                                    < div >
//                                        < label for= "parlist" > Liste:</ label >
//                                        < select id = "parlist" @bind = "ipar" >
//                                            @foreach(var tiea in tiwAfils.ToList())
//                                            {
//                                                < option value = "@tiea.Idafil" >@($"{tiea.Nom} {tiea.Pnom}") </ option >
//                                            }
//                                        </ select >
//                                    </ div >
//                                    < div >
//                                        < label for= "parnom" > Nom:</ label >
//                                        < InputText id = "parnom" name = "parnomInput" @bind - Value = "selpar.Nom" required
//                                                   placeholder = "Enter his-her nom" />
//                                    </ div >
//                                    < div >
//                                        < label for= "parpnom" > Prenom(s):</ label >
//                                        < InputText id = "parpnom" name = "parpnomInput" @bind - Value = "selpar.Pnom" required
//                                                   placeholder = "Enter his-her prenom(s)" />
//                                    </ div >
//                                    < div >
//                                        < label for= "parsexe" > Sexe:</ label >
//                                        < select id = "parsexe" @bind = "selpar.Sexe" >
//                                            @foreach(var isex in _lsVars.Where(iv => iv.Gvars == 5 && iv.Itb == 12).ToList())
//                                            {
//                                                < option value = "@isex.Elea" > @isex.Liba </ option >
//                                            }
//                                        </ select >
//                                    </ div >
//                                    < div >
//                                        < label for= "partel" > Telephone:</ label >
//                                        < InputText id = "partel" name = "partelInput" @bind - Value = "selpar.Usrphone" required
//                                                   placeholder = "Enter his-her phone" />
//                                    </ div >
//                                    < div >
//                                        < label for= "parema" > Email:</ label >
//                                        < InputText id = "parema" name = "paremaInput" @bind - Value = "selpar.Usremail" required
//                                                   placeholder = "Enter his-her email" />
//                                    </ div >
//                                    < div >
//                                        < label for= "paradr" > Adresse:</ label >
//                                        < InputText id = "paradr" name = "paradrInput" @bind - Value = "selpar.Adr" required
//                                                   placeholder = "Enter his-her address" />
//                                    </ div >
//                                </ div >
//                            }
//                            @if(Sienfant == 1)
//                            {
//                                < div >
//                                    < div >
//                                        < label > -----</ label >
//                                    </ div >
//                                    < div >
//                                        < label >< b >< u > ENFANT(S) </ u ></ b ></ label >
//                                        < button type = "button" class= "btn btn-sx btn-success bg-opacity-50" @onclick = "() => AddAflAsync(selenf, 3)" > Ajouter </ button >
//                                        @if(selenf.Idafil > 0)
//                                        {
//                                            < button type = "button" class= "btn btn-sx btn-success bg-opacity-50" @onclick = "() => DelAflAsync(selenf)" > Del </ button >
//                                        }
//                                    </ div >
//                                    < div >
//                                        < label for= "enflist" > Liste:</ label >
//                                        < select id = "enflist" @bind = "ienf" >
//                                            @foreach(var tiea in tiwAfils.ToList())
//                                            {
//                                                < option value = "@tiea.Idafil" >@($"{tiea.Nom} {tiea.Pnom}") </ option >
//                                            }
//                                        </ select >
//                                    </ div >
//                                    < label for= "enfrang" > Rang:</ label >
//                                    < InputNumber id = "enfrang" name = "enfrangInput" @bind - Value = "selenf.Trang" required
//                                               placeholder = "Enter his-her rang" />
//                                    < div >
//                                        < label for= "parlien" > Relation:</ label >
//                                        < select id = "parlien" @bind = "selpar.Ifami" >
//                                            @foreach(var rela in _lsFixes.Where(iv => iv.Gvars == 53 && iv.Itb == 6 && iv.Elea != 0).ToList())
//                                            {
//                                                < option value = "@rela.Elea" > @rela.Liba </ option >
//                                            }
//                                        </ select >
//                                    </ div >
//                                    < div >
//                                        < label for= "enfnom" > Nom:</ label >
//                                        < InputText id = "enfnom" name = "enfnomInput" @bind - Value = "selenf.Nom" required
//                                                   placeholder = "Enter his-her nom" />
//                                    </ div >
//                                    < div >
//                                        < label for= "enfpnom" > Prenom(s):</ label >
//                                        < InputText id = "enfpnom" name = "enfpnomInput" @bind - Value = "selenf.Pnom" required
//                                                   placeholder = "Enter his-her prenom(s)" />
//                                    </ div >
//                                    < div >
//                                        < label for= "enfsexe" > Sexe:</ label >
//                                        < select id = "enfsexe" @bind = "selenf.Sexe" >
//                                            @foreach(var isex in _lsVars.Where(iv => iv.Gvars == 5 && iv.Itb == 12).ToList())
//                                            {
//                                                < option value = "@isex.Elea" > @isex.Liba </ option >
//                                            }
//                                        </ select >
//                                    </ div >
//                                    < div >
//                                        < label for= "enfstyp" > Lien:</ label >
//                                        < select id = "enfstyp" @bind = "selenf.Styp" >
//                                            @foreach(var ityp in _lsVars)
//                                            {
//                                                < option value = "@ityp.Elea" > @ityp.Liba </ option >
//                                            }
//                                        </ select >
//                                    </ div >
//                                    < div >
//                                        < label for= "enftel" > Telephone:</ label >
//                                        < InputText id = "enftel" name = "enftelInput" @bind - Value = "selenf.Usrphone" required
//                                                   placeholder = "Enter his-her phone" />
//                                    </ div >
//                                    < div >
//                                        < label for= "enfema" > Email:</ label >
//                                        < InputText id = "enfema" name = "enfemaInput" @bind - Value = "selenf.Usremail" required
//                                                   placeholder = "Enter his-her email" />
//                                    </ div >
//                                    < div >
//                                        < label for= "enfadr" > Adresse:</ label >
//                                        < InputText id = "enfadr" name = "enfadrInput" @bind - Value = "selenf.Adr" required
//                                                   placeholder = "Enter his-her address" />
//                                    </ div >
//                                </ div >
//                            }
//                            @if(Siparain == 1)
//                            {
//                                < div >
//                                    < div >
//                                        < label > -----</ label >
//                                    </ div >
//                                    < div >
//                                        < label >< b >< u > PARRAIN(S) </ u ></ b ></ label >
//                                        < button type = "button" class= "btn btn-sx btn-success bg-opacity-50" @onclick = "() => AddAflAsync(selpai, 4)" > Ajouter </ button >
//                                        @if(selpai.Idafil > 0)
//                                        {
//                                            < button type = "button" class= "btn btn-sx btn-success bg-opacity-50" @onclick = "() => DelAflAsync(selpai)" > Del </ button >
//                                        }
//                                    </ div >
//                                    < div >
//                                        < label for= "pailist" > Liste:</ label >
//                                        < select id = "pailist" @bind = "ipai" >
//                                            @foreach(var tiea in tiwAfils.ToList())
//                                            {
//                                                < option value = "@tiea.Idafil" >@($"{tiea.Nom} {tiea.Pnom}") </ option >
//                                            }
//                                        </ select >
//                                    </ div >
//                                    < div >
//                                        < label for= "pairang" > Rang:</ label >
//                                        < InputNumber id = "pairang" name = "pairangInput" @bind - Value = "selpai.Trang" required
//                                            placeholder = "Enter his-her rang" />
//                                    </ div >
//                                    < div >
//                                        < label for= "painom" > Nom:</ label >
//                                        < InputText id = "painom" name = "painomInput" @bind - Value = "selpai.Nom" required
//                                                   placeholder = "Enter his-her nom" />
//                                    </ div >
//                                    < div >
//                                        < label for= "paipnom" > Prenom(s):</ label >
//                                        < InputText id = "paipnom" name = "paipnomInput" @bind - Value = "selpai.Pnom" required
//                                                   placeholder = "Enter his-her prenom(s)" />
//                                    </ div >
//                                    < div >
//                                        < label for= "paisexe" > Sexe:</ label >
//                                        < select id = "paisexe" @bind = "selpai.Sexe" >
//                                            @foreach(var isex in _lsVars.Where(iv => iv.Gvars == 5 && iv.Itb == 12).ToList())
//                                            {
//                                                < option value = "@isex.Elea" > @isex.Liba </ option >
//                                            }
//                                        </ select >
//                                    </ div >
//                                    < div >
//                                        < label for= "paitel" > Telephone:</ label >
//                                        < InputText id = "paitel" name = "paitelInput" @bind - Value = "selpai.Usrphone" required
//                                                   placeholder = "Enter his-her phone" />
//                                    </ div >
//                                    < div >
//                                        < label for= "paiema" > Email:</ label >
//                                        < InputText id = "paiema" name = "paiemaInput" @bind - Value = "selpai.Usremail" required
//                                                   placeholder = "Enter his-her email" />
//                                    </ div >
//                                    < div >
//                                        < label for= "paiadr" > Adresse:</ label >
//                                        < InputText id = "paiadr" name = "paiadrInput" @bind - Value = "selpai.Adr" required
//                                                   placeholder = "Enter his-her address" />
//                                    </ div >
//                                </ div >
//                            }
//                            @if(Sifiheul == 1)
//                            {
//                                < div >
//                                    < div >
//                                        < label > -----</ label >
//                                    </ div >
//                                    < div >
//                                        < label >< b >< u > FILHEUL(S) </ u ></ b ></ label >
//                                        < button type = "button" class= "btn btn-sx btn-success bg-opacity-50" @onclick = "() => AddAflAsync(selflh, 5)" > Ajouter </ button >
//                                        @if(selflh.Idafil > 0)
//                                        {
//                                            < button type = "button" class= "btn btn-sx btn-success bg-opacity-50" @onclick = "() => DelAflAsync(selflh)" > Del </ button >
//                                        }
//                                    </ div >
//                                    < div >
//                                        < label for= "flhlist" > Liste:</ label >
//                                        < select id = "flhlist" @bind = "iflh" >
//                                            @foreach(var tiea in tiwAfils.ToList())
//                                            {
//                                                < option value = "@tiea.Idafil" >@($"{tiea.Nom} {tiea.Pnom}") </ option >
//                                            }
//                                        </ select >
//                                    </ div >
//                                    < div >
//                                        < label for= "flhrang" > Rang:</ label >
//                                        < InputNumber id = "flhrang" name = "flhrangInput" @bind - Value = "selflh.Trang" required
//                                                   placeholder = "Enter his-her rang" />
//                                    </ div >
//                                    < div >
//                                        < label for= "flhnom" > Nom:</ label >
//                                        < InputText id = "flhnom" name = "flhnomInput" @bind - Value = "selflh.Nom" required
//                                                   placeholder = "Enter his-her nom" />
//                                    </ div >
//                                    < div >
//                                        < label for= "flhpnom" > Prenom(s):</ label >
//                                        < InputText id = "flhpnom" name = "flhpnomInput" @bind - Value = "selflh.Pnom" required
//                                                   placeholder = "Enter his-her prenom(s)" />
//                                    </ div >
//                                    < div >
//                                        < label for= "flhsexe" > Sexe:</ label >
//                                        < select id = "flhsexe" @bind = "selflh.Sexe" >
//                                            @foreach(var isex in _lsVars.Where(iv => iv.Gvars == 5 && iv.Itb == 12).ToList())
//                                            {
//                                                < option value = "@isex.Elea" > @isex.Liba </ option >
//                                            }
//                                        </ select >
//                                    </ div >
//                                    < div >
//                                        < label for= "flhtel" > Telephone:</ label >
//                                        < InputText id = "flhtel" name = "flhtelInput" @bind - Value = "selflh.Usrphone" required
//                                                   placeholder = "Enter his-her phone" />
//                                    </ div >
//                                    < div >
//                                        < label for= "flhema" > Email:</ label >
//                                        < InputText id = "flhema" name = "flhemaInput" @bind - Value = "selflh.Usremail" required
//                                                   placeholder = "Enter his-her email" />
//                                    </ div >
//                                    < div >
//                                        < label for= "flhadr" > Adresse:</ label >
//                                        < InputText id = "flhadr" name = "flhadrInput" @bind - Value = "selflh.Adr" required
//                                                   placeholder = "Enter his-her address" />
//                                    </ div >
//                                </ div >
//                            }
//                            22

//                            @if(Sifiheul == 1)
//                            {
//                                < div >
//                                    < label > -----</ label >
//                                </ div >
//                                < div >
//                                    < label >< b >< u > FILHEUL(S) </ u ></ b ></ label >
//                                    < button type = "button" class= "btn btn-sx btn-success bg-opacity-50" @onclick = "() => AddAflAsync(selflh, 5)" > Ajouter </ button >
//                                    @if(selflh.Idafil > 0)
//                                    {
//                                        < button type = "button" class= "btn btn-sx btn-success bg-opacity-50" @onclick = "() => DelAflAsync(selflh)" > Del </ button >
//                                    }
//                                </ div >
//                                < div >
//                                    < label for= "flhlist" > Liste:</ label >
//                                    < select id = "flhlist" @bind = "iflh" >
//                                        @foreach(var tiea in tiwAfils.ToList())
//                                        {
//                                            < option value = "@tiea.Idafil" >@($"{tiea.Nom} {tiea.Pnom}") </ option >
//                                        }
//                                    </ select >
//                                </ div >
//                                @foreach(var ucol in _lsVars.Where(uv => uv.Gvars == 12 && uv.Itb == 6 && uv.Elea != 0).ToList())
//                                {
//    @if(ucol.Jtyp == 1) //si col enrol present
//                                    {
//                                        < div >
//                                            < label >< b > @ucol.Liba :</ b ></ label >
//                                            @if(ucol.Zgpe == 1) //table-numerique
//                                            {
//                                                < TabselComponent Obj = @selflh PropertyName = @ucol.Scdrub Zfro = @ucol.Zfro tbElems = "@_tbElems" />
//                                            }
//                                            else
//        { //autre
//            @switch(ucol.Ztyp)
//                                                    {
//                                                        case 1: //string
//                                                            < StringComponent Obj = @selflh PropertyName = @ucol.Scdrub Placeholder = "@($"Enter { ucol.Liba}
//                    ")" />
//                                                            break;
//                case 2: //int
//                case 3: //long
//                case 4: //real
//                case 5: //decimal
//                                                            < NumberComponent Obj = @selflh PropertyName = @ucol.Scdrub Placeholder = "@($"Enter { ucol.Liba}
//                    ")" />
//                                                            break;
//                case 6: //boolean
//                                                            < BoolComponent Obj = @selflh PropertyName = @ucol.Scdrub Placeholder = "@($"Enter { ucol.Liba}
//                    ")" />
//                                                            break;
//                case 7: //date
//                                                            < DateComponent Obj = @selflh PropertyName = @ucol.Scdrub Placeholder = "@($"Enter { ucol.Liba}
//                    ")" />
//                                                            break;
//                default:
//                                                            < StringComponent Obj = @selflh PropertyName = @ucol.Scdrub Placeholder = "@($"Enter { ucol.Liba}
//                    ")" />
//                                                            break;
//                }
//            }
//                                        </ div >
//                                    }
//    }
//}
//                        </ div >

//                        < div class= "p-2" >
//                            < div d - flex flex - row >
//                                < InputText id = "lsrech" name = "lsrechInput" @bind - Value = "searchstr" required
//                                      placeholder = "search" />
//                                < button type="button" class= "btn btn-sm btn-success bg-opacity-50" > Ok </ button >
//                            </ div >
//                            < div d - flex flex - row >
//                                < label for= "insclist" > Liste:</ label >
//                                < select id = "insclist" @bind = "insc" @bind:after = "SelTiersAsync" >
//                                    @foreach(var tiea in _lsTwels)
//                                    {
//                                        < option value = "@tiea.Idtie" >@($"{tiea.Nom} {tiea.Pnom}") </ option >
//                                    }
//                                </ select >
//                            </ div >


//                        </ div >
//                    </ div >
//                </ div >
//                < div > TEST </ div >
//        }
//        </ EditForm >
//    </ Authorized >
//    < NotAuthorized >
//        < label > Non Autorise </ label >
//    </ NotAuthorized >
//</ AuthorizeView >
//< style >
//    .inputfl {
//opacity: 0;
//width: 0.1;
//height: 0.1;
//position: absolute;
//}
//</ style >
//    public async Task<IEnumerable<T>> ExecuteApiRequestAsync<T>(EntityQuery<T> query)
//    {
//        try
//        {
//            // Ensure metadata is imported
//            //await EnsureAuthorizationHeaderAsync();
//            // Execute the query using DataService
//            var result = await _entityManager.DataService.GetAsync("tiersaie");
//            Console.WriteLine($"REQUEST SUCCEEDED : {result.Count()}");
//            var resu5 = result.Cast<List<Tiersp>>();
//            Console.WriteLine($"PROCHE TIERS : {resu5.Count()}");
//            foreach (var ure in resu5)
//            {
//                Console.WriteLine($" uneva : {ure.First().Usremail}");
//            }
//            //List<Tiersp mytiers = resu5[0];
//            //mytiers.
//            var resul2 = await _entityManager.ExecuteQuery(query);

//            Console.WriteLine($"REQUEST2 SUCCEEDED : {resul2.Count()}");
//            //return resul2.Cast<T>().ToList();
//            return result.Cast<T>().ToList();
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"REQUEST ISSUE : {ex.Message}");
//            Console.WriteLine($"REQUEST Source : {ex.Source}");
//            foreach (var uva in ex.Data.Values)
//            {
//                Console.WriteLine($"ici/{uva.GetType}//{uva.ToString}");
//            }
//            //Console.WriteLine($"REQUEST DATA : {ex.Data.Values.Cast<string>().ToList()}");
//            Console.WriteLine($"REQUEST INNER : {ex.InnerException.Message}");
//        }
//        return null;
//    }


// private static object GetPropertyValue(object obj, string propname)
// {
//     Type proptype = obj.GetType();
//     PropertyInfo propinfo = proptype.GetProperty(propname);
//     return propinfo?.GetValue(obj);
// }
// private string GetAndSetPropertyValue(object obj, string propname)
// {
//     Type proptype = obj.GetType();
//     PropertyInfo propinfo = proptype.GetProperty(propname);

//     if (propinfo != null)
//     {
//         return propinfo.GetValue(obj)?.ToString();
//     }

//     return "";
// }

// private void SetPropertyValue(object obj, string propname, ChangeEventArgs e)
// {
//     Type proptype = obj.GetType();
//     PropertyInfo propinfo = proptype.GetProperty(propname);

//     if (propinfo != null && e.Value is not null)
//         propinfo.SetValue(obj, Convert.ChangeType(e.Value.ToString(), propinfo.PropertyType));
// }

// // To handle two-way binding properly with InputText,
// // we need to create an Expression that points to our custom method.
// // However, since we cannot directly bind an expression like this,
// // we will use EventCallback instead.

// private EventCallback<string> GetEventCallback(object obj, string propName)
// {
//     return EventCallback.Factory.Create(this,
//         new Action<string>(value => SetPropertyValue(obj, propName)));
// }
//    public async static Task EnsureAuthorizationHeaderAsync(HttpClient ehttpClient, ITokenService etokenService)
//    {
//        //var request = new HttpRequestMessage(HttpMethod.Get, _httpClient.BaseAddress);
//        //foreach (var header in _httpClient.DefaultRequestHeaders)
//        //{
//        //    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
//        //}
//        //if (_httpClient.DefaultRequestHeaders.Authorization == null)
//        //{
//        //}
//        var accessToken = await etokenService.GetATokenAsync();
//        if (!string.IsNullOrEmpty(accessToken))
//        {
//            ehttpClient.DefaultRequestHeaders.Authorization =
//                new AuthenticationHeaderValue("Bearer", accessToken);
//            Console.WriteLine($"1Authorization Header Manually Set: Bearer {accessToken}");
//        }
//        else
//        {
//            Console.WriteLine($"Token PAS PRET");
//        }
//    }
//}
//public class BzEntityManagerProvider
//{
//    private readonly IHttpClientFactory _httpClientFactory;
//    //private readonly MetadataService _metadataService;
//    private readonly ITokenService _tokenService;
//    private Task<BzEntityManager> _initializationTask;

//    public BzEntityManagerProvider(
//        IHttpClientFactory httpClientFactory,
//        ITokenService tokenService)
//    {
//        _httpClientFactory = httpClientFactory;
//        _tokenService = tokenService;
//        _initializationTask = InitializeAsync();
//    }
//    private async Task<BzEntityManager> InitializeAsync()
//    {
//        Console.WriteLine("11BZ INITIALIZATION");
//        var bzEntityManager = await BzEntityManager.CreateAsync(
//            "BZEClient",
//            _httpClientFactory,
//            _tokenService
//        );

//        Console.WriteLine("21BZ INITIALIZATION");
//        return bzEntityManager;
//        //return await BzEntityManager.CreateAsync(
//        //    "BZEClient",
//        //    _httpClientFactory,
//        //    _tokenService
//        //);
//    }
//    public Task<BzEntityManager> GetBzEntityManagerAsync()
//    {
//        Console.WriteLine("51BZ DEPART INIT");
//        var mytask = _initializationTask;
//        Console.WriteLine("25BZ INITIALIZATION");
//        return mytask;
//    }
//}
//indexedDB codes
/*
async function openDatabase(dbName) {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(dbName, 1); // Version 1

        request.onupgradeneeded = function (event) {
            db = event.target.result; // Get the database instance

            // Create object store if it doesn't exist
            if (!db.objectStoreNames.contains("bagstore")) {
                const objectStore = db.createObjectStore("bagstore", { keyPath: "id", autoIncrement: true });
                objectStore.createIndex("userid_idorg", ["userid", "idorg"], { unique: false });
                // You can add more indices here as needed
            }
            //alert('Database upgrade needed: Object store created.');
        };
        request.onsuccess = function (event) {
            db = event.target.result; // Assigning db here
            //alert('Database opened successfully.');
            resolve(db); // Resolve with the database instance
        };
        request.onerror = function (event) {
            //alert('Failed to open database.');
            console.error("Database error:", event.target.errorCode);
            reject(event.target.error); // Reject with error
        };
    });
}
//console.log('Dans IDBT check');
//if (typeof window.webkitIDBTransaction !== 'undefined') {
//    console.log('IDBT1', window.webkitIDBTransaction.READ_WRITE);
//    console.log('IDBT2', window.webkitIDBTransaction.READ_ONLY);
//    return isReadWrite ? window.webkitIDBTransaction.READ_WRITE : window.webkitIDBTransaction.READ_ONLY;
//} else {
//    console.log('IDBT3//readwrite/readonly', isReadWrite);
//    return isReadWrite ? 'readwrite' : 'readonly';
//}
//function detectWebKitAndSetTransaction() {
//    const userAgent = navigator.userAgent;
//    let transactionType;
//    const isWebkit = /AppleWebKit/.test(userAgent);
//    if (isWebkit) {
//        const webKitVersionMatch = userAgent.match(/AppleWebKit\/([\d.]+)/);
//        if (webKitVersionMatch) {
//            const webKitVersion = webKitVersionMatch[1];
//            console.log('WebKitVersion detected : ${webkitVersion}');
//            if (parseFloat(webKitVersion) < 10) {
//                transactionType = 'webKitIDBTransaction';
//            } else {
//                transactionType = 'IDBTransaction';
//            }
//        }
//    } else {
//        transactionType = 'IDBTransaction';
//    }
//    console.log('Using transaction type : ${transactionType}');
//    return transactionType;
//}
async function setBagAsync(apibag) {
    //alert('commence');
    if (!db || apibag == null) {
        throw new Error('Database not initialized or arguments out. Please open the database first.');
    }
    const inuserid = apibag.userid;
    const inidorg = apibag.idorg;
    const transaction = db.transaction(["bagstore"], "readwrite");
    const store = transaction.objectStore("bagstore");
    const index = store.index("userid_idorg");
    const request = index.get([inuserid, inidorg]);
    var yabag = false;
    request.onsuccess = function (event) {
        yabag = true;
        bzorbag = event.target.result;
        if (bzorbag) {
            // Update existing bag
            var svid = bzorbag.id; // Store original ID
            Object.assign(bzorbag, apibag); // Update properties with new data
            bzorbag.id = svid; // Restore original ID before putting back into store
            const bzupdated = store.put(bzorbag); // Update record in store
            console.log("Userbag updated successfully");
            // Update existing bag
            return bzupdated;
        } else {
            yabag = false;
            const bzadded = store.add(apibag);
            console.log("No matching record found (new added)."); // Handle case where no record was found
            return null;
        }
    };
    request.onerror = function (event) {
        yabag = false;
        const bzadded = store.add(apibag);
        console.error("Error finding userbag (new added):", event.target.error);
        return bzadded;
    };
}
*/
//@foreach(var product in list)
//{
//    < tr >
//        < td > @product.ProductName.</ td >
//        < td > @product.UnitPrice </ td >
//        @*<input @bind="unitPrice" />*@
//        < td >< input type = "button" class= "btn btn-primary" @onclick = "(() => PrepareForEdit(product))" data - toggle = "modal" data - target = "#taskModal" value = "Edit" /></ td >
//    </ tr >
//}

//@*<p>Product Name: @productName</p>*@
//< p > Product Name: < input @bind = "productName" /></ p >
//@*<p>Product Name: @unitPrice</p>*@
//< p > Unit Price: < input @bind = "unitPrice" /></ p >

//< button class= "btn btn-primary" @onclick = "AddProduct" > Add </ button >
//< button class= "btn btn-primary" @onclick = "SaveProduct" > Save </ button >
//< button class= "btn btn-primary" @onclick = "DeleteProduct" > Delete </ button >

//< table class= "table" >
//    < thead >
//        < tr >
//            < th > Product Name </ th >
//            < th > Unit Price(C) </ th >
//        </ tr >
//    </ thead >
//    < tbody >
//        @foreach(var product in list)
//        {
//            < tr >
//                < td > @product.ProductName.</ td >
//                < td > @product.UnitPrice </ td >
//                @*<input @bind="unitPrice" />*@
//                < td >< input type = "button" class= "btn btn-primary" @onclick = "(() => PrepareForEdit(product))" data - toggle = "modal" data - target = "#taskModal" value = "Edit" /></ td >
//            </ tr >
//        }
//    </ tbody >
//</ table >
//}
//[HttpGet]
//[Route("getkeytok")]
//[AllowAnonymous]
////[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//public async Task<ActionResult<Userjwt>> MyKeyToken(string username)
//{
//    var jwtResu = new Userjwt(); //{ JwOk = false, JwToken = "", JwKeys = new JsonWebKey() };
//    var user = await _userManager.FindByNameAsync(username);
//    if (user == null)
//    {
//        jwtResu.JwOk = true;
//        jwtResu.JwToken = "";
//        jwtResu.JwKeys = new JsonWebKey[]{};
//        jwtResu.Curkey = "";
//        return Ok(jwtResu); 
//    }
//    //prise roles seulement pour logResu
//    var userRoles = await _userManager.GetRolesAsync(user);
//    var strRoles = new List<string>(userRoles);
//    //logResu.Roles = strRoles; //userRoles.ToList();
//    //claims
//    var claims = new List<Claim>
//    {
//        new Claim(ClaimTypes.Name, user.UserName),
//        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
//        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
//    };
//    // Add any additional claims here
//    foreach (var role in strRoles)
//    {
//        claims.Add(new Claim(ClaimTypes.Role, role));
//    }
//    var claimsPlus = _httpContextAccessor.HttpContext?.User.Claims;
//    var claimName = claimsPlus?.FirstOrDefault(c => c.Type == ClaimTypes.Name); //?.Value;
//    if (claimName != null) claims.Add(claimName);
//    if (claimsPlus.Any()) claims.AddRange(claimsPlus);
//    //generate jwt token
//    Userjwt allTokens = await _genereToken.GenerateMyToken(claims);
//    jwtResu.JwOk = true; 
//    jwtResu.JwToken = allTokens.JwToken;
//    jwtResu.Curkey = allTokens.Curkey;
//    jwtResu.JwKeys = allTokens.JwKeys;
//    return Ok(jwtResu);
//}
//var claimsPlus = _httpContextAccessor.HttpContext?.User.Claims;
//var claimName = claimsPlus?.FirstOrDefault(c => c.Type == ClaimTypes.Name); //?.Value;
//if (claimName != null) claims.Add(claimName);
//if (claimsPlus.Any()) claims.AddRange(claimsPlus);
//public class SendLogReg
//{
//    private bool _isauthenticated = false;
//    private readonly AuthenticationStateProvider _custAuthenticater;
//    private readonly JsonSerializerOptions jsonSerializerOptions =
//        new()
//        {
//            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
//        };
//    private readonly ClaimsPrincipal Unauthenticated =
//        new(new ClaimsIdentity());
//    private readonly HttpClient _httpClient;
//    //private readonly ILocalStorageService _inlocalStorage;
//    //private readonly AuthenticationStateProvider? _custauthenticater;
//    public SendLogReg(IHttpClientFactory httpClientFactory,
//        //AuthenticationStateProvider authenticationStateProvider,
//        //ILocalStorageService inlocalStorage,
//        AuthenticationStateProvider custAuthenticater)
//    {
//        _httpClient = httpClientFactory.CreateClient("ASClient");
//        //_inlocalStorage = inlocalStorage;
//        _custAuthenticater = custAuthenticater; // (MyAuthStateProvider)authenticationStateProvider;
//    }
//public static T Merge<T>(params T[] sources) where T : new()
////public void MergeAndStoreUserData(UserModel newUserData)
////{
////    var existingUserData = UserDataService.ge
////    var result = new T();
////    foreach (var source in sources)
////    {
////        foreach (var prop in typeof(T).GetProperties())
////        {
////            if (prop.CanWrite)
////            {
////                var value = prop.GetValue(source);
////                if (value != null)
////                {
////                    prop.SetValue(result, value); ;
////                }
////            }
////        }
////    }
////    return result;
////}
// private async Task SeedAndSelectRoles(int ilevel)
// {
//     MyRolTable ulist = new MyRolTable();
//     userName = authUser.Identity.Name;
//     Console.Write($"user invite authentifie/roles: {authUser.Claims.Count() }");
//     string varimp = userName != null ? userName : "rien";
//     Console.WriteLine(varimp);
//     //du dernier au 1er
//     rolInv = ""; tyInv = 0;
//     if (authUser.IsInRole("Parain"))
//     {
//         rolInv = "Parain";
//         tyInv = 5;
//     }
//     if (authUser.IsInRole("Operat"))
//     {
//         rolInv = "Operat";
//         tyInv = 4;
//     }
//     if (authUser.IsInRole("Manager"))
//     {
//         rolInv = "Manager";
//         tyInv = 3;
//     }
//     if (authUser.IsInRole("Coordo"))
//     {
//         rolInv = "Coordo";
//         tyInv = 2;
//     }
//     Console.WriteLine($"role courant :{rolInv}");
//     if (tyInv == 2 || tyInv == 3) //coordo et manager
//     {
//         ulist.RoNo = 3; ulist.RoName = "Manager";
//         _inRoles.Add(ulist);
//         ulist.RoNo = 4; ulist.RoName = "Operat";
//         _inRoles.Add(ulist);
//         ulist.RoNo = 7; ulist.RoName = "Client";
//         _inRoles.Add(ulist);
//         ulist.RoNo = 8; ulist.RoName = "Fournis";
//         _inRoles.Add(ulist);
//         ulist.RoNo = 9; ulist.RoName = "Patern";
//         _inRoles.Add(ulist);
//     }
//     if (tyInv == 4) //operat
//     {
//         ulist.RoNo = 5; ulist.RoName = "Cible";
//         _inRoles.Add(ulist);
//         ulist.RoNo = 6; ulist.RoName = "Parain";
//         _inRoles.Add(ulist);
//     }
//     if (tyInv == 6) //parain
//     {
//         ulist.RoNo = 5; ulist.RoName = "Cible";
//         _inRoles.Add(ulist);
//     }
// }
//AuthService
//public class AuthorizService
//{
//    private readonly HttpClient _httpclient;
//    private readonly ILogger<AuthorizService> _logger;
//    private readonly StoreUserBag _storeUserBag;
//    private readonly ILocalStorageService _localStorage;
//    public AuthorizService(HttpClient httpClient,
//        ILogger<AuthorizService> logger,
//        IHttpClientFactory httpClientFactory,
//        StoreUserBag storeUserBag,
//        ILocalStorageService localStorage)
//    {
//        _httpclient = httpClientFactory.CreateClient("ASClient");
//        _logger = logger;
//        _storeUserBag = storeUserBag;
//        _localStorage = localStorage;
//    }
//    //Login
//    ////public async Task<LoginResult> LoginAsync(LoginModel logModel) //, string password)
//    ////{
//    ////    var logResu = new LoginResult();
//    ////    logResu.Usrbag = new UsrDa();
//    ////    logResu.ErrList.Add("");
//    ////    logResu.Succeeded = false;
//    ////    Console.WriteLine("entree dans appel login");
//    ////    try
//    ////    {
//    ////        var result = await _httpclient.PostAsJsonAsync(
//    ////                        "api/lgauth/aulogin", logModel, CancellationToken.None); //username=" + email + "&password=" + password, CancellationToken.None);
//    ////        if (result.IsSuccessStatusCode)
//    ////        {
//    ////            //Console.WriteLine("enfin logue 111");
//    ////            Console.Write(result);
//    ////            var resuobj = await result.Content.ReadFromJsonAsync<LoginResult>(); // await JsonSerializer.DeserializeAsync<JsonObject>(result);
//    ////            Console.WriteLine("enfin logue 222");
//    ////            Console.Write(resuobj.Succeeded);
//    ////            Console.WriteLine("resuobj", resuobj);
//    ////            Console.WriteLine("resuobj-utoken", resuobj.Usrbag.Utoken);
//    ////            logResu.Succeeded = true;
//    ////            logResu.Usrbag.Utoken = resuobj.Usrbag.Utoken;
//    ////            Console.WriteLine("utoken", logResu.Usrbag.Utoken);
//    ////            Console.WriteLine("usbag", logResu.Usrbag);
//    ////            logResu.Usrbag = resuobj.Usrbag;
//    ////            return logResu;
//    ////        }
//    ////    }
//    ////    catch (Exception ex)
//    ////    {
//    ////        logResu.ErrList.Add(ex.Message);
//    ////        throw new Exception($"Login55 failed: {ex.Message}");
//    ////    }
//    ////    return logResu;
//    ////}
//    //Registration
//    ////public async Task<LoginResult> xxLoginAsync(LoginModel logModel)
//    ////{
//    ////    _httpclient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer=" +"");
//    ////    //_httpclient.DefaultRequestHeaders.Add("Authorization", "Bearer " + "");
//    ////    var logResu = new LoginResult();
//    ////    //logResu.Usrbag = new UsrdaModel();
//    ////    logResu.ErrList = new List<string>();
//    ////    logResu.Succeeded = false;
//    ////    _logger.LogInformation("Entering login call");
//    ////    try
//    ////    {
//    ////        var result = await _httpclient.PostAsJsonAsync("api/lgauth/aulogin", logModel, CancellationToken.None);
//    ////        //await IJS.("1", result.Content);
//    ////        _logger.LogInformation("resu1 : {@resuin}", result);
//    ////        if (result.IsSuccessStatusCode)
//    ////        {
//    ////            var jsonString = await result.Content.ReadAsStringAsync();
//    ////            _logger.LogInformation("jsonar : {@jsonar}", jsonString);
//    ////            var resuobj = await result.Content.ReadFromJsonAsync<LoginResult>();
//    ////            _logger.LogInformation("resuArrive : {@resuin2}", resuobj);
//    ////            //= Jsonobj != null ? Jsonobj : null; 
//    ////            //LoginResult resuobj = System.Text.Json.JsonSerializer.DeserializeAsync<LoginResult>(resuText);
//    ////            //Console.WriteLine("Login successful");
//    ////            //Console.Write(resuobj); var jsonString = 
//    ////            if (resuobj != null)
//    ////            {
//    ////                logResu.Succeeded = false;
//    ////                if (resuobj.Succeeded)
//    ////                {
//    ////                    logResu.Succeeded = true;
//    ////                    logResu.Usrbag.Utoken = resuobj.Usrbag.Utoken; // Assuming Usrbag and Utoken are properly defined
//    ////                    //_localStorage.SetItemAsync("AuthToken", logResu.Usrbag.Utoken);
//    ////                    _logger.LogInformation("usrbag : {@usrbag}", logResu.Usrbag);
//    ////                    logResu.Usrbag = resuobj.Usrbag; // Copy the entire Usrbag
//    ////                    _logger.LogInformation("token : {@utoken}", resuobj.Usrbag.Utoken);
//    ////                    //store Usrbag
//    ////                    _userDataService.SetUserData(logResu.Usrbag);
//    ////                }
//    ////                else
//    ////                {
//    ////                    logResu.ErrList.Add("Echec de login.");
//    ////                }
//    ////                _logger.LogInformation("err. list : {@errlist}", logResu.ErrList);
//    ////                //Console.WriteLine("User token: " + logResu.Usrbag.Utoken);
//    ////            }
//    ////            else
//    ////            {
//    ////                logResu.ErrList.Add("Received null response from the server.");
//    ////            }
//    ////        }
//    ////        else
//    ////        {
//    ////            //var errorMessage = await result.Content.ReadAsStringAsync();
//    ////            logResu.ErrList.Add("Login failed from server: ");
//    ////        }
//    ////    }
//    ////    catch (Exception ex)
//    ////    {
//    ////        logResu.ErrList.Add(ex.Message);
//    ////        //Console.WriteLine($"Login failed: {ex.Message}");
//    ////    }
//    ////    return logResu; // Always return logResu
//    ////}
//    ////public async Task<RegisterResult> xxRegisterAsync(RegisterModel regModel) //string email, string password, InAobj rAobj)
//    ////{
//    ////    _httpclient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer=" + "");
//    ////    RegisterResult regResu = new RegisterResult();
//    ////    //string[] defaultDetail = ["An unknown error prevented registration from succeeding."];
//    ////    List<string> mderrors = new List<string>();
//    ////    regResu.Succeeded = false;
//    ////    mderrors.Add("An unknown error prevented registration from succeeding.");
//    ////    // prise de codInvit de l'URL
//    ////    //string incodinvit = Request.Query["invite"];
//    ////    try
//    ////    {
//    ////        if (string.IsNullOrWhiteSpace(regModel.Email) && string.IsNullOrWhiteSpace(regModel.Phonenumber))
//    ////        {
//    ////            //Erreur You must enter either an email or a phonenumber 
//    ////            mderrors.Add("Email et/ou Mot de passe obligatoires");
//    ////            return new RegisterResult { Succeeded = false };
//    ////        }
//    ////        // make the request
//    ////        _logger.LogInformation("//password : {@upass}", regModel.Password);
//    ////        var result = await _httpclient.PostAsJsonAsync(
//    ////          "api/lgauth/auregister", regModel, CancellationToken.None);
//    ////        if (result.IsSuccessStatusCode)
//    ////        {
//    ////            var resuobj = await result.Content.ReadFromJsonAsync<RegisterResult>(); // await JsonSerializer.DeserializeAsync<JsonObject>(result);
//    ////            mderrors.Add("Successfully registred");
//    ////            //Console.WriteLine("REUSSI");
//    ////            resuobj.Succeeded = true;
//    ////            _logger.LogInformation("login REUSSI");
//    ////            return resuobj;
//    ////        } 
//    ////    }
//    ////    catch (Exception ex)
//    ////    {
//    ////        regResu.ErrList.Add(ex.Message);
//    ////        mderrors.Add("Serveur, Donnees enregistrement refusees");
//    ////        throw new Exception($"Register55 failed: {ex.Message}");
//    ////    }
//    ////    return regResu;
//    ////}
//}
//    public async Task<PointResult> StdEnvoieMail(SndmModel sndModel) //int typenv, string pemail, string psubject, string pmessage)
//    {
//        var result = await _httpClient.PostAsJsonAsync(
//                "api/lgauth/stdmailsender", new
//                {
//                    sndModel
//                });
//        // success?
//        if (result.IsSuccessStatusCode)
//        {
//            // success!
//            return new PointResult { Succeeded = true };
//        }
//        else
//        {
//            // success!
//            return new PointResult { Succeeded = false };
//        }
//    }
//}
//public string? Ubit { get; set; }
//public string? Ubit2 { get; set; }
//public string? Ueth { get; set; }
//public string? Ueth2 { get; set; }
//public string? Ucryp2 { get; set; }
//public string? Ucryp { get; set; }
//public string? Ueth { get; set; }
//public string? Uet2 { get; set; }
////authentication
//public class MyAuthStateProvider :AuthenticationStateProvider
//{
//    private readonly ILocalStorageService _localStorage; 
//    public MyAuthStateProvider(ILocalStorageService localStorage)
//    {
//        _localStorage = localStorage;
//    }
//    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//    {
//        var token = await _localStorage.GetItemAsync<string>("authToken");
//        var identity = string.IsNullOrEmpty(token) ? new ClaimsIdentity() : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
//        var user = string.IsNullOrWhiteSpace(token) ? new ClaimsPrincipal(new ClaimsIdentity())
//                : new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
//        return new AuthenticationState(user);
//    }
//    public void MarkUserAsAuthenticated(string inatoken, Task<LoginResult> inudata)
//    {
//        Console.WriteLine("A Marquer");
//        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(inatoken), "jwt"));
//        var authState = Task.FromResult(new AuthenticationState(authenticatedUser)); 
//        _localStorage.SetItemAsync("isAuthenticated", true);
//        _localStorage.SetItemAsync("authToken", inatoken);
//        NotifyAuthenticationStateChanged(authState);
//        _localStorage.SetItemAsync("authData", inudata.Result);
//        Console.WriteLine("A Marquer2:");
//        Console.Write(inatoken);
//    }
//    public void NotifyUserAuthentication(string token)
//    {
//        var authenticateUser = new ClaimsPrincipal(new ClaimsIdentity(new ClaimsIdentity(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"))));
//        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(authenticateUser)));
//    }
//    public void NotifyUserLogout()
//    {
//        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
//        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
//    }
//    // Method to parse claims from JWT
//    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
//    {
//        var claims = new List<Claim>();
//        return claims;
//        // Implementation for parsing claims from your JWT
//    }
//}
//protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//{
//    var tokenResult = await _tokenProvider.RequestAccessToken();
//    if (tokenResult.TryGetToken(out var token))
//    {
//        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
//    }
//    return await base.SendAsync(request, cancellationToken);
//}
//}
////public class MyMessageHandler: AuthorizationMessageHandler
////{
////    public MyMessageHandler(
////            IAccessTokenProvider provider,
////            NavigationManager navigation) : base(provider, navigation)
////    {
////        ConfigureHandler(authorizedUrls: new[] { "https://localhost:7147" }, scopes: new[] { "api_scope" });
////    }
////}
//public class EprointTaches
//{
//    //private readonly HttpClient _httpClient;
//    private readonly ILocalStorageService _localStora;
//    public EprointTaches(IHttpClientFactory httpClientFactory,
//        ILocalStorageService localStora)
//    {
//        _localStora = localStora;
//        //var curToken = _localStora.GetItemAsStringAsync("authToken");
//        //Console.WriteLine("mon Token" + curToken);
//        //_httpClient = httpClientFactory.CreateClient("ASClient");
//        //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + curToken);
//    }
//    //public class RoleClaim
//    //{
//    //    public string? Issuer { get; set; }
//    //    public string? OriginalIssuer { get; set; }
//    //    public string? Type { get; set; }
//    //    public string? Value { get; set; }
//    //    public string? ValueType { get; set; }
//    //}
//}
//public class AuthHttpClientHandler : DelegatingHandler
//{
//    private readonly ILocalStorageService _localStora;
//    public AuthHttpClientHandler(ILocalStorageService localStorageService)
//    {
//        _localStora = localStorageService;
//    }
//    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//    {
//        var token = await _localStora.GetItemAsync<string>("authToken");
//        if (!string.IsNullOrEmpty(token))
//        {
//            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
//        }
//        return await base.SendAsync(request, cancellationToken);
//    }
//}
//public class xxxMyAuthStateProvider : AuthenticationStateProvider
//{
//    private readonly ILocalStorageService _localStorage;
//    public xxxMyAuthStateProvider(ILocalStorageService localStorage)
//    {
//        _localStorage = localStorage;`
//        }
//    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//    {
//        var token = await _localStorage.GetItemAsync<string>("authToken");
//        var identity = string.IsNullOrEmpty(token) ? new ClaimsIdentity() : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
//        var user = string.IsNullOrWhiteSpace(token) ? new ClaimsPrincipal(new ClaimsIdentity())
//                : new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
//        return new AuthenticationState(user);
//    }
//    public void MarkUserAsAuthenticated(string inatoken, Task<LoginResult> inudata)
//    {
//        Console.WriteLine("A Marquer");
//        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(inatoken), "jwt"));
//        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
//        _localStorage.SetItemAsync("isAuthenticated", true);
//        _localStorage.SetItemAsync("authToken", inatoken);
//        NotifyAuthenticationStateChanged(authState);
//        _localStorage.SetItemAsync("authData", inudata.Result);
//        Console.WriteLine("A Marquer2:");
//        Console.Write(inatoken);
//    }
//    public void NotifyUserAuthentication(string token)
//    {
//        var authenticateUser = new ClaimsPrincipal(new ClaimsIdentity(new ClaimsIdentity(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"))));
//        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(authenticateUser)));
//    }
//    public void NotifyUserLogout()
//    {
//        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
//        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
//    }
//    // Method to parse claims from JWT
//    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
//    {
//        var claims = new List<Claim>();
//        return claims;
//        // Implementation for parsing claims from your JWT
//    }
//}
////public class Breeze22Service
////{
////    private readonly HttpClient _httpClient;
////    private readonly EntityManager _entityManager;
////    public BreezeService(IHttpClientFactory httpClientFactory,
////            EntityManager entityManager)
////    {
////        _httpClient = httpClientFactory.CreateClient("ASClient");
////    }
////    public async Task LoadMetadataAsync()
////    {
////        var metadata = await _httpClient.GetStringAsync("api/lgbreeze/metadata");
////        _entityManager.MetadataStore.ImportMetadata(metadata); ;
////    }
////    public async Task<IEnumerable<T>> GetEntitiesAsync<Tiewel>(string query)
////    {
////        var response = await _httpClient.GetStringAsync($"api/lgbreeze/{query}");
////        return _entityManager.ExecuteQuery<T>(response);
////    }
////    public async Task SaveChanges()
////    {
////        var saveResult = await _httpClient.PostAsJsonAsync("api/lgbreeze/savechanges");
////        _entityManager.GetChanges();
////        _entityManager.AcceptChanges(saveResult);
////    }
////}
//New Authenticate Service
//public class AuthenticationService
// {
//     private readonly HttpClient _http;
//     private readonly ILocalStorageService _localStorage;
//     public AuthenticationService(HttpClient http, ILocalStorageService localStorage)
//     {
//         _http = http;
//         _localStorage = localStorage;
//     }

//     //public async Task<AuthenticationResult> Login(LoginModel loginModel)
//     //{
//     //    var response = await _http.PostAsJsonAsync("api/auth/login", loginModel);
//     //    if (response.IsSuccessStatusCode)
//     //    {
//     //        var token = await response.Content.ReadAsStringAsync();
//     //        await _localStorage.SetItemAsync("authToken", token);
//     //        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
//     //        return new AuthenticationResult { Success = true };
//     //    }
//     //    return new AuthenticationResult { Success = false, ErrorMessage = "Invalid login attempt" };
//     //}
//     public class AuthenticationResult
//     {
//         public  bool Success { get; set; }
//         public string ErrorMessage { get; set; }
//         public AuthenticationResult()
//         {
//             Success = false;
//             ErrorMessage = string.Empty;
//         }
//         public AuthenticationResult(bool success, string errorMessage = "")
//         {
//             Success = success;
//             ErrorMessage = errorMessage;
//         }
//     }
//     public async Task Logout()
//     {
//         await _localStorage.RemoveItemAsync("authToken");
//         _http.DefaultRequestHeaders.Authorization = null;
//     }

//     public async Task<bool> IsAuthenticated()
//     {
//         var token = await _localStorage.GetItemAsync<string>("authToken");
//         return !string.IsNullOrEmpty(token);
//     }
// }
//Breeze Service
//public class MyBreezeService
//{
//    private readonly EntityManager _manager;
//    public MyBreezeService(EntityManager manager)
//    {
//        _manager = manager;
//        var metadata = await _httpClient.GetStringAsync("https://localhost:7147/api/lgbreeze/metadata");
//        _manager = new EntityManager(metadataStore);
//    }

//}
// protected override async Task OnInitializedAsync()
// {
//     //isLoading = false;
//     //StateHasChanged(); //refresh the UI
//     //await _jsRun.InvokeVoidAsync("addMessageListener", DotNetObjectReference.Create(this));
// }
// [JSInvokable("GetParameters")]
// public async Taskstring GetParameters(IJSObjectReference jsObjectReference)
// {
//     GetContext
//     Console.WriteLine("ENTREE DANS PARAMETRES");
//     isatmsg = "";
//     int niv4 = 0;
//     //isLoading = true;
//     var authState = await AuthState;
//     claims = authState.User.Claims;
//     authUser = authState.User;
//     isAuthenticated = authUser.Identity.IsAuthenticated;
//     if (!isAuthenticated)
//     {
//         Console.WriteLine("Authentication ECHEC");
//         return "";
//     }
//     //curUname = authUser.Identity.Name; //or claims
//     //Console.WriteLine($"Authent User PRIS { curUname }");
//     //niv4 = 6;
//     _UserId = authUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//     Console.WriteLine($"Authent Prise REUSSIE {_UserId}");
//     if (!string.IsNullOrEmpty(_UserId))
//     {
//         await GetUserBag(_UserId);
//         //await LaunchAdm();
//         var token = await _localStorage.GetItemAsyncstring("blazToken");
//         // Set parameters in JavaScript before the iframe loads
//         var truser = _usrBag.Lkuser;
//         Console.WriteLine($"token : {token} et user : {truser.Email}");
//         // Return parameters as a JSON string or any format needed
//         var parameters = new {
//             type = "initParams",
//             paras = new {
//                 token = token,
//                 authen = true,
//                 lkuser = truser
//             }
//         };
//         return System.Text.Json.JsonSerializer.Serialize(parameters);
//         // try
//         // {
//         //     await _jsRun.InvokeVoidAsync("initializeIframe", token, true, truser);
//         //     Console.WriteLine("parametres installes");
//         //     await _jsRun.InvokeVoidAsync("setupIframeLister");
//         //     //isLoading = false;tok
//         //     //StateHasChanged(); //refresh the UI
//         // }
//         //    catch (Exception ex)
//         //    {
//         //        Console.WriteLine($"Invoke exception : {ex.Message} /// {ex.InnerException}");
//         //    }
//     }else
//     {
//         Console.WriteLine("Utilisateur inacceptable");
//         return "";
//     }
// }  
//public class BreezeService
//{
//    private readonly HttpClient _httpClient;
//    private readonly EntityManager _entityManager;
//    public BreezeService(IHttpClientFactory httpClientFactory)
//    {
//        _httpClient = httpClientFactory.CreateClient("ASClient");
//        // Initialize the EntityManager with the base URL of your Breeze API
//        _entityManager = new EntityManager("https://localhost:7147/api/lgbreeze");
//    }
//    public async Task FetchMetadataAsync()
//    {
//        // Fetch metadata from the Breeze API
//        var metadata = await _httpClient.GetStringAsync("https://localhost:7147/api/lgbreeze/metadata");
//        _entityManager.MetadataStore.ImportMetadata(metadata);
//    }
//    public EntityManager GetManager() => _entityManager;        
//    public async Task<IEnumerable<T>> GetEntitiesAsync<T>(Expression<Func<T, bool>> filterCondition)
//    {
//        // Create an EntityQuery from the query string
//        var entityQuery = new EntityQuery<T>("entityName").Where(filterCondition);

//        // Execute the query
//        var results = await _entityManager.ExecuteQuery(entityQuery);
//        return results;
//    }
//    public async Task AddIdentityAsync<T>(T newEntity) where T : class
//    {
//        _entityManager.AddEntity((IEntity)newEntity);
//        var saveresult = await _entityManager.SaveChanges();
//        //if (SaveResult.HasError)
//        //{
//        //    foreach(var error in SaveResult.Errors)
//        //    {
//        //
//        //    }
//        //}
//    }
//    public async Task SaveChangesAsync()
//    {
//        var changes = _entityManager.GetChanges();
//        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
//        var jsonChanges = JsonSerializer.Serialize(changes, jsonOptions);

//        var saveResult = await _httpClient.PostAsJsonAsync("https://your-api-url/api/breeze/save", jsonChanges);
//        saveResult.EnsureSuccessStatusCode();
//        _entityManager.AcceptChanges();
//    }
//}
//[HttpGet]
//[Route("aurebag55")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//public JsonResult GetAureBag55()
//{
//    // Get user orga BA
//    UserBag usrbag = new UserBag();
//    var user = GetAppUserAsync();
//    if (user == null)
//    {
//        usrbag.Yauser = false;
//        usrbag.Isauthen = false;
//        //usrbag = await usrbag.Lkfixes.Include

//        JsonResult JSauthbag = new JsonResult(usrbag);
//        return JSauthbag;
//        //return BadRequest(usrbag);
//    }
//    usrbag.Yauser = true; //permet de creer orga
//    usrbag.Isauthen = true;
//    int? myIdorg = user.InsIdorg;
//    if (myIdorg == null) myIdorg = 0;
//    usrbag.Idorg = user.InsIdorg;
//    List<Gxorga> lsorgas = _bcontext.Gxorgas.Where(ug => ug.Idorg == user.InsIdorg).ToList();
//    if (!lsorgas.Any())
//    {
//        usrbag.Sicde = user.Sicde;
//        usrbag.Yaorga = false;
//        usrbag.ErrList.Add("Pas organisation");
//        _logger.LogInformation("Lgauth_AuLogin_Pas de Orga.");
//        JsonResult JSorgbag = new JsonResult(usrbag);
//        return JSorgbag;
//        //return Ok(usrbag);
//        //return BadRequest(usrbag);
//    }
//    usrbag.Sicde = user.Sicde;
//    usrbag.Lkuser.Isauthen = true;
//    usrbag.Lkuser.Userid = user.Id;
//    usrbag.Lkuser.Username = user.UserName;
//    usrbag.Lkuser.Nompnom = user.InsNom + "/" + user.InsPnom;
//    usrbag.Lkuser.Email = user.Email;
//    usrbag.Lkuser.Phonenumber = user.PhoneNumber;
//    usrbag.Fonctio = 1;
//    usrbag.Affecta = 1;
//    usrbag.Grade = 1;
//    usrbag.Yauser = true;
//    usrbag.Userid = user.Id; //lien avec Lkuser
//    usrbag.Yaorga = true;
//    usrbag.Lkorga = lsorgas.First();
//    _logger.LogInformation("Lgauth_AuLogin_LkOrga: Idorg={Idorg}", usrbag.Lkorga.Idorg);
//    // User roles
//    usrbag.Lkhies = _bcontext.Gshiers.Where(uf => uf.Idorg == myIdorg).ToList();
//    usrbag.Lktabs = _bcontext.Gsgfixes.Where(su => su.Idorg == myIdorg && su.Idhie == 0 && (su.Gvars == 4 || su.Gvars == 5)).ToList();
//    usrbag.Lkatrs = _bcontext.Gsgfixes.Where(su => su.Idorg == myIdorg && su.Idhie == 0 && (su.Gvars == 6 || su.Gvars == 7)).ToList();
//    usrbag.Lkdics = _bcontext.Dicpros.Where(su => su.Idorg == usrbag.Lkorga.Idorg || su.Idorg == 0).ToList();
//    usrbag.Lkplns = _bcontext.Plngens.Where(up => up.Idorg == myIdorg && up.Ptyp == 1).ToList();
//    usrbag.Lkpgrs = _bcontext.Plngens.Where(up => up.Idorg == myIdorg && up.Ptyp == 9).ToList();
//    usrbag.Lkdivs = _bcontext.Gshiers.Where(uh => uh.Idorg == myIdorg && uh.Idpln == usrbag.Lkorga.Schba && uh.Udpd != 0).ToList();
//    //bagModel.lkroles = _bcontext.Usroles.Where(ur => ur.Bid == usrbag.Lkuser.Bid).ToList();
//    //grilles
//    usrbag.Lkgrids = _bcontext.Plngens.Where(up => up.Idorg == myIdorg && up.Ptyp == 3).ToList();//bagModel.lksaies = _bcontext.Actsaies.Where(ug => ug.Froac == 3 && ug.IdtieNavigation.Ttyp == 3 && ug.IdtieNavigation.Idorg == lkorga.Idorg).Include(ud => ud.Actdets).ToList();
//                                                                                                 //bagModel.lksaies = _bcontext.Actsaies.Where(ug => ug.Froac == 3 && ug.IdtieNavigation.Ttyp == 3 && ug.IdtieNavigation.Idorg == lkorga.Idorg).Include(ud => ud.Actdets).ToList();
//    var BgplnId = 0;
//    //postsites
//    var tplans = _bcontext.Plngens.Where(up => up.Idorg == usrbag.Lkorga.Idorg && up.Ptyp == 4).ToList();
//    if (tplans.Any())
//    {
//        BgplnId = tplans.First().Idpln;
//        usrbag.Lkpsits = _bcontext.Rubvars.Where(ur => ur.Idpln == BgplnId).ToList();
//    }
//    //schemas
//    tplans = _bcontext.Plngens.Where(up => up.Idorg == usrbag.Lkorga.Idorg && up.Ptyp == 5).ToList();
//    if (tplans.Any())
//    {
//        BgplnId = tplans.First().Idpln;
//        usrbag.Lkschms = _bcontext.Rubvars.Where(ur => ur.Idpln == BgplnId).ToList();
//    }
//    var lsfixes = _bcontext.Consfixes.ToList();
//    usrbag.Lkfixes = lsfixes;
//    foreach (var fix in lsfixes)
//    {
//        if (fix.Gvars == 53)
//        {
//            var asi = fix.Liba;
//        }
//    }
//    //Bon pour le dictionnaire
//    //if (lsfixes.Any())
//    //{
//    //    foreach (var ufixe in lsfixes)
//    //    {
//    //        Ifix unfix = new Ifix();
//    //        unfix.Fxseq = ufixe.Fxseq;
//    //        unfix.Itb = ufixe.Itb;
//    //        unfix.Elea = ufixe.Elea;
//    //        unfix.Gvars = ufixe.Gvars;
//    //        unfix.Liba = ufixe.Liba;
//    //        unfix.Lib2 = ufixe.Lib2;
//    //        unfix.Abg = ufixe.Abg;
//    //        unfix.Codi = ufixe.Codi;
//    //        unfix.Gp1 = ufixe.Gp1;
//    //        unfix.Gp2 = ufixe.Gp2;
//    //        unfix.Gp3 = ufixe.Gp3;
//    //        unfix.Eta = ufixe.Eta;
//    //        unfix.Sicur = ufixe.Sicur;
//    //        unfix.Scdrub = ufixe.Scdrub;
//    //        unfix.Refa = ufixe.Refa;
//    //        usrbag.Lk2fixes.Add(unfix);
//    //    }
//    //Console.WriteLine("est bon 22");
//    //}
//    //JsonResult JSusrbag = new JsonResult(usrbag);
//    return new JsonResult(usrbag);
//}
//[HttpGet]
//[Route("aurebag")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//public JsonResult GetAureBag()
//{
//    var myblazbag = GetBlazBag();
//    JsonResult JSusrbag = new JsonResult(myblazbag);
//    return new JsonResult(myblazbag);
//}
//[HttpGet]
//[Route("aurebag2")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
////[Produces("application/json")]
//public JsonResult GetAurebag2()
//{
//    var blazUbag = CrerOrga();
//    JsonResult JSusrbag = new JsonResult(blazUbag);
//    return JSusrbag;
//}
//[HttpGet]
//[Route("blazbag8")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//public async Task<ActionResult<UserBag>> GetBlazBag8()
//{
//    // Get user orga BA
//    UserBag usrbag = new UserBag();
//    usrbag.Lkuser = new LoginResult();
//    var user = GetAppUserAsync();
//    if (user == null)
//    {
//        usrbag.Yauser = false;
//        return BadRequest(usrbag);
//    }
//    int? myIdorg = user.InsIdorg;
//    if (myIdorg == null) myIdorg = 0;
//    usrbag.Idorg = user.InsIdorg;
//    List<Gxorga> lsorgas = _bcontext.Gxorgas.Where(ug => ug.Idorg == myIdorg).ToList();
//    if (!lsorgas.Any())
//    {
//        usrbag.Yauser = true; //permet de creer orga
//        usrbag.Sicde = user.Sicde;
//        usrbag.Yaorga = false;
//        //logResu.Succeeded = false;
//        usrbag.ErrList.Add("Pas organisation");
//        _logger.LogInformation("Lgauth_AuLogin_Pas de Orga.");
//        return Ok(usrbag);
//        //return BadRequest(usrbag);
//    }
//    usrbag.Sicde = user.Sicde;
//    usrbag.Lkuser.Isauthen = true;
//    usrbag.Lkuser.Userid = user.Id;
//    usrbag.Lkuser.Username = user.UserName;
//    usrbag.Lkuser.Nompnom = user.InsNom + "/" + user.InsPnom;
//    usrbag.Lkuser.Email = user.Email;
//    usrbag.Lkuser.Phonenumber = user.PhoneNumber;
//    usrbag.Fonctio = 1;
//    usrbag.Affecta = 1;
//    usrbag.Grade = 1;
//    usrbag.Yauser = true;
//    usrbag.Userid = user.Id; //lien avec Lkuser
//    usrbag.Yaorga = true;
//    usrbag.Lkorga = lsorgas.First();
//    _logger.LogInformation("Lgauth_AuLogin_LkOrga: Idorg={Idorg}", usrbag.Lkorga.Idorg);
//    // User roles
//    usrbag.Lkhies = _bcontext.Gshiers.Where(uf => uf.Idorg == myIdorg).ToList();
//    usrbag.Lktabs = _bcontext.Gsgfixes.Where(su => su.Idorg == myIdorg && su.Idhie == 0 && (su.Gvars == 4 || su.Gvars == 5)).ToList();
//    usrbag.Lkatrs = _bcontext.Gsgfixes.Where(su => su.Idorg == myIdorg && su.Idhie == 0 && (su.Gvars == 6 || su.Gvars == 7)).ToList();
//    usrbag.Lkdics = _bcontext.Dicpros.Where(su => su.Idorg == myIdorg || su.Idorg == 0).ToList();
//    usrbag.Lkplns = _bcontext.Plngens.Where(up => up.Idorg == myIdorg && up.Ptyp == 1).ToList();
//    usrbag.Lkpgrs = _bcontext.Plngens.Where(up => up.Idorg == myIdorg && up.Ptyp == 9).ToList();
//    usrbag.Lkdivs = _bcontext.Gshiers.Where(uh => uh.Idorg == myIdorg && uh.Idpln == usrbag.Lkorga.Schba && uh.Udpd != 0).ToList();
//    //bagModel.lkroles = _bcontext.Usroles.Where(ur => ur.Bid == usrbag.Lkuser.Bid).ToList();
//    //grilles
//    usrbag.Lkgrids = _bcontext.Plngens.Where(up => up.Idorg == myIdorg && up.Ptyp == 3).ToList();//bagModel.lksaies = _bcontext.Actsaies.Where(ug => ug.Froac == 3 && ug.IdtieNavigation.Ttyp == 3 && ug.IdtieNavigation.Idorg == lkorga.Idorg).Include(ud => ud.Actdets).ToList();

//    var BgplnId = 0;
//    //postsites
//    var tplans = _bcontext.Plngens.Where(up => up.Idorg == usrbag.Lkorga.Idorg && up.Ptyp == 4).ToList();
//    if (tplans.Any())
//    {
//        BgplnId = tplans.First().Idpln;
//        usrbag.Lkpsits = _bcontext.Rubvars.Where(ur => ur.Idpln == BgplnId).ToList();
//    }
//    //schemas
//    tplans = _bcontext.Plngens.Where(up => up.Idorg == usrbag.Lkorga.Idorg && up.Ptyp == 5).ToList();
//    if (tplans.Any())
//    {
//        BgplnId = tplans.First().Idpln;
//        usrbag.Lkschms = _bcontext.Rubvars.Where(ur => ur.Idpln == BgplnId).ToList();
//    }
//    var lsfixes = _bcontext.Consfixes.ToList();
//    usrbag.Lkfixes = lsfixes;
//    foreach (var ufi in lsfixes)
//    {
//        if (ufi.Gvars == 53)
//        {
//            _logger.LogInformation($"Gvars : {ufi.Gvars} Gliba : {ufi.Liba}");
//            //Console.Write(ufi.Gvars);
//            //Console.Write(ufi.Elea);
//            //Console.WriteLine(ufi.Liba);
//        }
//    }
//    if (lsfixes.Any())
//    {
//        foreach (var ufixe in lsfixes)
//        {
//            Ifix unfix = new Ifix();
//            unfix.Fxseq = ufixe.Fxseq;
//            unfix.Itb = ufixe.Itb;
//            unfix.Elea = ufixe.Elea;
//            unfix.Gvars = ufixe.Gvars;
//            unfix.Liba = ufixe.Liba;
//            unfix.Lib2 = ufixe.Lib2;
//            unfix.Abg = ufixe.Abg;
//            unfix.Codi = ufixe.Codi;
//            unfix.Gp1 = ufixe.Gp1;
//            unfix.Gp2 = ufixe.Gp2;
//            unfix.Gp3 = ufixe.Gp3;
//            unfix.Eta = ufixe.Eta;
//            unfix.Sicur = ufixe.Sicur;
//            unfix.Scdrub = ufixe.Scdrub;
//            unfix.Refa = ufixe.Refa;
//            usrbag.Lk2fixes.Add(unfix);
//        }
//        //Console.WriteLine("est bon 22");
//    }
//    return Ok(usrbag);
//}

//Check if the role exists first
//var roleExists = await _roleManager.RoleExistsAsync(rolename);
//if (!roleExists)
//{
//    return BadRequest($"Role '{rolename}' does not exist.");
//}
//var lsroles = await _userManager.GetRolesAsync(user);
//bool achrol = false;
//foreach (var urol in lsroles)
//{
//    if (urol == rolename) achrol = true;
//}
//if (!achrol)
//{
//    var calrole = await _roleManager.FindByNameAsync(rolename);
//    if (calrole == null)
//    {
//        var unrole = new ApplicationRole();
//        unrole.Name = rolename;
//        unrole.Description = rolename + " role";
//        await _roleManager.CreateAsync(unrole);
//    }
//}
//var addRoleResult = await _userManager.AddToRoleAsync(inUser, rolename);
//if (!addRoleResult.Succeeded)
//{
//    return BadRequest($"Failed to add user to role  '{rolename}'");
//}
//// Retrieve the latest version of the user from the database
//var user = await _userManager.FindByIdAsync(inUser.Id);

//// Check if the user is already in the specified role
//bool calrole = await _userManager.IsInRoleAsync(user, rolename);
//if (calrole == false)
//{
//    var result = await _userManager.AddToRoleAsync(inUser, rolename);
//    if (result.Succeeded)
//    {
//        //user added to role sucessfully
//        response = new OkResult();
//        return response;
//    }
//    else
//    {
//        //failed to add user to role
//        return BadRequest();
//    }
//}
//else 
//{
//    //failed role not present
//    return BadRequest();
//}
//[HttpGet]
//[AllowAnonymous]
//[Route("userinfo")]
////[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//public async Task<ActionResult> MyuserInfo()
//{
//    LoginResult logResu = new LoginResult();
//    logResu.IsAuthen = false;
//    logResu.Succeeded = false;
//    //if (User.Identity.IsAuthenticated)
//    //{
//        var user = GetAppUserAsync();
//        if (user == null)
//        {
//            logResu.IsAuthen = true;
//            logResu.Utoken = "";
//            return Unauthorized(logResu);
//        }
//        var iutoken = await GenerateJwtToken(user);
//        logResu.Utoken = iutoken;
//        logResu.Email = user.Email;
//        logResu.Username = user.UserName;
//        //string strvaria = "";
//        //Request.Cookies.TryGetValue("estauth", out strvaria);
//        //if (strvaria != null)
//        //{
//        //    if(strvaria.ToUpper() == "TRUE") logResu.IsAuthen = true;
//        //}
//        //Request.Cookies.TryGetValue("token", out strvaria);
//        //logResu.Utoken = strvaria;
//        //Request.Cookies.TryGetValue("email", out strvaria);
//        //logResu.Email = strvaria;
//        //Request.Cookies.TryGetValue("username", out strvaria);
//        //logResu.Username = strvaria;
//        //logResu.Succeeded = true;
//        return Ok(logResu);
//    //}
//    //return Unauthorized(logResu);
//}
//Authentication Service
//public class MyAuthStateProvider55 : AuthenticationStateProvider
//{
//    private readonly ILocalStorageService _localStorage;
//    private readonly ILogger<MyAuthStateProvider55> _logger;
//    private readonly HttpClient _httpclient;
//    private readonly HttpClient _dfthttpclient;
//    private readonly StoreUserBag _storeUserBag;
//    public MyAuthStateProvider55(ILocalStorageService localStorage,
//            ILogger<MyAuthStateProvider55> logger,
//            IHttpClientFactory httpClientFactory
//        )
//    {
//        _localStorage = localStorage;
//        _logger = logger;
//        _httpclient = httpClientFactory.CreateClient("ASClient");
//        _dfthttpclient = httpClientFactory.CreateClient("DefaultClient");
//    }
//    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//    {
//        var token = await _localStorage.GetItemAsync<string>("blazToken");
//        var identity = string.IsNullOrEmpty(token)
//            ? new ClaimsIdentity()
//            : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

//        // Set the authorization header for future requests
//        _httpclient.DefaultRequestHeaders.Authorization =
//            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

//        var authenticatedUser = new ClaimsPrincipal(identity);
//        //Console.Write("Mon User authentifie:");
//        return new AuthenticationState(authenticatedUser);
//    }
//    public async Task SetLogin(string token)
//    {
//        await _localStorage.SetItemAsync("blazToken", token);
//        //create ClaimPrincipal based on the token
//        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
//        var authenticatedUser = new ClaimsPrincipal(identity);
//        //notify the authenticationstate change
//        Console.Write("user a authentifier");
//        Console.WriteLine(authenticatedUser);
//        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(authenticatedUser)));
//    }
//    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
//    {
//        var claims = new List<Claim>();
//        var jwtHandler = new JwtSecurityTokenHandler();
//        var jsonToken = jwtHandler.ReadToken(jwt) as JwtSecurityToken;
//        if (jsonToken != null)
//        {
//            claims.AddRange(jsonToken.Claims);
//        }
//        return claims;
//    }
//    public async Task Logout()
//    {
//        await _localStorage.RemoveItemAsync("blazToken");
//        await _localStorage.RemoveItemAsync("clieToken");
//        await _localStorage.RemoveItemAsync("blazAuthen");
//        await _localStorage.RemoveItemAsync("clieAuthen");
//        _httpclient.DefaultRequestHeaders.Authorization = null;
//        NotifyUserLogout();
//    }
//    public async void NotifyUserLogout()
//    {
//        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
//        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
//        _logger.LogInformation("A marquer Sortie");
//        var response = await _httpclient.PostAsync("api/lgauth/logout", null);
//        if (response.IsSuccessStatusCode)
//        {
//            NotifyAuthenticationStateChanged(authState);
//        }
//    }
//    public async Task<Mytable> RendClieTbls()
//    {
//        try
//        {
//            Console.WriteLine("ENTREE CATALOG");
//            // Adjust the path according to your file's location
//            //myhttpclie = Ih
//            Console.WriteLine(_dfthttpclient.BaseAddress);
//            var jsdata = await _dfthttpclient.GetStringAsync("gdata/tables.json");
//            Mytable mdData = System.Text.Json.JsonSerializer.Deserialize<Mytable>(jsdata);
//            //Console.WriteLine(jsdata);
//            //Console.Write("Zone table");
//            //Console.WriteLine(mdData.domas.First().Liba);
//            return mdData;
//            //mytbObj = JsonConvert.DeserializeObject<Mytable>(jsonData);
//            //return mytbObj;
//        }
//        catch (Exception ex)
//        {
//            //jsonData = $"Error loading JSON: {ex.Message}";
//            Console.WriteLine($"era table: {ex.Message}");
//            return null;
//        }
//    }
//}
//Console.Write("Json converti:");
//Console.WriteLine(usrBag.Lkorga.Sigle);
//foreach (var item in usrBag.Lkfixes)
//{
//   Console.WriteLine(item.Liba);
//}
//niv2 = 22;
//Console.Write("mybag in blazor");
//Console.WriteLine(usrBag.Yaorga);
//niv2 = 23;
//Console.Write("avant storing");
//var inTies = usrBag.Lkfixes.Where(it => it.Gvars != null && it.Gvars == 53).ToList();
//Console.Write("in ties frais");
//Console.WriteLine(inTies.Count());
//niv2 = 24; //store data
//store user data
//try
//
// var myubags = await _dbManager.GetRecords<UserBag>("StoreBags");
// try
// {
//     if (myubags.Any())
//     {
//         // Update existing record logic here
//         usrBag.Id = 1;
//         await _dbManager.AddRecord(new TG.Blazor.IndexedDB.StoreRecord<UserBag>
//         {
//             Storename = "StoreBags",
//             Data = usrBag // Make sure usrBag is valid and not null.
//         });
//         Console.WriteLine("Updated existing record.");
//     }
//     else
//     {
//         // Add new record logic here
//         await _dbManager.AddRecord(new TG.Blazor.IndexedDB.StoreRecord<UserBag>
//         {
//             Storename = "StoreBags",
//             Data = usrBag // Make sure usrBag is valid and not null.
//         });
//         Console.WriteLine("Created new record.");
//     }
// }
// catch (Exception ex)
// {
//     Console.WriteLine($"Error during IndexedDB operation: {ex.Message}");
// }

// var myubags = await _dbManager.GetRecords<UserBag>("StoreBags");
//     usrBag.Id = 1;
//     if (myubags.Any())
//     {//update record
//         await _dbManager.AddRecord(new TG.Blazor.IndexedDB.StoreRecord<UserBag>
//             {
//                 Storename = "StoreBags",
//                 Data = usrBag
//             });
//         Console.WriteLine("a supprime et update");
//     } else
//     {//new
//         var nwrecord = new TG.Blazor.IndexedDB.StoreRecord<UserBag>
//             {
//                 Storename = "StoreBags",
//                 Data = usrBag
//             };
//         await _dbManager.AddRecord(nwrecord);
//         Console.WriteLine("A creer");
//     }
//niv2 = 251;
//await _localStorage.SetItemAsync<bool>("blazAuthen", true);
//niv2 = 252;
//await _localStorage.SetItemAsync<string>("blazToken", uitoken);
//niv2 = 253;
//}catch(Exception ex)
//{
//    Console.WriteLine($"except : {niv2 + ex.Message }");   
//}
//private IEnumerable<Claim> ParseClaimsFrom55Jwt(string jwtoken)
//    {
//        var claims = new List<Claim>();
//        var jwtHandler = new JwtSecurityTokenHandler();
//        var jsonToken = jwtHandler.ReadToken(jwtoken) as JwtSecurityToken;
//        if (jsonToken != null)
//        {
//            claims.AddRange(jsonToken.Claims);
//        }
//        return claims;
//    }
//}
//    .AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
//    //options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Use camel case
//    //options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
//    options.JsonSerializerOptions.WriteIndented = true; // For readability (optional)
//    //options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; // Ignore nulls
//    //options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
//})

//-- ControllersAndViews with NewtonsoftJson for Breeze
//{
// Set Breeze defaults for entity serialization
// var ss= JsonSerializationFns.UpdateWithDefaults(opt.SerializerSettings);
////if (ss.ContractResolver is DefaultContractResolver resolver)
////{
////  resolver.NamingStrategy = null;  // remove json camelCasing; names are converted on the client.
////}
//ss.Formatting = Newtonsoft.Json.Formatting.Indented; // format JSON for debugging
//}).AddMvcOptions(o => o.Filters.Add(new GlobalExceptionFilter()));                                              
//UserData Service
//public class MyRecordService
//{
//    private readonly TG.Blazor.IndexedDB.IndexedDBManager _dbManager;
//    public MyRecordService(TG.Blazor.IndexedDB.IndexedDBManager dbManager)
//    {
//        _dbManager = dbManager;
//    }

//    public async Task AddRecordAsync(UserBag record)
//    {
//        await _dbManager.AddRecord<UserBag>(record);
//    }

//    public async Task<UserBag> GetRecordByIdAsync(string id)
//    {
//        return await _dbManager.GetRecords<UserBag>();
//    }

//    public async Task<List<MyRecord>> GetAllRecordsAsync()
//    {
//        return await _dbManager.GetAllItems();
//    }

//    public async Task UpdateRecordAsync(MyRecord record)
//    {
//        await _dbManager.UpdateItem(record);
//    }

//    public async Task DeleteRecordAsync(string id)
//    {
//        await _dbManager.DeleteItem(id);
//    }
//}

//public class StoreUserBag
//{
//    private UserBag _userData;
//    public UserBag GetUserData() 
//    {   
//        return _userData;     
//    }
//    public void SetUserData(UserBag userda)
//    {
//        _userData = userda;
//    }
//    public void ClearUser()
//    {
//        _userData = null;
//    }
//}
//[HttpGet]
//[Route("getkeys8")]
//[AllowAnonymous]
//private async Task<string> GenerateJwtToken(ApplicationUser user)
//{
//    var roles = await _userManager.GetRolesAsync(user); 
//    var claims = new List<Claim>
//    {
//        new Claim(ClaimTypes.Name, user.UserName),
//        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
//        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
//        // Add any additional claims here
//    };
//    claims.AddRange(roles.Select(role =>
//        new Claim(ClaimTypes.Role, role)
//    ));
//    var keyin = _configuration["Jwtdev:Key"];
//    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyin));
//    var kid = _configuration["Jwtdev:Secret"];
//    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//    var token = new JwtSecurityToken(
//        expires: DateTime.Now.AddMinutes(30),
//        issuer: _configuration["Jwtdev:Issuer"],
//        audience: _configuration["Jwtdev:Audience"],
//        claims: claims,
//        signingCredentials: creds
//    );
//    token.Header.Add("kid", kid);
//    return new JwtSecurityTokenHandler().WriteToken(token);
//}
//public bool Validate4Token(string token)
//{
//    var tokenHandler = new JwtSecurityTokenHandler();
//    var validationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1unk9NsxWJztdUEuOJ2t0fy7bnTZldsgYu5qoqHEebhblQXcCc2yk0CNAFxNCROyFszOJ05HZhlvTBRKxx3aY512Z0GqFme5VfEMZxl16Yxa97gHK66dMQKgczXyiDRG8JPXvvzdddbUWre7P3fYUkrEnvwRIGZRA3Z2QunNyOoVhOwbXmk6UbTKc1BU25bfH8htu8UlOkxloKow3WXK0xO3bZ9HAyfoPNqg90T3ceX1pda3VnGpTAADs4nrjybE")),
//        ValidIssuer = "https://localhost:7147",
//        ValidAudience = "https://localhost:7171",
//        ValidateLifetime = true
//    };

//    try
//    {
//        Console.WriteLine($"verification token {token}");
//        tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
//        Console.WriteLine("Token verifie valide");
//        Console.WriteLine(validatedToken);
//        return true; // Token is valid
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Token pas valide : {ex.Message}");
//        return false; // Token is invalid
//    }
//}
//private async Task<bool> Validate6Token(string token)
//{
//    try
//    {
//        Console.WriteLine("passe ici11");
//        var response = await _httpClient.GetAsync("api/lgauth/getkeys");
//        var jwksJson = await response.Content.ReadAsStringAsync();
//        Console.WriteLine("passe ici1211");
//        var jwks = await response.Content.ReadFromJsonAsync<Jwks>();
//        var jwks23 = System.Text.Json.JsonSerializer.Deserialize<Jwks>(jwksJson);
//        Console.WriteLine("passe ici12");
//        var tokenHandler = new JwtSecurityTokenHandler();
//        var jwtToken = tokenHandler.ReadJwtToken(token);
//        Console.WriteLine($"passe ici13 : {jwtToken.Header.ToString()}"); ;
//        // Check the kid in the token header
//        var kid = jwtToken.Header["kid"].ToString();
//        Console.WriteLine($"kid : {kid}");
//        //Console.WriteLine($"jwks : {jwks.Keys.First().Kid}");

//        var signingKey = jwks.Keys.FirstOrDefault(k => k.Kid == kid);

//        if (signingKey == null)
//        {
//            Console.WriteLine("KID ABSENT");
//            throw new SecurityTokenInvalidSignatureException("Invalid kid.");
//        }
//        Console.WriteLine($"Verification validation : {token}");
//        tokenHandler.ValidateToken(token, new TokenValidationParameters
//        {
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKeys = new[] { new SymmetricSecurityKey(Convert.FromBase64String(signingKey.N)) }, // Adjust if using asymmetric keys
//            ValidIssuer = "https://localhost:7147",
//            ValidAudience = "https://localhost:7171",
//            ValidateIssuer = false,
//            ValidateAudience = false,
//            ClockSkew = TimeSpan.FromMinutes(360),
//        }, out SecurityToken validatedToken);
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"problme Kid: {ex.Message}");
//    }
//    return true;
//}

//private async Task Validate8Token(string token)
//{
//    var response = await _httpClient.GetAsync("api/lgauth/getkeys");
//    var jwksJson = await response.Content.ReadAsStringAsync();
//    //var jwks = JsonSerializer.Deserialize<Jwks>(jwksJson);
//    var jwks = System.Text.Json.JsonSerializer.Deserialize<Jwks>(jwksJson);
//    var tokenHandler = new JwtSecurityTokenHandler();
//    tokenHandler.ValidateToken(token, new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKeys = jwks.Keys.Select(k => new SymmetricSecurityKey(Convert.FromBase64String(k.N))),
//        ValidIssuer = "https://localhost:7147",
//        ValidAudience = "https://localhost:7171",
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ClockSkew = TimeSpan.Zero,
//        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1unk9NsxWJztdUEuOJ2t0fy7bnTZldsgYu5qoqHEebhblQXcCc2yk0CNAFxNCROyFszOJ05HZhlvTBRKxx3aY512Z0GqFme5VfEMZxl16Yxa97gHK66dMQKgczXyiDRG8JPXvvzdddbUWre7P3fYUkrEnvwRIGZRA3Z2QunNyOoVhOwbXmk6UbTKc1BU25bfH8htu8UlOkxloKow3WXK0xO3bZ9HAyfoPNqg90T3ceX1pda3VnGpTAADs4nrjybE")),
//        ValidateLifetime = true

//    }, out SecurityToken validatedToken);
//}
//public async Task<bool> NotifyUser(string nitoken)
//{
//    Console.WriteLine("Notification :");
//    Console.WriteLine(nitoken);
//    ((MyAuthStateProvider)AuthenticationStateProvider).NotifyUserAuthentication(nitoken);

//    var claims = ParseClaimsFromJwt(nitoken);
//    var authState = await _authStateProvider.GetAuthenticationStateAsync();
//    var authUser = authState.User;
//    var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
//        ((MyAuthStateProvider)AuthenticationStateProvider).NotifyAuthenticationStateChanged(authenticatedUser);
//    await _authSP.AuthenticationStateChanged();
//    _authStateProvider.NotifyUserAuthentication(nitoken);

//}
//public async Task<bool> Validate17Token(string token, string username)
//{
//    //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
//    var response = await _httpClient.GetAsync("api/lgauth/getkeytok?username=" + username);
//    var jwks = await response.Content.ReadFromJsonAsync<Userjwt>();
//    //var jwks = System.Text.Json.JsonSerializer.Deserialize<Jwks>(jwksJson);

//    var tokenHandler = new JwtSecurityTokenHandler();
//    var jwtToken = tokenHandler.ReadJwtToken(token);
//    //var tokena = jwks.JwToken;
//    Console.WriteLine($"mes tokens 1 : {jwtToken}");
//    //Console.WriteLine($"mes tokens 1 : {tokena}");
//    // Check the kid in the token header
//    var kid = jwtToken.Header["kid"].ToString();
//    //var kid = jwks.JwToken.FirstOrDefault();
//    string jwkt = jwks.JwToken;
//    var signingKey = jwks.JwKeys.FirstOrDefault(k => k.Kid == kid);
//    if (signingKey == null)
//    {
//        throw new SecurityTokenInvalidSignatureException("Invalid kid.");
//    }
//    try
//    {
//        Console.WriteLine($" signkey: {signingKey}");
//        tokenHandler.ValidateToken(jwkt, new TokenValidationParameters
//        {
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKeys = new[] { new SymmetricSecurityKey(Convert.FromBase64String(signingKey.N)) }, // Adjust if using asymmetric keys
//            ValidIssuer = "https://localhost:7147",
//            ValidAudience = "https://localhost:7171",
//            ValidateIssuer = false,
//            ValidateAudience = false,
//            ClockSkew = TimeSpan.FromMinutes(360),
//        }, out SecurityToken validatedToken);
//        Console.WriteLine($" resultat interne validate token success: {validatedToken}");
//        Console.WriteLine($" validate token success: {jwkt}");
//        return true;
//    }catch(Exception ex)
//    {
//        Console.WriteLine($" validate token fail: {ex.Message}");
//        return false;
//    }
//}   
// using (var db = await this._dbManager.Create<DbofContext>())
// {
//     if (!db.Userbags.Any()){
//         //add new record
//         usrBag.Id = 1;
//         db.Userbags.Add(usrBag);
//         await db.SaveChanges();
//         Console.WriteLine("Created new record.");
//     } else {
//         var firstUbag = db.Userbags.Single(x => x.Id == 1);
//         if (firstUbag != null)
//         {
//             if (firstUbag.Id == 1)
//             {
//                 //update old record
//                 firstUbag = usrBag;
//                 firstUbag.Id = 1;
//                 await db.SaveChanges();
//                 Console.WriteLine("Updated existing record.");
//             }   else
//             {
//                 //add new record
//                 usrBag.Id = 1;
//                 db.Userbags.Add(usrBag);
//                 await db.SaveChanges();
//                 Console.WriteLine("Created new record.");
//             }
//         } else
//         {
//             //add new record
//             usrBag.Id = 1;
//             db.Userbags.Add(usrBag);
//             await db.SaveChanges();
//             Console.WriteLine("Created new record.");
//         }
//     }
//}
//public class MyAuthStateProvider5 : AuthenticationStateProvider
//{
//    //public Userbag? currentUser { get; set; }
//    //public ClaimsPrincipal _currentUser; 
//    private readonly ILocalStorageService _localStorage;
//    private readonly IHttpClientFactory _httpClientFactory;
//    private LoginResult _logResu;
//    //private IAuthenticationService _authService;
//    public MyAuthStateProvider5(ILocalStorageService localStorage,
//        IHttpClientFactory httpClientFactory)
//    {
//        _localStorage = localStorage;
//        _httpClientFactory = httpClientFactory;
//        //_currentUser = currentUser;
//        _logResu = new LoginResult();
//    }
//    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//    {
//        var token = await _localStorage.GetItemAsync<string>("blazToken");
//        //Console.WriteLine(token);
//        ClaimsPrincipal user;
//        if (string.IsNullOrEmpty(token))
//        {
//            user = new ClaimsPrincipal(new ClaimsIdentity());
//        }
//        else
//        {
//            user = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token)));
//        }
//        var authuser = new ClaimsPrincipal(user);
//        //Console.WriteLine($"Peut etre enfin");
//        return new AuthenticationState(authuser);
//    }
//    public void NotifyAuthenticationStateChanged()
//    {
//        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
//    }
//    public async Task NotifyUserAuthentication(string token)
//    {
//        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
//        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
//        Console.WriteLine($"from usernotif : {authenticatedUser.Identity.Name}");
//        NotifyAuthenticationStateChanged(authState);
//    }
//    public async Task NotifyUserLogout()
//    {
//        //Blazor side
//        await _localStorage.RemoveItemAsync("blazToken");
//        await _localStorage.RemoveItemAsync("blazAuthen");
//        //Aurelia side
//        await _localStorage.RemoveItemAsync("clieAuthen");
//        await _localStorage.RemoveItemAsync("clieToken");
//        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
//        var anoauthstate = Task.FromResult(new AuthenticationState(anonymousUser));
//        NotifyAuthenticationStateChanged(anoauthstate);
//    }
//    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
//    {
//        var payload = jwt.Split('.')[1];
//        var jsonBytes = ParseBase64WithoutPadding(payload);
//        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
//        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
//    }
//    private byte[] ParseBase64WithoutPadding(string base64)
//    {
//        switch (base64.Length % 4)
//        {
//            case 2: base64 += "=="; break;
//            case 3: base64 += "="; break;
//        }
//        //Console.WriteLine($"base64: {base64}");
//        return Convert.FromBase64String(base64);
//    }
//    public async Task<bool> ValidateToken(string token, string username, string uikey)
//    {
//        var keys = await GetJsonWebKeys(username);
//        var tokenHandler = new JwtSecurityTokenHandler();
//        var jwToken = tokenHandler.ReadJwtToken(token);
//        var kid = jwToken.Header["kid"].ToString();
//        var signingKey = keys.FirstOrDefault(k => k.Kid == kid);
//        if (signingKey == null)
//        {
//            //Console.WriteLine($"vue kid : {kid}");
//            //Console.WriteLine($"vue signingKey : {signingKey}");
//            throw new SecurityTokenInvalidSignatureException("Invalid kid.");
//        }
//        byte[] nwsigningKey = Encoding.UTF8.GetBytes(uikey);
//        //Console.WriteLine($"kid : { kid } after signingKey : {signingKey}");
//        //decoding jsonweb key
//        byte[] verifyingKey = Convert.FromBase64String(signingKey.K);
//        var validationParameters = new TokenValidationParameters
//        {
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(nwsigningKey), // Adjust if using asymmetric keys
//            ValidateIssuer = true,
//            ValidIssuer = "https://localhost:7147",
//            ValidateAudience = true,
//            ValidAudience = "https://localhost:7171",
//            ClockSkew = TimeSpan.Zero,
//        };
//        try
//        {
//            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
//            var jwtSecurityToken = validatedToken as JwtSecurityToken;
//            if (jwtSecurityToken != null)
//            {
//                var roles = jwtSecurityToken.Claims.Where(c => c.Type == ClaimTypes.Role)
//                        .Select(c => c.Value).ToList();
//                //Use roles for further authorization logic
//            }
//            //Console.WriteLine("V---TOKEN VERIFIE");
//            return true;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"V---TOKEN NON VERIFIE {ex.Message}");
//            return false;
//        }
//    }
//    private async Task<JsonWebKey[]> GetJsonWebKeys(String username)
//    {
//        var gwtoken = await _localStorage.GetItemAsync<string>("blazToken");
//        var _apiClient = _httpClientFactory.CreateClient("ASClient");
//        var response = await _apiClient.GetAsync("api/lgauth/getkeytok?username=" + username);
//        response.EnsureSuccessStatusCode();
//        var jwks = await response.Content.ReadFromJsonAsync<Userjwt>();
//        return jwks.JwKeys.ToArray();
//    }
//}
// private async Task<bool> GetUserDataAsync(string hdltoken)
// {
//     //prise de UserBag
//     string hdlauthen = usrLog.Utoken;
//     try
//     {
//         Console.WriteLine("ENTREE HANDLE APPEL USER DON : ");
//         //await _localStorage.SetItemAsync("blazToken", true);
//         var ModResu = await _apiClient.GetAsync("api/lgbreeze/udonbag");
//         //attente _apiClient with token
//         if (ModResu.IsSuccessStatusCode)
//         {
//             isatmsg = "enregistrement de votre profil...";
//             usrBag = await ModResu.Content.ReadFromJsonAsync<Userbag>();
//             if (usrBag == null)
//             {
//                 Console.WriteLine("Failed to read UserBag from JSON.");
//                 return false; // Handle error appropriately
//             }
//             try
//             {
//                 //UserBag dans IndexedDB
//                 //Console.WriteLine($"Arrivee DonnUsrBag {usrBag.Lkfixes.Count() }");
//                 var dbUbag = await _dbManager.GetByIdAsync(1);
//                 if (dbUbag.Id == 1)
//                 { //updating/updating properties here
//                     await _dbManager.UpdateAsync(dbUbag);
//                 }
//                 else
//                 { //adding (Id = 1)/from usrBag, updating properties 
//                     usrBag.Id = 1;
//                     //await _dbManager.Test("5138", usrBag);
//                     //Console.WriteLine($"En adding// {usrBag.Idorg}");
//                     await _dbManager.AddAsync(usrBag);
//                 }
//                 errors = false; success = true;
//                 //Save for  Aurelia
//                 var objauth = new
//                 {
//                     clieAuthen = hdlauthen,
//                     clieToken = hdltoken
//                 };
//                 var jsonStr = JsonSerializer.Serialize(objauth);
//                 await JSRun.InvokeVoidAsync("localStorage.setItem", "inclieobj", jsonStr);
//                 return true;
//             }catch(Exception ex)
//             {
//                 Console.WriteLine($"except : {ex.Message}");
//             }
//             return false;
//         } else
//         {
//             Console.WriteLine($"call appel error: {ModResu.StatusCode}");
//             return false;
//         }
//     }catch(Exception ex)
//     {
//         errors = true;
//         Console.WriteLine($"PROBLEME sur BD : {ex.Message}");
//         errorList.Append(ex.Message);
//         return false;
//     }
// }
//protected override async Task OnInitializedAsync()
//{

//_apiClient = httpfacClient.CreateClient("ASDefault");
//var tetok = _localStorage.GetItemAsync<string>("blazToken");
//var tefin = _localStorage.GetItemAsync<string>("clieBackUrl");
//Console.WriteLine($"LANCEMENT ICI : {tetok}");
//Console.WriteLine($"LANCEMENT ICI backUrl : {tefin}");


// try {
//     //niv4 = 1;
//     if (AuthState == null)
//     {
//         return;
//     }
//     var authState = await AuthState;
//     claims = authState.User.Claims;
//     authUser = authState.User;
//     isAuthenticated = authUser.Identity.IsAuthenticated;
//     if (!isAuthenticated)
//     {
//         Console.WriteLine("Authentication ECHEC");
//         return;
//     }

//     curUname = authUser.Identity.Name; //or claims
//     Console.WriteLine($"Authent User PRIS { curUname }");
//     //niv4 = 6;
//     await GetUserBag(UserId);
//     Console.WriteLine($"Authent Prise REUSSIE { UserId }");
//     //await LaunchAdm();
//     var token = _localStorage.GetItemAsync<string>("blazToken");
//     _jsRun.InvokeVoidAsync("setParas", token, true, _usrBag.Lkuser);
//     isLoading = false;
//     //niv4 = 8;
//     Console.WriteLine("Lancement SUCCESS");
// }catch(Exception ex)
// {
//     Console.WriteLine($"probleme // lancement : {ex.Message}");
// }
//}
// private async Task LaunchAdm()
// private async Task SendParameters()
// {
//     //await _jsRun.InvokeVoidAsync("import", "/gxadm/entry.bundle.js");
// }
//     //Console.WriteLine("entree dans launch");
//     try
//     {
//         Console.WriteLine("commence lancement launch");

//         //await _jsRun.InvokeVoidAsync("import", "/gxadm/entry.bundle.js");
//         /////_navManager.NavigateTo("/gxadm/index.html");
//         //parametres a passer (a prendre dans lgauth)
//         //token, authen (a prendre dans lgauth/loadadm et a executer)
//         //Console.WriteLine($"token avant appel : {token}");
//         // var jsonStr = JsonSerializer.Serialize(objauth);
//         // await _jsRun.InvokeVoidAsync("localStorage.setItem", "inclieobj", jsonStr);

//         // var response = await _apiClient.GetAsync("api/lgauth/loadadm");
//         // if (response.IsSuccessStatusCode)
//         // {
//         //     //Console.WriteLine("1er succes");
//         //     var result = await response.Content.ReadFromJsonAsync<LoginResult>();
//         //     var url = result.Url + "/index.html";
//         //     var email = result.Email;
//         //     //var token = result.Utoken;
//         //     var authen = result.Isauthen;
//         //     var username = result.Username;
//         //     var phonenumber = result.Phonenumber;
//         //     Console.Write("token a envoyer dans lance/" + url);
//         //     var token = _tokenService.GetTokenAsync();
//         //     //Console.WriteLine(token);
//         //     _navManager.NavigateTo(url + "?sndtoken=" + token +"&sndauthen=" + authen + "&sndyuure=" + username + "&sndbanga=" + phonenumber);
//         // }
//         // else
//         // {
//         //     Console.WriteLine($"Non charge : {response.Content.ToString()}");
//         // }
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"Echec Depart:// {ex.Message}");
//     }
// }

// private class UrlModel
// {
//     public string Url { get; set; } = string.Empty;
// }
// [JSInvokable]
// public static Task<string> GetDivers(){
//     var parameters = "kdkd";
//     Console.WriteLine("passe par la");
//     return Task.FromResult(System.Text.Json.JsonSerializer.Serialize(parameters));
// }
// [JSInvokable]
// public static async Task<string> Get44Parameters()
// {
//     Console.WriteLine("APPELES");
//     var parameters = "kdkd";
//     return System.Text.Json.JsonSerializer.Serialize(parameters);
// }
// private async Task OnIframeLoad(){
//     isLoading = true;
//     var authState = await AuthState;
//     claims = authState.User.Claims;
//     authUser = authState.User;
//     isAuthenticated = authUser.Identity.IsAuthenticated;
//     if (!isAuthenticated)
//     {
//         Console.WriteLine("Authentication ECHEC");
//         return;
//     }
//     //curUname = authUser.Identity.Name; //or claims
//     //Console.WriteLine($"Authent User PRIS { curUname }");
//     //niv4 = 6;
//     _UserId = authUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//     Console.WriteLine($"Authent Prise REUSSIE {_UserId}");
//     if (!string.IsNullOrEmpty(_UserId))
//     {
//         await GetUserBag(_UserId);
//         //await LaunchAdm();
//         var token = await _localStorage.GetItemAsync<string>("blazToken");
//         // Set parameters in JavaScript before the iframe loads
//         var truser = _usrBag.Lkuser;
//         Console.WriteLine($"token : {token} et user : {truser.Email}");
//         try
//         {

//             await _jsRun.InvokeVoidAsync("setParas", token, true, truser);
//             Console.WriteLine("parametres installes");
//             //isLoading = false;
//             //StateHasChanged(); //refresh the UI
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"Invoke exception : {ex.Message} /// {ex.InnerException}");
//         }
//     }
//     else
//     {
//         Console.WriteLine("Utilisateur inacceptable");
//     }
//     //await _jsRun.InvokeVoidAsync("setParas", token, true, truser);
//}

// public void async idoid()
// {
//     Console.WriteLine("mess received 11");
//     if (message == "auLoaded")
//     {
//         Console.WriteLine("mess received 22");
//         isLoading = false;
//         InvokeAsync(StateHasChanged); //refresh the UI
//     }       
// }
//const iframe = document.getElementById('auIframe');
//let parameters = {
//                            type: "initParams",
//                            params: {
//token: null,
//                            authen: null,
//                            lkuser: null
//                            }
//                            };
// // Function to set parameters
// window.setParas = function (token, authen, lkuser) {
//     parameters.params.token = token;
//     parameters.params.authen = authen;
//     parameters.params.lkuser = lkuser;
// };
//function setupIframeLister()
//{
//    window.addEventListener("message", function(event) {
//        //if(event.origin !== 'https://your-iframe-origin.com') {
//        // return;
//        //}
//        // Handle the message
//        if (event.data === 'iframeLoaded') {
//            alert('DERNIER RETOUR DE AURELIA');
//            console.log('Event occurred in iframe:', event.data);
//            dotNetHelper.InvokeMethodAsync('GxClie', 'UpdateLoadingState', false);
//        } else
//        {
//            if (event.data == 'operationCompleted') {
//                dotNetHelper.InvokeMethodAsync('GxClie', 'UpdateLoadingState', false);
//            }
//        }
//    });
//}
//function initializeIframe(toka, autha, akuser)
//{
//    const iframeUrl = '/gxadm/index.html'; ///?token=${encodeURIComponent(toka)}&authen=${encodeURIComponent(autha)}&lkuser=${encodeURIComponent(akuser)}';
//    alert('bon1');
//    parameters.params.token = toka;
//    parameters.params.authen = autha;
//    parameters.params.lkuser = akuser;
//    const iframe = document.getElementById('auIframe');
//    iframe.scr = iframeUrl;
//    //iframe.contentWindow.postMessage(parameters, '*');
//    alert(iframe.scr);
//    iframe.onload = function() {
//        alert('y a quelque chose');
//    }
// iframe.onload = function () {
//     if (parameters.params.token !== null) { Ensure token is set
//         alert('toujours dedans11');
//         iframe.contentWindow.postMessage(parameters, '*');
//     } else {
//         alert('parametres non disponibles');
//     }
// }
// iframe.onload = function(){
//     alert('appel aurelia' + toka);
//     iframe.contentWindow.postMessage(parameters, '*');
//     //iframe.contentWindow.postMessage("startOperations", '*');
// }
//}
// window.addMessageListener = (dotNetHelper) => {
//     window.addEventListener('message', (event) => {
//         alert('message created/bien arrive');
//         // Check the origin of the message for security
//         if (event.origin !== 'https://your-iframe-origin.com') {
//             //return;
//         }

//         // Handle the message
//         if (event.data.action === 'auLoaded') {
//             alert('DERNIER RETOUR DE AURELIA');
//             console.log('Event occurred in iframe:', event.data.data);
//             dotNetHelper.InvokeMethodAsync('ReceiveMessage', event.data.data);
//             // Perform actions based on the message received
//         }
//     });
// };
//using System.Text.Json;
//using System.Net.Http.Headers;

//using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.AspNetCore.Components.Web;
//using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

////using Blazor.IndexedDB.WebAssembly;
//using Blazored.LocalStorage;
////using Microsoft.JSInterop;

//using GxClie.Components;
//using GxClie.ClieServices;
//using GxClie.Account;
//using Breeze.Sharp;
//using Microsoft.Extensions.DependencyInjection;
//using GxShared.ApModels;
//using System.Net.Http;

//var builder = WebAssemblyHostBuilder.CreateDefault(args);
//var backUrl = new Uri(builder.Configuration["BackendUrl"]);

//builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");

////loading configuration in a json file
//builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
//// register the cookie handler
//builder.Services.AddTransient<CookieHandler>();

//builder.Services.AddBlazoredLocalStorage();
//var JsonOpts = new JsonSerializerOptions
//{
//    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
//    //DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
//    WriteIndented = true,
//};
//builder.Services.AddSingleton(JsonOpts);
//// set up Authentication-Authorization Service
//builder.Services.AddAuthorizationCore();
//// register the custom state provider
//builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
////Authentication
//builder.Services.AddCascadingAuthenticationState();
//builder.Services.AddScoped<MyAuthStateProvider>();
//builder.Services.AddScoped<AuthenticationStateProvider>(
//    provider => provider.GetRequiredService<MyAuthStateProvider>());
//builder.Services.AddTransient<AuthMessageHandler>();
//builder.Services.AddScoped<ITokenService, TokenService>();
////builder.Services.AddScoped<EntityManager>(sp =>
////    new EntityManager(backUrl + "/lgbreeze"));
////builder.Services.AddScoped<IEntityManager, BlazEntityManager>();
//// invitations et divers exchanges
////builder.Services.AddScoped<ApiCalls>();
////builder.Services.AddScoped<IEncryptionHelper, EncryptionHelper>();
////builder.Services.AddScoped<GlobState>();
//builder.Services.AddScoped<UserDbService>();
////builder.Services.AddScoped<MyAuthorService>();
//// register the account management interface
//builder.Services.AddScoped(
//    sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());
//// set base address for default host
//builder.Services.AddHttpClient("DefaultClient", client =>
//{
//    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
//    client.DefaultRequestHeaders.Add("Accept", "application/json");
//}); //.AddHttpMessageHandler<AuthMessageHandler>();
//// client for refresh token
//builder.Services.AddHttpClient("LGClient", Lclie =>
//{
//    //Lclie.BaseAddress = new Uri(builder.Configuration["BackendUrl"]);
//    var backUrl = builder.Configuration["BackendUrl"];
//    // Append "/api/" if it's not already included
//    if (!backUrl.EndsWith("/"))
//    {
//        backUrl += "/";
//    }
//    backUrl += "api/";
//    Lclie.BaseAddress = new Uri(backUrl); //
//    //Lclie.DefaultRequestHeaders.Add("Accept", "application/json");
//    Lclie.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//    Lclie.DefaultRequestHeaders.Add("X-Requested-With", "Fetch");
//});
//// client for auth interactions
//builder.Services.AddHttpClient("ASClient", Aclie =>
//{
//    //Aclie.BaseAddress = new Uri(builder.Configuration["BackendUrl"]);
//    // Ensure the BackendUrl includes the base URL of your API
//    var backUrl = builder.Configuration["BackendUrl"];
//    // Append "/api/" if it's not already included
//    if (!backUrl.EndsWith("/"))
//    {
//        backUrl += "/";
//    }
//    backUrl += "api/";
//    Aclie.BaseAddress = new Uri(backUrl); //
//    //Aclie.DefaultRequestHeaders.Add("Accept", "application/json");
//    Aclie.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//    Aclie.DefaultRequestHeaders.Add("X-Requested-With", "Fetch");
//    //opt.DefaultRequestHeaders.Add("Content-Type", "application/json");
//    //})
//    //.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
//    //{
//    //AllowAutoRedirect = false
//}).AddHttpMessageHandler<AuthMessageHandler>();

//builder.Services.AddHttpClient("BZEClient", Bclie =>
//{
//    var backzUrl = builder.Configuration["BackendUrl"];
//    if (!backzUrl.EndsWith("/"))
//    {
//        backzUrl += "/";
//    }
//    backzUrl += "api/";
//    Bclie.BaseAddress = new Uri(backzUrl);
//    Bclie.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//    Bclie.DefaultRequestHeaders.Add("X-Requested-With", "Fetch");
//    Console.WriteLine($"Base Address: {Bclie.BaseAddress}");
//}).AddHttpMessageHandler<AuthMessageHandler>();
//// Register Metadata Service
//builder.Services.AddScoped<MetadataService>();
//// Register MyDataService
//builder.Services.AddScoped<MyDataService>(sp =>
//{
//    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("BZEClient");
//    var apiUrl = httpClient.BaseAddress.ToString() + "lgbreeze";
//    var entityManager = new EntityManager(new MyDataService(httpClient, apiUrl, null));
//    var mydataservice = new MyDataService(httpClient, apiUrl, entityManager);
//    //var fullbaseAddress = httpClient.BaseAddress.ToString() + "lgbreeze";
//    return mydataservice; // new MyDataService(httpClient, apiUrl, entityManager);
//});
//// Register EntityManager
//builder.Services.AddScoped<EntityManager>(sp =>
//{
//    //var entityManager = sp.GetRequiredService<EntityManager>();
//    var dataservice = sp.GetRequiredService<MyDataService>();
//    return new EntityManager(dataservice);
//});
//// Register BzEntityManager
//builder.Services.AddScoped<BzEntityManager>(sp =>
//{
//    var entityManager = sp.GetRequiredService<EntityManager>();
//    var dataService = sp.GetRequiredService<MyDataService>();
//    return new BzEntityManager(entityManager, dataService);
//});

//// configure client for auth interactions
//builder.Services.AddHttpClient(
//    "AspClient",
//    opt => opt.BaseAddress = new Uri(builder.Configuration["BackendUrl"] ?? "https://localhost:5001")
//).AddHttpMessageHandler<CookieHandler>();
//// Build the host
//var host = builder.Build();

//// Initialize the UserDbService
//var userDbService = host.Services.GetRequiredService<UserDbService>();
//await userDbService.InitializeAsync(); // Call InitializeAsync

//// Run the application
//await host.RunAsync();
////builder.Build().RunAsync();
////BREEZE WORKING REGISTER
////builder.Services.AddHttpClient("BZEClient", Bclie =>
////{
////    var backzUrl = builder.Configuration["BackendUrl"];
////    if (!backzUrl.EndsWith("/"))
////    {
////        backzUrl += "/";
////    }
////    backzUrl += "api/";
////    Bclie.BaseAddress = new Uri(backzUrl);
////    Bclie.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
////    Bclie.DefaultRequestHeaders.Add("X-Requested-With", "Fetch");
////    Console.WriteLine($"Base Address: {Bclie.BaseAddress}");
////}).AddHttpMessageHandler<AuthMessageHandler>();

////// Register MyDataService
////builder.Services.AddScoped<MyDataService>(sp =>
////{
////    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("BZEClient");
////    return new MyDataService(httpClient, httpClient.BaseAddress.ToString());
////});

////// Register EntityManager
////builder.Services.AddScoped<EntityManager>(sp =>
////{
////    var dataService = sp.GetRequiredService<MyDataService>();
////    return new EntityManager(dataService);
////});

////// Register BzEntityManager
////builder.Services.AddScoped<BzEntityManager>();
////builder.Services.AddScoped<MetadataService>();
//// client for Breeze Sharp client
////builder.Services.AddScoped<EntityManager>();
////builder.Services.AddScoped<BzEntityManager>();
////builder.Services.AddScoped<MetadataService>();
////builder.Services.AddHttpClient<MyDataService>("BZEClient", Bclie =>
////{
////    // Ensure the BackendUrl includes the base URL of your API
////    var backzUrl = builder.Configuration["BackendUrl"];
////    // Append "/api/" if it's not already included
////    if (!backzUrl.EndsWith("/"))
////    {
////        backzUrl += "/";
////    }
////    backzUrl += "api/";
////    Console.WriteLine($"Constructed Base Address: {backzUrl}"); // Log the constructed URL
////    Bclie.BaseAddress = new Uri(backzUrl); //
////    Bclie.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
////    Bclie.DefaultRequestHeaders.Add("X-Requested-With", "Fetch");
////}).AddHttpMessageHandler<AuthMessageHandler>();

////// Register HttpClient for Breeze
////builder.Services.AddHttpClient<MyDataService>("BZEClient", client =>
////{
////    var backzUrl = builder.Configuration["BackendUrl"];
////    if (!backzUrl.EndsWith("/"))
////    {
////        backzUrl += "/";
////    }
////    backzUrl += "api/";
////    Console.WriteLine($"Constructed Base Address: {backzUrl}");
////    client.BaseAddress = new Uri(backzUrl);
////    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
////    client.DefaultRequestHeaders.Add("X-Requested-With", "Fetch");
////}).AddHttpMessageHandler<AuthMessageHandler>();


////// Register HttpClient for Breeze
////builder.Services.AddHttpClient<MyDataService>("BZEClient", client =>
////{
////    var backzUrl = builder.Configuration["BackendUrl"];
////    if (!backzUrl.EndsWith("/"))
////    {
////        backzUrl += "/";
////    }
////    backzUrl += "api/";
////client.BaseAddress = new Uri(backzUrl);
////client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
////client.DefaultRequestHeaders.Add("X-Requested-With", "Fetch");
////});
//// Register MyDataService with a concrete service name
////builder.Services.AddScoped<MyDataService>(sp =>
////{
////    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("BZEClient");
////    var serviceName = "BZEClient"; // Replace with your actual service name or fetch from config
////    return new MyDataService(httpClient, serviceName);
////});
////// Register other services
////builder.Services.AddScoped<BzEntityManager>();
////builder.Services.AddScoped<MetadataService>();
////builder.Services.AddHttpClient("BZEClient", Bclie =>
////{
////    // Ensure the BackendUrl includes the base URL of your API
////    var backzUrl = builder.Configuration["BackendUrl"];
////    // Append "/api/" if it's not already included
////    if (!backzUrl.EndsWith("/"))
////    {
////        backzUrl += "/";
////    }
////    backzUrl += "api/";
////    Bclie.BaseAddress = new Uri(backzUrl); //
////    Bclie.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
////    Bclie.DefaultRequestHeaders.Add("X-Requested-With", "Fetch");
////}).AddHttpMessageHandler<AuthMessageHandler>();
////Breeze services
//// Register EntityManager using the service URL
////builder.Services.AddScoped<EntityManager>(sp =>
////{
////    // Use the base address of your API for Breeze
////    var apiUrl = backUrl + "/lgbreeze"; // ReplacSe with your actual API URL
////    return new EntityManager(apiUrl); // Pass the API URL as a string
////});
//////builder.Services.AddScoped<BzEntityManager>(sp =>
//////{
//////    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
//////    var tokenService = sp.GetRequiredService<ITokenService>();
//////    if (httpClientFactory != null)
//////    {
//////        Console.WriteLine("YA FACTORY");
//////    }
//////    if (tokenService != null)
//////    {
//////        Console.WriteLine("YA TOKSERVICE");
//////    }
//////    return new BzEntityManager("ASClient", httpClientFactory, tokenService);
//////});
////builder.Services.AddScoped<EntityManager>();
////builder.Services.AddScoped<BzEntityManagerService>();
//// IndexDBServices
////builder.Services.AddIndexedDB(dbStore =>
////{
////    dbStore.DbName = "bzidxDB"; //physique
////    dbStore.Version = 1;
////    dbStore.Stores.Add(new StoreSchema
////    {
////        Name = "bagstore",
////        PrimaryKey = new IndexSpec
////        {
////            Name = "id",
////            KeyPath = "id",
////            Auto = true
////        },
////        Indexes = new List<IndexSpec>
////        {
////            new IndexSpec {
////                Name = "userid",
////                KeyPath = "userid",
////                Auto = false
////            }
////        }
////    });
////});
////builder.Services.AddScoped<IUdbManagement, UdbManagement>();
//private HttpClient CreateHttpClient(string accessToken)
//{
//    var httpClient = _httpClientFactory.CreateClient();

//    if (!string.IsNullOrEmpty(accessToken))
//    {
//        httpClient.DefaultRequestHeaders.Authorization =
//            new AuthenticationHeaderValue("Bearer", accessToken);
//        Console.WriteLine($"Bearer header set for httpClient : {accessToken}");
//    }

//    return httpClient;
//}
//}
//public class BreezeConfigService
//{
//    public void ConfigureBreeze(string accessToken)
//    {
//        // Register the AjaxFetchAdapter
//        AjaxFetchAdapter.register();

//        // Set default headers, including the Authorization header
//        if (!string.IsNullOrEmpty(accessToken))
//        {
//            AjaxFetchAdapter.defaultSettings = new DefaultSettings
//            {
//                headers = new Dictionary<string, string>
//            {
//                { "Authorization", $"Bearer {accessToken}" }
//            }
//            };
//        }
//    }
//}

///////////
///
//// Check if BzEntityManager is already created
//if (_bzEntityManager == null)
//{
//    var httpClient = _httpClientFactory.CreateClient("BZEClient");

//    // Set Authorization header if an access token is provided
//    if (!string.IsNullOrEmpty(accessToken))
//    {
//        httpClient.DefaultRequestHeaders.Authorization =
//            new AuthenticationHeaderValue("Bearer", accessToken);
//    }

//    Console.WriteLine($"Last Url : {fullServiceUrl}");
//    // Create a new instance of BzEntityManager with the full service URL
//    _bzEntityManager = new BzEntityManager(httpClient, "lgbreeze");
//}
//else
//{
//    // Update Authorization header if needed
//    if (!string.IsNullOrEmpty(accessToken))
//    {
//        _bzEntityManager.HttpClient.DefaultRequestHeaders.Authorization =
//            new AuthenticationHeaderValue("Bearer", accessToken);
//    }
//}

//return _bzEntityManager;

////public class BzEntityManager : EntityManager //: Breeze.Sharp.EntityManager
////{
////    private readonly HttpClient _httpClient;
////    //private readonly EntityManager _entityManager;
////    // Constructor that initializes the base EntityManager with a DataService
////    public BzEntityManager(HttpClient httpClient, string serviceName)
////        : base(new DataService(httpClient.BaseAddress + serviceName))
////    {
////        _httpClient = httpClient;
////    }
////    //public BzEntityManager(EntityManager entityManager,
////    //    HttpClient httpClient //, ITokenService tokenService
////    //   ) //, MyDataService myDataService, HttpClient httpClient)
////    //{
////    //    _entityManager = entityManager;
////    //    _httpClient = httpClient; // ((DataService)_entityManager.DataService).HttpClient; // Access existing HttpClient 
////    //}
////    public static async Task<BzEntityManager> CreateAsync(string serviceName, IHttpClientFactory httpClientFactory, string accessToken)
////    {
////        Console.WriteLine("12BZ INITIALIZATION");
////        var httpClient = httpClientFactory.CreateClient(serviceName);
////        var baseAddress = httpClient.BaseAddress.ToString() + "lgbreeze";
////        Console.WriteLine("13BZ INITIALIZATION");

////        var dataService = new DataService(baseAddress);
////        var entityManager = new EntityManager(dataService);

////        var existingHttpClient = ((DataService)entityManager.DataService).HttpClient;
////        Console.WriteLine("15BZ INITIALIZATION");
////        ////// Ensure metadata is imported
////        //////await EnsureAuthorizationHeaderAsync(existingHttpClient, tokenService);
////        //////string accessToken = null;
////        //////try
////        //////{
////        //////    Console.WriteLine("161BZ INITIALIZATION");
////        //////    accessToken = await tokenService.GetATokenAsync();
////        //////    await Task.Delay(1); // Allow context switching
////        //////    Console.WriteLine("162BZ INITIALIZATION");
////        //////}
////        //////catch (Exception ex)
////        //////{
////        //////    Console.WriteLine($"TOKEN RETRIEVAL ERROR: {ex.Message}");
////        //////    // Handle accordingly (e.g., throw, return null, etc.)
////        //////    return null; // or throw new Exception("Token retrieval failed.");
////        //////}
////        Console.WriteLine("171BZ INITIALIZATION");
////        if (!string.IsNullOrEmpty(accessToken))
////        {
////            existingHttpClient.DefaultRequestHeaders.Authorization =
////                new AuthenticationHeaderValue("Bearer", accessToken);
////            Console.WriteLine($"1Authorization Header Manually Set: Bearer {accessToken}");
////        }
////        else
////        {
////            Console.WriteLine($"Token IS NOT STRING {accessToken}");
////        }
////        Console.WriteLine("172BZ INITIALIZATION");
////        try
////        {
////            //await entityManager.FetchMetadata();
////            Console.WriteLine("18BZ INITIALIZATION");
////            //var enreg = entityManager.MetadataStore.EntityTypes.Count();
////            //Console.WriteLine($"19BZ INITIALIZATION : {enreg}");
////        }
////        catch (Exception ex)
////        {
////            Console.WriteLine($"TOKEN RETRIEVAL ERROR: {ex.Message}");
////            throw;
////        }

////        var bzEntityManager = new BzEntityManager(httpClient, serviceName);
////        //var bzEntityManager = new BzEntityManager(entityManager, existingHttpClient);
////        //var bzEntityManager._entityManager.DataService.HttpClient = httpClient;
////        return bzEntityManager; //new BzEntityManager(entityManager, tokenService);
////    }
////    private async Task InitializeAsync(string inAToken)
////    {
////        try
////        {
////            Console.WriteLine("16STARTBZ INITIALIZATION");
////            //var accessToken = await _tokenService.GetATokenAsync();
////            Console.WriteLine("16ENDEDBZ INITIALIZATION");
////            if (!string.IsNullOrEmpty(inAToken))
////            {
////                _httpClient.DefaultRequestHeaders.Authorization =
////                    new AuthenticationHeaderValue("Bearer", inAToken);
////                Console.WriteLine($"1Authorization Header Manually Set: Bearer {inAToken}");
////            }
////            Console.WriteLine("17BZ INITIALIZATION");
////            // Set the correct EntityManager in MyDataService
////            //dataService.SetEntityManager(entityManager);

////            // Perform dummy metadata call to preload metadata
////            //await _entityManager.FetchMetadata();
////            ///var curmeta = _entityManager.ExportEntities();
////            //_entityManager.ImportEntities(curmeta);
////            //Console.WriteLine($"NB ENTITIES : {curmeta.Count()}");

////            //await _entityManager.
////            //Console.WriteLine("18BZ INITIALIZATION");
////            //var enreg = _entityManager.MetadataStore.EntityTypes.Count();
////            //Console.WriteLine($"19BZ INITIALIZATION : {enreg}");
////        }
////        catch (Exception ex)
////        {
////            Console.WriteLine($"Error initializing BzEntityManager: {ex.Message}");
////            throw;
////        }
////    }
////    public async Task EnsureAuthorizationHeaderAsync(string inAToken) // (HttpClient ehttpClient, ITokenService etokenService)
////    {
////        //var accessToken = await _tokenService.GetATokenAsync();
////        if (!string.IsNullOrEmpty(inAToken))
////        {
////            _httpClient.DefaultRequestHeaders.Authorization =
////                new AuthenticationHeaderValue("Bearer", inAToken);
////            Console.WriteLine($"1Authorization Header Manually Set: Bearer {inAToken}");
////        }
////        else
////        {
////            Console.WriteLine($"Token PAS PRET");
////        }
////    }
////    public async Task<IEnumerable<T>> ExecuteApiRequestAsync<T>(EntityQuery<T> query)
////    {
////        try
////        {
////            // Ensure metadata is fetched
////            if (!_entityManager.MetadataStore.IsEmpty())
////            {
////                Console.WriteLine("Metadata already loaded.");
////            }
////            else
////            {
////                Console.WriteLine("Fetching metadata...");
////                await _entityManager.FetchMetadata();
////                Console.WriteLine("Metadata fetched successfully.");
////            }
////            var result = await _entityManager.ExecuteQuery(query);
////            Console.WriteLine($" META VERSION : {Breeze.Sharp.MetadataStore.MetadataVersion}");
////            if (_entityManager.MetadataStore == null)
////            {
////                Console.WriteLine($"METASTORE  : NULL");
////            }
////            else
////            {
////                if (!_entityManager.MetadataStore.EntityTypes.Any())
////                {
////                    Console.WriteLine($"METASTORE ENTITYTYPES : NULL");
////                }
////                else
////                {
////                    Console.WriteLine($"METASTORE COUNT ENTITYTYPES : {_entityManager.MetadataStore.EntityTypes.Count()}");
////                }
////            }
////            Console.WriteLine($"REQUEST2 SUCCEEDED : {result.Count()}");
////            return result;
////        }
////        catch (Exception ex)
////        {
////            Console.WriteLine($"REQUEST ISSUE : {ex.Message}");
////            foreach (var uva in ex.Data.Values)
////            {
////                Console.WriteLine($"ici/{uva.GetType}//{uva.ToString}");
////            }
////            Console.WriteLine($"REQUEST INNER : {ex.InnerException.Message}");
////            return null;
////        }
////    }
////}

//////////
//private async Task<BzEntityManager> InitializeAsync(string serviceName, string accessToken )
//{
//    Console.WriteLine("11BZ INITIALIZATION");
//    var bzEntityManager = await BzEntityManager.CreateAsync(
//        "BZEClient",
//        _httpClientFactory,
//        _curToken
//    );

//await _entityManager.MetadataStore.FetchMetadata(Breeze.Sharp.DataService);
//FetchMetadata;
// adi.GetDataService().ServerMetadata()
//await EnsureAuthorizationHeaderAsync();
// Execute the query using DataService
//var result = await _entityManager.DataService.GetAsync("tiersaie");
//await _entityManager.DataService.
//Console.WriteLine($"REQUEST SUCCEEDED : {result.Count()}");
//var resu5 = result.Cast<List<Tiersp>>();
//Console.WriteLine($"PROCHE TIERS : {resu5.Count()}");
//foreach(var ure in resu5)
//{
//    Console.WriteLine($" uneva : {ure.First().Usremail}");
//}
//List<Tiersp mytiers = resu5[0];


//    Console.WriteLine("21BZ INITIALIZATION");
//    return bzEntityManager;
//    //return await BzEntityManager.CreateAsync(
//    //    "BZEClient",
//    //    _httpClientFactory,
//    //    _tokenService
//    //);
//}

//_myDataService = myDataService;
//_tokenService = tokenService; 
//_accessToken = accessToken;
//private readonly ITokenService _tokenService;
//private readonly MyDataService _myDataService;

//public static async Task<BzEntityManager> CreateAsync(string serviceName, IHttpClientFactory httpClientFactory, ITokenService tokenService)
//{
//    Console.WriteLine("12BZ INITIALIZATION");
//    var httpClient = httpClientFactory.CreateClient(serviceName);
//    var baseAddress = httpClient.BaseAddress.ToString() + "lgbreeze";
//    Console.WriteLine("13BZ INITIALIZATION");
//    // Create MyDataService with the configured HttpClient
//    var dataService = new DataService(baseAddress); // MyDataService(httpClient, baseAddress, null, tokenService);
//    //dataService.HttpClient = httpClient;
//    var entityManager = new EntityManager(dataService);
//    // Access the existing HttpClient used by DataService
//    Console.WriteLine("14BZ INITIALIZATION");
//    var existingHttpClient = ((DataService)entityManager.DataService).HttpClient;
//    Console.WriteLine("15BZ INITIALIZATION");

//    var accessToken = await tokenService.GetATokenAsync();
//    Console.WriteLine("16BZ INITIALIZATION");
//    if (!string.IsNullOrEmpty(accessToken))
//    {
//        existingHttpClient.DefaultRequestHeaders.Authorization =
//            new AuthenticationHeaderValue("Bearer", accessToken);
//        Console.WriteLine($"1Authorization Header Manually Set: Bearer {accessToken}");
//    }
//    Console.WriteLine("17BZ INITIALIZATION");
//    // Set the correct EntityManager in MyDataService
//    //dataService.SetEntityManager(entityManager);

//    // Perform dummy metadata call to preload metadata
//    await entityManager.FetchMetadata();

//    Console.WriteLine("18BZ INITIALIZATION");
//    var enreg = entityManager.MetadataStore.EntityTypes.Count();
//    Console.WriteLine($"19BZ INITIALIZATION : { enreg }");
//    return new BzEntityManager(entityManager, tokenService);
//}

//{
//    private readonly IHttpClientFactory _httpClientFactory;
//    //private readonly MetadataService _metadataService;
//    private readonly ITokenService _tokenService;
//    private Task<BzEntityManager> _initializationTask;

//    public BzEntityManagerProvider(
//        IHttpClientFactory httpClientFactory,
//        //MetadataService metadataService,
//        ITokenService tokenService)
//    {
//        _httpClientFactory = httpClientFactory;
//        //_metadataService = metadataService;
//        _tokenService = tokenService;
//        _initializationTask = InitializeAsync();
//    }
//    private async Task<BzEntityManager> InitializeAsync()
//    {
//        return await BzEntityManager.CreateAsync(
//            "BZEClient",
//            _httpClientFactory,
//            //_metadataService,
//            _tokenService
//        );
//    }
//    public Task<BzEntityManager> GetBzEntityManagerAsync()
//    {
//        return _initializationTask;
//    }
//}
//}
//public class BzEntityManagerProvider
//{
//    private readonly IHttpClientFactory _httpClientFactory;
//    //private readonly MetadataService _metadataService;
//    private readonly ITokenService _tokenService;
//    private Task<BzEntityManager> _initializationTask;

//    public BzEntityManagerProvider(
//        IHttpClientFactory httpClientFactory,
//        //MetadataService metadataService,
//        ITokenService tokenService)
//    {
//        _httpClientFactory = httpClientFactory;
//        //_metadataService = metadataService;
//        _tokenService = tokenService;
//        _initializationTask = InitializeAsync();
//    }
//    private async Task<BzEntityManager> InitializeAsync()
//    {
//        return await BzEntityManager.CreateAsync(
//            "BZEClient",
//            _httpClientFactory,
//            //_metadataService,
//            _tokenService
//        );
//    }
//    public Task<BzEntityManager> GetBzEntityManagerAsync()
//    {
//        return _initializationTask;
//    }
//}
//public class BzEntityManager57
//{
//    private readonly EntityManager _entityManager;
//    private readonly MyDataService57 _dataService;

//    public BzEntityManager57(EntityManager entityManager, MyDataService57 dataService)
//    {
//        _entityManager = entityManager;
//        _dataService = dataService;
//    }

//    public static async Task<BzEntityManager57> CreateAsync(string serviceName,
//        IHttpClientFactory httpClientFactory, //MetadataService metadataService,
//        ITokenService tokenService)
//    {
//        Console.WriteLine("2CALLED //BzEntityManager-CreateAsync");

//        var httpClient = httpClientFactory.CreateClient(serviceName);
//        Console.WriteLine($"baseAdr : {httpClient.BaseAddress}");

//        var baseAddress = httpClient.BaseAddress.ToString() + "lgbreeze";
//        Console.WriteLine($"baseAdr2 : {baseAddress}");
//        //Fetch the token and set the authorization header manually
//        var accessToken = await tokenService.GetATokenAsync();
//        if (!string.IsNullOrEmpty(accessToken))
//        {
//            httpClient.DefaultRequestHeaders.Authorization =
//                new AuthenticationHeaderValue("Bearer", accessToken);
//            Console.WriteLine($"1Authorization Header Manually Set: Bearer {accessToken}");
//        }
//        else
//        {
//            Console.WriteLine("The fectch token is EMPTY");
//        }
//        var dataService = new MyDataService57(httpClient, baseAddress, null, tokenService);
//        //Console.WriteLine($"dataServi : {dataService.ServiceName}");

//        var entityManager = new EntityManager(dataService);

//        // Set the correct EntityManager
//        dataService.SetEntityManager(entityManager);

//        await entityManager.FetchMetadata();
//        if (!httpClient.DefaultRequestHeaders.Contains("Authorization"))
//        {
//            httpClient.DefaultRequestHeaders.Authorization =
//                new AuthenticationHeaderValue("Bearer", accessToken);
//        }
//        Console.WriteLine($"2Authorization Header Manually Set: Bearer {accessToken}");
//        //string hshead = GetAuthorizationHeader(httpClient);
//        //Console.WriteLine($" auth text: {hshead}");

//        //var cachedMetadata = await metadataService.GetMetadataAsync(serviceName);
//        //Console.WriteLine($"3Metadata : {cachedMetadata.Length}");

//        //entityManager.MetadataStore.ImportMetadata(cachedMetadata);
//        // Execute the query using MyDataService
//        //if (entityManager.MetadataStore.EntityTypes.Count == 0)
//        //{
//        //    Console.WriteLine($"et ensuite : {entityManager.MetadataStore.ComplexTypes.Count}");
//        //    throw new InvalidOperationException("Bz_Metadata not initialized");
//        //}
//        //entityManager.MetadataStore.AllowedMetadataMismatchTypes
//        //entityManager.MetadataStore.MetadataMismatch
//        return new BzEntityManager57(entityManager, dataService);
//    }
//    public async Task<IEnumerable<T>> ExecuteApiRequestAsync<T>(EntityQuery<T> query)
//    {
//        Console.WriteLine($"BzEntityManager : ENTER FOR EXECUTION {_entityManager.MetadataStore.ToString()}");
//        // Execute the query using MyDataService
//        //if (_entityManager.MetadataStore.EntityTypes.Count == 0)
//        //{
//        //    Console.WriteLine($"et autres : {_entityManager.MetadataStore.EntityTypes.Count}");
//        //    throw new InvalidOperationException("bz_metadata not initialized");
//        //}
//        Console.WriteLine("BzEntityManager : GO FOR EXECUTION");
//        return await _dataService.ExecuteQueryAsync(query);
//    }
//}
//public class MyDataService57 : DataService
//{
//    private readonly HttpClient _httpClient;
//    private EntityManager _entityManager;
//    private ITokenService _tokenService;
//    public MyDataService57(HttpClient httpClient,
//        string serviceName, EntityManager entityManager, ITokenService tokenService) : base(serviceName) //, EntityManager entityManager) : base(serviceName)
//    {
//        _httpClient = httpClient;
//        _entityManager = entityManager;
//        _tokenService = tokenService;
//        //InitializeHttpClient(httpClient);
//    }
//    public void SetEntityManager(EntityManager entityManager)
//    {
//        _entityManager = entityManager;
//    }
//    public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(EntityQuery<T> query)
//    {
//        Console.WriteLine("MyDataService : GO FOR EXECUTION");
//        try
//        {
//            // Ensure the authorization header is set for the request
//            var request = new HttpRequestMessage(HttpMethod.Get, _httpClient.BaseAddress);
//            foreach (var header in _httpClient.DefaultRequestHeaders)
//            {
//                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
//            }
//            // Verify the authorization header
//            Console.WriteLine($"Authorization header in ExecuteQueryAsync : {request.Headers.Authorization}");
//            var result = await ExecuteQueryWithRetryAsync(query, 3);
//            return result.Cast<T>().ToList();
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error executing query : {ex.Message}");
//            return null;
//        }
//    }
//    private async Task<IEnumerable<T>> ExecuteQueryWithRetryAsync<T>(EntityQuery<T> query, int maxRetryAttempts)
//    {
//        int retryAttempt = 0;

//        while (retryAttempt < maxRetryAttempts)
//        {
//            try
//            {
//                var result = await _entityManager.ExecuteQuery(query);
//                return result.Cast<T>().ToList();
//            }
//            catch (Exception ex) when (ex.Message.Contains("unauthorized"))
//            {
//                retryAttempt++;
//                Console.WriteLine($"Unauthorized error detected. Attempt {retryAttempt} of {maxRetryAttempts}. Refreshing token...");

//                if (retryAttempt >= maxRetryAttempts)
//                {
//                    throw new UnauthorizedAccessException("Maximum retry attempts reached. Failed to refresh token.");
//                }

//                // Refresh the token and retry the request
//                var newTokens = await _tokenService.RefreshToken(await _tokenService.GetRTokenAsync());
//                if (!string.IsNullOrEmpty(newTokens?.Atoken))
//                {
//                    await _tokenService.StoreStokensAsync(newTokens.Atoken, newTokens.Rtoken);

//                    // Set the new authorization header
//                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newTokens.Atoken);
//                    Console.WriteLine("Retrying request with new authorization header.");
//                }
//                else
//                {
//                    throw new UnauthorizedAccessException("Failed to refresh token.");
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error executing query: {ex.Message}");
//                return null;
//            }
//        }

//        return null; // Return null if all retry attempts fail
//    }
//}
//public class MetadataService
//{
//    private readonly IHttpClientFactory _httpClientFactory;
//    private static string _cachedMetadata;
//    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

//    public MetadataService(IHttpClientFactory httpClientFactory)
//    {
//        _httpClientFactory = httpClientFactory;
//    }

//    public async Task<string> GetMetadataAsync(string serviceName)
//    {
//        Console.WriteLine($"CALLED //GetMetadataAsync : {serviceName}");

//        if (!string.IsNullOrEmpty(_cachedMetadata))
//        {
//            return _cachedMetadata;
//        }

//        await _semaphore.WaitAsync();

//        try
//        {
//            if (!string.IsNullOrEmpty(_cachedMetadata))
//            {
//                return _cachedMetadata;
//            }
//            Console.WriteLine("METADATA IS CALLED ONCE");
//            var httpClient = _httpClientFactory.CreateClient(serviceName);
//            var response = await httpClient.GetAsync("lgbreeze/gxmetadata");
//            response.EnsureSuccessStatusCode();
//            _cachedMetadata = await response.Content.ReadAsStringAsync();
//            Console.WriteLine($"METADATA GOT ONCE : {_cachedMetadata.Length}");
//            return _cachedMetadata;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Failed to get Metadata: {ex.Message}");
//            return null; // Consider returning null instead of an empty string
//        }
//        finally
//        {
//            _semaphore.Release();
//        }
//    }
//}
//public class BzDataService : DataService
//{
//    public BzDataService(string serviceName)
//        : base(serviceName)
//    {

//    }
//    public new HttpClient httpClient
//    {
//        get => base.HttpClient;
//        set => SetHttpClient(value);
//    }
//    private void SetHttpClient(HttpClient httpClient)
//    {
//        base.HttpClient = httpClient;
//    }
//}
//public class MyDataService : DataService
//{
//    private readonly HttpClient _httpClient;
//    private EntityManager _entityManager;
//    private readonly ITokenService _tokenService;

//    public MyDataService(HttpClient httpClient, string serviceName, EntityManager entityManager, ITokenService tokenService)
//        : base(serviceName)
//    {
//        _httpClient = httpClient;
//        _entityManager = entityManager;
//        _tokenService = tokenService;
//    }

//    public void SetEntityManager(EntityManager entityManager)
//    {
//        _entityManager.DataService. = entityManager;
//    }

//    public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(EntityQuery<T> query)
//    {
//        Console.WriteLine("MyDataService: GO FOR EXECUTION");
//        // Ensure the authorization header is set for the request
//        await EnsureAuthorizationHeaderAsync();
//        // Verify the authorization header
//        Console.WriteLine($"Authorization header in ExecuteQueryAsync: {_httpClient.DefaultRequestHeaders.Authorization}");
//        // Retry logic to handle unauthorized error
//        return await ExecuteQueryWithRetryAsync(query, 3); // Retry up to 3 times
//    }

//    // EnsureAuthorizationHeaderAsync and other methods remain unchanged
//    private async Task EnsureAuthorizationHeaderAsync()
//    {
//        //var request = new HttpRequestMessage(HttpMethod.Get, _httpClient.BaseAddress);
//        //foreach (var header in _httpClient.DefaultRequestHeaders)
//        //{
//        //    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
//        //}

//        //if (_httpClient.DefaultRequestHeaders.Authorization == null)
//        //{
//        //}
//        var accessToken = await _tokenService.GetATokenAsync();
//        if (!string.IsNullOrEmpty(accessToken))
//        {
//            _httpClient.DefaultRequestHeaders.Authorization =
//                new AuthenticationHeaderValue("Bearer", accessToken);
//            Console.WriteLine($"1Authorization Header Manually Set: Bearer {accessToken}");
//        }
//    }
//    private async Task<IEnumerable<T>> ExecuteQueryWithRetryAsync<T>(EntityQuery<T> query, int maxRetryAttempts)
//    {
//        int retryAttempt = 0;

//        while (retryAttempt < maxRetryAttempts)
//        {
//            try
//            {
//                var result = await _entityManager.ExecuteQuery(query);
//                return result.Cast<T>().ToList();
//            }
//            catch (Exception ex) when (ex.Message.Contains("unauthorized"))
//            {
//                retryAttempt++;
//                Console.WriteLine($"Unauthorized error detected. Attempt {retryAttempt} of {maxRetryAttempts}. Refreshing token...");

//                if (retryAttempt >= maxRetryAttempts)
//                {
//                    throw new UnauthorizedAccessException("Maximum retry attempts reached. Failed to refresh token.");
//                }

//                // Refresh the token and retry the request
//                var newTokens = await _tokenService.RefreshToken(await _tokenService.GetRTokenAsync());
//                if (!string.IsNullOrEmpty(newTokens?.Atoken))
//                {
//                    await _tokenService.StoreStokensAsync(newTokens.Atoken, newTokens.Rtoken);

//                    // Set the new authorization header
//                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newTokens.Atoken);
//                    Console.WriteLine("Retrying request with new authorization header.");
//                }
//                else
//                {
//                    throw new UnauthorizedAccessException("Failed to refresh token.");
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error executing query: {ex.Message}");
//                return null;
//            }
//        }

//        return null; // Return null if all retry attempts fail
//    }
//}
//public async Task<IEnumerable<Tiersp>> ExecuteQueryAsync7(EntityQuery<Tiersp> query)
//{
//    Console.WriteLine("IS CALLED");
//    try
//    {
//       //Serialize the EntityQuery into a JSON string
//       var jsonQuery = JsonSerializer.Serialize(query); // Adjust as needed for proper serialization

//        // Execute the request using MyDataService's ExecuteRequestAsync method
//        var response = await _dataService.ExecuteRequestAsync(HttpMethod.Get, "tiersaie", jsonQuery);

//        // Deserialize response content into your entity type
//        return JsonSerializer.Deserialize<IEnumerable<Tiersp>>(response);
//    }
//      catch(Exception ex)
//    {
//        Console.WriteLine($"ECHEC DESERZ : {ex.Message}");
//    }
//    return null;
//}
//public async Task<IEnumerable<Tiersp>> ExecuteQueryAsync(EntityQuery<Tiersp> query)
//{
//    int niv = 0;
//    try
//    {
//        var asi = query.Filters()
//        // Serialize the EntityQuery into a JSON string
//        //var jsonQuery = JsonSerializer.Serialize(query); // Adjust as needed for proper serialization
//        var jsonQuery = JsonSerializer.Serialize(query); // new
//        niv = 1;
//        var response = await _dataService.ExecuteRequestAsync(HttpMethod.Post, "tiersaie", jsonQuery);
//        niv = 2;
//        return JsonSerializer.Deserialize<IEnumerable<Tiersp>>(response);
//    }
//    catch(Exception ex)
//    {
//        Console.WriteLine($"exception : {ex.Message} niv : {niv}");
//    }
//    // Deserialize response content into your entity type
//    return null;
//}

//public async Task<IEnumerable<Tiersp>> ExecuteQueryAsync(EntityQuery<Tiersp> query)
//{
//    // Convert EntityQuery to a URI-compatible format
//    var requestUri = $"{query.ResourceName}?{query.ToODataUri()}"; // Assuming you have a method to convert to OData URI

//    // Execute the request using MyDataService's ExecuteRequestAsync method
//    var response = await _dataService.ExecuteRequestAsync(HttpMethod.Get, requestUri);

//    // Deserialize response content into your entity type
//    return JsonSerializer.Deserialize<IEnumerable<Tiersp>>(response);
//}
//public async Task<IEnumerable<Tiersp>> ExecuteQueryAsync(EntityQuery<Tiersp> query)
//{
//    Console.WriteLine("IS CALLED");
//    try
//    {
//        // Serialize the query to JSON format expected by Breeze
//        var jsonQuery = JsonSerializer.Serialize(query); // Adjust as needed for proper serialization
//        Console.WriteLine("IS CALLED2");
//        // Execute the request using MyDataService's ExecuteRequestAsync method
//        var response = await _dataService.ExecuteRequestAsync(HttpMethod.Get, "tiersaie", null, jsonQuery);
//        Console.WriteLine("IS CALLED3");
//        // Deserialize response content into your entity type
//        return JsonSerializer.Deserialize<IEnumerable<Tiersp>>(response);
//    }
//    catch(Exception ex)
//    {
//        Console.WriteLine($"ECHEC DESERZ : {ex.Message}");
//    }
//    return null;
//}

//public async Task<string> ExecuteApiRequestAsync(HttpMethod method, string resourceName, string queryString = null, string requestBody = null)
//{
//    Console.WriteLine("MARCHE...");
//    return await _dataService.ExecuteRequestAsync(method, resourceName, queryString, requestBody);
//}
//public async Task<string> ExecuteRequestAsync(HttpMethod method, string resourceName, string queryString = null, string requestBody = null)
//{
//    Console.WriteLine("DataService is called");
//    var requestUri = new Uri(new Uri(ServiceName), resourceName + (string.IsNullOrEmpty(queryString) ? "" : "?" + queryString));
//    var request = new HttpRequestMessage(method, requestUri);

//    if (!string.IsNullOrEmpty(requestBody))
//    {
//        Console.WriteLine($"RequestBody Before: {requestBody}");
//        request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
//        Console.WriteLine($"RequestBody After : {request.Content}");
//    }
//    try
//    {
//        var response = await _httpClient.SendAsync(request);
//        Console.WriteLine($" dataservice response1 : {response}");
//        if (!response.IsSuccessStatusCode)
//        {
//            var errorContent = await response.Content.ReadAsStringAsync();
//            Console.WriteLine($"Error response content: {errorContent}");
//            response.EnsureSuccessStatusCode(); // This will throw an exception
//        }
//        Console.WriteLine($" dataservice response2 : {response}");
//        var result = await response.Content.ReadAsStringAsync();
//        return result;
//    } catch(Exception ex)
//    {
//        Console.WriteLine($"the issue here : {ex.Message}");
//        Console.WriteLine($"inner : {ex.InnerException} trace: {ex.StackTrace}");
//    }
//    return null;
//}
//public class BzEntityManagerFactory
//{
//    private readonly IHttpClientFactory _httpClientFactory;
//    private readonly MetadataService _metadataService;
//    private readonly ITokenService _tokenService;
//    public BzEntityManagerFactory(
//        IHttpClientFactory httpClientFactory, MetadataService metadataService,
//        ITokenService tokenService)
//    {
//        _httpClientFactory = httpClientFactory;
//        _metadataService = metadataService;
//        _tokenService = tokenService;
//    }
//    public async Task<BzEntityManager> CreateAsync()
//    {
//        return await BzEntityManager.CreateAsync(
//           "BZEClient",
//           _httpClientFactory,
//           _metadataService,
//           _tokenService
//        );
//    }
//}
//public static class QueryExtensions
//{
//    //public static string ToODataQueryString<T>(this EntityQuery<T> query)
//    //{
//    //    var uriBuilder = new UriBuilder(query.ResourceName);
//    //    var queryParams = HttpUtility.ParseQueryString(string.Empty);
//    //    foreach(var filter in query.To)
//    //    //Use Breeze built-in
//    //    return query.ToODataQueryString();
//    //}
//}
//method, resourceName, queryString, requestBody
//    public async Task<string> ExecuteRequestAsync(HttpMethod method, string resourceName, string jsonQuery = null, string requestBody = null)
//    {
//        try
//        {
//            Console.WriteLine("PAS1 - Depart");
//            // Construct the request URI
//            var requestUri = new Uri(new Uri(ServiceName), resourceName);
//            var request = new HttpRequestMessage(method, requestUri)
//            {
//                Content = new StringContent(jsonQuery, Encoding.UTF8, "application/json")
//            };
//            Console.WriteLine("PAS2");
//            // Create the   if (!string.IsNullOrEmpty(requestBody))
//            if (!string.IsNullOrEmpty(requestBody))
//            {
//                Console.WriteLine($"RequestBody Before: {requestBody}");
//                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
//            }
//            Console.WriteLine("PAS2");
//            var response = await _httpClient.SendAsync(request);
//            Console.WriteLine($"DataService response1: {response}");

//            if (!response.IsSuccessStatusCode)
//            {
//                var errorContent = await response.Content.ReadAsStringAsync();
//                Console.WriteLine($"Error response content: {errorContent}");
//                response.EnsureSuccessStatusCode();
//            }

//            var result = await response.Content.ReadAsStringAsync();
//            return result;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"The issue here: {ex.Message}");
//            return null;
//        }
//    }
//}
//public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(EntityQuery<T> query)
//{
//    try
//    {
//        // Construct the full request URL, including any filters from the query
//        var requestUri = $"{query.ResourceName}{query.QueryString}";

//        var response = await _httpClient.GetAsync(requestUri);
//        response.EnsureSuccessStatusCode();

//        return await response.Content.ReadFromJsonAsync<IEnumerable<T>>();
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Error executing query: {ex.Message}");
//        return null; // Or handle according to your error management strategy
//    }
//}

//public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(EntityQuery<T> query)
//{
//    try
//    {
//        // Construct the full request URL
//        var requestUri = $"{query.ResourceName}{query.QueryString}";

//        var response = await _httpClient.GetAsync(requestUri);
//        response.EnsureSuccessStatusCode();

//        return await response.Content.ReadFromJsonAsync<IEnumerable<T>>();
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Error executing query: {ex.Message}");
//        return null; // Or handle according to your error management strategy
//    }
//}
//}
//public class MetadataService
//{
//    private readonly IHttpClientFactory _httpClientFactory;
//    private static string _cachedMetadata;
//    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

//    public MetadataService(IHttpClientFactory httpClientFactory)
//    {
//        _httpClientFactory = httpClientFactory;
//    }

//    public async Task<string> GetMetadataAsync()
//    {
//        Console.WriteLine("CALLED //GetMetadataAsync");

//        if (!string.IsNullOrEmpty(_cachedMetadata))
//        {
//            return _cachedMetadata;
//        }

//        await _semaphore.WaitAsync();

//        try
//        {
//            if (!string.IsNullOrEmpty(_cachedMetadata))
//            {
//                return _cachedMetadata;
//            }

//            var httpClient = _httpClientFactory.CreateClient("LGClient");
//            var response = await httpClient.GetAsync("lgbreeze/Metadata");
//            response.EnsureSuccessStatusCode();

//            _cachedMetadata = await response.Content.ReadAsStringAsync();
//            Console.WriteLine($"METADATA GOT : {_cachedMetadata.Length}");

//            return _cachedMetadata;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Failed to get Metadata: {ex.Message}");
//            return null; // Consider returning null instead of an empty string
//        }
//        finally
//        {
//            _semaphore.Release();
//        }
//    }
//}
//public class EntityQuery<T>
//{
//    public string ResourceName { get; set; }
//    public string QueryString { get; set; } // Add this property
//    public EntityQuery(string resourceName)
//    {
//        ResourceName = resourceName;
//        QueryString = string.Empty; // Initialize it
//    }
//    public EntityQuery<T> Where(Func<T, bool> predicate)
//    {
//        // Implement logic to build the query string based on the predicate
//        // For example, you might convert the predicate to a query string format
//        // This is just a basic example and may need to be adjusted based on your needs
//        QueryString = $"?filter={predicate}"; // This is a placeholder; implement as needed
//        return this;
//    }
//    // You can add more methods for other query operations (e.g., OrderBy, Select)
//}
//public class BzHttpClieFactory : DelegatingHandler //HttpClientHandler
//{
//    private readonly ITokenService _tokenService;
//    public BzHttpClieFactory(ITokenService tokenService)
//    {
//        _tokenService = tokenService;
//    }
//    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//    {   
//        Console.WriteLine("CALLED //BzClientFactory");
//        var token = await _tokenService.GetATokenAsync();
//        if (!string.IsNullOrEmpty(token))
//        {
//            Console.WriteLine($"HEADER INSTALLE : {token}");
//            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
//        }
//        Console.WriteLine($"ClieFactory TOKEN set : {request.Headers.Authorization}");
//        return await base.SendAsync(request, cancellationToken);
//    }
//}
// //public class BzEntityManager
//{
//    private readonly EntityManager _entityManager;
//    private readonly DataService _dataService;
//    public BzEntityManager(EntityManager entityManager, DataService dataService)
//    {
//        _entityManager = entityManager;
//        _dataService = dataService;
//    }
//    public static async Task<BzEntityManager> CreateAsync(string serviceName,
//        IHttpClientFactory httpClientFactory, MetadataService metadataService,
//        ITokenService tokenService)
//    {
//        Console.WriteLine("2CALLED //BzEntityManager-CreateAsync");
//        var httpClient = httpClientFactory.CreateClient(serviceName); //BZEClient
//        Console.WriteLine($"baseAdr : {httpClient.BaseAddress}");
//        // Use httpClient's base address to avoid duplication of 'lgbreeze'
//        var baseAddress = httpClient.BaseAddress.ToString() + "lgbreeze";
//        Console.WriteLine($"baseAdr2 : {baseAddress}");
//        var dataService = new MyDataService(httpClient, baseAddress);// baseAddress, tokenService);
//        Console.WriteLine($"dataServi : {dataService.ServiceName}");
//        var entityManager = new EntityManager(dataService);
//        string hshead = GetAuthorizationHeader(httpClient);
//        Console.WriteLine($" auth text: {hshead}");
//        // Fetch metadata using the MetadataService
//        var cachedMetadata = await metadataService.GetMetadataAsync();
//        Console.WriteLine($"3Metadata : {cachedMetadata.Length}");
//        // Import cached metadata into the entity manager
//        entityManager.MetadataStore.ImportMetadata(cachedMetadata);
//        return new BzEntityManager(entityManager, dataService);
//    }
//    private static string GetAuthorizationHeader(HttpClient httpClient)
//    {
//        if (httpClient.DefaultRequestHeaders.Authorization != null)
//        {
//            return httpClient.DefaultRequestHeaders.Authorization.ToString();
//        }
//        return null; // Or return a default value
//    }
//    public async Task<IEnumerable<T>> ExecuteQuery<T>(EntityQuery<T> query)
//    {
//        //var queryResult = await _dataService.ExecuteRequestAsync(HttpMethod.Get, query.ResourceName, query.QueryString);
//        try
//        {
//            var asi = _entityManager.MetadataStore.GetDataService;
//            Console.WriteLine($"DataService : {asi}");
//            Console.WriteLine($"autres {_entityManager.DataService.ServiceName}");
//            Console.WriteLine($"nb entities : {_entityManager.GetEntities().Count()}");

//            var resu = await _entityManager.ExecuteQuery(query);
//            Console.WriteLine($"resultat : {resu}");
//            //return await _entityManager.ExecuteQuery(query);
//            return resu;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"exception//mesg : {ex.Message} trace : {ex.StackTrace}");
//            return null;
//        }
//    }
//}
//public class MetadataService
//{
//    private readonly IHttpClientFactory _httpClientFactory;
//    private static string _cachedMetadata;
//    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
//    public MetadataService(IHttpClientFactory httpClientFactory)
//    {
//        _httpClientFactory = httpClientFactory;
//    }
//    public async Task<string> GetMetadataAsync()
//    {
//        Console.WriteLine("CALLED //GetMetadataAsync");
//        // Return cached metadata if available
//        if (!string.IsNullOrEmpty(_cachedMetadata))
//        {
//            return _cachedMetadata;
//        }
//        await _semaphore.WaitAsync();
//        try
//        {
//            // Double-check if the metadata was fetched while waiting
//            if (!string.IsNullOrEmpty(_cachedMetadata))
//            {
//                return _cachedMetadata;
//            }
//            var httpClient = _httpClientFactory.CreateClient("LGClient");
//            var response = await httpClient.GetAsync("lgbreeze/Metadata");
//            response.EnsureSuccessStatusCode();
//            _cachedMetadata = await response.Content.ReadAsStringAsync();
//            Console.WriteLine($"METADATA GOT : {_cachedMetadata.Length}");
//            return _cachedMetadata;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"fail to get Metadata : {ex.Message}");
//            return "";
//        }
//        finally
//        {
//            _semaphore.Release();
//        }
//    }
//}
//public async Task<string> ExecuteRequestAsync8(HttpMethod method, string resourceName, string jsonQuery = null) //queryString = null, string requestBody = null)
//{
//    int niv = 0;
//    try
//    {
//        var requestUri = new Uri(new Uri(ServiceName), resourceName);

//        //if (method == HttpMethod.Get && !string.IsNullOrEmpty(queryString))
//        //{
//        //    requestUri = new Uri($"{requestUri}?{queryString}");
//        //}
//        niv = 21;

//        var request = new HttpRequestMessage(method, requestUri);
//        //{
//        //    Content = method != HttpMethod.Get ? new StringContent(requestBody ?? "", Encoding.UTF8, "application/json") : null
//        //};
//        niv = 22;
//        if (!string.IsNullOrEmpty(jsonQuery))
//        {
//            request.Content = new StringContent(jsonQuery, Encoding.UTF8, "application/json");
//        }
//        niv = 23;
//        var response = await _httpClient.SendAsync(request);
//        niv = 24;
//        response.EnsureSuccessStatusCode();
//        niv = 25;
//        return await response.Content.ReadAsStringAsync();
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"The issue here: {ex.Message} niv: /{niv}");
//        return null;
//    }
//}
//public class BzMessageHandler : DelegatingHandler
//{
//    private readonly ITokenService _tokenService;
//    public BzMessageHandler(ITokenService tokenService)
//    {
//        _tokenService = tokenService;
//    }
//    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//    {
//        var token = await _tokenService.GetATokenAsync();
//        if (!string.IsNullOrEmpty(token))
//        {
//            request.Headers.Authorization =
//                new AuthenticationHeaderValue("Bearer", token);
//        }
//        Console.WriteLine($"Message HANDLER : {token}");
//        return await base.SendAsync(request, cancellationToken);
//    }
//}
//}

//public class BzEntityManagerFactory
//{
//    private readonly IHttpClientFactory _httpClientFactory;
//    private readonly ITokenService _tokenService;

//    public BzEntityManagerFactory(IHttpClientFactory httpClientFactory, ITokenService tokenService)
//    {
//        Console.WriteLine("CALLED //BzEntityManagerFactory");
//        _httpClientFactory = httpClientFactory;
//        _tokenService = tokenService;
//    }
//}

//public class MyDataService : DataService
//{
//    private readonly HttpClient _httpclient;
//    public MyDataService(HttpClient httpclient, string serviceName)
//        : base(serviceName)
//    {
//        _httpclient = httpclient;
//    }
//    //public override async Task<string> GetAsync(string uri)
//    //{
//    //    var response = await _httpclient.GetAsync(uri);
//    //    response.EnsureSuccessStatusCode();
//    //    return await response.Content.ReadAsStringAsync();
//    //}
//    //public override async Task<string> PostAsync(string uri, string json)
//    //{
//    //    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
//    //    var response = await _httpclient.PostAsync(uri, content);
//    //    response.EnsureSuccessStatusCode();
//    //    return await response.Content.ReadAsStringAsync();
//    //}
//    public Task<HttpResponseMessage> HttpRequestAsync(HttpMethod method,
//        string requestUri, string queryString = null, string requestBody = null, bool isJson = false)
//    {
//        var fullUri = new Uri(new Uri(ServiceName), requestUri + (string.IsNullOrEmpty(queryString) ? "" : "?" + queryString));
//        var request2 = new HttpRequestMessage(method, fullUri);
//        if (!string.IsNullOrEmpty(requestBody))
//        {
//            request2.Content = new StringContent(requestBody,
//                System.Text.Encoding.UTF8, isJson ? "application/json" : "application/x-www-form-urlencoded");
//        }
//        return _httpclient.SendAsync(request2);
//    }
//    public async Task<string> ExecuteRequestAsync(HttpMethod method,
//        string resourceName, string queryString = null, string requestBody = null) //Dictionary<string, string> headers = null)
//{
//    Console.WriteLine("CALLED //DataService");
//    var requestUri = new Uri(new Uri(ServiceName), resourceName + (string.IsNullOrEmpty(queryString) ? "" : "?" + queryString));
//    var request = new HttpRequestMessage(method, requestUri);
//    if (!string.IsNullOrEmpty(requestBody))
//    {
//        request.Content = new StringContent(requestBody,
//            Encoding.UTF8, "application/json");
//    }
//    Console.WriteLine($"REQUEST HEADER : {string.Join(", ", request.Headers)}");
//    var response = await _httpclient.SendAsync(request);
//    response.EnsureSuccessStatusCode();
//    return await response.Content.ReadAsStringAsync();
//}
//}

//}
//protected override Task<HttpResponseMessage> HttpRequestAsync(HttpRequestMessage requestMessage)
//{
//    return _httpclient.SendAsync(requestMessage);
//}
//protected override string GetResponse(HttpResponseMessage responseMessage)
//{
//    return responseMessage.Content.ReadAsStringAsync().Result;
//}
//protected override void CheckResponse(HttpResponseMessage responseMessage)
//{
//    responseMessage.EnsureSuccessStatusCode();
//}
//protected override HttpClient CreateHttpClient()
//{
//    return _httpclient;
//}
//}
////private async Task FetchAndSetMetadata(ITokenService tokenService)
////{
////    try
////    {
////        var token = await tokenService.GetATokenAsync();
////        if (!String.IsNullOrEmpty(token))
////        {
////            _beezClient.DefaultRequestHeaders.Authorization
////                = new AuthenticationHeaderValue("Bearer", token);
////            var response = await _beezClient.GetAsync($"{_baseAddress}/Metadata");
////            response.EnsureSuccessStatusCode();
////            var metadata = await response.Content.ReadAsStringAsync();
////            SetMetadata(metadata);
////        }
////    } catch(Exception ex)
////    {
////        Console.WriteLine($"Error fetching Metadata: {ex.Message}");
////    }
////}
////private void SetMetadata(string metadata)
////{
////    try 
////    {
////        //assuming Metadata is in JSON format
////        Console.WriteLine("Metadata set successfully");
////    }catch(Exception ex){
////        Console.WriteLine($"Error setting metadata : {ex.Message}");
////    }
////}
// Fetch metadata only if it hasn't been cached

//if (string.IsNullOrEmpty(_cacheMetadata))
//{
//    await _semaphore.WaitAsync(); // Wait to enter the semaphore
//    try
//    {
//        if (string.IsNullOrEmpty(_cacheMetadata)) // Double-check locking
//        {
//            Console.WriteLine($"Fetching metadata...");
//            var response = await httpClient.GetAsync("lgbreeze/Metadata");
//            response.EnsureSuccessStatusCode();
//            _cacheMetadata = await response.Content.ReadAsStringAsync();
//            Console.WriteLine($"Metadata length: {_cacheMetadata.Length}");
//        }
//    }
//    finally
//    {
//        _semaphore.Release(); // Ensure the semaphore is released
//    }
//}
//////public async Task<BzEntityManager> CreateBzEntityManagerAsync()
//{
//    return await BzEntityManager.CreateAsync("ASClient", _httpClientFactory, _tokenService);
//}
//}
//public class DataService
//{
//    private readonly BzEntityManager _bzEntityManager;
//    public DataService(BzEntityManager bzEntityManager)
//    {
//        _bzEntityManager = bzEntityManager;
//    }
//    public async Task<IEnumerable<Tiersp>> GetTiersAsync(int _UorgId)
//    { 
//        var query = new EntityQuery<Tiersp>("tiersaie")
//            .Where(p =>
//                p.Idorg == _UorgId);
//        return await entityManager.ExecuteQuery(query); 
//    }
//}
//public class BzEntityManager
//{
//    private readonly EntityManager _entityManager;
//    public BzEntityManager(EntityManager entityManager)
//    {
//        _entityManager = entityManager;
//    }

//    public static async Task<BzEntityManager> CreateAsync(string serviceName, IHttpClientFactory httpfactclient, ITokenService tokenService)
//    {
//        try
//        {
//            //var token = await tokenService.GetATokenAsync(); // Await the token retrieval
//            //Console.WriteLine("before CreateClient"); //NULL
//            var httpclient = httpfactclient.CreateClient(serviceName);

//            var token = await tokenService.GetATokenAsync(); // Await the token retrieval
//            Console.WriteLine($"Token received for Etm: {token}");
//            //set the header
//            if (!string.IsNullOrEmpty(token))
//            {
//                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
//                Console.WriteLine("Token has been set");
//            }
//            else
//            {
//                Console.WriteLine("No token found. Authorization header will not be set.");
//            }
//            // Initialize EntityManager with the base address of the HttpClient
//            // Initialize EntityManager with the base address of the HttpClient
//            var baseAddress = httpclient.BaseAddress.ToString(); // Get the base address as a string
//            baseAddress = baseAddress + "lgbreeze";
//            var entityManager = new EntityManager(baseAddress);
//            try
//            {
//                await entityManager.FetchMetadata();
//                //Console.WriteLine("Trying to fetch metadata...");
//                //var dataService = await entityManager.FetchMetadata(); // This should return a string
//                //if (dataService != null)
//                //{
//                //    var jsonMetadata = dataService.ServerMetadata;
//                //    // Ensure jsonMetadata is a string
//                //    if (jsonMetadata is string metadataString)
//                //    {
//                //        Console.WriteLine("Got metadata successfully.");
//                //        // Import the fetched metadata into the MetadataStore
//                //        //entityManager.MetadataStore.ImportMetadata(metadataString); // Pass as string
//                //        Console.WriteLine("Imported metadata into MetadataStore.");
//                //    }
//                //    else
//                //    {
//                //        Console.WriteLine("Fetched metadata is not in the expected string format.");
//                //    }
//                //}
//                return new BzEntityManager(entityManager); // Pass the base address
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Error fetching or importing metadata: " + ex.Message);
//                //Console.WriteLine(ex.StackTrace);
//                throw;
//            }
//            //NamingConvention.camelCase.setAsDefault()
//            Console.WriteLine("Return Successfully Manager");
//            //return new BzEntityManager(entityManager); // Pass the base address
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error in BzEntityManager: {ex.Message}");
//            //Console.WriteLine($"Trace : {ex.StackTrace}");
//            //throw; // Optionally rethrow or handle as needed
//            return null;
//        }
//    }
//    public EntityManager GetEntityManager() => _entityManager;
//}
//public class BzEntityManager
//{
//    private readonly EntityManager _entityManager;

//    private BzEntityManager(EntityManager entityManager)
//    {
//        _entityManager = entityManager;
//    }
//var nwetm = new EntityManager(baseAddress);
//Console.WriteLine("try fetch metadata ");
//var jsonMetadata = await nwetm.FetchMetadata(); // Fetch metadata from server
//Console.WriteLine("Got metadata successfully.");

//// Import the fetched metadata into the MetadataStore
//nwetm.MetadataStore.ImportMetadata(jsonMetadata);
//Console.WriteLine("Trying to fetch metadata...");
//var metadata = await nwetm.FetchMetadata(); // Fetch metadata from server
//Console.WriteLine("Got metadata successfully.");

//// Import the fetched metadata into the MetadataStore
//nwetm.MetadataStore.ImportMetadata(metadata); // Ensure 'metadata' is in correct format
//Console.WriteLine("Imported metadata into MetadataStore.");

//Console.WriteLine("Imported metadata into MetadataStore.");

//var inmeta = await nwetm.FetchMetadata();
//Console.WriteLine("got metadata");
//Console.WriteLine($"the metadata : {inmeta}");
//var allents = nwetm.GetEntities();
//foreach(var uet in allents)
//{
//    uet.GetStructuralAspect().SetValue("Actdet", uet.GetType());
//    isu = 1;
//    var atyp = uet.GetStructuralAspect().StructuralType;
//    isu = 2;
//    Console.WriteLine($"Type1 : {atyp.ShortName} Namespace1 : {atyp.Namespace}");
//    isu = 3;
//    var vtyp = uet.GetType();
//    isu = 4;
//    var vstruct = nwetm.MetadataStore.GetStructuralType(vtyp);
//    isu = 5;
//    Console.WriteLine($"Type2 : {vstruct.ShortName} Namespace2 : {vstruct.Namespace}");
//}
//var wstype = nwetm.MetadataStore.GetStructuralType(typeof(Actdet));
//    public static async Task<BzEntityManager> CreateAsync(string serviceName, IHttpClientFactory httpfactclient, ITokenService tokenService)
//    {
//        try
//        {
//            var httpclient = httpfactclient.CreateClient(serviceName);
//            var token = await tokenService.GetATokenAsync(); // Await the token retrieval
//            Console.WriteLine($"Token received for Etm: {token}");

//            if (!string.IsNullOrEmpty(token))
//            {
//                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
//            }

//            // Initialize EntityManager with the HttpClient instance directly
//            return new BzEntityManager(new EntityManager(httpclient)); // Pass HttpClient instead of base address
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error in BzEntityManager: {ex.Message}");
//            throw; // Optionally rethrow or handle as needed
//        }
//    }

//    public EntityManager GetEntityManager() => _entityManager;
//}

//public class BzEntityManagerService
//{
//    private readonly IHttpClientFactory _httpClientFactory;
//    private readonly ITokenService _tokenService;

//    public BzEntityManagerService(IHttpClientFactory httpClientFactory, ITokenService tokenService)
//    {
//        _httpClientFactory = httpClientFactory;
//        _tokenService = tokenService;
//    }

//    public async Task<BzEntityManager> CreateBzEntityManagerAsync()
//    {
//        return await BzEntityManager.CreateAsync("ASClient", _httpClientFactory, _tokenService);
//    }
//}
//public class BzEntityManager
//{
//    private readonly EntityManager _entityManager;

//    public BzEntityManager(EntityManager entityManager)
//    {
//        _entityManager = entityManager;
//    }

//    public static async Task<BzEntityManager> CreateAsync(string serviceName, IHttpClientFactory httpfactclient, ITokenService tokenService)
//    {
//        try
//        {
//            var httpclient = httpfactclient.CreateClient(serviceName);
//            var token = await tokenService.GetATokenAsync(); // Await the token retrieval
//            Console.WriteLine($"Token received for Etm: {token}");

//            if (!string.IsNullOrEmpty(token))
//            {
//                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
//            }

//            // Initialize EntityManager with the base address of the HttpClient
//            return new BzEntityManager(new EntityManager(httpclient.BaseAddress.ToString()));
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error in BzEntityManager: {ex.Message}");
//            throw; // Optionally rethrow or handle as needed
//        }
//    }

//    public EntityManager GetEntityManager() => _entityManager;
//}

//public class BzEntityManager
//{
//    private readonly EntityManager _entityManager;
//    public BzEntityManager(string serviceName, IHttpClientFactory httpfactclient, ITokenService tokenService)
//    {
//        //try
//        //{
//            var httpclient = httpfactclient.CreateClient(serviceName);
//            var token = tokenService.GetATokenAsync().Result;
//            Console.WriteLine($"token recu pour Etm :", token);
//            if (!string.IsNullOrEmpty(token))
//            {
//                httpclient.DefaultRequestHeaders.Authorization
//                    = new AuthenticationHeaderValue("Bearer", token);
//            }
//            _entityManager = new EntityManager(httpclient.BaseAddress.ToString());

//        //}
//        //catch (Exception ex)
//        //{
//        //    Console.WriteLine($"erreur dans BzEntityManager : {ex.Message}");
//        //}
//    }
//    public EntityManager GetEntityManager() => _entityManager;
//}

//public MyDataService5(HttpClient httpClient,
//    string serviceName, ITokenService tokenService)
//    : base(serviceName)
//{
//    _httpClient = httpClient;
//    _tokenService = tokenService;
//    HasServerMetadata = true;
//    InitializeHttpClient();

//    IDataServiceAdapter
//    {

//    }
//    InitializeHttpClient(httpClient);
//}
//}
//protected override async void InitializeHttpClient(HttpClient httpClient)
//{
//    var token = await _tokenService.GetATokenAsync();
//    if (!string.IsNullOrEmpty(token))
//    {
//        httpClient.DefaultRequestHeaders.Authorization =
//            new AuthenticationHeaderValue("Bearer", token);
//    }
//}
//}
//public override async Task<string> GetAsync(string uri)
//{
//    var response = await _httpclient.GetAsync(uri);
//    response.EnsureSuccessStatusCode();
//    return await response.Content.ReadAsStringAsync();
//}
//public override async Task<string> PostAsync(string uri, string json)
//{
//    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
//    var response = await _httpclient.PostAsync(uri, content);
//    response.EnsureSuccessStatusCode();
//    return await response.Content.ReadAsStringAsync();
//}
//    public Task<HttpResponseMessage> HttpRequestAsync(HttpMethod method,
//        string requestUri, string queryString = null, string requestBody = null, bool isJson = false)
//    {
//        var fullUri = new Uri(new Uri(ServiceName), requestUri + (string.IsNullOrEmpty(queryString) ? "" : "?" + queryString));
//        var request2 = new HttpRequestMessage(method, fullUri);
//        if (!string.IsNullOrEmpty(requestBody))
//        {
//            request2.Content = new StringContent(requestBody,
//                System.Text.Encoding.UTF8, isJson ? "application/json" : "application/x-www-form-urlencoded");
//        }
//        return _httpclient.SendAsync(request2);
//    }
//    public async Task<string> ExecuteRequestAsync(HttpMethod method,
//        string resourceName, string queryString = null, string requestBody = null) //Dictionary<string, string> headers = null)
//    {
//        Console.WriteLine("CALLED //DataService");
//        var requestUri = new Uri(new Uri(ServiceName), resourceName + (string.IsNullOrEmpty(queryString) ? "" : "?" + queryString));
//        var request = new HttpRequestMessage(method, requestUri);
//        if (!string.IsNullOrEmpty(requestBody))
//        {
//            request.Content = new StringContent(requestBody,
//                Encoding.UTF8, "application/json");
//        }
//        Console.WriteLine($"REQUEST HEADER : {string.Join(", ", request.Headers)}");
//        var response = await _httpclient.SendAsync(request);
//        response.EnsureSuccessStatusCode();
//        return await response.Content.ReadAsStringAsync();
//    }
//}