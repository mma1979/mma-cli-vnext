using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using MmaSolution.EntityFramework;

namespace ApplicationDbMigrations;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ?? "Data Source=.;Initial Catalog=application-db;TrustServerCertificate=True;User ID=sa; Password=Abc@1234; Application Name=MmaSolution";
        optionsBuilder.UseSqlServer(connectionString, b => {
            b.MigrationsAssembly("ApplicationDbMigrations");
            b.MigrationsHistoryTable("ApplicationDbMigrationsHistory", "ef");
        });
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}