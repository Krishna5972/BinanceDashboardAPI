using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class FuturesAccountBalance
    {
        public string AccountAlias { get; set; }              // Unique account code
        public string Asset { get; set; }                     // Asset name
        public float Balance { get; set; }                  // Wallet balance
        public float CrossWalletBalance { get; set; }       // Crossed wallet balance
        public float CrossUnPnl { get; set; }               // Unrealized profit of crossed positions
        public float AvailableBalance { get; set; }         // Available balance
        public float MaxWithdrawAmount { get; set; }        // Maximum withdrawal amount
        public bool MarginAvailable { get; set; }             // Whether the asset can be used as margin
        public long UpdateTime { get; set; }                  // Update timestamp
    }
}
