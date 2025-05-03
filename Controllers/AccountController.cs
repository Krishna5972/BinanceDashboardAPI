using Common.Settings;
using Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models.DTOs;

namespace BinanceDashboardAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {   
        private readonly IBinanceService _binanceService;
        private readonly ApiSettings _apiSettings;
        public AccountController(IBinanceService binanceService, IOptions<ApiSettings> apiSettings) {
            _binanceService = binanceService;
            _apiSettings = apiSettings.Value;
        }

        [HttpGet("GetBalance")]
        public async Task<IActionResult> GetBalance()
        {
            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_apiSettings.TimeoutSeconds));

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
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_apiSettings.TimeoutSeconds));

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
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_apiSettings.TimeoutSeconds));

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
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_apiSettings.TimeoutSeconds));

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
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_apiSettings.TimeoutSeconds));

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
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_apiSettings.TimeoutSeconds));

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


        [HttpGet("GetBalanceSnapshot")]
        public async Task<IActionResult> GetBalanceSnapshot()
        {
            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_apiSettings.TimeoutSeconds));

                var response = await _binanceService.GetBalanceSnapshotAsync().WaitAsync(cts.Token);

                if (response is null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Message = "Balance Snapshot not found.",
                        Error = "Balance Snapshot  information could not be retrieved."
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
                    Message = "An error occurred while retrieving Balance Snapshot.",
                    Error = ex.Message
                };
                var result = new ObjectResult(errorResponse);
                result.StatusCode = 500;
                return result;
            }
        }


        [HttpGet("GetDailyPNL")]
        public async Task<IActionResult> GetDailyPNL()
        {
            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_apiSettings.TimeoutSeconds));

                var response = await _binanceService.GetDailyPNLAsync().WaitAsync(cts.Token);

                if (response is null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Message = "Daily PNL not found.",
                        Error = "Daily PNL  information could not be retrieved."
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
                    Message = "An error occurred while retrieving Daily PNL.",
                    Error = ex.Message
                };
                var result = new ObjectResult(errorResponse);
                result.StatusCode = 500;
                return result;
            }
        }


        [HttpGet("GetMonthlySummary")]
        public async Task<IActionResult> GetMonthlySummary()
        {
            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_apiSettings.TimeoutSeconds));

                var response = await _binanceService.GetMonthlySummaryAsync().WaitAsync(cts.Token);

                if (response is null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Message = "Monthly Summary not found.",
                        Error = "Monthly Summary  information could not be retrieved."
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
                    Message = "An error occurred while retrieving Monthly Summary.",
                    Error = ex.Message
                };
                var result = new ObjectResult(errorResponse);
                result.StatusCode = 500;
                return result;
            }
        }

        [HttpGet("GetHistory")]
        public async Task<IActionResult> GetHistory()
        {
            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_apiSettings.TimeoutSeconds));

                var response = await _binanceService.GetHistoryAsync().WaitAsync(cts.Token);

                if (response is null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Message = "History Summary not found.",
                        Error = "History Summary  information could not be retrieved."
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
                    Message = "An error occurred while retrieving History.",
                    Error = ex.Message
                };
                var result = new ObjectResult(errorResponse);
                result.StatusCode = 500;
                return result;
            }
        }


        [HttpGet("GetLastUpdatedTime")]
        public async Task<IActionResult> GetLastUpdatedTime()
        {
            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_apiSettings.TimeoutSeconds));

                var dateTime = await _binanceService.GetLastUpdatedTime().WaitAsync(cts.Token);

                return Ok(new {LastUpdatedTime = dateTime });
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
                    Message = "An error occurred while retrieving Last Updated Time.",
                    Error = ex.Message
                };
                var result = new ObjectResult(errorResponse);
                result.StatusCode = 500;
                return result;
            }
        }
    }
}
