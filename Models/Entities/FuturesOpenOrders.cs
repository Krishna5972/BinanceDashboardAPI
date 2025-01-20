using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class FuturesOpenOrders
    {
            [JsonProperty("avgPrice")]
            public string AvgPrice { get; set; }

            [JsonProperty("clientOrderId")]
            public string ClientOrderId { get; set; }

            [JsonProperty("cumQuote")]
            public string CumQuote { get; set; }

            [JsonProperty("executedQty")]
            public string ExecutedQty { get; set; }

            [JsonProperty("orderId")]
            public long OrderId { get; set; }

            [JsonProperty("origQty")]
            public string OrigQty { get; set; }

            [JsonProperty("origType")]
            public string OrigType { get; set; }

            [JsonProperty("price")]
            public string Price { get; set; }

            [JsonProperty("reduceOnly")]
            public bool ReduceOnly { get; set; }

            [JsonProperty("side")]
            public string Side { get; set; }

            [JsonProperty("positionSide")]
            public string PositionSide { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("stopPrice")]
            public string StopPrice { get; set; }

            [JsonProperty("closePosition")]
            public bool ClosePosition { get; set; }

            [JsonProperty("symbol")]
            public string Symbol { get; set; }

            [JsonProperty("time")]
            public string Time { get; set; }

            [JsonProperty("timeInForce")]
            public string TimeInForce { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("activatePrice")]
            public string ActivatePrice { get; set; }

            [JsonProperty("priceRate")]
            public string PriceRate { get; set; }

            [JsonProperty("updateTime")]
            public string UpdateTime { get; set; }

            [JsonProperty("workingType")]
            public string WorkingType { get; set; }

            [JsonProperty("priceProtect")]
            public bool PriceProtect { get; set; }

            [JsonProperty("priceMatch")]
            public string PriceMatch { get; set; }

            [JsonProperty("selfTradePreventionMode")]
            public string SelfTradePreventionMode { get; set; }

            [JsonProperty("goodTillDate")]
            public long GoodTillDate { get; set; }

}
}
