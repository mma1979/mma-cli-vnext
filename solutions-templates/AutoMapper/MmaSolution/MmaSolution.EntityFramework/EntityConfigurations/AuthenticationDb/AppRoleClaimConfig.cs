namespace MmaSolution.EntityFramework.EntityConfigurations.AuthenticationDb;

public class AppRoleClaimConfig : IEntityTypeConfiguration<AppRoleClaim>
{
    private readonly string _schema;
    public AppRoleClaimConfig(string schema = "dbo")
    {
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<AppRoleClaim> builder)
    {
        builder.ToTable("AppRoleClaims", _schema);
        builder.Property(e => e.Id)
            .HasValueGenerator<GuidV7ValueGenerator>()
            .ValueGeneratedOnAdd();

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


        builder.Property(e => e.ClaimType).HasMaxLength(2000);
        builder.Property(e => e.ClaimValue).HasMaxLength(1000);

        builder.HasIndex(e => e.RoleId);
    }
}
