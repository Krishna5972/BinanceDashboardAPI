using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Clients
{
    public interface IBinanceApiClient
    {
        Task<string> GetAsync(string endpoint, Dictionary<string, string> queryParams = null);
        Task<string> PostAsync(string endpoint, HttpContent content);
    }
}
