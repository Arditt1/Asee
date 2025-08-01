using Asee.Helper;
using Asee.Models.Domain;
using Asee.Models.Entities;
using Asee.Models.ViewM;
using Asee.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace Asee.Controllers
{
    [Route("api/rules")]
    [ApiController]
    public class FeeRuleController : ControllerBase
    {
        private readonly FeeRuleService _feeRuleService;

        public FeeRuleController(FeeRuleService feeRuleService)
        {
            _feeRuleService = feeRuleService;
        }

        // Get all active fee rules
        [HttpGet]
        public async Task<IActionResult> GetActiveRules()
        {
            var rules = await _feeRuleService.GetActiveRulesAsync();
            return Ok(rules);
        }

        // Add or Update a fee rule
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateRule([FromBody] FeeRules rule)
        {
            await _feeRuleService.AddOrUpdateRuleAsync(rule);
            return Ok();
        }

        // Disable a fee rule
        [HttpDelete("{ruleId}")]
        public async Task<IActionResult> DisableRule(string ruleId)
        {
            await _feeRuleService.DisableRuleAsync(ruleId);
            return Ok();
        }
    }

}
