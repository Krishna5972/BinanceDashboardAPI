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
        
    }
}
