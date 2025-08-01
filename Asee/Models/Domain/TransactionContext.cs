namespace Asee.Models.Domain
{
    public class TransactionContext
    {
        public string TransactionId { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public bool IsInternational { get; set; }
        public ClientProfile Client { get; set; }
    }
}
