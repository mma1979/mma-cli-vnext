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
