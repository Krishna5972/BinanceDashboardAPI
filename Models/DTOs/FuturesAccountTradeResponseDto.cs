using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class FuturesAccountTradeResponseDto
    {

        public string Symbol { get; set; }
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string Side { get; set; }
        public float Price { get; set; }
        public float Quantity { get; set; }
        public float RealizedPnl { get; set; }
        public float QuoteQuantity { get; set; }
        public float Commission { get; set; }
        public string PositionSide { get; set; }
        public bool Buyer { get; set; }
        public bool Maker { get; set; }
        public DateTime Time { get; set; }


    }
}
