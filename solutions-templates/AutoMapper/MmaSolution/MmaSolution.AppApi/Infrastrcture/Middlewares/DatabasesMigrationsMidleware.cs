namespace MmaSolution.AppApi.Infrastrcture.Middlewares;

public class DatabasesMigrationsMidleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<DatabasesMigrationsMidleware> _logger;
    private readonly AuthenticationDbContext _authenticationDbContext;
    private readonly ApplicationDbContext _applicationDbContext;

    public DatabasesMigrationsMidleware(RequestDelegate next,
        ILogger<DatabasesMigrationsMidleware> logger,
        AuthenticationDbContext authenticationDbContext,
        ApplicationDbContext applicationDbContext)
    {
        _next = next;
        _logger = logger;
        _authenticationDbContext = authenticationDbContext;
        _applicationDbContext = applicationDbContext;
    }

    public async Task InvokeAsync(HttpContext context)
    {
    

       

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
