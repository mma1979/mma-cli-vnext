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
    }
}