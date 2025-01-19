using Interfaces.Services;
using Interfaces.Clients;
using Models.DTOs;
using Clients;
using Models.Entities;
using Newtonsoft.Json;
using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Common.Constants;

namespace Services
{
    public class BinanceService : IBinanceService
    {
        private readonly IBinanceApiClient _binanceApiClient;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration;

        public BinanceService(IBinanceApiClient binanceApiClient, IMemoryCache cache, IConfiguration configuration) {
            _binanceApiClient = binanceApiClient;
            _cache = cache;

            int cacheMinutes = configuration.GetValue<int>("CacheSettings:CacheDurationMinutes", 5);
            _cacheDuration = TimeSpan.FromMinutes(cacheMinutes);
        }
        public async Task<FuturesAccountBalanceResponseDto> GetBalanceAsync()
        {
            if (_cache.TryGetValue("Binance_Balance", out FuturesAccountBalanceResponseDto cachedBalance))
                return cachedBalance;

            var response = await FetchBalanceFromBinance();
            return response;
        }

        public async Task<FuturesAccountBalanceResponseDto> FetchBalanceFromBinance()
        {
            var jsonResponse = await _binanceApiClient.GetAsync(BinanceEndpoints.FuturesBalanceEndpoint);
            List<FuturesAccountBalance> balances = JsonConvert.DeserializeObject<List<FuturesAccountBalance>>(jsonResponse);

            decimal currBalance = balances.Where(x => x.Asset == "USDT").FirstOrDefault()?.CrossWalletBalance ?? 0;

            var result = new FuturesAccountBalanceResponseDto()
            {
                Balance = currBalance,
                UpdateTime = DateTime.UtcNow
            };

            _cache.Set("Binance_Balance", result, _cacheDuration);

            return result;
        }
    }
}
