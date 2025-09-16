namespace MmaSolution.EntityFramework.EntityConfigurations.ApplicationDb
{
    public class AttachmentConfig : IEntityTypeConfiguration<Attachment>
    {
        private readonly string _schema;
        public AttachmentConfig(string schema = "dbo")
        {
            _schema = schema;
        }

       
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder.ToTable("Attachments", _schema);

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

            builder.Property(e => e.FilePath).HasMaxLength(1000);
            builder.Property(e => e.ContentType).HasMaxLength(100);
            builder.Property(e => e.FileSize).HasColumnType("decimal(9,3)");
         
        }
    }
}
