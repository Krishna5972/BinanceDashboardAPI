using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Models.DTOs;
using Repository.Context;
using Interfaces.Repository;

namespace DatabaseLayer
{
    public class TradeRepository : ITradeRepository
    {
        private readonly DbContext _dbContext;

        public TradeRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<FuturesAccountTradeResponseDto>> GetAllAccountTradesAsync()
        {
            var accountTrades = new List<FuturesAccountTradeResponseDto>();

            using (var connection = _dbContext.GetConnection())
            {
                string query = "SELECT * FROM [dbo].[TradeDetails]";
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
                                                                    ? ((DateTimeOffset)reader["Time"]).UtcDateTime  // Correct handling of DateTimeOffset
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
            return accountTrades;
        }
    }
}

