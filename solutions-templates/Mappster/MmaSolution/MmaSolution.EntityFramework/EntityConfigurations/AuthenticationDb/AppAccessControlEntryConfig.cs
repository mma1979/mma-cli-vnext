using MmaSolution.Core.Database.Localization;

namespace MmaSolution.EntityFramework.EntityConfigurations.AuthenticationDb;

internal class AppAccessControlEntryConfig : IEntityTypeConfiguration<AppAccessControlEntry>
{
    private readonly string _schema;
    public AppAccessControlEntryConfig(string schema = "dbo")
    {
        _schema = schema;
    }
    public void Configure(EntityTypeBuilder<AppAccessControlEntry> builder)
    {
        builder.ToTable("AppAccessControlEntries", _schema);

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

        builder.Property(e => e.ResourcePattern).HasMaxLength(500);
        builder.Property(e => e.PermissionPattern).HasMaxLength(500);


        builder.HasMany(ace => ace.AppRoles)
               .WithMany(r => r.AccessControlEntries)
               .UsingEntity(j => j.ToTable("AppRoleAccessControlEntries"));

        builder.HasMany(ace => ace.AppUsers)
             .WithMany(p => p.AccessControlEntries)
             .UsingEntity(j => j.ToTable("AppUserAccessControlEntries"));

        builder.HasOne(ace => ace.Feature)
        .WithMany(f=> f.AccessControlEntries)
        .HasForeignKey(ace => ace.FeatureId)
        .IsRequired(false);

        builder.HasOne(ace => ace.AppResource)
       .WithMany(res=> res.AccessControlEntries)
       .HasForeignKey(ace => ace.ResourceId)
       .IsRequired(true);

        builder.HasData(
           new AppAccessControlEntry(new()
           {
               Id = new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f47"),
               ResourcePattern = "*",
               PermissionPattern = "*",
               FeatureId = null,
               ResourceId = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61738"),
               CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
               CreatedDate = new DateTime(2025, 7, 23),
               IsDeleted = false
           }),
       new AppAccessControlEntry(new()
       {
           Id = new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f48"),
           ResourcePattern = "account/*",
           PermissionPattern = "*",
           FeatureId = null,
           ResourceId = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61739"),
           CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
           CreatedDate = new DateTime(2025, 7, 23),
           IsDeleted = false
       }),
       new AppAccessControlEntry(new()
       {
           Id = new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f49"),
           ResourcePattern = "localization/*",
           PermissionPattern = "*",
           FeatureId = null,
           ResourceId = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61740"),
           CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
           CreatedDate = new DateTime(2025, 7, 23),
           IsDeleted = false
       }),
       new AppAccessControlEntry(new()
       {
           Id = new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f50"),
           ResourcePattern = "localization/*",
           PermissionPattern = "Read",
           FeatureId = null,
           ResourceId = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61740"),
           CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
           CreatedDate = new DateTime(2025, 7, 23),
           IsDeleted = false
       }),
       new AppAccessControlEntry(new()
       {
           Id = new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f51"),
           ResourcePattern = "notification*",
           PermissionPattern = "*",
           FeatureId = null,
           ResourceId = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61741"),
           CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
           CreatedDate = new DateTime(2025, 7, 23),
           IsDeleted = false
       }),
       new AppAccessControlEntry(new()
       {
           Id = new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f52"),
           ResourcePattern = "notification*",
           PermissionPattern = "Read,Update",
           FeatureId = null,
           ResourceId = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61741"),
           CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
           CreatedDate = new DateTime(2025, 7, 23),
           IsDeleted = false
       }),
       new AppAccessControlEntry(new()
       {
           Id = new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f53"),
           ResourcePattern = "roles/*",
           PermissionPattern = "*",
           FeatureId = null,
           ResourceId = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61744"),
           CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
           CreatedDate = new DateTime(2025, 7, 23),
           IsDeleted = false
       }),
       new AppAccessControlEntry(new()
       {
           Id = new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f54"),
           ResourcePattern = "roles/*",
           PermissionPattern = "Read",
           FeatureId = null,
           ResourceId = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61744"),
           CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
           CreatedDate = new DateTime(2025, 7, 23),
           IsDeleted = false
       }),
       new AppAccessControlEntry(new()
       {
           Id = new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f55"),
           ResourcePattern = "syssettings/*",
           PermissionPattern = "*",
           FeatureId = null,
           ResourceId = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61745"),
           CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
           CreatedDate = new DateTime(2025, 7, 23),
           IsDeleted = false
       }),
       new AppAccessControlEntry(new()
       {
           Id = new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f56"),
           ResourcePattern = "syssettings/*",
           PermissionPattern = "Read",
           FeatureId = null,
           ResourceId = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61745"),
           CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
           CreatedDate = new DateTime(2025, 7, 23),
           IsDeleted = false
       }),
       new AppAccessControlEntry(new()
       {
           Id = new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f57"),
           ResourcePattern = "attachment/*",
           PermissionPattern = "*",
           FeatureId = null,
           ResourceId = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61746"),
           CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
           CreatedDate = new DateTime(2025, 7, 23),
           IsDeleted = false
       }),
       new AppAccessControlEntry(new()
       {
           Id = new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f58"),
           ResourcePattern = "attachment/*",
           PermissionPattern = "Read",
           FeatureId = null,
           ResourceId = new Guid("f10e8ad9-8458-4a22-81be-908f6aa61746"),
           CreatedBy = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
           CreatedDate = new DateTime(2025, 7, 23),
           IsDeleted = false
       })
     );
    }
}
