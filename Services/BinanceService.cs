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
using System.Collections.Generic;
using System.Diagnostics;
using Interfaces.Repository;
using System.Linq;
using Common.Comparers;
using System.Text.Json;
using System.IO;
using Common.Constants;

namespace Services
{
    public class BinanceService : IBinanceService
    {
        private readonly IBinanceApiClient _binanceApiClient;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration;
        private readonly ITradeRepository _tradeRepository;

        public BinanceService(IBinanceApiClient binanceApiClient, IMemoryCache cache, IConfiguration configuration, ITradeRepository tradeRepository) {
            _binanceApiClient = binanceApiClient;
            _cache = cache;
            _tradeRepository = tradeRepository;

            int cacheMinutes = configuration.GetValue<int>("CacheSettings:CacheDurationMinutes", 5);
            _cacheDuration = TimeSpan.FromMinutes(cacheMinutes);
        }
        public virtual async Task<FuturesAccountBalanceResponseDto> GetBalanceAsync()
        {
            if (_cache.TryGetValue(CacheKeys.BinanceBalance, out FuturesAccountBalanceResponseDto cachedBalance))
                return cachedBalance;

            var response = await FetchBalanceFromBinance();

            _cache.Set(CacheKeys.BinanceBalance, response, _cacheDuration);

            return response;
        }


        public virtual async Task<FuturesAccountBalanceResponseDto> FetchBalanceFromBinance()
        {
            var jsonResponse = await _binanceApiClient.GetAsync(BinanceEndpoints.FuturesBalanceEndpoint);

            if (string.IsNullOrWhiteSpace(jsonResponse))
                throw new Exception("Received an empty response from Binance API.");


            if (jsonResponse.Contains("code") && jsonResponse.Contains("msg"))
            {
                var errorResponse = JsonConvert.DeserializeObject<BinanceErrorResponse>(jsonResponse);
                throw new Exception($"Binance API error: {errorResponse.Msg} (Code: {errorResponse.Code})");
            }

            List<FuturesAccountBalance> balances = JsonConvert.DeserializeObject<List<FuturesAccountBalance>>(jsonResponse);

            float currBalance = balances.Where(x => x.Asset == "USDT").FirstOrDefault()?.CrossWalletBalance ?? 0.0f;

            var result = new FuturesAccountBalanceResponseDto()
            {
                Balance = currBalance,
                UpdateTime = DateTime.UtcNow
            };

            return result;
        }

        public virtual async Task<List<FuturesAccountTradeResponseDto>> GetAccountTradesAsync()
        {
            if (_cache.TryGetValue(CacheKeys.BinanceAccountTrades, out List<FuturesAccountTradeResponseDto> cachedAccountTrades))
                return cachedAccountTrades;

            var response = await FetchAccountTradesFromBinance();

            _cache.Set(CacheKeys.BinanceAccountTrades, response, _cacheDuration);

            return response;
        }

        public virtual async Task<List<FuturesAccountTradeResponseDto>> FetchAccountTradesFromBinance()
        {
            var jsonResponse = await _binanceApiClient.GetAsync(BinanceEndpoints.FuturesAccountTradesEndpoint);

            if (string.IsNullOrWhiteSpace(jsonResponse))
                throw new Exception("Received an empty response from Binance API.");


            if (jsonResponse.Contains("code") && jsonResponse.Contains("msg"))
            {
                var errorResponse = JsonConvert.DeserializeObject<BinanceErrorResponse>(jsonResponse);
                throw new Exception($"Binance API error: {errorResponse.Msg} (Code: {errorResponse.Code})");
            }

            List<FuturesAccountTrade> accountTrades = JsonConvert.DeserializeObject<List<FuturesAccountTrade>>(jsonResponse);


            var result = accountTrades.Select(trade => new FuturesAccountTradeResponseDto
            {
                Symbol = trade.Symbol,
                Id = trade.Id,
                OrderId = trade.OrderId,
                Side = trade.Side,
                Price = Convert.ToSingle(trade.Price),
                Quantity = Convert.ToSingle(trade.Quantity),
                RealizedPnl = Convert.ToSingle(trade.RealizedPnl),
                QuoteQuantity = Convert.ToSingle(trade.QuoteQuantity),
                Commission = Convert.ToSingle(trade.Commission),
                PositionSide = trade.PositionSide,
                Buyer = trade.Buyer,
                Maker = trade.Maker,
                Time = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(trade.Time)).UtcDateTime
            }).ToList();

            

            return result;
        }

