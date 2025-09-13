using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

using MmaSolution.ProxyServices;
using MmaSolution.Services.Account;
using MmaSolution.Services.Chache;

using MmaSolution.Services.Settings;


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

            builder.Services.AddTransient<LocalizationSerivce>();
            builder.Services.AddTransient<SysSettingsService>();
            builder.Services.AddTransient<AccountService>();
            builder.Services.AddTransient<FeatureService>();
            builder.Services.AddTransient<PermissionService>();
           
            builder.Services.AddTransient<EmailService>();
            builder.Services.AddTransient<RoleService>();
            builder.Services.AddTransient<AttachmentsService>();
            

            return builder;
        }
    }
}
