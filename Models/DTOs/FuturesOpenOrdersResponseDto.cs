using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class FuturesOpenOrdersResponseDto
    {
        public float Price { get; set; }
        public string Symbol { get; set; }
        public DateTime Time { get; set; }
        public string EntryType { get; set; }

        public string OrderType { get; set; }

    }
}
