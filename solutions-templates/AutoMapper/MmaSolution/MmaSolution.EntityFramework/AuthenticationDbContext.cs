namespace MmaSolution.EntityFramework;

public class AuthenticationDbContext : IdentityDbContext<AppUser, AppRole, Guid, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
{
    public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("name=AuthenticationConnection");
        }
#if DEBUG
        optionsBuilder.EnableSensitiveDataLogging();
#endif
    }

    // Tables
    public virtual DbSet<AppRefreshToken> AppRefreshTokens { get; set; }
    public virtual DbSet<AppResource> AppResources { get; set; }
    public virtual DbSet<AppRole> AppRoles { get; set; }
    public virtual DbSet<AppRoleClaim> AppRoleClaims { get; set; }
    public virtual DbSet<AppUser> AppUsers { get; set; }
    public virtual DbSet<AppUserClaim> AppUserClaims { get; set; }
    public virtual DbSet<AppUserLogin> AppUserLogins { get; set; }
    public virtual DbSet<AppUserRole> AppUserRoles { get; set; }
    public virtual DbSet<AppUserToken> AppUserTokens { get; set; }


    public virtual DbSet<AppAccessControlEntry> AccessControlEntries { get; set; }

    public virtual DbSet<AppFeature> Features { get; set; }
    public virtual DbSet<AppFeatureFlag> FeatureFlags { get; set; }

    // Views
    public virtual DbSet<VwAcl> VwAcls { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("dbo");

        // Tables
        builder.ApplyConfiguration(new AppAccessControlEntryConfig());
        builder.ApplyConfiguration(new AppFeatureConfig());
        builder.ApplyConfiguration(new AppFeatureFlagConfig());
        builder.ApplyConfiguration(new AppRefreshTokenConfig());
        builder.ApplyConfiguration(new AppResourceConfig());
        builder.ApplyConfiguration(new AppRoleClaimConfig());
        builder.ApplyConfiguration(new AppRolesConfig());
        builder.ApplyConfiguration(new AppUserClaimConfig());
        builder.ApplyConfiguration(new AppUserConfig());
        builder.ApplyConfiguration(new AppUserLoginConfig());
        builder.ApplyConfiguration(new AppUserRoleConfig());
        builder.ApplyConfiguration(new AppUserTokenConfig());

        //Views
        builder.ApplyConfiguration(new VwAclConfig());
    }

    public bool HardDelete<T>(Expression<Func<T, bool>> expression) where T : class
    {
        try
        {
            int deletedCount = Set<T>()
                .Where(expression)
                .ExecuteDelete(); // Single round-trip to the database

            return deletedCount > 0;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error hard deleting {Entity}", typeof(T).Name);
            return false;
        }
    }

}
