using System.Net.Http.Headers;

using AutoMapper;

using BlazorDownloadFile;

using Blazored.LocalStorage;

using GxShared.GxGuards;
using GxShared.Helpers;
using GxShared.Interfaces;
using GxShared.Mapping;
using GxShared.Sess;

using GxStk;
using GxStk.Services;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Simple.OData.Client;

using TG.Blazor.IndexedDB;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// ----------------------------
// Configuration & URLs
// ----------------------------
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

var backendBase = builder.Configuration["BackendUrl"]?.TrimEnd('/') + "/"; var backUrlBrut = builder.Configuration["BackendUrl"] ?? throw new InvalidOperationException("BackendUrl missing");
var backendUrl = backUrlBrut.TrimEnd('/') + "/";

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ----------------------------
// Core Services
// ----------------------------
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazorBootstrap();
builder.Services.AddBlazorDownloadFile();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

// ----------------------------
// Auth
// ----------------------------
builder.Services.AddTransient<AuthDelegatingHandler>();
builder.Services.AddScoped<MyAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<MyAuthStateProvider>());

//// ----------------------------
//// Consolidated HttpClients (3)
//// ----------------------------
//builder.Services.AddHttpClient("AuthClient", client =>
//    client.BaseAddress = new Uri($"{backendBase}api/"))
//    .AddHttpMessageHandler<AuthDelegatingHandler>();

//builder.Services.AddHttpClient("ODataClient", client =>
//{
//    client.BaseAddress = new Uri($"{backendBase}odata/");
//    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//})
//.AddHttpMessageHandler<AuthDelegatingHandler>();

//builder.Services.AddHttpClient("LocalClient", client =>
//{
//    client.BaseAddress = new Uri($"{backendBase}api/");
//    client.DefaultRequestHeaders.Add("X-Requested-With", "Fetch");
//});

//// ----------------------------
//// Simple.OData.Client & IndexedDB
//// ----------------------------
//builder.Services.AddScoped<ODataClient>(sp =>
//{
//    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("ODataClient");
//    return new ODataClient(new ODataClientSettings(httpClient) { PayloadFormat = ODataPayloadFormat.Json });
//});

//builder.Services.AddScoped<IODataClientFactory>(sp => new DefaultODataClientFactory(new Uri($"{backendBase}odata/")));

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
// ----------------------------
// IndexedDB & Guards
// ----------------------------
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
// App Services
// ----------------------------
builder.Services.AddAutoMapper(typeof(SharedMappingProfile).Assembly);
builder.Services.AddScoped<Userbag>();
builder.Services.AddScoped<IPuzzleSyncService, PuzzleSyncService>();
builder.Services.AddScoped<ChildVars>();
builder.Services.AddScoped<RendAgres>();
builder.Services.AddScoped<ClieAppState>();
builder.Services.AddScoped<SessionContextService>();
builder.Services.AddScoped<SessionContextClient>();
builder.Services.AddScoped<HttpClientService>();
builder.Services.AddScoped<TokenAwareClientManager>();
builder.Services.AddScoped<TblJsonRender>();
builder.Services.AddScoped<LinkSerialiser>();
//builder.Services.AddScoped<PouchDbService>();
builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddSingleton<LoadingService>();

// ----------------------------
// Startup
// ----------------------------
var host = builder.Build();

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
//using Microsoft.Extensions.Http; // for IHttpClientFactory
//using System.Text.Json;

//using AutoMapper;

//using BlazorDownloadFile;

//using Blazored.LocalStorage;

//using GxShared.GxGuards;
//using GxShared.Helpers;
//using GxShared.Interfaces;
//using GxShared.Mapping;
//using GxShared.Sess;

//using GxStk;
//using GxShared.Helpers;
//using GxStk.Services;

//using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.AspNetCore.Components.Web;
//using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

//using Simple.OData.Client;
//using TG.Blazor.IndexedDB;

//var builder = WebAssemblyHostBuilder.CreateDefault(args);
//var backUrl = new Uri(builder.Configuration["BackendUrl"]);

//builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddBlazoredLocalStorage();
//// ✅ 2. Auth Handler (injects token automatically)
//builder.Services.AddTransient<AuthDelegatingHandler>();
////loading configuration in a json file
//builder.Services.AddSingleton(builder.Configuration);
//builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
//builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

