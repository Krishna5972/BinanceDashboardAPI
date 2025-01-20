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
        public async Task<IActionResult> GetBalance()
        {
            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

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

        [HttpGet("GetAccountTrades")]
        public async Task<IActionResult> GetAccountTrades()
        {
            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

                var response = await _binanceService.GetAccountTradesAsync().WaitAsync(cts.Token);

                if (response is null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Message = "Account Trades not found.",
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
                    Message = "An error occurred while retrieving the Account Trades.",
                    Error = ex.Message
                };
                var result = new ObjectResult(errorResponse);
                result.StatusCode = 500;
                return result;
            }
        }


        [HttpGet("GetIncomeHistory")]
        public async Task<IActionResult> GetIncomeHistory()
        {
            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

                var response = await _binanceService.GetIncomeHistoryAsync().WaitAsync(cts.Token);

                if (response is null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Message = "Income History not found.",
                        Error = "Income History information could not be retrieved."
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
                    Message = "An error occurred while retrieving the Income History.",
                    Error = ex.Message
                };
                var result = new ObjectResult(errorResponse);
                result.StatusCode = 500;
                return result;
            }
        }


        [HttpGet("GetOpenPositions")]
        public async Task<IActionResult> GetOpenPositions()
        {
            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

                var response = await _binanceService.GetOpenPositionsAsync().WaitAsync(cts.Token);

                if (response is null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Message = "Open Positions not found.",
                        Error = "Open Positions  information could not be retrieved."
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
                    Message = "An error occurred while retrieving the Open Positions.",
                    Error = ex.Message
                };
                var result = new ObjectResult(errorResponse);
                result.StatusCode = 500;
                return result;
            }
        }

        [HttpGet("GetOpenOrders")]
        public async Task<IActionResult> GetOpenOrders()
        {
            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

                var response = await _binanceService.GetOpenOrdersAsync().WaitAsync(cts.Token);

                if (response is null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Message = "Open Orders not found.",
                        Error = "Open Orders  information could not be retrieved."
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
                    Message = "An error occurred while retrieving the Open Orders.",
                    Error = ex.Message
                };
                var result = new ObjectResult(errorResponse);
                result.StatusCode = 500;
                return result;
            }
        }


        [HttpGet("GetPositionHistory")]
        public async Task<IActionResult> GetPositionHistory()
        {
            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

                var response = await _binanceService.GetPositionHistoryAsync().WaitAsync(cts.Token);

                if (response is null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Message = "Open Orders not found.",
                        Error = "Open Orders  information could not be retrieved."
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
                    Message = "An error occurred while retrieving the Open Orders.",
                    Error = ex.Message
                };
                var result = new ObjectResult(errorResponse);
                result.StatusCode = 500;
                return result;
            }
        }
    }
}
