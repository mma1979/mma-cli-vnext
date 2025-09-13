namespace MmaSolution.Core.Models.Stripe
{
    public class CouponModel
    {
        public decimal Percent { get; set; }
        public string Duration { get; set; }
        public int DurationInMonth { get; set; }
    }
}
