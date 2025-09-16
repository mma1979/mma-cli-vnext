namespace MmaSolution.AppApi.Infrastrcture.Middlewares;

public class DatabasesMigrationsMidleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<DatabasesMigrationsMidleware> _logger;
    private readonly LoggingDbContext _loggingDbContext;
    private readonly AuthenticationDbContext _authenticationDbContext;
    private readonly ApplicationDbContext _applicationDbContext;

    public DatabasesMigrationsMidleware(RequestDelegate next,
        ILogger<DatabasesMigrationsMidleware> logger,
        LoggingDbContext loggingDbContext,
        AuthenticationDbContext authenticationDbContext,
        ApplicationDbContext applicationDbContext)
    {
        _next = next;
        _logger = logger;
        _loggingDbContext = loggingDbContext;
        _authenticationDbContext = authenticationDbContext;
        _applicationDbContext = applicationDbContext;
    }

    public async Task InvokeAsync(HttpContext context)
    {
    

        try
        {
            await _loggingDbContext.Database.EnsureCreatedAsync();
            await _loggingDbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "An error occurred while migrating the loggingDb database.");
           
        }

        try
        {
            await _authenticationDbContext.Database.EnsureCreatedAsync();
            await _authenticationDbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "An error occurred while migrating the authenticationDb database.");

        }

        try
        {
            await _applicationDbContext.Database.EnsureCreatedAsync();
            await _applicationDbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "An error occurred while migrating the applicationDb database.");

        }

        await _next(context);
    }
}
