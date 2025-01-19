using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Interfaces.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class BinanceDataRefreshService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _refreshInterval;

    public BinanceDataRefreshService(IServiceScopeFactory serviceScopeFactory, IMemoryCache cache, IConfiguration configuration)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _cache = cache;

        // Read interval from configuration (default to 5 minutes)
        int intervalMinutes = configuration.GetValue<int>("BackgroundTaskSettings:BackgroundTaskIntervalMinutes", 5);
        _refreshInterval = TimeSpan.FromMinutes(intervalMinutes);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var binanceService = scope.ServiceProvider.GetRequiredService<IBinanceService>();

                    Console.WriteLine("Hitting  Binance API's...");

                    var balance = await binanceService.GetBalanceAsync();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error refreshing Binance data: {ex.Message}");
            }

            await Task.Delay(_refreshInterval, stoppingToken);
        }
    }
}
