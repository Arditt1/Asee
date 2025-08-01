namespace Asee.Models.ViewM
{
    public class TransactionRequest
    {
        public string TransactionId { get; set; }
        public string Type { get; set; }  // POS, ECOM, etc.
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public bool IsInternational { get; set; }

        public ClientDto Client { get; set; }
    }
}
