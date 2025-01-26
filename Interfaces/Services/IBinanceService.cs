using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Services
{
    public interface IBinanceService
    {
        Task<FuturesAccountBalanceResponseDto> GetBalanceAsync();

        Task<List<FuturesAccountTradeResponseDto>> GetAccountTradesAsync();

        Task<List<FuturesIncomeHistoryResponseDto>> GetIncomeHistoryAsync();

        Task<List<FutureOpenPositionsResponseDto>> GetOpenPositionsAsync(); 

        Task<List<FuturesOpenOrdersResponseDto>> GetOpenOrdersAsync();

        Task<List<PositionHistoryResponseDto>> GetPositionHistoryAsync();

        Task<List<BalanceSnapshotResponseDto>> GetBalanceSnapshotAsync();

        Task<List<DailyPNLResponseDTO>> GetDailyPNLAsync();

        Task<List<MonthlySummaryResponseDto>> GetMonthlySummaryAsync();

        Task<List<HistoryResponseDto>> GetHistoryAsync();

        Task<DateTime> GetLastUpdatedTime();
    }
}
