namespace MmaSolution.AppApi.Infrastrcture.Middlewares;

public class PermissionMiddleware
{
    private readonly RequestDelegate _next;
    public PermissionMiddleware(RequestDelegate next)
    {
        _next = next;
    }


    public async Task Invoke(HttpContext context)
    {

        var endpoint = context.GetEndpoint();
        var actionDescriptor = endpoint?.Metadata?.GetMetadata<ControllerActionDescriptor>();



        if (actionDescriptor != null)
        {
            // Check for the RequiredPermission attribute
            var allowAnonymous = actionDescriptor.MethodInfo
                .GetCustomAttributes(typeof(AllowAnonymousAttribute), true)
                .FirstOrDefault();

            if (allowAnonymous != null)
            {
                await _next(context);
                return;
            }

            var userId = context.User.FindFirst("Id")?.Value;
            var userManager = context.RequestServices.GetRequiredService<UserManager<AppUser>>();

            var roles = await userManager.GetRolesAsync(await userManager.FindByIdAsync(userId));
            if (roles.Contains("Admin"))
            {
                await _next(context);
                return;
            }
            var controller = context.GetRouteValue("controller")?.ToString().ToLower();
            var action = context.GetRouteValue("action")?.ToString().ToLower();
            //var tenantId = context.Request.Headers["Tenant-Id"].ToString();

            // Check for the RequiredPermission attribute
            var requiredPermissions = actionDescriptor.MethodInfo
                .GetCustomAttributes(typeof(RequiredPermissionAttribute), true)
                .Select(attr => attr as RequiredPermissionAttribute)
                .Select(attr => attr.Permission)
                .FirstOrDefault();

            var resource = $"{controller}/{action}";

            var permissionService = context.RequestServices.GetRequiredService<PermissionService>();

            if (requiredPermissions.Any())
            {
                if (await permissionService.UserHasPermissionForResourceAsync(userId.ToGuid(), resource, requiredPermissions))
                {
                    await _next(context);
                    return;
                }
            }

            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Permission Denied");
        }

    }

}
