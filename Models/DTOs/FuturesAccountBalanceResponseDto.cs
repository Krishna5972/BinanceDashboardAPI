using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class FuturesAccountBalanceResponseDto
    {
        public decimal Balance { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
