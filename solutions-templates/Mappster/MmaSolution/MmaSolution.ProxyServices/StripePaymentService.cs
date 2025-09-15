
namespace MmaSolution.ProxyServices
{
    public class StripePaymentService
    {
        public string BaseUrl { get; set; }
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }

        public StripePaymentService()
        {
            var stripeConfiguration = GetConfiguration();
            BaseUrl ??= stripeConfiguration.GetValue<string>("BaseUrl");
            SecretKey ??= stripeConfiguration.GetValue<string>("SecretKey");
            PublishableKey ??= stripeConfiguration.GetValue<string>("PublishableKey");

            StripeConfiguration.ApiKey = SecretKey;

        }

        public StripePaymentService(IConfigurationSection stripeConfiguration)
        {
            BaseUrl ??= stripeConfiguration.GetValue<string>("BaseUrl");
            SecretKey ??= stripeConfiguration.GetValue<string>("SecretKey");
            PublishableKey ??= stripeConfiguration.GetValue<string>("PublishableKey");

            StripeConfiguration.ApiKey = SecretKey;
        }
        private static IConfigurationSection GetConfiguration()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
				   .AddEnvironmentVariables();

            var Configuration = builder.Build();

            var config = Configuration.GetSection("Stripe");

            return config;
        }




        public async Task<PaymentIntent> Payment(PaymentIntentCreateRequest request)
        {
            var paymentIntentService = new PaymentIntentService();

            var paymentIntent = await paymentIntentService.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = request.Amount,
                Currency = request.Currency,
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
                ReturnUrl = request.ReturnUrl
            });

            return paymentIntent;

        }

        #region Products

        public async Task<List<Product>> GetProducts()
        {


            var service = new ProductService();
            var products = await service.ListAsync();
            return products.Data;
        }

        public async Task<Product> GetProduct(string productId)
        {


            var service = new ProductService();
            var product = await service.GetAsync(productId);
            return product;
        }

        public async Task<Product> CreateProduct(ProductModel model)
        {


            var options = new ProductCreateOptions
            {
                Id = model.ProductId,
                Name = model.ProductName,

            };
            var service = new ProductService();
            var product = await service.CreateAsync(options);
            return product;
        }
        #endregion

        #region Prices

        public async Task<List<Price>> GetPrices()
        {
            var service = new PriceService();
            var prices = await service.ListAsync();

            return prices.Data;
        }

        public async Task<Price> GetPrice(string priceId)
        {
            var service = new PriceService();
            var price = await service.GetAsync(priceId);

            return price;
        }

        public async Task<Price> CreatePrice(PriceModel model)
        {


            var options = new PriceCreateOptions
            {
                UnitAmount = model.Amount,
                Currency = model.Currency.ToString(),
                Recurring = new PriceRecurringOptions
                {
                    Interval = model.Interval.ToString(),
                },
                Product = model.ProductId,
            };
            var service = new PriceService();
            var price = await service.CreateAsync(options);

            return price;
        }
        #endregion

        public Coupon CreateCoupon(CouponModel model)
        {


            var options = new CouponCreateOptions
            {
                PercentOff = model.Percent,
                Duration = model.Duration,
                DurationInMonths = model.DurationInMonth,
            };
            var service = new CouponService();
            var coupon = service.Create(options);
            return coupon;
        }

        #region Customers

        public async Task<Customer> CreateCustomer(CustomerModel model)
        {


            var options = new CustomerCreateOptions
            {
                Description = model.Description,
                Email = model.Email,
                Source = model.SourceId
            };
            var service = new CustomerService();
            var customer = await service.CreateAsync(options);
            return customer;
        }

        #endregion

        #region Sources

        public async Task<Source> CreateSource(SourceModel model)
        {


            var options = new SourceCreateOptions
            {
                Type = SourceType.Card,
                Currency = model.Currency.ToString(),
                //Flow = Stripe.SourceFlow.Receiver,
                Owner = new SourceOwnerOptions
                {
                    Email = model.Email
                },
                Card = new SourceCardOptions
                {
                    Cvc = model.Card.Cvc,
                    Number = model.Card.Number,
                    ExpMonth = model.Card.ExpMonth,
                    ExpYear = model.Card.ExpYear,
                }
            };

            var service = new SourceService();
            var source = await service.CreateAsync(options);
            return source;
        }

        #endregion

        public Card CreateCard(string customerId, string sourceId)
        {


            var options = new CardCreateOptions
            {
                Source = sourceId,

            };
            var service = new CardService();
            var card = service.Create(customerId, options);
            return card;
        }


        #region Subscriptions

        public async Task<List<Subscription>> GetSubscriptions()
        {
            var options = new SubscriptionListOptions
            {
                Limit = 3,
            };
            var service = new SubscriptionService();
            StripeList<Subscription> subscriptions = await service.ListAsync(
                options);

            return subscriptions.Data;
        }

        public async Task<Subscription> GetSubscription(string subscriptionId)
        {

            var service = new SubscriptionService();
            var subscription = await service.GetAsync(subscriptionId);

            return subscription;
        }


        public async Task<Subscription> CreateSubscription(SubscriptionModel model)
        {


            try
            {
                var options = new SubscriptionCreateOptions
                {
                    Customer = model.CustomerId,
                    Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Price = model.PriceId,
                    },
                },
                    DefaultSource = model.SourceId
                };
                var service = new SubscriptionService();
                var subscription = await service.CreateAsync(options);
                return subscription;
            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion

        public Charge CreateCharge(ChargeModel model)
        {

            var options = new ChargeCreateOptions
            {

                Amount = model.Amount,
                Currency = model.Currency.ToString(),
                Source = model.SourceId,
                Customer = model.CustomerId,
                Description = $"Customer {model.CustomerId}Charge with {model.SourceId}",

            };
            var service = new ChargeService();
            var charge = service.Create(options);
            return charge;
        }

        public Token CreateToken(CardModel model)
        {

            var options = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Number = model.Number,
                    ExpMonth = model.ExpMonth.ToString(),
                    ExpYear = model.ExpYear.ToString(),
                    Cvc = model.Cvc,
                },
            };
            var service = new TokenService();
            var token = service.Create(options);
            return token;
        }

        public Charge CreateChargeWithToken(ChargeModel model)
        {

            var options = new ChargeCreateOptions
            {
                Amount = model.Amount,
                Currency = model.Currency.ToString(),
                Source = model.TokenId,
                Description = model.Description,
            };
            var service = new ChargeService();
            var charge = service.Create(options);
            return charge;
        }



        public Refund CreateRefund(string chargeId)
        {

            var options = new RefundCreateOptions
            {
                Charge = chargeId,
            };
            var service = new RefundService();
            var refund = service.Create(options);
            return refund;
        }
    }

    public static class StripePaymentExtensions
    {
        public static IServiceCollection AddStripeService(this IServiceCollection serviceCollection, Action<StripePaymentService> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        {
            var stripe = new StripePaymentService();
            optionsAction?.Invoke(stripe);
            serviceCollection.TryAdd(
                new ServiceDescriptor(typeof(StripePaymentService),
                    provider => stripe,
                    optionsLifetime));

            return serviceCollection;
        }
    }
}
