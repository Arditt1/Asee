using Asee.Models.Domain;
using Asee.Rules;

namespace Asee.Rules
{
    public class CreditScoreDiscountRule : IFeeRule
    {
        public bool IsMatch(TransactionContext tx)
        {
            return tx.Client != null && tx.Client.CreditScore > 400;
        }

        public AppliedRuleResult Apply(TransactionContext tx)
        {
            // This is a discount: 1% off on all fees
            // To implement this, we need to reduce the total fee by 1%
            // But rules are applied one by one, so we assume discount applies on total fee
            // We can handle this rule differently (later) or apply as negative fee here.
            // For simplicity, we return a negative fee equal to 1% of the transaction amount as discount.

            // Alternative: We can handle discount after summing fees — but let's do this per spec:
            decimal discountAmount = -(tx.Amount * 0.01m);

            return new AppliedRuleResult
            {
                RuleId = "CREDIT_SCORE_DISCOUNT",
                Description = "1% discount on all transactions for creditScore > 400",
                Amount = discountAmount
            };
        }
    }
}
