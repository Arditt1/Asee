namespace Asee.Models.Entities
{
    public class FeeCalculationHistory
    {
        public Guid Id { get; set; }
        public string TransactionId { get; set; }
        public string InputJson { get; set; }
        public string OutputJson { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
