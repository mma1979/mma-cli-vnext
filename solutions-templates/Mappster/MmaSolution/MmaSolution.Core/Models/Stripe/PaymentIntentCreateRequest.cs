namespace MmaSolution.Core.Models.Stripe
{
    public class PaymentIntentCreateRequest
    {
        public long Amount { get; set; }
        public string Currency { get; set; }
        public string ReturnUrl { get; set; }
    }
}
