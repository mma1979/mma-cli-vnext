namespace MmaSolution.Services.Account;

public class PermissionService
{
    private readonly AuthenticationDbContext _context;
    private readonly FeatureService _featureService;

    public PermissionService(AuthenticationDbContext context, FeatureService featureService)
    {
        _context = context;
        _featureService = featureService;
    }

    public async Task<bool> UserHasPermissionForResourceAsync(Guid? userId, string resourceName, string permissionName)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.AppRole)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        var aceList = await _context.VwAcls
            .Where(a => a.UserId == userId)
            .ToListAsync();

        /*
         *  && WildcardHelper.IsMatch(a.ResourcePattern, resourceName) 
            && WildcardHelper.IsMatch(a.PermissionPattern, permissionName)
         */
        var ace = aceList
            .Where(a => WildcardHelper.IsMatch(a.ResourcePattern, resourceName))
            .Where(a => WildcardHelper.IsMatch(a.PermissionPattern, permissionName))
            .FirstOrDefault();

        if (ace != null)
        {
            if (ace.FeatureIsEnabled != null)
            {
                // TODO: Check if feature is enabled
                return true;
            }
            return true;
        }



        return false;
    }
}