using Microsoft.IdentityModel.JsonWebTokens;

namespace MmaSolution.AppApi.Infrastrcture.Middlewares;

public class JwtAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TokenValidationParameters tokenValidationParameters;
    private readonly IConfiguration _configuration;
    public JwtAuthenticationMiddleware(RequestDelegate next, TokenValidationParameters tokenValidationParameters, IConfiguration configuration)
    {
        _next = next;
        this.tokenValidationParameters = tokenValidationParameters;
        _configuration = configuration;
    }

    public async Task Invoke(HttpContext context)
    {
        if (IsAllowedPath(context))
        {
            await _next(context);
            return;
        }

        var authHeader = context.Request.Headers["Authorization"].ToString();


        var splits = authHeader.Split(' ');
        if (splits.Length < 2)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("Invalid Token Type"));
            await context.Response.Body.FlushAsync();
            await _next(context);
            return;
        }
        var token = splits[1];
        if (!IsTokenAlive(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("Invalid Session"));
            await context.Response.Body.FlushAsync();
            await _next(context);
            return;
        }
        // Use JsonWebTokenHandler for JWE validation
        var tokenHandler = new JsonWebTokenHandler();

        TokenValidationResult validationResult = null;
        try
        {
            validationResult = await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);

            if (validationResult == null || !validationResult.IsValid)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("Invalid Token"));
                await context.Response.Body.FlushAsync();
                await _next(context);
                return;
            }


        }
        catch (Exception)
        {

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("Invalid Token"));
            await context.Response.Body.FlushAsync();
            await _next(context);
            return;
        }

        var claimsIdentity = validationResult?.ClaimsIdentity;

        var exp = long.Parse(claimsIdentity.Claims.Where(c => c.Type == "exp").FirstOrDefault()?.Value ?? "0");
        var now = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        if (exp <= now)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("Expired Token"));
            await context.Response.Body.FlushAsync();
            await _next(context);
            return;
        }

        var roles = claimsIdentity.Claims
        .Where(c => c.Type == ClaimTypes.Role)
        .Select(e => e.Value)
        .ToList();

        if (!roles.Any())
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("No Roles"));
            await context.Response.Body.FlushAsync();
            await _next(context);
            return;
        }

        var identity = new ClaimsPrincipal(claimsIdentity);
        context.User = identity;
        await _next(context);
    }

    private bool IsTokenAlive(string token)
    {
        try
        {

            var connectionStr = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            using var connection = new SqlConnection(connectionStr);
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
            var query = "select * from AppUserTokens where Name=@Name and Token=@Token and Value=@Value";
            var data = connection.Query<AppUserTokenReadModel>(query, new { Name = TokenTypes.LOGIN_TOKEN, Token = token.ToUniqueNumericValue().ToString(), Value = token }).AsList();

            return data.Any();
        }
        catch (Exception ex)
        {
            Log.Error("Invalid Token", ex, token);
            return false;
        }
    }

    private bool IsAllowedPath(HttpContext context)
    {
        return  context?.GetEndpoint()?.Metadata?.Any(e => e is AllowAnonymousAttribute) == true ||
            context.Request?.Path.ToString().Contains("swagger") == true ||
            context.Request?.Path.ToString().Contains("api-docs") == true ||
            context.Request?.Path.ToString().Contains("health") == true ||
            context.Request?.Path.ToString().Contains("robots.txt") == true ||
            context.Request?.Path.ToString().Contains("favicon.ico") == true;
    }
}