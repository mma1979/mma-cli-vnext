using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MmaSolution.Core.Database.Notifications;
using MmaSolution.EntityFramework.Infrastrcture.ValueGenerator;

namespace MmaSolution.EntityFramework.EntityConfigurations
{
    public class NotificationTypeConfig : IEntityTypeConfiguration<NotificationType>
    {
        private readonly string _schema;
        public NotificationTypeConfig(string schema = "dbo")
        {
            _schema = schema;
        }

       
        public void Configure(EntityTypeBuilder<NotificationType> builder)
        {
            builder.ToTable("NotificationTypes", _schema);


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

            builder.Property(e => e.Name).HasMaxLength(150);
            builder.Property(e => e.Description).HasMaxLength(500);


        }
    }
}