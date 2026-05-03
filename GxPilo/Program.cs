using System.Net.Http.Headers;

using BlazorDownloadFile;

using Blazored.LocalStorage;

using GxPilo;
using GxPilo.ClieModels;
using GxPilo.Services;

using GxShared.GxGuards;
using GxShared.Helpers;
using GxShared.Interfaces;
using GxShared.Mapping;
using GxShared.Sess;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Simple.OData.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ----------------------------
// Configuration
// ----------------------------
builder.Services.AddSingleton(builder.Configuration);
var backUrlBrut = builder.Configuration["BackendUrl"] ?? throw new InvalidOperationException("BackendUrl missing");
var backendUrl = backUrlBrut.TrimEnd('/') + "/";

// ----------------------------
// Core Services
// ----------------------------
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazorBootstrap();
builder.Services.AddBlazorDownloadFile();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// ----------------------------
// Auth & Security
// ----------------------------
builder.Services.AddTransient<AuthDelegatingHandler>();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<MyAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<MyAuthStateProvider>());

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


//// ------------------------------------------------
//// Consolidated HttpClients (3) Configured for Auth
//// ------------------------------------------------
//builder.Services.AddHttpClient("AuthClient", client =>
//{
//    client.BaseAddress = new Uri($"{backendUrl}api/");
//})
//.AddHttpMessageHandler<AuthDelegatingHandler>();
//// Register a dedicated OData HttpClient
//builder.Services.AddHttpClient("ODataClient", client =>
//{
//    client.BaseAddress = new Uri($"{backendUrl}odata/");
//    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//})
//.AddHttpMessageHandler<AuthDelegatingHandler>();
//// access simple local folders
//builder.Services.AddHttpClient("LocalClient", client =>
//{
//    client.BaseAddress = new Uri($"{backendUrl}api/");
//    client.DefaultRequestHeaders.Add("X-Requested-With", "Fetch");
//});

//// 2. Register ODataClient directly using the factory
//builder.Services.AddScoped<IODataClient>(sp =>
//{
//    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("ODataClient");
//    var settings = new ODataClientSettings(httpClient)
//    {
//        PayloadFormat = ODataPayloadFormat.Json
//    };
//    return new ODataClient(settings);
//});

//// Simple.OData.Client
//builder.Services.AddScoped<ODataClient>(sp =>
//{
//    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("ODataClient");
//    return new ODataClient(new ODataClientSettings(httpClient) { PayloadFormat = ODataPayloadFormat.Json });
//});
//builder.Services.AddScoped<IODataClientFactory>(sp => new DefaultODataClientFactory(new Uri($"{backendUrl}odata/")));

// ----------------------------
// Guard Services (Dependency Fix)
// ----------------------------
builder.Services.AddScoped<IPendingOpsStore, NoOpOpsStore>(); // Required for PendingChangesGuard
builder.Services.AddScoped<IPendingChangesGuard, PendingChangesGuard>();

// ----------------------------
// App Services
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

builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddSingleton<PouchDbService>();

// ----------------------------
// Startup
// ----------------------------
var host = builder.Build();

var localStorage = host.Services.GetRequiredService<ILocalStorageService>();
var authProvider = host.Services.GetRequiredService<MyAuthStateProvider>();

