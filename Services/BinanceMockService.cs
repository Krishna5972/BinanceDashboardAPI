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

            float randomBalance = (float)(random.NextDouble() * (double)(maxBalance - minBalance) + (double)minBalance);

            FuturesAccountBalanceResponseDto result = new FuturesAccountBalanceResponseDto()
            {
                Balance = randomBalance,
                UpdateTime = DateTime.UtcNow
            };

            return Task.FromResult(result);
        }

        public Task<List<FuturesAccountTradeResponseDto>> GetAccountTradesAsync()
        {
            var random = new Random();
            int numberOfTrades = random.Next(3, 7); 
            List<FuturesAccountTradeResponseDto> trades = new List<FuturesAccountTradeResponseDto>();

            string[] symbols = { "BTCUSDT", "ETHUSDT", "BNBUSDT", "ADAUSDT" }; 
            string[] sides = { "BUY", "SELL" };
            string[] positionSides = { "LONG", "SHORT" };

            for (int i = 0; i < numberOfTrades; i++)
            {
                var trade = new FuturesAccountTradeResponseDto
                {
                    Symbol = symbols[random.Next(symbols.Length)],
                    Id = random.NextInt64(10000000, 99999999),
                    OrderId = random.NextInt64(10000000, 99999999),
                    Side = sides[random.Next(sides.Length)],
                    Price = (float)(random.NextDouble() * (60000 - 30000) + 30000), 
                    Quantity = (float)(random.NextDouble() * (2 - 0.01) + 0.01), 
                    RealizedPnl = (float)(random.NextDouble() * (100 - (-100)) + (-100)), 
                    QuoteQuantity = (float)(random.NextDouble() * (6000 - 100) + 100), 
                    Commission = (float)(random.NextDouble() * (5 - 0.1) + 0.1), 
                    PositionSide = positionSides[random.Next(positionSides.Length)],
                    Buyer = random.Next(2) == 1,
                    Maker = random.Next(2) == 1,
                    Time = DateTime.UtcNow.AddMinutes(-random.Next(0, 5))
                };

                trades.Add(trade);
            }

            return Task.FromResult(trades);
        }

        public Task<List<FuturesIncomeHistoryResponseDto>> GetIncomeHistoryAsync()
        {
            var random = new Random();
            int numberOfRecords = random.Next(2, 5); // Between 2 to 4 random income records
            List<FuturesIncomeHistoryResponseDto> incomeHistory = new List<FuturesIncomeHistoryResponseDto>();

            string[] symbols = { "BTCUSDT", "ETHUSDT", "BNBUSDT", "XRPUSDT" };
            string[] incomeTypes = { "REALIZED_PNL", "COMMISSION", "FUNDING_FEE", "TRANSFER" };

            for (int i = 0; i < numberOfRecords; i++)
            {
                var incomeRecord = new FuturesIncomeHistoryResponseDto
                {
                    Symbol = symbols[random.Next(symbols.Length)],
                    IncomeType = incomeTypes[random.Next(incomeTypes.Length)],
                    Income = (float)(random.NextDouble() * (1000 - (-500)) + (-500)), // Random income between -500 and 1000
                    Asset = "USDT", // Assuming the asset is USDT
                    Info = random.Next(1000000, 9999999).ToString(), // Random trade-related info
                    Time = DateTime.UtcNow.AddMinutes(-random.Next(0, 60)), // Recent time within the last 60 minutes
                    TranId = random.NextInt64(100000000, 999999999),
                    TradeID = random.NextInt64(100000000, 999999999)
                };

                incomeHistory.Add(incomeRecord);
            }

            return Task.FromResult(incomeHistory);
        }

        public Task<List<FutureOpenPositionsResponseDto>> GetOpenPositionsAsync()
        {
            var random = new Random();
            int numberOfRecords = random.Next(2, 6); // Between 2 and 5 random open positions
            List<FutureOpenPositionsResponseDto> openPositions = new List<FutureOpenPositionsResponseDto>();

            string[] symbols = { "BTCUSDT", "ETHUSDT", "BNBUSDT", "SOLUSDT" };
            string[] positionSides = { "LONG", "SHORT" };

            for (int i = 0; i < numberOfRecords; i++)
            {
                var position = new FutureOpenPositionsResponseDto
                {
                    Symbol = symbols[random.Next(symbols.Length)],
                    PositionSide = positionSides[random.Next(positionSides.Length)],
                    EntryPrice = (float)(random.NextDouble() * (60000 - 30000) + 30000), 
                    UnRealizedProfit = (float)(random.NextDouble() * (1000 - (-1000)) + (-1000)), 
                    LiquidationPrice = (float)(random.NextDouble() * (25000 - 15000) + 15000), 
                    Notional = (float)(random.NextDouble() * (50000 - 5000) + 5000) 
                };

                openPositions.Add(position);
            }

            return Task.FromResult(openPositions);
        }

        public Task<List<FuturesOpenOrdersResponseDto>> GetOpenOrdersAsync()
        {
            var random = new Random();
            int numberOfRecords = random.Next(2, 6); // Between 2 and 5 random open orders
            List<FuturesOpenOrdersResponseDto> openOrders = new List<FuturesOpenOrdersResponseDto>();

            string[] symbols = { "BTCUSDT", "ETHUSDT", "BNBUSDT", "SOLUSDT" };
            string[] entryTypes = { "LIMIT", "MARKET", "STOP_LOSS", "TAKE_PROFIT" };
            string[] orderTypes = { "BUY", "SELL" };

            for (int i = 0; i < numberOfRecords; i++)
            {
                var order = new FuturesOpenOrdersResponseDto
                {
                    Symbol = symbols[random.Next(symbols.Length)],
                    Price = (float)(random.NextDouble() * (60000 - 30000) + 30000), // Random price between 30,000 and 60,000
                    Time = DateTime.UtcNow.AddMinutes(-random.Next(0, 60)), // Recent time within the last 60 minutes
                    EntryType = entryTypes[random.Next(entryTypes.Length)],
                    OrderType = orderTypes[random.Next(orderTypes.Length)],
                    Amount = (float)(random.NextDouble() * (10 - 0.01) + 0.01) // Random amount between 0.01 and 10
                };

                openOrders.Add(order);
            }

            return Task.FromResult(openOrders);
        
        }

        public Task<List<PositionHistoryResponseDto>> GetPositionHistoryAsync()
        {
            var random = new Random();
            List<PositionHistoryResponseDto> positionHistory = new List<PositionHistoryResponseDto>();

            string[] symbols = { "BTCUSDT", "ETHUSDT", "BNBUSDT", "SOLUSDT" };
            string[] positionSides = { "LONG", "SHORT" };

            for (int i = 0; i < 5; i++)
            {
                DateTime openTime = DateTime.UtcNow.Date.AddDays(-i).AddHours(random.Next(0, 23));
                DateTime closeTime = openTime.AddHours(random.Next(1, 12));
                float entryPrice = (float)(random.NextDouble() * (60000 - 30000) + 30000);
                float avgClosePrice = entryPrice + (float)(random.NextDouble() * (2000 - (-2000)) + (-2000));
                float pnl = avgClosePrice - entryPrice;

                positionHistory.Add(new PositionHistoryResponseDto
                {
                    Symbol = symbols[random.Next(symbols.Length)],
                    PositionSide = positionSides[random.Next(positionSides.Length)],
                    EntryPrice = entryPrice,
                    AvgClosePrice = avgClosePrice,
                    OpenTime = openTime,
                    CloseTime = closeTime,
                    PNL = pnl
                });
            }

            int totalPositions = random.Next(30, 40);
            for (int i = 0; i < totalPositions - 5; i++)
            {
                DateTime openTime = DateTime.UtcNow.AddDays(-random.Next(5, 30));
                DateTime closeTime = openTime.AddHours(random.Next(1, 12));
                float entryPrice = (float)(random.NextDouble() * (60000 - 30000) + 30000);
                float avgClosePrice = entryPrice + (float)(random.NextDouble() * (2000 - (-2000)) + (-2000));
                float pnl = avgClosePrice - entryPrice;

                positionHistory.Add(new PositionHistoryResponseDto
                {
                    Symbol = symbols[random.Next(symbols.Length)],
                    PositionSide = positionSides[random.Next(positionSides.Length)],
                    EntryPrice = entryPrice,
                    AvgClosePrice = avgClosePrice,
                    OpenTime = openTime,
                    CloseTime = closeTime,
                    PNL = pnl
                });
            }

            return Task.FromResult(positionHistory);
        }

        public Task<List<BalanceSnapshotResponseDto>> GetBalanceSnapshotAsync()
        {
            var random = new Random();
            List<BalanceSnapshotResponseDto> balanceSnapshots = new List<BalanceSnapshotResponseDto>();

            int totalDays = random.Next(31, 35); 

            for (int i = 0; i < totalDays; i++)
            {
                DateOnly date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-i));
                float balance = (float)(random.NextDouble() * (15000 - 10000) + 10000);

                balanceSnapshots.Add(new BalanceSnapshotResponseDto
                {
                    Date = date,
                    Balance = balance
                });
            }

            return Task.FromResult(balanceSnapshots);
        }

        public Task<List<DailyPNLResponseDTO>> GetDailyPNLAsync()
        {
            var random = new Random();
            List<DailyPNLResponseDTO> dailyPNLRecords = new List<DailyPNLResponseDTO>();

            int totalDays = random.Next(31, 35); // Generate between 31 and 34 records

            for (int i = 0; i < totalDays; i++)
            {
                DateOnly date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-i));
                float pnl = (float)(random.NextDouble() * (500 - (-500)) + (-500)); // Random PNL between -500 and 500
                DateTime lastUpdated = DateTime.UtcNow.AddMinutes(-random.Next(0, 1440)); // Last updated within the last 24 hours

                dailyPNLRecords.Add(new DailyPNLResponseDTO
                {
                    Date = date,
                    PNL = pnl,
                    LastUpdated = lastUpdated
                });
            }

            return Task.FromResult(dailyPNLRecords);
        }

        public Task<List<MonthlySummaryResponseDto>> GetMonthlySummaryAsync()
        {
            var random = new Random();
            List<MonthlySummaryResponseDto> monthlySummaries = new List<MonthlySummaryResponseDto>();

            int totalMonths = random.Next(3, 6); // Generate between 3 and 5 sequential months

            DateTime currentMonth = DateTime.UtcNow;

            for (int i = 0; i < totalMonths; i++)
            {
                DateTime summaryDate = currentMonth.AddMonths(-i); // Sequential months backward

                float pnl = (float)(random.NextDouble() * (10000 - (-5000)) + (-5000)); // Random PNL between -5000 and 10000
                int daysInMonth = DateTime.DaysInMonth(summaryDate.Year, summaryDate.Month);
                float dailyAverage = pnl / daysInMonth;

                monthlySummaries.Add(new MonthlySummaryResponseDto
                {
                    Month = (Int16)summaryDate.Month,
                    Year = (Int16)summaryDate.Year,
                    PNL = pnl,
                    DailyAverage = dailyAverage
                });
            }

            return Task.FromResult(monthlySummaries);
        }

        public Task<List<HistoryResponseDto>> GetHistoryAsync()
        {
            var random = new Random();
            List<HistoryResponseDto> historyRecords = new List<HistoryResponseDto>();

            for (int i = 0; i < 6; i++)
            {
                DateOnly date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-i));
                float commission = (float)(random.NextDouble() * (100 - 20) + 20); // Random commission between 20 and 100
                float pnl = (float)(random.NextDouble() * (2000 - (-1000)) + (-1000)); // Random PNL between -1000 and 2000
                float income = (float)(random.NextDouble() * (500 - 100) + 100); // Random income between 100 and 500
                float multiplier = (float)(random.NextDouble() * (5000 - 1000) + 1000); // Random multiplier between 1000 and 5000

                historyRecords.Add(new HistoryResponseDto
                {
                    Date = date,
                    Data = new HistorySummary
                    {
                        commision = commission,
                        PNL = pnl,
                        income = income,
                        multipler = multiplier
                    }
                });
            }

            return Task.FromResult(historyRecords);
        }

        public Task<DateTime> GetLastUpdatedTime()
        {
            var random = new Random();
            int secondsAgo = random.Next(0, 280);
            DateTime lastUpdatedTime = DateTime.UtcNow.AddSeconds(-secondsAgo);

            return Task.FromResult(lastUpdatedTime);
        }
    }
}
