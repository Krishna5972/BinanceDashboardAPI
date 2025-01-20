using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class FuturesIncomeHistoryResponseDto
    {
        public string Symbol { get; set; }
        public string IncomeType { get; set; }
        public float Income { get; set; }
        public string Asset { get; set; }
        public string Info { get; set; }
        public DateTime Time { get; set; }
        public long TranId { get; set; }
        public long TradeID { get; set; }
    }
}
