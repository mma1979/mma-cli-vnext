namespace MmaSolution.EntityFramework.EntityConfigurations.ApplicationDb
{
    public class SysSettingConfig : IEntityTypeConfiguration<SysSetting>
    {
        private readonly string _schema;
        public SysSettingConfig(string schema = "dbo")
        {
            _schema = schema;
        }


        public void Configure(EntityTypeBuilder<SysSetting> builder)
        {
            builder.ToTable("SysSettings", _schema);


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


            builder.Property(e => e.SysKey).HasMaxLength(200);
            builder.HasIndex(e => e.SysKey);

            builder.Property(e => e.SysValue)
                .HasConversion<JsonDictionaryConverter>();

        }
    }
}
