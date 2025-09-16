namespace MmaSolution.EntityFramework;

public class ApplicationDbContext : DbContext
{


   

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("name=DefaultConnection");
        }
#if DEBUG
        optionsBuilder.EnableSensitiveDataLogging();
#endif
    }

    public virtual DbSet<SysSetting> SysSettings { get; set; }
    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<Language> Languages { get; set; }
    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<EmailNotification> EmailNotifications { get; set; }
    public virtual DbSet<SmsNotification> SmsNotifications { get; set; }
    public virtual DbSet<PushNotification> PushNotifications { get; set; }
    public virtual DbSet<NotificationType> NotificationTypes { get; set; }
    public virtual DbSet<NotificationStatus> NotificationStatuses { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("dbo");

        modelBuilder.Ignore<AppRefreshToken>();
        modelBuilder.Ignore<AppRole>();
        modelBuilder.Ignore<AppRoleClaim>();
        modelBuilder.Ignore<AppUser>();
        modelBuilder.Ignore<AppUserClaim>();
        modelBuilder.Ignore<AppUserLogin>();
        modelBuilder.Ignore<AppUserRole>();
        modelBuilder.Ignore<AppUserToken>();
        modelBuilder.Ignore<AppResource>();
        modelBuilder.Ignore<AppAccessControlEntry>();
        modelBuilder.Ignore<AppFeature>();
        modelBuilder.Ignore<AppFeatureFlag>();

        modelBuilder.ApplyConfiguration(new SysSettingConfig());
        modelBuilder.ApplyConfiguration(new AttachmentConfig());
        modelBuilder.ApplyConfiguration(new NotificationConfig());
        modelBuilder.ApplyConfiguration(new NotificationStatusConfig());
        modelBuilder.ApplyConfiguration(new NotificationTypeConfig());
        modelBuilder.ApplyConfiguration(new LanguageConfig());
        modelBuilder.ApplyConfiguration(new ResourcesConfig());
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