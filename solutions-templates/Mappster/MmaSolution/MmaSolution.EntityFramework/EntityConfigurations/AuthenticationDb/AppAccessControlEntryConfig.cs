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
    }
}
