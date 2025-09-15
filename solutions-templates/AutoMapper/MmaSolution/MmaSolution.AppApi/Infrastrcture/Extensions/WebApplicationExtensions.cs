namespace MmaSolution.AppApi.Infrastrcture.Extensions;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder UseDevelopment(this IApplicationBuilder app, bool isNotProduction)
    {
        if (isNotProduction)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseScalar(c =>
            {
                c.UseSpecUrl("/swagger/v1/swagger.json");
                c.RoutePrefix = string.Empty;
                c.UseTheme(Theme.BluePlanet);
            });
        }
        return app;
    }

    public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app, string allowedHosts, IWebHostEnvironment env)
    {
        var origins = allowedHosts.Split(',')
            .Select(s => s.Trim());

        app.UseCors(opt =>
            opt.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed(origin =>
                {
                    if (allowedHosts == "*") return true;
                    if (env.IsDevelopment()) return true;

                    // Only add this to allow testing with localhost, remove this line in production!  
                    if (origin.ToLower().Contains("localhost")) return true;


                    // Insert your production domain here.  
                    if (string.IsNullOrWhiteSpace(origin)) return false;
                    var allowedOrigin = origins.Any(x => x.CompareUrls(origin));
                    return allowedOrigin;
                }));
        return app;
    }

    public static IApplicationBuilder UseRemoveHeaders(this IApplicationBuilder app, params string[] headers)
    {
        app.Use(async (ctx, next) =>
        {
            foreach (var header in headers)
            {
                ctx.Response.Headers.Remove(header);
                ctx.Response.Headers.Remove(header.ToLower());
            }

            await next();

        });
        return app;
    }

    public static IApplicationBuilder UseMapRoutes(this WebApplication app, params string[] headers)
    {
        app.MapControllers();
        app.MapRazorPages();
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                },
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        return app;
    }

    public static IApplicationBuilder UseSecuredHangfireDashboard(this IApplicationBuilder app, IConfiguration configuration)
    {
        var endpoint = configuration.GetValue<string>("HangFireDashboard:Endpoint");
        var username = configuration.GetValue<string>("HangFireDashboard:Username");
        var password = configuration.GetValue<string>("HangFireDashboard:Password");
        var requireSsl = configuration.GetValue<bool>("HangFireDashboard:RequireSsl");
        app.UseHangfireDashboard(endpoint, new DashboardOptions
        {
            Authorization = [ new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
        {
            RequireSsl = requireSsl,
            SslRedirect = requireSsl,
            LoginCaseSensitive = true,
            Users =
            [
                new BasicAuthAuthorizationUser
                {
                    Login = username,
                    PasswordClear =  password
                }
            ]
         })]
        });

        return app;
    }


}
