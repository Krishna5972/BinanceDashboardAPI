using Interfaces.Services;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BinanceMockService : IBinanceService
    {
        

        public Task<FuturesAccountBalanceResponseDto> GetBalanceAsync()
        {

            var random = new Random();

            decimal minBalance = 1000m;
            decimal maxBalance = 5000m;

            decimal randomBalance = (decimal)(random.NextDouble() * (double)(maxBalance - minBalance) + (double)minBalance);

            FuturesAccountBalanceResponseDto result = new FuturesAccountBalanceResponseDto()
            {
                Balance = randomBalance,
                UpdateTime = DateTime.UtcNow
            };

            return Task.FromResult(result);
        }

        public Task<List<FuturesAccountTradeResponseDto>> GetAccountTradesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<FuturesIncomeHistoryResponseDto>> GetIncomeHistoryAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<FutureOpenPositionsResponseDto>> GetOpenPositionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<FuturesOpenOrdersResponseDto>> GetOpenOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<PositionHistoryResponseDto>> GetPositionHistoryAsync()
        {
            throw new NotImplementedException();
        }
    }
}
