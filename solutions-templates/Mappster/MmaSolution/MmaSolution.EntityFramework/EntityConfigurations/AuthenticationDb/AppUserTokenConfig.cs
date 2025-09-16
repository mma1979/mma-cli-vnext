namespace MmaSolution.EntityFramework.EntityConfigurations.AuthenticationDb;

public class AppUserTokenConfig : IEntityTypeConfiguration<AppUserToken>
{
    private readonly string _schema;
    public AppUserTokenConfig(string schema = "dbo")
    {
        _schema = schema;
    }


    public void Configure(EntityTypeBuilder<AppUserToken> builder)
    {
        builder.ToTable("AppUserTokens", _schema);

        builder.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

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


        builder.Property(e => e.Name).HasMaxLength(100);

    }
}
