using Interfaces.Clients;
using Interfaces.Services;
using Clients;
using Services;

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

bool useMock = builder.Configuration.GetValue<bool>("UseMock");

// Register BinanceApiClient with DI
builder.Services.AddScoped<IBinanceApiClient, BinanceApiClient>();
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

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
