using Serilog;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using MmaSolution.AppApi.Infrastrcture.Middlewares;
using MmaSolution.AppApi.Infrastrcture.Extensions;
using MmaSolution.Services;


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

app.Run();