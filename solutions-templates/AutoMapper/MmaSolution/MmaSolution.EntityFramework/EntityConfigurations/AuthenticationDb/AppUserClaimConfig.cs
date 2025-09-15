namespace MmaSolution.EntityFramework.EntityConfigurations.AuthenticationDb;

public class AppUserClaimConfig : IEntityTypeConfiguration<AppUserClaim>
{
    private readonly string _schema;
    public AppUserClaimConfig(string schema = "dbo")
    {
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<AppUserClaim> builder)
    {
        builder.ToTable("AppUserClaims", _schema);
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


        builder.Property(e => e.ClaimType).HasMaxLength(2000);
        builder.Property(e => e.ClaimValue).HasMaxLength(1000);

        builder.HasIndex(e => e.UserId);
    }
}
