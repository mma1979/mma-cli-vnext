namespace MmaSolution.AppApi.Filters
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class LoggingFilter : Attribute, IActionFilter
    {
        public LoggingFilter()
        {

        }
        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.HttpContext.Request.RouteValues["controller"];
            var action = context.HttpContext.Request.RouteValues["action"];
            var userId = context.HttpContext.User?.Claims
                .FirstOrDefault(c => c.Type == "Id")?.Value;
            var userName = context.HttpContext.User?.Claims
               .FirstOrDefault(c => c.Type == "UserName")?.Value;


            Log.Information($"User {userId}- {userName}: Executed {controller}/{action} at {DateTime.UtcNow}", context.HttpContext.Request);
        }
    }
}
