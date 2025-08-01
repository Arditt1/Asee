using Asee.Models.Domain;

namespace Asee.Rules
{
    public interface IFeeRule
    {
        bool IsMatch(TransactionContext tx);
        AppliedRuleResult Apply(TransactionContext tx);
    }
}