        public virtual async Task<List<FuturesIncomeHistoryResponseDto>> GetIncomeHistoryAsync()
        {
            if (_cache.TryGetValue(CacheKeys.BinanceIncomeHistory, out List<FuturesIncomeHistoryResponseDto> cachedIncomeHistory))
                return cachedIncomeHistory;

            var response = await FetchIncomeHistoryFromBinance();

            _cache.Set(CacheKeys.BinanceIncomeHistory, response, _cacheDuration);

            return response;
        }

        public virtual async Task<List<FuturesIncomeHistoryResponseDto>> FetchIncomeHistoryFromBinance()
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams["limit"] = "1000";
            var jsonResponse = await _binanceApiClient.GetAsync(BinanceEndpoints.FuturesIncomeHistoryEndpoint, queryParams);

            if (string.IsNullOrWhiteSpace(jsonResponse))
                throw new Exception("Received an empty response from Binance API.");


            if (jsonResponse.Contains("code") && jsonResponse.Contains("msg"))
            {
                var errorResponse = JsonConvert.DeserializeObject<BinanceErrorResponse>(jsonResponse);
                throw new Exception($"Binance API error: {errorResponse.Msg} (Code: {errorResponse.Code})");
            }

            List<FuturesIncomeHistory> cachedIncomeHistory = JsonConvert.DeserializeObject<List<FuturesIncomeHistory>>(jsonResponse);


