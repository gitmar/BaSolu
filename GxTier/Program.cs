using System.Net.Http.Headers;

using BlazorDownloadFile;

using Blazored.LocalStorage;

using GxFormula;
using GxFormula.ForaBizz;
using GxFormula.Forasource;

using GxShared.GxGuards;
using GxShared.Helpers;
using GxShared.Interfaces;
using GxShared.Mapping;
using GxShared.Sess;

using GxTie;
using GxTie.Services;
using GxTie.Components.Cols;
using GxTie.Services.Calculation;

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

// --------------------------------------------
// 1. Consolidated HttpClient Registrations (3)
// --------------------------------------------
builder.Services.AddHttpClient("AUTHClient", client =>
    client.BaseAddress = new Uri($"{backendUrl}api/"))
    .AddHttpMessageHandler<AuthDelegatingHandler>();

builder.Services.AddHttpClient("ODataClient", client =>
{
    client.BaseAddress = new Uri($"{backendUrl}odata/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AuthDelegatingHandler>();
builder.Services.AddHttpClient("ODataDocuClient", client =>
{
    client.BaseAddress = new Uri($"{backendUrl}odata3/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AuthDelegatingHandler>();

builder.Services.AddHttpClient("LOCALClient", client =>
{
    client.BaseAddress = new Uri($"{backendUrl}api/");
    client.DefaultRequestHeaders.Add("X-Requested-With", "Fetch");
});
builder.Services.AddHttpClient("LOCODAClient", client =>
{
    client.BaseAddress = new Uri($"{backendUrl}odata/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});
builder.Services.AddHttpClient("DefaultClient", client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
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
builder.Services.AddScoped<ChildVars>();
builder.Services.AddScoped<RendAgres>();
//builder.Services.AddScoped<RendAOlne>();
//columns preferences and using
builder.Services.AddScoped<ColumnPreferencesClient>();
builder.Services.AddScoped<ColumnFormValidator>();
builder.Services.AddScoped<PublicColumnPreferencesClient>();
builder.Services.AddScoped<PublicColumnFormValidator>();

builder.Services.AddScoped<ClieAppState>();
builder.Services.AddScoped<SessionContextService>();
builder.Services.AddScoped<SessionContextClient>();
builder.Services.AddScoped<HttpClientService>();
builder.Services.AddScoped<TokenAwareClientManager>();
builder.Services.AddScoped<TblJsonRender>();
builder.Services.AddScoped<LinkSerialiser>();
builder.Services.AddScoped<OrgDocService>();
builder.Services.AddScoped<IClientMatriculeOrdreService, ClientMatriculeOrdreService>();
//builder.Services.AddScoped<PouchDbService>();
builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddSingleton<LoadingService>();

// Load Plngen before building the host
//var plngen = await LoadProgAsync(httpClient); // your custom method

//Formula services
builder.Services.AddScoped<FormulaEngine>();
builder.Services.AddScoped<FormulaEvaluationService>();
builder.Services.AddScoped<FormulaEvaluationContext>();
builder.Services.AddScoped<ITokenResolver, TokenResolver>();
// Add calculation pipeline
builder.Services.AddCalculationPipeline();

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
