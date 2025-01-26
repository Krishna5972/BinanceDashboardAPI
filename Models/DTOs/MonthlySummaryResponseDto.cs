using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class MonthlySummaryResponseDto
    {
        public Int16 Month { get; set; }

        public Int16 Year { get; set; }

        public float PNL { get; set; }  

        public float DailyAverage { get; set; }
    }
}
