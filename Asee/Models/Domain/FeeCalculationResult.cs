namespace Asee.Models.Domain
{
    public class FeeCalculationResult
    {
        public string TransactionId { get; set; }
        public decimal TotalFee { get; set; }
        public decimal TotalAmountWithFee { get; set; }
        public List<AppliedRuleResult> AppliedRules { get; set; } = new();
    }
}
