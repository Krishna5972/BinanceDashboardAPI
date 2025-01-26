using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class DailyPNLResponseDTO
    {
        public DateOnly Date { get; set; }

        public float PNL { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
