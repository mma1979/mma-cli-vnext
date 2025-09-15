namespace MmaSolution.EntityFramework;

public class LocalizationDbContext : DbContext
{

    public LocalizationDbContext()
    {

    }

    public LocalizationDbContext(DbContextOptions<LocalizationDbContext> options)
        : base(options)
    {

    }


    public DbSet<Language> Languages { get; set; }
    public DbSet<Resource> Resources { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("name=LocalizationConnection");
        }
#if DEBUG
        optionsBuilder.EnableSensitiveDataLogging();
#endif
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasMany(e => e.Resources)
            .WithOne(e => e.Language)
            .HasForeignKey(e => e.LanguageId)
            .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.Property(e => e.Id).UseIdentityColumn(1);
            entity.ToTable("Resources");
            entity.HasKey(e => e.Id);
        });
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
