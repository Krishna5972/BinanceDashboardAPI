using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repository
{
    public interface ITradeRepository
    {
        Task<IEnumerable<FuturesAccountTradeResponseDto>> GetAllAccountTradesAsync();

        Task<IEnumerable<BalanceSnapshotResponseDto>> GetBalanceSnapshotAsync();

        Task<IEnumerable<FuturesIncomeHistoryResponseDto>> GetAllIncomeHistoryAsync();

        Task<IEnumerable<DailyPNLResponseDTO>> GetDailyPNLAsync();
    }
}
