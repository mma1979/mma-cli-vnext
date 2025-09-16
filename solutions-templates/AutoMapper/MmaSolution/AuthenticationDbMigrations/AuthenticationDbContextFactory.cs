using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using MmaSolution.EntityFramework;

namespace AuthenticationDbMigrations;

public class AuthenticationDbContextFactory : IDesignTimeDbContextFactory<AuthenticationDbContext>
{
    public AuthenticationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AuthenticationDbContext>();
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__AuthenticationConnection") ?? "Data Source=.;Initial Catalog=application-db;TrustServerCertificate=True;User ID=sa; Password=Abc@1234; Application Name=MmaSolution";
        optionsBuilder.UseSqlServer(connectionString, b => {
            b.MigrationsAssembly("AuthenticationDbMigrations");
            b.MigrationsHistoryTable("AuthenticationDbMigrationsHistory", "ef");
        });
        return new AuthenticationDbContext(optionsBuilder.Options);
    }
}