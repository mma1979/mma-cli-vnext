namespace MmaSolution.EntityFramework.EntityConfigurations.AuthenticationDb;

public class AppUserRoleConfig : IEntityTypeConfiguration<AppUserRole>
{
    private readonly string _schema;
    public AppUserRoleConfig(string schema = "dbo")
    {
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<AppUserRole> builder)
    {
        builder.ToTable("AppUserRoles", _schema);
        builder.HasKey(e => new { e.UserId, e.RoleId });

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

        builder.HasIndex(e => e.UserId);

        builder.HasIndex(e => e.RoleId);

        builder.HasData(new AppUserRole
        {
            UserId = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
            RoleId = new Guid("6d86280e-f691-4d17-a1c5-d12183f3fafa"),
            CreatedBy = null,
            CreatedDate = new DateTime(2025, 7, 23),
            ModifiedBy = null,
            ModifiedDate = null,
            IsDeleted = false,
            DeletedBy = null,
            DeletedDate = null
        });
    }
}