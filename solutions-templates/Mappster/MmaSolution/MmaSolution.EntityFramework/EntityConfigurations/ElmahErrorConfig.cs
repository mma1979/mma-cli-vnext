namespace MmaSolution.EntityFramework.EntityConfigurations
{
    public class ElmahErrorConfig : IEntityTypeConfiguration<ElmahError>
    {
        private readonly string _schema;
        public ElmahErrorConfig(string schema = "dbo")
        {
            _schema = schema;
        }
        public void Configure(EntityTypeBuilder<ElmahError> builder)
        {

            builder.HasKey(e => e.ErrorId)
                     .IsClustered(false);

            builder.ToTable("ELMAH_Error", _schema);

            builder.HasIndex(e => new { e.Application, e.TimeUtc, e.Sequence })
                .HasDatabaseName("IX_ELMAH_Error_App_Time_Seq");

            builder.Property(e => e.ErrorId).IsRequired().HasDefaultValueSql("(newid())");

            builder.Property(e => e.AllXml)
                .IsRequired()
                .HasColumnType("ntext");

            builder.Property(e => e.Application)
                .IsRequired()
                .HasMaxLength(60);

            builder.Property(e => e.Host)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Message)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.Sequence).ValueGeneratedOnAdd();

            builder.Property(e => e.Source)
                .IsRequired()
                .HasMaxLength(60);

            builder.Property(e => e.TimeUtc).HasColumnType("datetime");

            builder.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.User)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
