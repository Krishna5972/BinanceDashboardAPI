using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class FutureOpenPositions
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("positionSide")]
        public string PositionSide { get; set; }

        [JsonProperty("positionAmt")]
        public string PositionAmt { get; set; }

        [JsonProperty("entryPrice")]
        public string EntryPrice { get; set; }

        [JsonProperty("breakEvenPrice")]
        public string BreakEvenPrice { get; set; }

        [JsonProperty("markPrice")]
        public string MarkPrice { get; set; }

        [JsonProperty("unRealizedProfit")]
        public string UnRealizedProfit { get; set; }

        [JsonProperty("liquidationPrice")]
        public string LiquidationPrice { get; set; }

        [JsonProperty("isolatedMargin")]
        public string IsolatedMargin { get; set; }

        [JsonProperty("notional")]
        public string Notional { get; set; }

        [JsonProperty("marginAsset")]
        public string MarginAsset { get; set; }

        [JsonProperty("isolatedWallet")]
        public string IsolatedWallet { get; set; }

        [JsonProperty("initialMargin")]
        public string InitialMargin { get; set; }

        [JsonProperty("maintMargin")]
        public string MaintMargin { get; set; }

        [JsonProperty("positionInitialMargin")]
        public string PositionInitialMargin { get; set; }

        [JsonProperty("openOrderInitialMargin")]
        public string OpenOrderInitialMargin { get; set; }

        [JsonProperty("adl")]
        public int Adl { get; set; }

        [JsonProperty("bidNotional")]
        public string BidNotional { get; set; }

        [JsonProperty("askNotional")]
        public string AskNotional { get; set; }

        [JsonProperty("updateTime")]
        public string UpdateTime { get; set; }
    }
}
