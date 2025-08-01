using Asee.Models.Domain;
using Asee.Rules;

namespace Asee.Rules
{
    public class ECommerceCapRule : IFeeRule
    {
        public bool IsMatch(TransactionContext tx)
        {
            return tx.Type.Equals("ECOM", StringComparison.OrdinalIgnoreCase) ||
                   tx.Type.Equals("E-COMMERCE", StringComparison.OrdinalIgnoreCase);
        }

        public AppliedRuleResult Apply(TransactionContext tx)
        {
            decimal fee = tx.Amount * 0.018m + 0.15m; // 1.8% + 0.15€
            if (fee > 120m)
                fee = 120m;

            return new AppliedRuleResult
            {
                RuleId = "ECOM_CAP",
                Description = "1.8% + €0.15, capped at €120 for e-commerce",
                Amount = fee
            };
        }
    }
}
