using Asee.Models.Domain;
using Asee.Rules;

namespace Asee.Services
{
    public class FeeCalculator
    {
        private readonly IEnumerable<IFeeRule> _rules;

        public FeeCalculator(IEnumerable<IFeeRule> rules)
        {
            _rules = rules;
        }

        public FeeCalculationResult Calculate(TransactionContext tx)
        {
            var result = new FeeCalculationResult
            {
                TransactionId = tx.TransactionId,
                TotalFee = 0,
                AppliedRules = new List<AppliedRuleResult>()
            };

            foreach (var rule in _rules)
            {
                if (rule.IsMatch(tx))
                {
                    var ruleResult = rule.Apply(tx);
                    result.AppliedRules.Add(ruleResult);
                    result.TotalFee += ruleResult.Amount;
                }
            }
            decimal allAdjustments = result.AppliedRules.Sum(r => r.Amount);
            result.TotalAmountWithFee = tx.Amount + allAdjustments;
            //result.TotalAmountWithFee = tx.Amount + result.TotalFee;

            return result;
        }
    }
}
