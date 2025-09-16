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

        builder.HasData(new AppResource(new()
        {
            Id = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61738"),
            Url = "*",
            Description = "Match all resources for admin",
            ResourceType = ResourceTypes.API,
            CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
            CreatedDate = new DateTime(2025, 7, 23),
            IsDeleted = false
        }),
        new AppResource(new()
        {
            Id = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61739"),
            Url = "api/account",
            Description = null,
            ResourceType = ResourceTypes.API,
            CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
            CreatedDate = new DateTime(2025, 7, 23),
            IsDeleted = false
        }),
        new AppResource(new()
        {
            Id = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61740"),
            Url = "api/localization",
            Description = null,
            ResourceType = ResourceTypes.API,
            CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
            CreatedDate = new DateTime(2025, 7, 23),
            IsDeleted = false
        }),
        new AppResource(new()
        {
            Id = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61741"),
            Url = "api/notifications",
            Description = null,
            ResourceType = ResourceTypes.API,
            CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
            CreatedDate = new DateTime(2025, 7, 23),
            IsDeleted = false
        }),
        new AppResource(new()
        {
            Id = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61742"),
            Url = "api/notification-statuses",
            Description = null,
            ResourceType = ResourceTypes.API,
            CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
            CreatedDate = new DateTime(2025, 7, 23),
            IsDeleted = false
        }),
        new AppResource(new()
        {
            Id = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61743"),
            Url = "api/notification-types",
            Description = null,
            ResourceType = ResourceTypes.API,
            CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
            CreatedDate = new DateTime(2025, 7, 23),
            IsDeleted = false
        }),
        new AppResource(new()
        {
            Id = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61744"),
            Url = "api/roles",
            Description = null,
            ResourceType = ResourceTypes.API,
            CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
            CreatedDate = new DateTime(2025, 7, 23),
            IsDeleted = false
        }),
        new AppResource(new()
        {
            Id = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61745"),
            Url = "api/syssettings",
            Description = null,
            ResourceType = ResourceTypes.API,
            CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
            CreatedDate = new DateTime(2025, 7, 23),
            IsDeleted = false
        }),
        new AppResource(new()
        {
            Id = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61746"),
            Url = "api/attachment",
            Description = null,
            ResourceType = ResourceTypes.API,
            CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
            CreatedDate = new DateTime(2025, 7, 23),
            IsDeleted = false
        })
        );

    }
}
