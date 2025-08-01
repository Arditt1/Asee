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

        public FeeController(FeeCalculator calculator, FeeHistoryService history)
        {
            _calculator = calculator;
            _history = history;
        }

        //[HttpGet("ping")]
        //public IActionResult Ping()
        //{
        //    return Ok("API is reachable");
        //}

        /// <summary>
        /// Calculate fee for a single transaction
        /// </summary>
        [HttpPost("calculate")]
        public async Task<ActionResult<FeeResponse>> Calculate([FromBody] TransactionRequest request)
        {
            var context = Mapper.ToDomain(request);
            var result = _calculator.Calculate(context);
            var response = Mapper.ToResponse(result);

            await _history.SaveAsync(request, response, request.TransactionId);
            return Ok(response);
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

        [HttpPost("batch")]
        public async Task<ActionResult<List<FeeResponse>>> CalculateBatch([FromBody] List<TransactionRequest> requests)
        {
            const int maxDegreeOfConcurrency = 100; // Maximum number of concurrent tasks
            var semaphore = new SemaphoreSlim(maxDegreeOfConcurrency);

            // Create a list to hold the Task<FeeResponse> objects
            var tasks = requests.Select(async request =>
            {
                await semaphore.WaitAsync(); // Acquire the semaphore

                try
                {
                    var context = Mapper.ToDomain(request);
                    var result = _calculator.Calculate(context);
                    var response = Mapper.ToResponse(result);

                    await _history.SaveAsync(request, response, request.TransactionId);
                    return response;
                }
                finally
                {
                    semaphore.Release(); // Release the semaphore
                }
            }).ToList();

            // Await all tasks to complete, ensure we use Task.WhenAll on the Task<FeeResponse> list
            var responses = await Task.WhenAll(tasks);

            // Convert the responses to a List and return
            return Ok(responses.OrderBy(r => r.TransactionId).ToList());
        }


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
