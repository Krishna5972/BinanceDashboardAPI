using Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;

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
            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

                var response = await _binanceService.GetBalanceAsync().WaitAsync(cts.Token);

                if (response is null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Message = "Balance not found.",
                        Error = "The balance information could not be retrieved."
                    });
                }

                return Ok(response);  
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Invalid request.", 
                    Error = ex.Message
                });
            }
            catch (TaskCanceledException)
            {
                ErrorResponse errorResponse = new ErrorResponse()
                {
                    Message = "The request timed out.",
                    Error = "The service took too long to respond."
                };
                var result = new ObjectResult(errorResponse);
                result.StatusCode = 504;
                return result;
            }
            catch (Exception ex)
            {
                ErrorResponse errorResponse = new ErrorResponse()
                {
                    Message = "An error occurred while retrieving the balance.",
                    Error = ex.Message
                };  
                var result = new ObjectResult(errorResponse);
                result.StatusCode = 500;
                return result;
            }
        }
    }
}
