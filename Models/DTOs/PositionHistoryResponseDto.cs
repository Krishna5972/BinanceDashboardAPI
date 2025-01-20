using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class PositionHistoryResponseDto
    {

        public string Symbol { get; set; }
        public string PositionSide { get; set; }
        public float EntryPrice { get; set; }
        public float AvgClosePrice { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }
        public float PNL { get; set; }
    }
}
