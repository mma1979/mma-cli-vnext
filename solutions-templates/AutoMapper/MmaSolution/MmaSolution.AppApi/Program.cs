var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAppConfiguration()
    .ConfigureSeriLog()
    .ConfigureAuoMapper()
    .ConfigureStronglyTypedSettings()
    .ConfigureAllowedServices()
    .ConfigureApiVersioning()
    .ConfigureDataAccess()
    .ConfigureJwtAuthentication()
    .ConfigureRedisCache()
    .ConfigureSwagger()
    .ConfigurePollyResilience()
    .ConfigureFluentEmail()
    .ConfigureHangfire()
    .ConfigureHealthChecks()
    .AddApplicationServices();



var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseDevelopment(!builder.Environment.IsProduction())
    .UseMiddleware<RequestSanitizationMiddleware>()
    .UseHttpsRedirection()
    .UseRouting()
    .UseCorsPolicy(builder.Configuration["AllowedOrigins"], builder.Environment)
    .UseSerilogRequestLogging()
    .UseAuthentication()
    .UseAuthorization()
    .UseMiddleware<JwtAuthenticationMiddleware>()
    .UseMiddleware<PermissionMiddleware>()
    .UseRemoveHeaders("Server", "X-Powered-By");

app.UseMapRoutes();


// Migarte database
app.UseMiddleware<DatabasesMigrationsMidleware>();


app.Run();