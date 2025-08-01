namespace Asee.Models.ViewM
{
    public class FeeResponse
    {
        public string TransactionId { get; set; }
        public decimal CalculatedFee { get; set; }
        public decimal TotalAmountWithFee { get; set; }
        public List<AppliedRuleDto> AppliedRules { get; set; }
    }
}