if (await localStorage.GetItemAsync<bool>("force-reset"))
{
    await localStorage.ClearAsync();
    await authProvider.NotifyUserLogout();
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

//using Blazored.LocalStorage;

//using GxShared.GxGuards;
//using GxShared.Helpers;
//using GxShared.Interfaces;
//using GxShared.Mapping;
//using GxShared.Sess;

//using GxPilo;
//using GxPilo.ClieModels;
//using GxPilo.Services;

//using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.AspNetCore.Components.Web;
//using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

//using Simple.OData.Client;

//using AutoMapper;
//using GxShared.GxDtos;

//var builder = WebAssemblyHostBuilder.CreateDefault(args);
//var backUrl = new Uri(builder.Configuration["BackendUrl"]);

//builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddBlazoredLocalStorage();
//builder.Services.AddLogging();
//builder.Logging.SetMinimumLevel(LogLevel.Information);
//// ✅ 2. Auth Handler (injects token automatically)
//builder.Services.AddTransient<AuthDelegatingHandler>();
////loading configuration in a json file
//builder.Services.AddSingleton(builder.Configuration);
//builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
//builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));


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
//builder.Services.AddSingleton<IMessageService, MessageService>();
//// Register CLIENT SERVICES
//builder.Services.AddScoped<HttpClientService>();
//builder.Services.AddScoped<TokenAwareClientManager>();

//// Program.cs - Blazor WASM (REPLACE your current)
//builder.Services.AddHttpClient("ODataClient", client =>
//{
//    client.BaseAddress = new Uri($"{builder.Configuration["BackendUrl"]}/odata/");
//    client.DefaultRequestHeaders.Add("Accept", "application/json");
//});

//builder.Services.AddScoped(sp =>
//{
//    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
//    var httpClient = httpClientFactory.CreateClient("ODataClient");

//    return new ODataClient(new ODataClientSettings(httpClient)
//    {
//        PayloadFormat = ODataPayloadFormat.Json
//    });
//});

////AuthClient
//builder.Services.AddHttpClient("AuthClient", client =>
//{
//    client.BaseAddress = new Uri($"{builder.Configuration["BackendUrl"]}/api/");
//    // Optional: add default headers like Accept if needed
//})
//    .AddHttpMessageHandler<AuthDelegatingHandler>();

////OData TEST client - for testing only
//builder.Services.AddHttpClient("OTESTClient", client =>
//{
//    client.BaseAddress = new Uri(builder.Configuration["BackendUrl"]!);  // e.g. https://localhost:7095/
//})
//    .AddHttpMessageHandler<AuthDelegatingHandler>();
//// Program.cs (Blazor WASM)
////builder.Services.AddScoped<MyODataClient>();
////builder.Services.AddScoped<GsglneHttpService>();   // optional JSON alternative

//builder.Services.AddHttpClient("api", client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7095/odata/");
//})
//.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
//{

//    // optional settings
//});


//// Guard ODATAClient - depends on IODataClientFactory + AutoMapper
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

//// Guard
//builder.Services.AddScoped<IPendingChangesGuard, PendingChangesGuard>();

//// 🔥 2. OData Context Factory (loads token + creates context)
//builder.Services.AddHttpClient("", client =>
//{
//    var backUrl = builder.Configuration["BackendUrl"];
//    if (!backUrl.EndsWith("/")) backUrl += "/";
//    client.BaseAddress = new Uri(backUrl + "odata/");
//})
//.AddHttpMessageHandler<AuthDelegatingHandler>();

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
////pouch services 
//builder.Services.AddSingleton<PouchDbService>();
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

////builder.Services.AddScoped<IODataClientFactory>(sp =>
////{
////    var backUrl = builder.Configuration["BackendUrl"];
////    if (!backUrl.EndsWith("/")) backUrl += "/";
////    var baseUri = new Uri(backUrl);

////    return new DefaultODataClientFactory(baseUri);
////});
////builder.Services.AddScoped<IPendingChangesGuard, PendingChangesGuard>();

////// ✅ AutoMapper configuration
////builder.Services.AddSingleton<LoadingService>();
////// ✅ Register MapperConfiguration correctly
////builder.Services.AddSingleton(sp =>
////{
////    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

////    var config = new MapperConfiguration(cfg =>
////    {
////        cfg.AddProfile<GxorgaProfile>();
////        cfg.AddProfile<PlngenProfile>();
////    }, loggerFactory);

////    config.AssertConfigurationIsValid();

////    return config;
////});

////// ✅ Register IMapper
////builder.Services.AddSingleton<IMapper>(sp =>
////{
////    var config = sp.GetRequiredService<MapperConfiguration>();
////    return config.CreateMapper(sp.GetService);
////});
//builder.Services.AddScoped<IODataContextFactory, ODataContextFactory>();
//builder.Services.AddHttpClient("ODataClient", client =>
//{
//    var backUrl = builder.Configuration["BackendUrl"];
//    if (!backUrl.EndsWith("/")) backUrl += "/";
//    backUrl += "odata/";   // ✅ Ensure /odata is part of the base address

//    client.BaseAddress = new Uri(backUrl);
//})
//.AddHttpMessageHandler<AuthDelegatingHandler>();
// 🔥 1. HTTP CLIENT for parallel batch saves (ROOT base address)
// Register named OData client
