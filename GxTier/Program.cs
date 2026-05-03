using System.Net.Http.Headers;

using BlazorDownloadFile;

using Blazored.LocalStorage;

using GxShared.GxGuards;
using GxShared.Helpers;
using GxShared.Interfaces;
using GxShared.Mapping;
using GxShared.Sess;

using GxTie;
using GxTie.ClieModels;
using GxTie.Services;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Simple.OData.Client;

using TG.Blazor.IndexedDB;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// ----------------------------
// Configuration
// ----------------------------
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

var backUrlBrut = builder.Configuration["BackendUrl"] ?? throw new InvalidOperationException("BackendUrl missing");
var backendUrl = backUrlBrut.TrimEnd('/') + "/";

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ----------------------------
// Core Services
// ----------------------------
builder.Services.AddLogging(logging => logging.SetMinimumLevel(LogLevel.Information));
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazorBootstrap();
builder.Services.AddBlazorDownloadFile();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

// ----------------------------
// Auth
// ----------------------------
builder.Services.AddScoped<MyAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<MyAuthStateProvider>());
builder.Services.AddTransient<AuthDelegatingHandler>();

//// ------------------------------------------------
//// Consolidated HttpClients (3) Configured for Auth
//// ------------------------------------------------
//builder.Services.AddHttpClient("AuthClient", client =>
//    client.BaseAddress = new Uri($"{backendUrl}api/"))
//    .AddHttpMessageHandler<AuthDelegatingHandler>();

//builder.Services.AddHttpClient("ODataClient", client =>
//{
//    client.BaseAddress = new Uri($"{backendUrl}odata/");
//    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//})
//.AddHttpMessageHandler<AuthDelegatingHandler>();
//// Register a dedicated OData HttpClient
//builder.Services.AddHttpClient("LocalClient", client =>
//{
//    client.BaseAddress = new Uri($"{backendUrl}api/");
//    client.DefaultRequestHeaders.Add("X-Requested-With", "Fetch");
//});
//// Simple.OData.Client & IndexedDB
//builder.Services.AddScoped<ODataClient>(sp =>
//{
//    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("ODataClient");
//    return new ODataClient(new ODataClientSettings(httpClient) { PayloadFormat = ODataPayloadFormat.Json });
//});
//builder.Services.AddScoped<IODataClientFactory>(sp => new DefaultODataClientFactory(new Uri($"{backendUrl}odata/")));
// --------------------------------------------
// 1. Consolidated HttpClient Registrations (3)
// --------------------------------------------
builder.Services.AddHttpClient("AuthClient", client =>
    client.BaseAddress = new Uri($"{backendUrl}api/"))
    .AddHttpMessageHandler<AuthDelegatingHandler>();
