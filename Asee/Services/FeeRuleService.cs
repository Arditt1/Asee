using Asee.Data;
using Asee.Models.Domain;
using Asee.Models.Entities;
using Asee.Rules;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class FeeRuleService
{
    private readonly FeeDbContext _dbContext;

    public FeeRuleService(FeeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Get all active fee rules
    public async Task<List<FeeRules>> GetActiveRulesAsync()
    {
        return await _dbContext.FeeRules
                               .Where(r => r.IsActive)
                               .ToListAsync();
    }

    // Get a specific rule by RuleType
    public async Task<List<FeeRules>> GetRulesByTypeAsync(string ruleType)
    {
        return await _dbContext.FeeRules
                               .Where(r => r.IsActive && r.RuleType == ruleType) // Filter by RuleType
                               .ToListAsync();
    }

    // Add or update a fee rule
    public async Task AddOrUpdateRuleAsync(FeeRules rule)
    {
        var existingRule = await _dbContext.FeeRules
                                           .FirstOrDefaultAsync(r => r.RuleId == rule.RuleId);

        if (existingRule == null)
        {
            // Add new rule
            _dbContext.FeeRules.Add(rule);
        }
        else
        {
            // Update existing rule
            existingRule.RuleType = rule.RuleType;
            existingRule.Description = rule.Description;
            existingRule.Criteria = rule.Criteria;
            existingRule.Action = rule.Action;
            existingRule.Amount = rule.Amount;
            existingRule.IsActive = rule.IsActive;
        }

        await _dbContext.SaveChangesAsync();
    }

    // Disable a rule
    public async Task DisableRuleAsync(string ruleId)
    {
        var rule = await _dbContext.FeeRules
                                   .FirstOrDefaultAsync(r => r.RuleId == ruleId);

        if (rule != null)
        {
            rule.IsActive = false;
            await _dbContext.SaveChangesAsync();
        }
    }
}

