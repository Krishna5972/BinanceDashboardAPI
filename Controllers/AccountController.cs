using Microsoft.AspNetCore.Mvc;

namespace BinanceDashboardAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpGet("GetBalance")]
        public IActionResult Balance()
        {
            return new JsonResult(new {balance = 6323});
        }
    }
}