//// Configure logging first
//builder.Logging.SetMinimumLevel(LogLevel.Information);
//// Register services
//builder.Services.AddHttpClient();
////builder.Services.AddTransient<CookieHandler>();

//builder.Services.AddBlazorBootstrap();
//builder.Services.AddBlazorDownloadFile();

//builder.Services.AddAuthorizationCore();
//builder.Services.AddCascadingAuthenticationState();
//builder.Services.AddScoped<MyAuthStateProvider>();
//builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<MyAuthStateProvider>());

////builder.Services.AddTransient<FormulaEvalService>();
//builder.Services.AddScoped<Userbag>();
////builder.Services.AddScoped<IGridActionService, GridActionService>();
//builder.Services.AddScoped<IPuzzleSyncService, PuzzleSyncService>();
////builder.Services.AddScoped<Usaibag>();
//builder.Services.AddScoped<MyShareVars>();
//builder.Services.AddScoped<RendAgres>();
//builder.Services.AddScoped<ClieAppState>();
//builder.Services.AddScoped<SessionContextService>();
//builder.Services.AddScoped<SessionContextClient>();
////builder.Services.AddScoped<LookupService>();

//builder.Services.AddSingleton<IMessageService, MessageService>();

//builder.Services.AddSingleton<LoadingService>();
//// Configure logging first
//builder.Logging.SetMinimumLevel(LogLevel.Information);
//// Register CLIENT SERVICES
//builder.Services.AddScoped<HttpClientService>();
//builder.Services.AddScoped<TokenAwareClientManager>();

//// Register named clients
////AuthClient
//builder.Services.AddHttpClient("AuthClient", client =>
//{
//    client.BaseAddress = new Uri($"{builder.Configuration["BackendUrl"]}/api/");
//    // Optional: add default headers like Accept if needed
//});
////OfflineCLIENT
//builder.Services.AddHttpClient("LocalClient", client =>
//{
//    var backUrl = builder.Configuration["BackendUrl"];
//    if (!backUrl.EndsWith("/"))
//    {
//        backUrl += "/";
//    }
//    backUrl += "api/";
//    client.BaseAddress = new Uri(backUrl);
//    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//    client.DefaultRequestHeaders.Add("X-Requested-With", "Fetch");
//});

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
//}); ;
//builder.Services.AddScoped<IPendingOpsStore, IndexedDbOpsStore>();
//builder.Services.AddScoped<IPendingChangesGuard, PendingChangesGuard>();


//// 🔥 1. HTTP CLIENT for parallel batch saves (ROOT base address)
//// Register named OData client
//builder.Services.AddHttpClient("ODataClient", client =>
//{
//    var backUrl = builder.Configuration["BackendUrl"];
//    if (!backUrl.EndsWith("/")) backUrl += "/";
//    backUrl += "odata/";   // ✅ Ensure /odata is part of the base address

//    client.BaseAddress = new Uri(backUrl);
//})
//.AddHttpMessageHandler<AuthDelegatingHandler>();
//// 🔥 2. OData Context Factory (loads token + creates context)
////builder.Services.AddScoped<IODataContextFactory, ODataContextFactory>();//ODataClient

//builder.Services.AddScoped<TblJsonRender>();
//builder.Services.AddScoped<LinkSerialiser>();

//builder.Services.AddHttpClient("DefaultClient", client =>
//{
//    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
//    client.DefaultRequestHeaders.Add("Accept", "application/json");
//});
//// test odata client ✅ CORRECT
//builder.Services.AddHttpClient("ODaTestClient", client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7095/");  // Root = /odata routes work
//});

//// Build the host
//var host = builder.Build();
//// Get webApi is online or not
//var localStorage = host.Services.GetRequiredService<ILocalStorageService>();
//var authProvider = host.Services.GetRequiredService<MyAuthStateProvider>();
//var reset = await localStorage.GetItemAsync<bool>("force-reset");
//if (reset)
//{
//    await localStorage.ClearAsync();
//    await authProvider.NotifyUserLogout();
//}
//await host.RunAsync();

// Stock services
//builder.Services.AddScoped<ItemClientService>();
//builder.Services.AddScoped<StockClientService>();
//builder.Services.AddScoped<PurchaseClientService>();
//builder.Services.AddScoped<SaleClientService>();
//pouch services 
//builder.Services.AddSingleton<PouchDbService>();