using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class FuturesIncomeHistory
    {
        [JsonProperty("symbol")]
        public string? Symbol { get; set; }

        [JsonProperty("incomeType")]
        public string IncomeType { get; set; }

        [JsonProperty("income")]
        public string Income { get; set; }

        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("info")]
        public string Info { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("tranId")]
        public string TranId { get; set; }

        [JsonProperty("tradeId")]
        public string? TradeID { get; set; }
    }

}
