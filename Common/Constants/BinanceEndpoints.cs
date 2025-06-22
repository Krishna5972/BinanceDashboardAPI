using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Constants
{
    public class BinanceEndpoints
    {
        public const string FuturesBalanceEndpoint = "fapi/v3/balance";
        public const string FuturesAccountTradesEndpoint = "fapi/v1/userTrades";
        public const string FuturesIncomeHistoryEndpoint = "/fapi/v1/income";
        public const string FuturesOpenPositionsEndpoint = "/fapi/v3/positionRisk"; 
        public const string FuturesOpenOrdersEndpoint = "/fapi/v1/openOrders";
        public const string FuturesTickerPrice = "fapi/v1/ticker/price";
    }
}
