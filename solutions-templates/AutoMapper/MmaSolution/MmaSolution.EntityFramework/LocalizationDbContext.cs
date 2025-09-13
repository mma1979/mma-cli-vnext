using Microsoft.EntityFrameworkCore;

using MmaSolution.Core.Database.Localization;

namespace MmaSolution.EntityFramework
{
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

    }
}
