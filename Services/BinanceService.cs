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

        public async Task<List<FuturesAccountTradeResponseDto>> GetAccountTradesAsync()
        {
            if (_cache.TryGetValue("Binance_Account_Trades", out List<FuturesAccountTradeResponseDto> cachedAccountTrades))
                return cachedAccountTrades;

            var response = await FetchAccountTradesFromBinance();
            return response;
        }

        public async Task<List<FuturesAccountTradeResponseDto>> FetchAccountTradesFromBinance()
        {
            var jsonResponse = await _binanceApiClient.GetAsync(BinanceEndpoints.FuturesAccountTradesEndpoint);
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

            _cache.Set("Binance_Account_Trades", result, _cacheDuration);

            return result;
        }

        public async Task<List<FuturesIncomeHistoryResponseDto>> GetIncomeHistoryAsync()
        {
            if (_cache.TryGetValue("Binance_Income_History", out List<FuturesIncomeHistoryResponseDto> cachedIncomeHistory))
                return cachedIncomeHistory;

            var response = await FetchIncomeHistoryFromBinance();
            return response;
        }

        public async Task<List<FuturesIncomeHistoryResponseDto>> FetchIncomeHistoryFromBinance()
        {
            var jsonResponse = await _binanceApiClient.GetAsync(BinanceEndpoints.FuturesIncomeHistoryEndpoint);
            List<FuturesIncomeHistory> cachedIncomeHistory = JsonConvert.DeserializeObject<List<FuturesIncomeHistory>>(jsonResponse);


            var result = cachedIncomeHistory.Select(trade => new FuturesIncomeHistoryResponseDto
            {
                Symbol = trade.Symbol,
                IncomeType = trade.IncomeType,
                Income = string.IsNullOrWhiteSpace(trade.Income) ? 0.0f : Convert.ToSingle(trade.Income),
                Asset = trade.Asset,
                Info = trade.Info,
                Time = string.IsNullOrWhiteSpace(trade.Time) ? DateTime.MinValue : DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(trade.Time)).UtcDateTime,
                TranId = string.IsNullOrWhiteSpace(trade.TranId) ? 0 : Convert.ToInt64(trade.TranId),
                TradeID = string.IsNullOrWhiteSpace(trade.TradeID) ? 0 : Convert.ToInt64(trade.TradeID)
            }).ToList();


            
            _cache.Set("Binance_Income_History", result, _cacheDuration);

            return result;
        }

        public async Task<List<FutureOpenPositionsResponseDto>> GetOpenPositionsAsync()
        {
            if (_cache.TryGetValue("Binance_Open_Positions", out List<FutureOpenPositionsResponseDto> cachedOpenPositions))
                return cachedOpenPositions;

            var response = await FetchOpenPositionsFromBinance();
            return response;
        }

        public async Task<List<FutureOpenPositionsResponseDto>> FetchOpenPositionsFromBinance()
        {
            var jsonResponse = await _binanceApiClient.GetAsync(BinanceEndpoints.FuturesOpenPositionsEndpoint);
            List<FutureOpenPositions> cachedOpenPositions = JsonConvert.DeserializeObject<List<FutureOpenPositions>>(jsonResponse);

            cachedOpenPositions = cachedOpenPositions.Where(position => Convert.ToSingle(position.PositionAmt) != 0).ToList();

            var result = cachedOpenPositions.Select(position => new FutureOpenPositionsResponseDto
            {

                Symbol = position.Symbol,
                PositionSide = position.PositionSide,
                EntryPrice = Convert.ToSingle(position.EntryPrice),
                UnRealizedProfit = Convert.ToSingle(position.UnRealizedProfit),
                LiquidationPrice = Convert.ToSingle(position.LiquidationPrice),
                Notional = Convert.ToSingle(position.Notional)
            }).ToList();

        _cache.Set("Binance_Open_Positions", result, _cacheDuration);

        return result;
        }

        public async Task<List<FuturesOpenOrdersResponseDto>> GetOpenOrdersAsync()
        {
            if (_cache.TryGetValue("Binance_Open_Orders", out List<FuturesOpenOrdersResponseDto> cachedOpenOrders))
                return cachedOpenOrders;

            var response = await FetchOpenOrdersFromBinance();
            return response;
        }

        public async Task<List<FuturesOpenOrdersResponseDto>> FetchOpenOrdersFromBinance()
        {
            var jsonResponse = await _binanceApiClient.GetAsync(BinanceEndpoints.FuturesOpenOrdersEndpoint);
            List<FuturesOpenOrders> cachedOpenOrders = JsonConvert.DeserializeObject<List<FuturesOpenOrders>>(jsonResponse);

            Console.WriteLine(cachedOpenOrders);

            var result = cachedOpenOrders.Select(order => new FuturesOpenOrdersResponseDto
            {
                Symbol = order.Symbol,
                Price = Convert.ToSingle(order.Price) == 0 ? Convert.ToSingle(order.StopPrice) : Convert.ToSingle(order.Price),
                EntryType = GetOrderType(order.Side,order.PositionSide),
                OrderType = order.Type,
                Time = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(order.UpdateTime)).UtcDateTime,
            }).ToList();

            _cache.Set("Binance_Open_Orders", result, _cacheDuration);

            return result;
        }

        
        public async Task<List<PositionHistoryResponseDto>> GetPositionHistoryAsync()
        {
            var accountTrades = await GetAccountTradesAsync();

            var positions = ProcessTrades(accountTrades);

            return positions;


        }


        #region private functions

        private string GetOrderType(string side, string positionSide)
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



        public List<PositionHistoryResponseDto> ProcessTrades(List<FuturesAccountTradeResponseDto> trades)
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

                bool checkLong = dfLong.Count > 0;
                if (checkLong)
                {
                    float sumLongQty = dfLong.Sum(x => x.Quantity);
                    if (Math.Round(sumLongQty, 8) != 0)
                    {
                        issueCoins.Add(coin);
                        checkLong = false;
                    }
                }
                if (checkLong)
                {
                    var positionsLong = GetPositionsCoinLong(dfLong);
                    masterResults.AddRange(positionsLong);
                }

                // ========== SHORT side ==========
                var dfShort = tradesForSymbol
                    .Where(t => t.PositionSide.Equals("SHORT", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                bool checkShort = dfShort.Count > 0;
                if (checkShort)
                {
                    float sumShortQty = dfShort.Sum(x => x.Quantity);
                    if (Math.Round(sumShortQty, 8) != 0)
                    {
                        issueCoins.Add(coin);
                        checkShort = false;
                    }
                }
                if (checkShort)
                {
                    var positionsShort = GetPositionsCoinShort(dfShort);
                    masterResults.AddRange(positionsShort);
                }
            }

            // --- Sort final results by CloseTime (similar to `sort_values(by='closed')`) ---
            return masterResults.OrderBy(r => r.CloseTime).ToList();
        }

        private List<PositionHistoryResponseDto> GetPositionsCoinLong(List<FuturesAccountTradeResponseDto> df)
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
                    if (Math.Round(currentPosition, 8) == 0)
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

        private List<PositionHistoryResponseDto> GetPositionsCoinShort(List<FuturesAccountTradeResponseDto> df)
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
                    if (Math.Round(currentPosition, 8) == 0)
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
