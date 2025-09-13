namespace MmaSolution.Core.Models.Stripe
{
    public class CardModel
    {
        public string Cvc { get; set; }
        public string Number { get; set; }
        public long? ExpMonth { get; set; }
        public long? ExpYear { get; set; }
    }
}
