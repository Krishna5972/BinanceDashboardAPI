using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Interfaces.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

public class BinanceDataRefreshService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _refreshInterval;
    private readonly TimeSpan _cacheDuration;

    public BinanceDataRefreshService(IServiceScopeFactory serviceScopeFactory, IMemoryCache cache, IConfiguration configuration)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _cache = cache;

        // Read interval from configuration (default to 5 minutes)
        int intervalMinutes = configuration.GetValue<int>("BackgroundTaskSettings:BackgroundTaskIntervalMinutes", 5);
        _refreshInterval = TimeSpan.FromMinutes(intervalMinutes);


        int cacheMinutes = configuration.GetValue<int>("CacheSettings:CacheDurationMinutes", 5);
        _cacheDuration = TimeSpan.FromMinutes(cacheMinutes);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var _binanceService = scope.ServiceProvider.GetRequiredService<IBinanceService>();

                    Console.WriteLine($"Hitting  Binance API's... {DateTime.Now}");

                    var balance = await _binanceService.GetBalanceAsync();
                    var incomeHistory = await _binanceService.GetIncomeHistoryAsync();
                    var openPositions = await _binanceService.GetOpenPositionsAsync();
                    var openOrders = await _binanceService.GetOpenOrdersAsync();
                    var accountTrades = await _binanceService.GetAccountTradesAsync();

                    _cache.Set("Last_Updated_Time", DateTime.Now, _cacheDuration);


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
