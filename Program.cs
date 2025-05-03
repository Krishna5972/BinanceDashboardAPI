using Interfaces.Clients;
using Interfaces.Services;
using Clients;
using Services;
using Interfaces.Repository;
using DatabaseLayer;
using Repository.Context;
using Common.Settings;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Load configuration in order of precedence (Local Vaults, then appsettings)
builder.Configuration
    .AddEnvironmentVariables()            // Highest precedence
    .AddJsonFile("appsettings.json")       // Default settings
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddUserSecrets<Program>(optional: true);  // Load user secrets in development

builder.Services.AddHttpClient("BinanceClient", client =>
{
    client.BaseAddress = new Uri("https://fapi.binance.com");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddMemoryCache();
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

bool useMock = builder.Configuration.GetValue<bool>("UseMock");

builder.Services.AddScoped<DbContext>();
builder.Services.AddScoped<IBinanceApiClient, BinanceApiClient>();
builder.Services.AddScoped<ITradeRepository, TradeRepository>();
if (useMock)
    builder.Services.AddScoped<IBinanceService, BinanceMockService>();
else
{
    builder.Services.AddScoped<IBinanceService, BinanceService>();
    builder.Services.AddHostedService<BinanceDataRefreshService>();
}



builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Developement");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
