namespace MmaSolution.EntityFramework.EntityConfigurations.AuthenticationDb;

public class AppFeatureConfig : IEntityTypeConfiguration<AppFeature>
{
    private readonly string _schema;

    public AppFeatureConfig(string schema = "dbo")
    {
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<AppFeature> builder)
    {
        builder.ToTable("AppFeatures", _schema);

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

        builder.Property(e => e.Name).HasMaxLength(500);
        builder.Property(e => e.DeletedBy).HasMaxLength(1500);


        builder.HasMany(f => f.FeatureFlags)
           .WithOne(ff => ff.Feature)
           .HasForeignKey(ff => ff.FeatureId);
    }
}
