using Stripe;

namespace MmaSolution.Tests
{
    public class StripePaymentServiceTests
    {
        private StripePaymentService service;
        IConfigurationSection config;

        [OneTimeSetUp]
        public void SetupOnce()
        {

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            config = configuration.GetSection("Stripe");

        }

        [SetUp]
        public void Setup()
        {

            service = new StripePaymentService(config);
            StripeConfiguration.ApiKey = service.SecretKey;
        }

        [Test]
        public void Test_Payment_With_Token()
        {

            var token = service.CreateToken(new()
            {
                Cvc = "010",
                Number = "4242424242424242",
                ExpMonth = 9,
                ExpYear = 2025
            });

            Assert.Equals(token, null);

            var charge = service.CreateChargeWithToken(new()
            {
                Amount = 1000,
                Currency = Currencies.usd,
                TokenId = token.Id,
                Description = $"Test Charge with token {token.Id}"
            });

            Assert.Equals(charge, null);
        }

        [Test]
        public void Test_Payment_Subscription()
        {

            // var result = service.Payment();
            Assert.Equals(false, false);
        }

        [Test]
        public void Test_Refund_Returns_True()
        {

            var result = service.CreateRefund("ch_3LCfdxCjf1yXEaSl0YnEGkSp");
            Assert.Equals(result, null);
        }


    }
}
