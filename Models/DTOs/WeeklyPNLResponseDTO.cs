using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class WeeklyPNLResponseDTO
    {
        public DateOnly WeekStartDate { get; set; }
        public DateOnly WeekEndDate { get; set; }
        public float WeeklyPNL { get; set; }
        public int ActualDays { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