builder.Services.AddHttpClient("ODataClient", client =>
{
    client.BaseAddress = new Uri($"{backendUrl}odata/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AuthDelegatingHandler>();
builder.Services.AddHttpClient("LocalClient", client =>
{
    client.BaseAddress = new Uri($"{backendUrl}api/");
    client.DefaultRequestHeaders.Add("X-Requested-With", "Fetch");
});
// ------------------------------------------------
// 2. Register Custom OData Client Factory
// ------------------------------------------------
// Register your custom implementation of the factory
builder.Services.AddScoped<IODataClientFactory>(sp =>
    new SimpleODataClientFactory(sp.GetRequiredService<IHttpClientFactory>()));
// Optional: If you want to inject ODataClient directly in some services,
// you can still register it as a scoped service using your factory
builder.Services.AddScoped<IODataClient>(sp =>
    sp.GetRequiredService<IODataClientFactory>().CreateClient());
// IndexedDB for pending operations
builder.Services.AddIndexedDB(db =>
{
    db.DbName = "PendingOpsDb";
    db.Version = 1;
    db.Stores.Add(new StoreSchema
    {
        Name = "PendingOps",
        PrimaryKey = new IndexSpec { Name = "id", Auto = true },
        Indexes = new List<IndexSpec> { new IndexSpec { Name = "entity", Unique = false } }
    });
});
builder.Services.AddScoped<IPendingOpsStore, IndexedDbOpsStore>();
builder.Services.AddScoped<IPendingChangesGuard, PendingChangesGuard>();

// ----------------------------
// App & Client Services
// ----------------------------
builder.Services.AddAutoMapper(typeof(SharedMappingProfile).Assembly);
builder.Services.AddScoped<Userbag>();
builder.Services.AddScoped<IPuzzleSyncService, PuzzleSyncService>();
builder.Services.AddScoped<MyShareVars>();
builder.Services.AddScoped<RendAgres>();
builder.Services.AddScoped<ClieAppState>();
builder.Services.AddScoped<SessionContextService>();
builder.Services.AddScoped<SessionContextClient>();
builder.Services.AddScoped<HttpClientService>();
builder.Services.AddScoped<TokenAwareClientManager>();
builder.Services.AddScoped<TblJsonRender>();
builder.Services.AddScoped<LinkSerialiser>();
builder.Services.AddScoped<PouchDbService>();
builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddSingleton<LoadingService>();

var host = builder.Build();

// ----------------------------
// Initialization
// ----------------------------
using (var scope = host.Services.CreateScope())
{
    var localStorage = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();
    var authProvider = scope.ServiceProvider.GetRequiredService<MyAuthStateProvider>();

    if (await localStorage.GetItemAsync<bool?>("force-reset") == true)
    {
        await localStorage.ClearAsync();
        await authProvider.NotifyUserLogout();
    }
}

await host.RunAsync();

public class ApiSettings
{
    public string BackendUrl { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = string.Empty;
    public string FrontendUrl { get; set; } = string.Empty;
    public string BackendUrl22 { get; set; } = string.Empty;
    public string ApiUrl22 { get; set; } = string.Empty;
    public string FrontendUrl22 { get; set; } = string.Empty;
}

//using System.Net.Http.Headers;
//using System.Text.Json;

//using BlazorDownloadFile;   
//using TG.Blazor.IndexedDB;
//using Blazored.LocalStorage;
//using GxShared.GxGuards;
//using GxShared.Helpers;
//using GxShared.Interfaces;
//using GxShared.Mapping;
//using GxShared.Sess;

//using GxTie;
//using GxTie.ClieModels;
//using GxTie.Services;

//using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.AspNetCore.Components.Web;
//using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

//using Simple.OData.Client;

//var builder = WebAssemblyHostBuilder.CreateDefault(args);

//// ✅ 1. Load config FIRST
//builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
//builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
//var backUrlStr = builder.Configuration["BackendUrl"] ?? throw new InvalidOperationException("BackendUrl missing");
//var backUrl = new Uri(backUrlStr);

//builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");

//// ✅ Core services (logging first for AutoMapper)
//builder.Services.AddLogging(logging => logging.SetMinimumLevel(LogLevel.Information));
//builder.Services.AddBlazoredLocalStorage();
//builder.Services.AddBlazorBootstrap();
//builder.Services.AddBlazorDownloadFile();
//builder.Services.AddAuthorizationCore();
//builder.Services.AddCascadingAuthenticationState();

//// ✅ Auth
//builder.Services.AddScoped<MyAuthStateProvider>();
//builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<MyAuthStateProvider>());
//builder.Services.AddTransient<AuthDelegatingHandler>();

//// ✅ 2. Consolidated HttpClients (3 total)
//builder.Services.AddHttpClient("BackendApi", client =>
//{
//    var apiUrl = backUrlStr.TrimEnd('/') + "/api/";
//    client.BaseAddress = new Uri(apiUrl);
//}).AddHttpMessageHandler<AuthDelegatingHandler>();

////AuthClient
//builder.Services.AddHttpClient("AuthClient", client =>
//{
//    client.BaseAddress = new Uri($"{builder.Configuration["BackendUrl"]}/api/");
//    // Optional: add default headers like Accept if needed
//})
//    .AddHttpMessageHandler<AuthDelegatingHandler>();

//builder.Services.AddHttpClient("OData", client =>
//{
//    var odataUrl = backUrlStr.TrimEnd('/') + "/odata/";
//    client.BaseAddress = new Uri(odataUrl);
//    client.DefaultRequestHeaders.Add("Accept", "application/json");
//}).AddHttpMessageHandler<AuthDelegatingHandler>();

//builder.Services.AddHttpClient("OfflineApi", client =>
//{
//    var offlUrl = backUrlStr.TrimEnd('/') + "/api/";
//    client.BaseAddress = new Uri(offlUrl);
//    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//    client.DefaultRequestHeaders.Add("X-Requested-With", "Fetch");
//});

//// ✅ 3. Your Scoped services (deduplicated)
//builder.Services.AddScoped<Userbag>();
//builder.Services.AddScoped<IPuzzleSyncService, PuzzleSyncService>();
//builder.Services.AddScoped<MyShareVars>();
//builder.Services.AddScoped<RendAgres>();
//builder.Services.AddScoped<ClieAppState>();
//builder.Services.AddScoped<SessionContextService>();
//builder.Services.AddScoped<SessionContextClient>();
//builder.Services.AddSingleton<IMessageService, MessageService>();
//builder.Services.AddScoped<TblJsonRender>();
//builder.Services.AddScoped<LinkSerialiser>();
//builder.Services.AddScoped<PouchDbService>();

//// AutoMapper
//builder.Services.AddAutoMapper(typeof(SharedMappingProfile).Assembly);
//// ODataClient direct registration
//builder.Services.AddScoped<ODataClient>(sp =>
//{
//    var backUrl = builder.Configuration["BackendUrl"];
//    if (!backUrl.EndsWith("/")) backUrl += "/";
//    backUrl += "odata/";
//    var settings = new ODataClientSettings(new Uri(backUrl))
//    {
//        PayloadFormat = ODataPayloadFormat.Json
//    };
//    return new ODataClient(settings);
//});
//// IODataClientFactory (shared interface)
//builder.Services.AddScoped<GxShared.Interfaces.IODataClientFactory>(sp =>
//{
//    var backUrl = builder.Configuration["BackendUrl"];
//    if (!backUrl.EndsWith("/")) backUrl += "/";
//    var baseUri = new Uri(backUrl);

//    return new DefaultODataClientFactory(baseUri);
//});
//// Guard INDEX DB
//builder.Services.AddIndexedDB(db =>
//{
//    db.DbName = "PendingOpsDb";
//    db.Version = 1;
//    db.Stores.Add(new StoreSchema
//    {
//        Name = "PendingOps",
//        PrimaryKey = new IndexSpec { Name = "id", Auto = true },
//        Indexes = new List<IndexSpec>
//        {
//            new IndexSpec { Name = "entity", Unique = false }
//        }
//    });
//});;
//builder.Services.AddScoped<IPendingOpsStore, IndexedDbOpsStore>();
//builder.Services.AddScoped<IPendingChangesGuard, PendingChangesGuard>();

//// ✅ 6. Client services
//builder.Services.AddScoped<HttpClientService>();
//builder.Services.AddScoped<TokenAwareClientManager>();

//// ✅ 7. AutoMapper (fixed resolver)
//builder.Services.AddSingleton<LoadingService>();
//// ✅ 8. Default client
//builder.Services.AddHttpClient("DefaultClient", client =>
//{
//    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
//    client.DefaultRequestHeaders.Add("Accept", "application/json");
//});
//// Build
//var host = builder.Build();

//// ✅ 9. FIXED post-build init (plain using, no await)
//using var scope = host.Services.CreateScope();
//var localStorage = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();
//var authProvider = scope.ServiceProvider.GetRequiredService<MyAuthStateProvider>();
//var reset = await localStorage.GetItemAsync<bool?>("force-reset") ?? false;
//if (reset)
//{
//    await localStorage.ClearAsync();
//    await authProvider.NotifyUserLogout();
//}

//await host.RunAsync();

//builder.Services.AddSingleton<MapperConfiguration>(sp =>
//{
//    var config = new MapperConfiguration(cfg =>
//    {
//        cfg.AddProfile<GxorgaProfile>();
//        cfg.AddProfile<PlngenProfile>();
//    });
//    config.AssertConfigurationIsValid();
//    return config;
//});
//builder.Services.AddSingleton<IMapper>(sp => sp.GetRequiredService<MapperConfiguration>().CreateMapper());

