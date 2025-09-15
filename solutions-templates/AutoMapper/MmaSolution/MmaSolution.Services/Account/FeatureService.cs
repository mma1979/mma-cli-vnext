namespace MmaSolution.Services.Account;

public class FeatureService
{
    private readonly AuthenticationDbContext _context;

    public FeatureService(AuthenticationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsFeatureEnabledAsync(string featureName, string userId = null, string roleId = null, string resourceId = null)
    {
        var feature = await _context.Features
            .Include(f => f.FeatureFlags)
            .FirstOrDefaultAsync(f => f.Name == featureName);

        if (feature == null)
        {
            return false;
        }

        if (feature.Scope == FeatureScope.Global)
        {
            return feature.IsEnabled;
        }

        string scopeIdentifier = feature.Scope switch
        {
            FeatureScope.User => userId,
            FeatureScope.Role => roleId,
            FeatureScope.Resource => resourceId,
            _ => throw new ArgumentException("Invalid feature scope")
        };

        var featureFlag = feature.FeatureFlags
            .FirstOrDefault(ff => ff.ScopeIdentifier == Guid.Parse(scopeIdentifier));

        return featureFlag?.IsEnabled ?? feature.IsEnabled;
    }
}