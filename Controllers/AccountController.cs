using Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BinanceDashboardAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {   
        private readonly IBinanceService _binanceService;
        public AccountController(IBinanceService binanceService) {
            _binanceService = binanceService;
        }



        [HttpGet("GetBalance")]
        public async Task<IActionResult> Balance()
        {
            var response = await _binanceService.GetBalanceAsync();
            return new JsonResult(response);
        }
    }
}
