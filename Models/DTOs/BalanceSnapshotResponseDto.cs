using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class BalanceSnapshotResponseDto
    {
        public DateOnly Date { get; set; }

        public float Balance { get; set; }
    }
}
