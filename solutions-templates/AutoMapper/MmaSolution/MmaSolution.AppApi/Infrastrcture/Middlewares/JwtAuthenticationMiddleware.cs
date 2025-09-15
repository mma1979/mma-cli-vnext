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
        if (context?.GetEndpoint()?.Metadata?.Any(e => e is AllowAnonymousAttribute) == true)
        {
            await _next(context);
        }

        var token = context.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("No Token Provided"));
            await context.Response.Body.FlushAsync();
            await _next(context);
        }

        (bool isValid, string jwtToken) = DecryptAndValidate(token);
        if (!isValid)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("Invalid Token"));
            await context.Response.Body.FlushAsync();
            await _next(context);
        }

        //TODO: invock token from user tokens

        var splits = jwtToken.Split('.');
        if (splits.Length < 3)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("Invalid Token Type"));
            await context.Response.Body.FlushAsync();
            await _next(context);
        }
        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken tokenInfo = null;
        try
        {
            handler.ValidateToken(jwtToken, tokenValidationParameters, out SecurityToken validatedToken);

            if (validatedToken == null)
            {
                throw new Exception();
            }

            tokenInfo = handler.ReadJwtToken(jwtToken);
        }
        catch (Exception)
        {

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("Invalid Token"));
            await context.Response.Body.FlushAsync();
            await _next(context);
        }

        // Expiration Handle
        long.TryParse(tokenInfo.Claims.FirstOrDefault(c => c.Type == "exp")?.Value, out var exp);

        var now = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        if (exp <= now)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("Expired Token"));
            await context.Response.Body.FlushAsync();
            await _next(context);
        }

        var roles = tokenInfo.Claims
        .Where(c => c.Type == "role")
        .Select(e => e.Value)
        .ToList();

        if (!roles.Any())
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("No Roles"));
            await context.Response.Body.FlushAsync();
            await _next(context);
        }

        var identity = new ClaimsPrincipal(new ClaimsIdentity(tokenInfo.Claims));
        context.User = identity;
        await _next(context);
    }

    private (bool isValid, string jwtToken) DecryptAndValidate(string token)
    {
        try
        {

            var secToken = token.Split(' ')[1];
            var secParts = secToken.Split('.');

            var connectionStr = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            using var connection = new SqlConnection(connectionStr);
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
            var query = "select * from AppUserTokens where Name=@Name and UserId=@UserId and Value=@Value";
            var data = connection.Query<AppUserTokenReadModel>(query, new { Name = TokenTypes.LOGIN_TOKEN, UserId = secParts[^1], Value = secToken }).AsList();
            if (!data.Any())
            {
                return (false, string.Empty);
            }

            var decrypted = secParts[0].DecryptMe();
            var hash = decrypted.GenerateUniqueNumber().ToString();
            return (hash == secParts[1], decrypted);
        }
        catch (Exception ex)
        {
            Log.Error("Invalid Token", ex, token);
            return (false, string.Empty);
        }
    }
}
