using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Models.Entities
{
    public class FuturesAccountTrade
    {
        [JsonProperty("buyer")]
        public bool Buyer { get; set; }

        [JsonProperty("commission")]
        public string Commission { get; set; }

        [JsonProperty("commissionAsset")]
        public string CommissionAsset { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("maker")]
        public bool Maker { get; set; }

        [JsonProperty("orderId")]
        public long OrderId { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("qty")]
        public string Quantity { get; set; }

        [JsonProperty("quoteQty")]
        public string QuoteQuantity { get; set; }

        [JsonProperty("realizedPnl")]
        public string RealizedPnl { get; set; }

        [JsonProperty("side")]
        public string Side { get; set; }

        [JsonProperty("positionSide")]
        public string PositionSide { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }
    }
}
