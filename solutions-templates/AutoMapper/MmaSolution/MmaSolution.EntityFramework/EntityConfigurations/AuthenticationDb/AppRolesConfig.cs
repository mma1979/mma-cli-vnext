using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MmaSolution.Core.Database.Identity;
using MmaSolution.EntityFramework.Infrastrcture.ValueGenerator;

namespace MmaSolution.EntityFramework.EntityConfigurations.AuthenticationDb;

public class AppRolesConfig : IEntityTypeConfiguration<AppRole>
{
    private readonly string _schema;
    public AppRolesConfig(string schema = "dbo")
    {
        _schema = schema;
    }


    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.ToTable("AppRoles", _schema);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<GuidV7ValueGenerator>();

        builder.HasQueryFilter(e => e.IsDeleted != true);
        builder.Property(e => e.IsDeleted).IsRequired()
            .HasDefaultValueSql("((0))");

        builder.Property(e => e.CreatedDate)
            .HasColumnType("datetime")
            .ValueGeneratedOnAdd()
            .HasValueGenerator<CreatedDateTimeValueGenerator>();

        builder.Property(e => e.ModifiedDate)
            .HasColumnType("datetime");


        builder.HasIndex(e => e.IsDeleted);
        builder.Property(e => e.DeletedDate).HasColumnType("datetime");


        builder.HasMany(e => e.AppUserRoles)
            .WithOne(e => e.AppRole)
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.AppRoleClaims)
           .WithOne(e => e.AppRole)
           .HasForeignKey(e => e.RoleId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.Name).HasMaxLength(100);
        builder.Property(e => e.NormalizedName).HasMaxLength(100);
        builder.Property(e => e.ConcurrencyStamp).HasMaxLength(1000);

    }
}