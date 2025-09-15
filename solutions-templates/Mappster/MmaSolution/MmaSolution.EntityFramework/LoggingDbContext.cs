namespace MmaSolution.EntityFramework;

public class LoggingDbContext : DbContext
{

    public LoggingDbContext(DbContextOptions<LoggingDbContext> options)
        : base(options) { }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("name=LogsConnection");
        }
#if DEBUG
        optionsBuilder.EnableSensitiveDataLogging();
#endif
    }

    public virtual DbSet<AppLog> AppLogs { get; set; }
    public virtual DbSet<ElmahError> ElmahError { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("dbo");


        modelBuilder.Entity<AppLog>(entity =>
        {
            entity.Property(e => e.Id)
            .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.Level)
            .HasColumnType("nvarchar(128)");

            entity.Property(e => e.TimeStamp)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()");


        });

        modelBuilder.ApplyConfiguration(new ElmahErrorConfig());

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
