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
    .UseAuthorization();


app.UseMapRoutes();
app.UseMiddleware<JwtAuthenticationMiddleware>()
    .UseMiddleware<PermissionMiddleware>()
    .UseRemoveHeaders("Server", "X-Powered-By");

// Migarte database
using var scope = app.Services.CreateScope();
var logContext = scope.ServiceProvider.GetRequiredService<LoggingDbContext>();
var authContext = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

//logContext.Database.EnsureCreated();
//logContext.Database.Migrate();


authContext.Database.EnsureCreated();
authContext.Database.Migrate();

dbContext.Database.EnsureCreated();
dbContext.Database.Migrate();


app.Run();