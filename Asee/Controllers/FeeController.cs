using Asee.Services;
using Asee.Helper;
using Asee.Models.Entities;
using Asee.Models.ViewM;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace Asee.Controllers
{
    [ApiController]
    [Route("api/fee")]
    public class FeeController : ControllerBase
    {
        private readonly FeeCalculator _calculator;
        private readonly FeeHistoryService _history;
        private readonly FeeRuleService _feeRuleService;

        public FeeController(FeeCalculator calculator, FeeHistoryService history, FeeRuleService feeRuleService)
        {
            _calculator = calculator;
            _history = history;
            _feeRuleService = feeRuleService;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("API is reachable");
        }

        /// <summary>
        /// Calculate fee for a single transaction
        /// </summary>
        //[HttpPost("calculate")]
        //public async Task<ActionResult<FeeResponse>> Calculate([FromBody] TransactionRequest request)
        //{
        //    var context = Mapper.ToDomain(request);
        //    var result = _calculator.Calculate(context);
        //    var response = Mapper.ToResponse(result);

        //    await _history.SaveAsync(request, response, request.TransactionId);
        //    return Ok(response);
        //}

        [HttpPost("calculate")]
        public async Task<ActionResult<FeeResponse>> Calculate([FromBody] TransactionRequest request)
        {
            var context = Mapper.ToDomain(request);

            var result = _calculator.Calculate(context);

            var response = Mapper.ToResponse(result);

            await _history.SaveAsync(request, response, request.TransactionId);

            return Ok(response);
        }

        [HttpPost("batch")]
        public async Task<ActionResult<List<FeeResponse>>> CalculateBatch([FromBody] List<TransactionRequest> requests)
        {
            var responses = new List<FeeResponse>();

            var activeRules = await _feeRuleService.GetActiveRulesAsync();

            if (activeRules == null || activeRules.Count == 0)
            {
                return NotFound("No active rules found.");
            }

            var tasks = requests.Select(async request =>
            {
                var context = Mapper.ToDomain(request);

                var filteredRules = activeRules.Where(rule => rule.RuleType == request.Type || rule.RuleType == "CreditScoreDiscount").ToList();

                var result = _calculator.Calculate(context);

                var response = Mapper.ToResponse(result);

                await _history.SaveAsync(request, response, request.TransactionId);

                return response;
            }).ToList();

            responses = (await Task.WhenAll(tasks)).ToList();

            return Ok(responses.OrderBy(r => r.TransactionId).ToList());
        }

        /// <summary>
        /// Calculate fees for a batch of transactions (parallel processing)
        /// </summary>
        //[HttpPost("batch")]
        //public async Task<ActionResult<List<FeeResponse>>> CalculateBatch([FromBody] List<TransactionRequest> requests)
        //{
        //    var responses = new ConcurrentBag<FeeResponse>();

        //    await Task.Run(() =>
        //    {
        //        Parallel.ForEach(requests, async request =>
        //        {
        //            var context = Mapper.ToDomain(request);
        //            var result = _calculator.Calculate(context);
        //            var response = Mapper.ToResponse(result);
        //            responses.Add(response);

        //            await _history.SaveAsync(request, response, request.TransactionId);
        //        });
        //    });

        //    return Ok(responses.OrderBy(r => r.TransactionId).ToList());
        //}

        //[HttpPost("batch")]
        //public async Task<ActionResult<List<FeeResponse>>> CalculateBatch([FromBody] List<TransactionRequest> requests)
        //{
        //    const int maxDegreeOfConcurrency = 100; // Maximum number of concurrent tasks
        //    var semaphore = new SemaphoreSlim(maxDegreeOfConcurrency);

        //    var tasks = requests.Select(async request =>
        //    {
        //        await semaphore.WaitAsync(); // Acquire the semaphore to limit concurrency

        //        try
        //        {
        //            // Fetch the fee rule dynamically from the database based on the rule type (e.g., "POS")
        //            var feeRule = await _feeRuleService.GetRulesByTypeAsync(request.Type); // Assuming `Type` is part of the request

        //            if (feeRule == null)
        //            {
        //                // If no rule found for the given Type, return an appropriate response
        //                return new FeeResponse
        //                {
        //                    TransactionId = request.TransactionId,
        //                    CalculatedFee = 0,
        //                    TotalAmountWithFee = request.Amount,
        //                    AppliedRules = new List<AppliedRuleDto> { new AppliedRuleDto { RuleId = "NoRule", Description = "No matching rule found", FeeComponent = 0 } }
        //                };
        //            }

        //            // Convert TransactionRequest to TransactionContext
        //            var context = Mapper.ToDomain(request);

        //            // Calculate the fee using the dynamic rule fetched from the database
        //            var result = _calculator.Calculate(context, feeRule);

        //            // Map the result to FeeResponse
        //            var response = Mapper.ToResponse(result);

        //            // Save the fee calculation history
        //            await _history.SaveAsync(request, response, request.TransactionId);

        //            return response;
        //        }
        //        finally
        //        {
        //            semaphore.Release(); // Release the semaphore
        //        }
        //    }).ToList();

        //    // Wait for all tasks to complete and return the responses
        //    var responses = await Task.WhenAll(tasks);

        //    // Convert the responses to a List and order by TransactionId
        //    return Ok(responses.OrderBy(r => r.TransactionId).ToList());
        //}


        /// <summary>
        /// View history of fee calculations
        /// </summary>
        [HttpGet("history")]
        public async Task<ActionResult<List<FeeCalculationHistory>>> History()
        {
            var history = await _history.GetAllAsync();
            return Ok(history);
        }
    }
}
