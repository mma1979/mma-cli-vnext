namespace MmaSolution.EntityFramework.EntityConfigurations.ApplicationDb
{
    public class NotificationStatusConfig : IEntityTypeConfiguration<NotificationStatus>
    {
        private readonly string _schema;
        public NotificationStatusConfig(string schema = "dbo")
        {
            _schema = schema;
        }

       
        public void Configure(EntityTypeBuilder<NotificationStatus> builder)
        {
            builder.ToTable("NotificationStatuses", _schema);


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