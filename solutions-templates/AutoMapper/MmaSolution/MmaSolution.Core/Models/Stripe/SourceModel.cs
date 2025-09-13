namespace MmaSolution.Core.Models.Stripe
{
    public class SourceModel
    {
        public Currencies Currency { get; set; }
        public string Email { get; set; }
        public CardModel Card { get; set; }
    }
}