            var result = cachedIncomeHistory.Select(trade => new FuturesIncomeHistoryResponseDto
            {
                Symbol = trade.Symbol,
                IncomeType = trade.IncomeType,
                Income = string.IsNullOrWhiteSpace(trade.Income) ? 0.0f : Convert.ToSingle(trade.Income),
                Asset = trade.Asset,
                Info = trade.Info,
                Time = string.IsNullOrWhiteSpace(trade.Time) ? DateTime.MinValue : DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(trade.Time)).UtcDateTime,
                TranID = string.IsNullOrWhiteSpace(trade.TranId) ? 0 : Convert.ToInt64(trade.TranId),
                TradeID = string.IsNullOrWhiteSpace(trade.TradeID) ? 0 : Convert.ToInt64(trade.TradeID)
            }).ToList();


            return result;
        }

        public virtual async Task<List<FutureOpenPositionsResponseDto>> GetOpenPositionsAsync()
        {
            if (_cache.TryGetValue(CacheKeys.BinanceOpenPositions, out List<FutureOpenPositionsResponseDto> cachedOpenPositions))
                return cachedOpenPositions;

            var response = await FetchOpenPositionsFromBinance();

            _cache.Set(CacheKeys.BinanceOpenPositions, response, _cacheDuration);

            return response;
        }

        public virtual async Task<List<FutureOpenPositionsResponseDto>> FetchOpenPositionsFromBinance()
        {
            var jsonResponse = await _binanceApiClient.GetAsync(BinanceEndpoints.FuturesOpenPositionsEndpoint);

            if (string.IsNullOrWhiteSpace(jsonResponse))
                throw new Exception("Received an empty response from Binance API.");


            if (jsonResponse.Contains("code") && jsonResponse.Contains("msg"))
            {
                var errorResponse = JsonConvert.DeserializeObject<BinanceErrorResponse>(jsonResponse);
                throw new Exception($"Binance API error: {errorResponse.Msg} (Code: {errorResponse.Code})");
            }

            List<FutureOpenPositions> openPositionsList = JsonConvert.DeserializeObject<List<FutureOpenPositions>>(jsonResponse);

            openPositionsList = openPositionsList.Where(position => Convert.ToSingle(position.PositionAmt) != 0).ToList();

            var result = openPositionsList.Select(position => new FutureOpenPositionsResponseDto
            {

                Symbol = position.Symbol,
                PositionSide = position.PositionSide,
                EntryPrice = Convert.ToSingle(position.EntryPrice),
                UnRealizedProfit = Convert.ToSingle(position.UnRealizedProfit),
                LiquidationPrice = Convert.ToSingle(position.LiquidationPrice),
                Notional = Convert.ToSingle(position.Notional)
            }).ToList();

            

            return result;
        }

        public virtual async Task<List<FuturesOpenOrdersResponseDto>> GetOpenOrdersAsync()
        {
            if (_cache.TryGetValue(CacheKeys.BinanceOpenOrders, out List<FuturesOpenOrdersResponseDto> cachedOpenOrders))
                return cachedOpenOrders;

            var response = await FetchOpenOrdersFromBinance();

            _cache.Set(CacheKeys.BinanceOpenOrders, response, _cacheDuration);

            return response;
        }

        public virtual async Task<List<FuturesOpenOrdersResponseDto>> FetchOpenOrdersFromBinance()
        {
            var jsonResponse = await _binanceApiClient.GetAsync(BinanceEndpoints.FuturesOpenOrdersEndpoint);

            if (string.IsNullOrWhiteSpace(jsonResponse))
                throw new Exception("Received an empty response from Binance API.");


            if (jsonResponse.Contains("code") && jsonResponse.Contains("msg"))
            {
                var errorResponse = JsonConvert.DeserializeObject<BinanceErrorResponse>(jsonResponse);
                throw new Exception($"Binance API error: {errorResponse.Msg} (Code: {errorResponse.Code})");
            }

            List<FuturesOpenOrders> cachedOpenOrders = JsonConvert.DeserializeObject<List<FuturesOpenOrders>>(jsonResponse);

            Console.WriteLine(cachedOpenOrders);

            var result = cachedOpenOrders.Select(order => new FuturesOpenOrdersResponseDto
            {
                Symbol = order.Symbol,
                Price = Convert.ToSingle(order.Price) == 0 ? Convert.ToSingle(order.StopPrice) : Convert.ToSingle(order.Price),
                EntryType = GetOrderType(order.Side, order.PositionSide),
                OrderType = order.Type,
                Amount = Convert.ToSingle(order.OrigQty) * Convert.ToSingle(order.Price),
                Time = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(order.UpdateTime)).UtcDateTime,
            }).ToList();

            

            return result;
        }


        public virtual async Task<List<PositionHistoryResponseDto>> GetPositionHistoryAsync()
        {
            List<FuturesAccountTradeResponseDto> accountTrades = await GetAccountTradesAsync();
            List<FuturesAccountTradeResponseDto> accountTradeDB = (await _tradeRepository.GetAllAccountTradesAsync()).ToList();


            List<FuturesAccountTradeResponseDto> combinedTrades = accountTrades
                                                                    .Concat(accountTradeDB)
                                                                    .Distinct(new FuturesAccountTradeComparer())
                                                                    .ToList();

            List<PositionHistoryResponseDto> positions = ProcessTrades(combinedTrades);

            return positions;
        }

        public virtual async Task<List<BalanceSnapshotResponseDto>> GetBalanceSnapshotAsync()
        {
            List<BalanceSnapshotResponseDto> balanceSnaphot = (await _tradeRepository.GetBalanceSnapshotAsync()).ToList();
            balanceSnaphot[balanceSnaphot.Count - 1].Balance = (await GetBalanceAsync()).Balance;
            return balanceSnaphot;
        }

        public virtual async Task<List<DailyPNLResponseDTO>> GetDailyPNLAsync()
        {
            List<DailyPNLResponseDTO> dailyPNL = (await _tradeRepository.GetDailyPNLAsync()).ToList();
            List<FuturesIncomeHistoryResponseDto> incomeHistory = await GetIncomeHistoryAsync();

            float todayIncome = incomeHistory
                                .Where(x => x.IncomeType != "TRANSFER" && x.Time.Day == DateTime.Now.Day)
                                .Sum(t => t.Income);

            dailyPNL[dailyPNL.Count - 1].PNL = todayIncome;

            return dailyPNL;

        }

        public virtual async Task<List<MonthlySummaryResponseDto>> GetMonthlySummaryAsync()
        {
            List<DailyPNLResponseDTO> dailyPNL = await GetDailyPNLAsync();

            var monthlySummaryList = dailyPNL
                                        .GroupBy(x => new { x.Date.Year, x.Date.Month })
                                        .Select(g => new MonthlySummaryResponseDto
                                        {
                                            Year = (short)g.Key.Year,
                                            Month = (short)g.Key.Month,
                                            PNL = g.Sum(x => x.PNL),
                                            DailyAverage = g.Sum(x => x.PNL) / g.Count()
                                        })
                                        .ToList();
            return monthlySummaryList;
        }

        public virtual async Task<List<HistoryResponseDto>> GetHistoryAsync()
        {
            List<FuturesIncomeHistoryResponseDto> incomeHistory = await GetIncomeHistoryAsync();

            List<FuturesIncomeHistoryResponseDto> incomeHistoryDB = (await _tradeRepository.GetAllIncomeHistoryAsync()).ToList();

            List<FuturesIncomeHistoryResponseDto> combinedIncomeHistory = incomeHistory
                                                                    .Concat(incomeHistoryDB)
                                                                    .Distinct(new FuturesIncomeHistoryComparer())
                                                                    .ToList();

            var cutoffDate = DateTime.UtcNow.AddDays(-BinanceServiceConstants.DAYS_TO_FETCH);

            var lastSixDays = combinedIncomeHistory
                .Where(i => i.Time >= cutoffDate)
                .ToList();

            var dailyData = lastSixDays
                .GroupBy(i => new { i.Time.Year, i.Time.Month, i.Time.Day })
                .Select(g =>
                {
                    var commissionsForDay = g.Where(i =>
                        i.IncomeType == BinanceServiceConstants.FUNDING_FEE || i.IncomeType == BinanceServiceConstants.COMMISSION)
                        .Sum(x => x.Income);

                    var pnlForDay = g.Where(i =>
                        i.IncomeType == BinanceServiceConstants.REALIZED_PNL)
                        .Sum(x => x.Income);

                    var totalIncome = commissionsForDay + pnlForDay;

                    return new HistoryResponseDto
                    {
                        Date = new DateOnly(g.Key.Year, g.Key.Month, g.Key.Day),
                        Data = new HistorySummary
                        {
                            commision = commissionsForDay,
                            PNL = pnlForDay,
                            income = totalIncome,
                            multipler = totalIncome * BinanceServiceConstants.INCOME_MULTIPLIER
                        }
                    };
                });


            return dailyData.OrderBy(d => d.Date).ToList();
        }
        public virtual Task<DateTime> GetLastUpdatedTime()
        {
            if (_cache.TryGetValue(CacheKeys.LastUpdatedTime, out DateTime cachedLastUpdatedTime))
                return Task.FromResult(cachedLastUpdatedTime);

            return Task.FromResult(DateTime.Now);
        }

        #region private functions

        public virtual string GetOrderType(string side, string positionSide)
        {
            if (side == "BUY")
            {
                if (positionSide == "LONG")
                    return "OPEN LONG";
                else
                    return "CLOSE SHORT";
            }
            else
            {
                if (positionSide == "LONG")
                    return "CLOSE LONG";
                else
                    return "OPEN SHORT";
            }
        }



        public virtual List<PositionHistoryResponseDto> ProcessTrades(List<FuturesAccountTradeResponseDto> trades)
        {
            var masterResults = new List<PositionHistoryResponseDto>();
            var issueCoins = new List<string>();

            // --- 1) Convert SELL quantities to negative ---
            foreach (var t in trades)
            {
                if (t.Side.Equals("SELL", StringComparison.OrdinalIgnoreCase))
                {
                    t.Quantity = -t.Quantity;
                }
            }

            // --- 2) Group by symbol ---
            var symbols = trades.Select(x => x.Symbol).Distinct();

            foreach (var coin in symbols)
            {
                var tradesForSymbol = trades
                    .Where(t => t.Symbol == coin)
                    .OrderBy(t => t.Time)
                    .ToList();

                // ========== LONG side ==========
                var dfLong = tradesForSymbol
                    .Where(t => t.PositionSide.Equals("LONG", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                dfLong.Where(x => x.Side == "SELL")
                      .ToList()
                      .ForEach(s => s.Quantity = -Math.Abs(s.Quantity));

                bool checkLong = dfLong.Count > 0;
                
                if (checkLong)
                {
                    var positionsLong = GetPositionsCoinLong(dfLong);
                    masterResults.AddRange(positionsLong);
                }

                // ========== SHORT side ==========
                var dfShort = tradesForSymbol
                    .Where(t => t.PositionSide.Equals("SHORT", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                dfShort.Where(x => x.Side == "SELL")
                      .ToList()
                      .ForEach(s => s.Quantity = -Math.Abs(s.Quantity));

                bool checkShort = dfShort.Count > 0;
                if (checkShort)
                {
                    var positionsShort = GetPositionsCoinShort(dfShort);
                    masterResults.AddRange(positionsShort);
                }
            }

            // --- Sort final results by CloseTime (similar to `sort_values(by='closed')`) ---
            return masterResults.OrderBy(r => r.CloseTime).ToList();
        }

        public virtual List<PositionHistoryResponseDto> GetPositionsCoinLong(List<FuturesAccountTradeResponseDto> df)
        {
            var results = new List<PositionHistoryResponseDto>();
            if (!df.Any()) return results;

            var symbol = df[0].Symbol;
            var side = df[0].PositionSide;  // "LONG"

            int startIndex = 0;
            while (startIndex < df.Count)
            {
                float currentPosition = 0f;
                float entryPriceSum = 0f;
                float avgClosePrice = 0f;
                float pnlSum = 0f;

                DateTime? openTime = null;
                DateTime? closeTime = null;
                int timesOpened = 0;
                int timesClosed = 0;
                bool positionClosed = false;

                for (int i = startIndex; i < df.Count; i++)
                {
                    var row = df[i];

                    // Start a new position if we are at 0 and see a positive Qty
                    if (Math.Abs(currentPosition) < 1e-8 && row.Quantity > 0)
                    {
                        openTime = row.Time;
                        entryPriceSum = 0f;
                        pnlSum = 0f;
                        avgClosePrice = 0f;
                        timesOpened = 0;
                        timesClosed = 0;
                    }

                    currentPosition += row.Quantity;

                    if (row.Quantity > 0)
                    {
                        // Opening trade => accumulate entry price
                        entryPriceSum += row.Price;
                        timesOpened++;
                    }
                    else
                    {
                        // Closing trade => add realized PnL
                        pnlSum += row.RealizedPnl;
                        avgClosePrice += row.Price;
                        timesClosed++;
                    }

                    closeTime = row.Time;

                    // Check if fully closed
                    if (Math.Abs(Math.Round(currentPosition, 8)) < 9E-4 | Math.Abs(Math.Round(currentPosition, 8)) == 0)
                    {
                        var position = new PositionHistoryResponseDto
                        {
                            Symbol = symbol,
                            PositionSide = side,
                            OpenTime = openTime ?? row.Time,
                            EntryPrice = timesOpened > 0 ? entryPriceSum / timesOpened : 0f,
                            PNL = (float)Math.Round(pnlSum, 2),
                            CloseTime = closeTime ?? row.Time,
                            AvgClosePrice = timesClosed > 0 ? avgClosePrice / timesClosed : 0f
                        };

                        results.Add(position);

                        startIndex = i + 1;
                        positionClosed = true;
                        break;  // exit for-loop
                    }
                }

                // If we never broke out -> no close found in this segment
                if (!positionClosed)
                {
                    startIndex++;
                }
            }

            return results;
        }

        public virtual List<PositionHistoryResponseDto> GetPositionsCoinShort(List<FuturesAccountTradeResponseDto> df)
        {
            var results = new List<PositionHistoryResponseDto>();
            if (!df.Any()) return results;

            var symbol = df[0].Symbol;
            var side = df[0].PositionSide;  // "SHORT"

            int startIndex = 0;
            while (startIndex < df.Count)
            {
                float currentPosition = 0f;
                float entryPriceSum = 0f;
                float avgClosePrice = 0f;
                float pnlSum = 0f;

                DateTime? openTime = null;
                DateTime? closeTime = null;
                int timesOpened = 0;
                int timesClosed = 0;
                bool positionClosed = false;

                for (int i = startIndex; i < df.Count; i++)
                {
                    var row = df[i];

                    // If flat and we see negative => new short
                    if (Math.Abs(currentPosition) < 1e-8 && row.Quantity < 0)
                    {
                        openTime = row.Time;
                        entryPriceSum = 0f;
                        pnlSum = 0f;
                        avgClosePrice = 0f;
                        timesOpened = 0;
                        timesClosed = 0;
                    }

                    currentPosition += row.Quantity;

                    if (row.Quantity < 0)
                    {
                        // Opening short position
                        entryPriceSum += row.Price;
                        timesOpened++;
                    }
                    else
                    {
                        // Closing short => add realized PnL
                        pnlSum += row.RealizedPnl;
                        avgClosePrice += row.Price;
                        timesClosed++;
                    }

                    closeTime = row.Time;

                    // If back to zero -> fully closed
                    if (Math.Abs(Math.Round(currentPosition, 8)) < 9E-4)
                    {
                        var position = new PositionHistoryResponseDto
                        {
                            Symbol = symbol,
                            PositionSide = side,
                            OpenTime = openTime ?? row.Time,
                            EntryPrice = timesOpened > 0 ? entryPriceSum / timesOpened : 0f,
                            PNL = (float)Math.Round(pnlSum, 2),
                            CloseTime = closeTime ?? row.Time,
                            AvgClosePrice = timesClosed > 0 ? avgClosePrice / timesClosed : 0f
                        };

                        results.Add(position);

                        startIndex = i + 1;
                        positionClosed = true;
                        break;
                    }
                }

                if (!positionClosed)
                {
                    startIndex++;
                }
            }

            return results;
        }

        












        #endregion private functions

    }
}
