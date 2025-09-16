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

        builder.HasIndex(e => e.NormalizedName)
            .HasDatabaseName("RoleNameIndex")
            .IsUnique()
            .HasFilter("[NormalizedName] IS NOT NULL");

        builder.HasData(new
        {
            Id = new Guid("6d86280e-f691-4d17-a1c5-d12183f3fafa"),
            CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
            CreatedDate = new DateTime(2025, 7, 23),
            IsDeleted = false,
            Name = "Admin",
            NormalizedName = "ADMIN",
            ConcurrencyStamp = "e6c6df1d-e2f8-4e1f-9926-81ec842e7c84"
        },
        new
        {
            Id = new Guid("6d86280e-f691-4d17-a1c5-d12183f3fafb"),
            CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
            CreatedDate = new DateTime(2025, 7, 23),
            IsDeleted = false,
            Name = "User",
            NormalizedName = "USER",
            ConcurrencyStamp = "ccce9a7d-1a2c-4faf-9e76-ec3740ef4f10"
        });

    }
}