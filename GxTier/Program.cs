using System.Net.Http.Headers;
using BlazorDownloadFile;
using Blazored.LocalStorage;
using GxTie;
using GxShared.Sess;
using GxShared.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using GxTie.ClieModels;
using GxTie.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var backUrl = new Uri(builder.Configuration["BackendUrl"]);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
//loading configuration in a json file
builder.Services.AddSingleton(builder.Configuration);
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// Configure logging first
builder.Logging.SetMinimumLevel(LogLevel.Information);
// Register services
builder.Services.AddHttpClient();
//builder.Services.AddTransient<CookieHandler>();

builder.Services.AddBlazorBootstrap();
builder.Services.AddBlazorDownloadFile();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<MyAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<MyAuthStateProvider>());

//builder.Services.AddTransient<FormulaEvalService>();
builder.Services.AddScoped<Userbag>();
//builder.Services.AddScoped<IGridActionService, GridActionService>();
builder.Services.AddScoped<IPuzzleSyncService, PuzzleSyncService>();
//builder.Services.AddScoped<Usaibag>();
builder.Services.AddScoped<MyShareVars>();
builder.Services.AddScoped<RendAgres>();
builder.Services.AddScoped<ClieAppState>();
builder.Services.AddScoped<SessionContextService>();
builder.Services.AddScoped<SessionContextClient>();

builder.Services.AddSingleton<LoadingService>();
// Configure logging first
builder.Logging.SetMinimumLevel(LogLevel.Information);
// Register CLIENT SERVICES
builder.Services.AddScoped<HttpClientService>();
builder.Services.AddScoped<TokenAwareClientManager>();

// Register named clients
//AUTHClient
builder.Services.AddHttpClient("AUTHClient", client =>
{
    client.BaseAddress = new Uri($"{builder.Configuration["BackendUrl"]}/api/");
    // Optional: add default headers like Accept if needed
});
//OfflineCLIENT
builder.Services.AddHttpClient("OFFLClient", client =>
{
    var backUrl = builder.Configuration["BackendUrl"];
    if (!backUrl.EndsWith("/"))
    {
        backUrl += "/";
    }
    backUrl += "api/";
    client.BaseAddress = new Uri(backUrl);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.DefaultRequestHeaders.Add("X-Requested-With", "Fetch");
});
//ODATAClient
builder.Services.AddScoped<IODataContextFactory, ODataContextFactory>();
builder.Services.AddHttpClient("ODataClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7095/odata");
    // Add any default headers or auth here if needed
});
builder.Services.AddScoped<LinkSerialiser>();
//pouch services 
builder.Services.AddSingleton<PouchDbService>();

builder.Services.AddHttpClient("DefaultClient", client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
// Build the host
var host = builder.Build();
// Get webApi is online or not
var localStorage = host.Services.GetRequiredService<ILocalStorageService>();
var authProvider = host.Services.GetRequiredService<MyAuthStateProvider>();
// Clear local storage and notify logout
await localStorage.ClearAsync(); // or remove specific items
await authProvider.NotifyUserLogout();
// Run the application
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



