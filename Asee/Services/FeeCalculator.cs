using Asee.Models.Domain;
using Asee.Rules;

namespace Asee.Services
{
    public class FeeCalculator
    {
        private readonly IEnumerable<IFeeRule> _rules;
        private readonly FeeRuleService _feeRuleService;

        public FeeCalculator(IEnumerable<IFeeRule> rules, FeeRuleService feeRuleService)
        {
            _rules = rules;
            _feeRuleService = feeRuleService;
        }

        //public FeeCalculationResult Calculate(TransactionContext tx)
        //{
        //    var result = new FeeCalculationResult
        //    {
        //        TransactionId = tx.TransactionId,
        //        TotalFee = 0,
        //        AppliedRules = new List<AppliedRuleResult>()
        //    };

        //    foreach (var rule in _rules)
        //    {
        //        if (rule.IsMatch(tx))
        //        {
        //            var ruleResult = rule.Apply(tx);
        //            result.AppliedRules.Add(ruleResult);
        //            result.TotalFee += ruleResult.Amount;
        //        }
        //    }
        //    decimal allAdjustments = result.AppliedRules.Sum(r => r.Amount);
        //    result.TotalAmountWithFee = tx.Amount + allAdjustments;
        //    //result.TotalAmountWithFee = tx.Amount + result.TotalFee;

        //    return result;
        //}


        // This method calculates fees dynamically based on the rules that match

        public FeeCalculationResult Calculate(TransactionContext tx)
        {
            var result = new FeeCalculationResult
            {
                TransactionId = tx.TransactionId,
                TotalFee = 0,
                AppliedRules = new List<AppliedRuleResult>()
            };

            // Get all active rules from the database
            var activeRules = _feeRuleService.GetActiveRulesAsync().Result;

            foreach (var rule in activeRules)
            {
                // Evaluate if the rule applies to this transaction
                if (rule.IsMatch(tx, rule))
                {
                    var ruleResult = rule.Apply(tx, rule);  // Apply the matching rule dynamically
                    result.AppliedRules.Add(ruleResult);
                    result.TotalFee += ruleResult.Amount;  // Add the fee or discount to the total fee
                }
            }

            result.TotalAmountWithFee = tx.Amount + result.TotalFee;

            return result;
        }



    }
}
