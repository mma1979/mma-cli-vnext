

namespace MmaSolution.Tests
{
    public class StartupBuilder
    {

        public ServiceProvider ServiceProvider { get; set; }
        public StartupBuilder Init()
        {
            var services = new ServiceCollection();

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);

            var Configuration = builder.Build();

            services.AddOptions();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddLogging(config =>
            {
                config.AddDebug();
                config.AddConsole();
            });
            services.AddDbContext<AuthenticationDbContext>();
            services.AddIdentity<AppUser, AppRole>()
               .AddEntityFrameworkStores<AuthenticationDbContext>()
               .AddDefaultTokenProviders();

            // Service DI
            services.AddTransient<AccountService>();
            services.AddTransient<IEmailService,ResendService>();

            services
                  .AddFluentEmail(Configuration.GetValue<string>("SMTP:Email"))
                  .AddRazorRenderer()
                  .AddSmtpSender(new SmtpClient() { 
                  Host = Configuration.GetValue<string>("SMTP:SmtpServer"),
                  Port = Configuration.GetValue<int>("SMTP:Port"),
                      Credentials = new NetworkCredential(Configuration.GetValue<string>("SMTP:Email"), Configuration.GetValue<string>("SMTP:Password")),
                      EnableSsl= Configuration.GetValue<bool>("SMTP:EnableSsl")
                  });

            // Controllers DI
            services.AddTransient<AccountController>();



            ServiceProvider = services.BuildServiceProvider();

            return this;
        }

        public T GetInstance<T>()
        {
            var res = ServiceProvider.GetService<T>();
            return res;
        }
    }
}
