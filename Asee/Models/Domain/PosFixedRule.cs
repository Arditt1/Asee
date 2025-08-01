using Asee.Models.Domain;
using Asee.Rules;

namespace Asee.Rules
{
    public class PosFixedRule : IFeeRule
    {
        public bool IsMatch(TransactionContext tx)
        {
            return tx.Type.Equals("POS", StringComparison.OrdinalIgnoreCase);
        }

        public AppliedRuleResult Apply(TransactionContext tx)
        {
            decimal fee = 0m;
            if (tx.Amount <= 100m)
                fee = 0.20m;
            else
                fee = tx.Amount * 0.002m; // 0.2%

            return new AppliedRuleResult
            {
                RuleId = "POS_FIXED",
                Description = "Fixed €0.20 for POS <= 100€, otherwise 0.2%",
                Amount = fee
            };
        }
    }
}
