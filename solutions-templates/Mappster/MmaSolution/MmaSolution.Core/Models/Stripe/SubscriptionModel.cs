namespace MmaSolution.Core.Models.Stripe
{
    public class SubscriptionModel
    {
        public string CustomerId { get; set; }
        public string PriceId { get; set; }
        public string SourceId { get; set; }
    }

    public class SubscriptionModifyModel
    {
        public string CustomerName { get; set; }
        public string ProductId { get; set; }
        public string CustomerId { get; set; }
        public string PriceId { get; set; }
        public SourceModel Source { get; set; }

    }
}
