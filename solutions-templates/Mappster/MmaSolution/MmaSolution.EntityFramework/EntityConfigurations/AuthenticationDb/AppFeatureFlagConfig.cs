namespace MmaSolution.EntityFramework.EntityConfigurations.AuthenticationDb;

public class AppFeatureFlagConfig : IEntityTypeConfiguration<AppFeatureFlag>
{
    private readonly string _schema;
    public AppFeatureFlagConfig(string schema = "dbo")
    {
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<AppFeatureFlag> builder)
    {
        builder.ToTable("AppFeatureFlags", _schema);

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


    }
}
