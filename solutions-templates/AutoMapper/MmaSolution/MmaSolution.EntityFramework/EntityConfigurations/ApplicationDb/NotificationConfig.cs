namespace MmaSolution.EntityFramework.EntityConfigurations.ApplicationDb
{
    public class NotificationConfig : IEntityTypeConfiguration<Notification>
    {
        private readonly string _schema;
        public NotificationConfig(string schema = "dbo")
        {
            _schema = schema;
        }

       
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications", _schema);


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

            builder.HasIndex(e => new { e.NotificationType, e.NotificationStatus, e.Periority, e.ExpireTime, e.IsRead });

            builder.Property(e => e.Message).HasMaxLength(1000);
            builder.Property(e => e.IsRead).HasDefaultValue(false);

        }
    }
}