namespace MmaSolution.EntityFramework.Infrastrcture;


public class CurrentUserService
{
    private readonly IHttpContextAccessor _contextAccessor;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _contextAccessor = httpContextAccessor;
    }
    public Guid? GetCurrentUser()
    {
        var user = _contextAccessor.HttpContext?.User;
        _ = Guid.TryParse(user.FindFirstValue("Id"), out Guid userId);
        return userId;
    }
}