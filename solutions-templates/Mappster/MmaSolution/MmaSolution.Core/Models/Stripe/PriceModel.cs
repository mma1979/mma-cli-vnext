namespace MmaSolution.Core.Models.Stripe
{
    public class PriceModel
    {
        public string ProductId { get; set; }
        public long Amount { get; set; }
        public Currencies Currency { get; set; }
        public PriceIntervals Interval { get; set; }
    }

    public enum PriceIntervals
    {
        day,
        week,
        month,
        year
    }

    public enum Currencies
    {
        usd,
        eru,
        egp,
        sar
    }
}
