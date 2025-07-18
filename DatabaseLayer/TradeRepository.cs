﻿using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Models.DTOs;
using Repository.Context;
using Interfaces.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace DatabaseLayer
{
    public class TradeRepository : ITradeRepository
    {
        private readonly DbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration;

        public TradeRepository(DbContext dbContext, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
            _cacheDuration = TimeSpan.FromHours(24);

        }

        public async Task<IEnumerable<FuturesAccountTradeResponseDto>> GetAllAccountTradesAsync()
        {
            const string cacheKey = "AllAccountTradesDB";

            // Try to get data from the cache
            if (_cache.TryGetValue(cacheKey, out IEnumerable<FuturesAccountTradeResponseDto> cachedTrades))
            {
                return cachedTrades;
            }

            var accountTrades = new List<FuturesAccountTradeResponseDto>();

            using (var connection = _dbContext.GetConnection())
            {
                string query = @"SELECT * FROM (
                                               SELECT TOP 10000 * 
                                               FROM [dbo].[TradeDetails]
                                               ORDER BY TradeDetailsID DESC
                                            ) AS LatestRecords
                                            ORDER BY Time ASC;";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var trade = new FuturesAccountTradeResponseDto
                            {
                                Symbol = reader["Symbol"]?.ToString(),
                                Id = reader["ID"] != DBNull.Value ? Convert.ToInt64(reader["ID"]) : 0,
                                OrderId = reader["OrderID"] != DBNull.Value ? Convert.ToInt64(reader["OrderID"]) : 0,
                                Side = reader["Side"]?.ToString(),
                                Price = reader["Price"] != DBNull.Value ? Convert.ToSingle(reader["Price"]) : 0f,
                                Quantity = reader["Qty"] != DBNull.Value ? Convert.ToSingle(reader["Qty"]) : 0f,
                                RealizedPnl = reader["RealizedPnl"] != DBNull.Value ? Convert.ToSingle(reader["RealizedPnl"]) : 0f,
                                QuoteQuantity = reader["QuoteQty"] != DBNull.Value ? Convert.ToSingle(reader["QuoteQty"]) : 0f,
                                Commission = reader["Commission"] != DBNull.Value ? Convert.ToSingle(reader["Commission"]) : 0f,
                                Time = reader["Time"] != DBNull.Value
                                    ? ((DateTimeOffset)reader["Time"]).UtcDateTime
                                    : DateTime.MinValue,
                                PositionSide = reader["PositionSide"]?.ToString(),
                                Buyer = reader["Buyer"] != DBNull.Value && Convert.ToBoolean(reader["Buyer"]),
                                Maker = reader["Maker"] != DBNull.Value && Convert.ToBoolean(reader["Maker"])
                            };

                            accountTrades.Add(trade);
                        }
                    }
                }
            }

            _cache.Set(cacheKey, accountTrades, _cacheDuration);

            return accountTrades;
        }

        public async Task<IEnumerable<BalanceSnapshotResponseDto>> GetBalanceSnapshotAsync()
        {
            const string cacheKey = "BalanceDB";

            // Try to get data from the cache
            if (_cache.TryGetValue(cacheKey, out IEnumerable<BalanceSnapshotResponseDto> cachedBalanceSnaphot))
            {
                return cachedBalanceSnaphot;
            }

            var balanceSnapshot = new List<BalanceSnapshotResponseDto>();

            using (var connection = _dbContext.GetConnection())
            {
                string query = "SELECT * FROM [dbo].[AccountHistory]";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var snapshot = new BalanceSnapshotResponseDto
                            {
                                Balance = reader["Balance"] != DBNull.Value ? Convert.ToSingle(reader["Balance"]) : 0f,
                                Date = reader["Date"] != DBNull.Value
                                         ? DateOnly.FromDateTime(Convert.ToDateTime(reader["Date"]))  
                                         : DateOnly.MinValue
                            };

                            balanceSnapshot.Add(snapshot);
                        }
                    }
                }
            }

            // Cache the data
            _cache.Set(cacheKey, balanceSnapshot, _cacheDuration);

            return balanceSnapshot;
        }

        public async Task<IEnumerable<FuturesIncomeHistoryResponseDto>> GetAllIncomeHistoryAsync()
        {
            const string cacheKey = "AllIncomeHistoryDB";

            // Try to get data from the cache
            if (_cache.TryGetValue(cacheKey, out IEnumerable<FuturesIncomeHistoryResponseDto> cachedTrades))
            {
                return cachedTrades;
            }

            var accountTrades = new List<FuturesIncomeHistoryResponseDto>();

            using (var connection = _dbContext.GetConnection())
            {
                string query = @"SELECT * FROM (
                                                SELECT TOP 10000 * 
                                                FROM [dbo].[TradingData]
                                                ORDER BY TradingDataID DESC
                                            ) AS LatestRecords
                                            ORDER BY Time ASC;";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string asset = reader["Asset"]?.ToString();

                            var trade = new FuturesIncomeHistoryResponseDto
                            {
                                Symbol = reader["Symbol"]?.ToString(),
                                IncomeType = reader["IncomeType"]?.ToString(),
                                Income = asset == "BNB"
                                                    ? (reader["IncomeUSDT"] != DBNull.Value ? Convert.ToSingle(reader["IncomeUSDT"]) : 0f)
                                                    : (reader["Income"] != DBNull.Value ? Convert.ToSingle(reader["Income"]) : 0f),
                                Asset = asset,
                                Info = reader["Info"]?.ToString(),
                                TranID = reader["TranID"] != DBNull.Value ? Convert.ToInt64(reader["TranID"]) : 0,
                                TradeID = reader["TradeID"] != DBNull.Value ? Convert.ToInt64(reader["TradeID"]) : 0,
                                Time = reader["Time"] != DBNull.Value
                                    ? ((DateTimeOffset)reader["Time"]).UtcDateTime
                                    : DateTime.MinValue
                            };

                            accountTrades.Add(trade);
                        }
                    }
                }
            }

            _cache.Set(cacheKey, accountTrades, _cacheDuration);

            return accountTrades;
        }
        public async Task<IEnumerable<DailyPNLResponseDTO>> GetDailyPNLAsync()
        {
            const string cacheKey = "DailyPNLDB";

            // Try to get data from the cache
            if (_cache.TryGetValue(cacheKey, out IEnumerable<DailyPNLResponseDTO> cachedDailyPNL))
            {
                return cachedDailyPNL;
            }

            var dailyPNLList = new List<DailyPNLResponseDTO>();

            using (var connection = _dbContext.GetConnection())
            {
                string query = "SELECT * FROM [dbo].[DailyPNL]";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dailyPNL = new DailyPNLResponseDTO
                            {
                                PNL = reader["TotalIncome"] != DBNull.Value ? Convert.ToSingle(reader["TotalIncome"]) : 0f,
                                Date = reader["TransactionDate"] != DBNull.Value
                                         ? DateOnly.FromDateTime(Convert.ToDateTime(reader["TransactionDate"]))
                                         : DateOnly.MinValue,
                                LastUpdated = reader["LastUpdated"]!= DBNull.Value ? Convert.ToDateTime(reader["TransactionDate"]) : DateTime.MinValue
                            };

                            dailyPNLList.Add(dailyPNL);
                        }
                    }
                }
            }

            // Cache the data
            _cache.Set(cacheKey, dailyPNLList, _cacheDuration);

            return dailyPNLList;
        }
    }
}

