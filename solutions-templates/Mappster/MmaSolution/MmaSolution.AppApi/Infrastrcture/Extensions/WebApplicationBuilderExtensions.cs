
namespace MmaSolution.AppApi.Infrastrcture.Extensions;

public static class WebApplicationBuilderExtensions
{

    public static WebApplicationBuilder ConfigureAppConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName.Trim(' ')}.json", optional: true, reloadOnChange: true)
            .AddSqlServerJson(source =>
            {
                source.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
                source.TableName = "SysSettings";
                source.KeyColumn = "SysKey";
                source.JsonColumn = "SysValue";
                source.EnvironmentColumn = "Environment";
                source.Environment = builder.Environment.EnvironmentName;
                source.AutoCreateTable = true;
                source.Optional = true;
                source.MaxRetryAttempts = 5;
                source.RetryDelay = TimeSpan.FromSeconds(30);
                source.ReloadOnChange = true;
                source.ReloadInterval = TimeSpan.FromMinutes(5);
                source.Optional = true;
            })
            .AddEnvironmentVariables();

        return builder;
    }

    public static WebApplicationBuilder ConfigureSeriLog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, config) =>
        {
            config.Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.Console()
            .WriteTo.MSSqlServer(
                connectionString: context.Configuration.GetConnectionString("LogsConnection"),
                sinkOptions: new MSSqlServerSinkOptions { TableName = "AppLogs", AutoCreateSqlTable = true }
                    )

                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("ApplicationName", context.Configuration["Serilog:ApplicationName"])
                .Enrich.WithEnvironmentUserName()
                .ReadFrom.Configuration(context.Configuration);
        });

        return builder;
    }

    public static WebApplicationBuilder ConfigureMapster(this WebApplicationBuilder builder)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(System.Reflection.Assembly.GetAssembly(typeof(Core.MappingProfile)));
        builder.Services.AddSingleton(config);
        builder.Services.AddScoped<IMapper, Mapper>();
        return builder;
    }

    public static WebApplicationBuilder ConfigureStronglyTypedSettings(this WebApplicationBuilder builder)
    {

        // configure strongly typed settings objects
        IConfigurationSection appSettingsSection = builder.Configuration.GetSection("AppSettings");
        builder.Services.Configure<AppSettings>(appSettingsSection);
        return builder;
    }

    public static WebApplicationBuilder ConfigureAllowedServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.

        builder.Services.AddMvc().AddNewtonsoftJson(options =>
        {
            //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });
        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });

        builder.Services.AddLogging();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSignalR();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddSysSettingsHelper();
        builder.Services.AddTransient<AppApi.Services.Translator>();

        builder.Services.AddTransient<RequestSanitizationMiddleware>();
        builder.Services.AddTransient<FeatureService>();
        builder.Services.AddScoped<PermissionService>();
        builder.Services.AddSingleton<CurrentUserService>();
        builder.Services.AddSingleton<SoftDeleteInterceptor>();
        builder.Services.AddSingleton<AuditUpdateInterceptor>();
        builder.Services.AddSingleton<AuditCreateInerceptor>();

        return builder;
    }

    public static WebApplicationBuilder ConfigureApiVersioning(this WebApplicationBuilder builder)
    {

        #region API Versioning
        builder.Services.AddApiVersioning(c =>
        {
            c.DefaultApiVersion = new ApiVersion(1, 0);
            c.AssumeDefaultVersionWhenUnspecified = true;
            c.ReportApiVersions = true;
            c.ApiVersionReader = new UrlSegmentApiVersionReader();
        });
        #endregion
        return builder;
    }

    public static WebApplicationBuilder ConfigureDataAccess(this WebApplicationBuilder builder)
    {

        #region  For Entity Framework
        builder.Services.AddDbContext<AuthenticationDbContext>((serviceProvider, options) =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("AuthenticationConnection"))
        .AddInterceptors(
            serviceProvider.GetRequiredService<SoftDeleteInterceptor>(),
            serviceProvider.GetRequiredService<AuditUpdateInterceptor>(),
            serviceProvider.GetRequiredService<AuditCreateInerceptor>()
        ));

        builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .AddInterceptors(
            serviceProvider.GetRequiredService<SoftDeleteInterceptor>(),
            serviceProvider.GetRequiredService<AuditUpdateInterceptor>(),
            serviceProvider.GetRequiredService<AuditCreateInerceptor>()
        ));


        #endregion
        return builder;
    }

    public static WebApplicationBuilder ConfigureJwtAuthentication(this WebApplicationBuilder builder)
    {

        #region Authentication & JWT

        builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JWT"));
        var jwtConfig = builder.Configuration.GetSection("JWT").Get<JwtConfig>();
        // add validation paramter as singletone so that we can use it across the appications
        TokenValidationParameters tokenValidationParameters = new()
        {
            // **JWE Decryption Key**
            TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.TokenDecryptionKey)),

            // **Inner JWT Validation Parameters**
            ValidateAudience = true,
            ValidAudience = jwtConfig.ValidAudience,
            ValidateIssuer = true,
            ValidIssuer = jwtConfig.ValidIssuer,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            // **Inner JWT Signing Key**
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.IssuerSigningKey)),

            // Important: Set these for proper JWE handling
            RequireSignedTokens = true,
            ValidateTokenReplay = false
        };

        builder.Services.AddSingleton(tokenValidationParameters);

        // Adding Authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        // Adding Jwt Bearer
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = tokenValidationParameters;
        });

        builder.Services.AddAuthorization();

        // For Identity
        builder.Services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<AuthenticationDbContext>()
            .AddDefaultTokenProviders();



        #endregion
        return builder;
    }


    public static WebApplicationBuilder ConfigureRedisCache(this WebApplicationBuilder builder)
    {

        #region Redis Cache

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = $"{builder.Configuration.GetValue<string>("Redis:Server")}:{builder.Configuration.GetValue<int>("Redis:Port")}";
            options.ConfigurationOptions = Redis.ConfigurationOptions.Parse(options.Configuration);
            options.ConfigurationOptions.Password = builder.Configuration.GetValue<string>("Redis:Password");



        });



        #endregion
        return builder;
    }

    public static WebApplicationBuilder ConfigureSwagger(this WebApplicationBuilder builder)
    {

        #region Swagger
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "MmaSolution.AppApi", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}

                    }
                });
        });
        #endregion
        return builder;
    }

    public static WebApplicationBuilder ConfigurePollyResilience(this WebApplicationBuilder builder)
    {

        #region Polly resilience 
        builder.Services.AddResiliencePipeline("proxy-services", pipelineBuilder =>
        {
            pipelineBuilder
                .AddRetry(new RetryStrategyOptions()
                {
                    MaxRetryAttempts = builder.Configuration.GetValue<int>("Polly:MaxRetryAttempts"),
                    Delay = TimeSpan.FromSeconds(builder.Configuration.GetValue<int>("Polly:Delay"))
                })
                .AddTimeout(TimeSpan.FromSeconds(builder.Configuration.GetValue<int>("Polly:Timeout")));
        });

        #endregion
        return builder;
    }

    public static WebApplicationBuilder ConfigureFluentEmail(this WebApplicationBuilder builder)
    {

        builder.Services
      .AddFluentEmail(builder.Configuration.GetValue<string>("SMTP:Email"))
      .AddRazorRenderer()
      .AddSmtpSender(new SmtpClient()
      {
          Host = builder.Configuration.GetValue<string>("SMTP:SmtpServer"),
          Port = builder.Configuration.GetValue<int>("SMTP:Port"),
          Credentials = new NetworkCredential(builder.Configuration.GetValue<string>("SMTP:Email"), builder.Configuration.GetValue<string>("SMTP:Password")),
          EnableSsl = builder.Configuration.GetValue<bool>("SMTP:EnableSsl")
      });
        return builder;
    }

    public static WebApplicationBuilder ConfigureHangfire(this WebApplicationBuilder builder)
    {
        builder.Services.AddHangfire(config => config
           .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
           .UseSimpleAssemblyNameTypeSerializer()
           .UseRecommendedSerializerSettings()
           .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"),
               new SqlServerStorageOptions
               {
                   CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                   SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                   QueuePollInterval = TimeSpan.Zero,
                   UseRecommendedIsolationLevel = true,
                   DisableGlobalLocks = true
               })
           .WithJobExpirationTimeout(TimeSpan.FromHours(1)));
        return builder;
    }

    public static WebApplicationBuilder ConfigureHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
   .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        return builder;
    }
}
