using GxDicto.Api.Data;
using GxDicto.Api.Models;

using Microsoft.EntityFrameworkCore;

var dxpolice = "GxCorsPolicy";
var builder = WebApplication.CreateBuilder(args);



builder.WebHost.UseUrls(
    "https://localhost:7097",
    "http://localhost:5095");
builder.Services.AddDbContext<dxdocuContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("LocalizationDb")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   // <-- comes from Swashbuckle.AspNetCore

// Register repository (in-memory for now, replace with EF Core later)
//builder.Services.AddSingleton<LocalizationRepository>();
builder.Services.AddSingleton<Gdbse>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(dxpolice, policy =>
    {
        policy.WithOrigins(
    "https://localhost:7097",
    "https://localhost:7510",
    "https://www.a24soft.com"
)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();

app.UseCors(dxpolice);

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();