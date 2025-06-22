using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Comparers
{
    public class FuturesIncomeHistoryComparer : IEqualityComparer<FuturesIncomeHistoryResponseDto>
    {
        private const int Precision = 8;

        public bool Equals(FuturesIncomeHistoryResponseDto x, FuturesIncomeHistoryResponseDto y)
        {
            if (x == null || y == null)
                return false;

            return x.Symbol == y.Symbol &&
                   x.IncomeType == y.IncomeType &&
                   Math.Abs(x.Income - y.Income) < Math.Pow(10, -Precision) &&
                   x.Asset == y.Asset &&
                   x.Info == y.Info &&
                   x.Time == y.Time &&
                   x.TranID == y.TranID &&
                   x.TradeID == y.TradeID;
        }

        public int GetHashCode(FuturesIncomeHistoryResponseDto obj)
        {
            if (obj == null)
                return 0;

            return HashCode.Combine(
                obj.Symbol,
                obj.IncomeType,
                Math.Round(obj.Income, Precision),
                obj.Asset,
                obj.Info,
                obj.Time,
                obj.TranID,
                obj.TradeID
            );
        }
    }
}

