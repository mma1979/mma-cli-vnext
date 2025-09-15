namespace MmaSolution.EntityFramework.EntityConfigurations.AuthenticationDb;

public class AppResourceConfig : IEntityTypeConfiguration<AppResource>
{
    private readonly string _schema;

    public AppResourceConfig(string schema = "dbo")
    {
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<AppResource> builder)
    {
        builder.ToTable("AppResources", _schema);

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

        builder.Property(e => e.Url).HasMaxLength(500);
        builder.Property(e => e.Description).HasMaxLength(1000);
        builder.Property(e => e.ResourceType).HasDefaultValue(ResourceTypes.API);

    }
}
