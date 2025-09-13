using MmaSolution.Core.Database.Tables;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MmaSolution.EntityFramework.Infrastrcture.ValueGenerator;

namespace MmaSolution.EntityFramework.EntityConfigurations
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
            builder.Property(e => e.SysValue).HasMaxLength(2000);

            builder.HasIndex(e => e.SysKey);

        }
    }
}
