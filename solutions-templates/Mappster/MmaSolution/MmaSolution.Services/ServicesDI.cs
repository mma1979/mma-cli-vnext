using MmaSolution.Services.Files;
using MmaSolution.Services.SmsService;

namespace MmaSolution.Services
{
    public static class ServicesDI
    {
        public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
        {
            
            builder.Services.AddStripeService();
            builder.Services.AddTransient<IMemoryCache, MemoryCache>();
            builder.Services.AddRedisCacheService();
            builder.Services.AddSingleton<Common.Infrastructure.PasswordHasher>();
            builder.Services.AddRestHelper();

            builder.Services.AddTransient<LocalizationSerivce>();
            builder.Services.AddTransient<AccountService>();
            builder.Services.AddTransient<FeatureService>();
            builder.Services.AddTransient<PermissionService>();
           
            builder.Services.AddTransient<IEmailService,ResendService>();
            builder.Services.AddTransient<ISmsService,WhatsAppService>();
            builder.Services.AddTransient<RoleService>();
            builder.Services.AddTransient<AttachmentsService>();
            builder.Services.AddTransient<AccessControlService>();
            builder.Services.AddTransient<UserProfileService>();
            builder.Services.AddTransient<MinioService>();
            

            return builder;
        }
    }
}
