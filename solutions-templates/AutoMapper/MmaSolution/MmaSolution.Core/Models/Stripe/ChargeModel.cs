namespace MmaSolution.Core.Models.Stripe
{
    public class ChargeModel
    {
        public long? Amount { get; set; }
        public Currencies Currency { get; set; }
        public string SourceId { get; set; }
        public string CustomerId { get; set; }
        public string TokenId { get; set; }
        public string Description { get; set; }
    }
}
