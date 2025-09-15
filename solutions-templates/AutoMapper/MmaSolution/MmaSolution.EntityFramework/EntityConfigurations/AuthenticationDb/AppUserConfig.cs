namespace MmaSolution.EntityFramework.EntityConfigurations.AuthenticationDb
{
    public class AppUserConfig : IEntityTypeConfiguration<AppUser>
    {
        private readonly string _schema;
        public AppUserConfig(string schema = "dbo")
        {
            _schema = schema;
        }


        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers", _schema);
            builder.HasMany(x => x.UserRoles).WithOne(x => x.AppUser).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);




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



            builder.HasMany(e => e.UserRoles)
                    .WithOne(e => e.AppUser)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.UserTokens)
                .WithOne(e => e.AppUser)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(navigationExpression: e => e.RefreshTokens)
               .WithOne(e => e.AppUser)
               .HasForeignKey(e => e.UserId)
               .OnDelete(DeleteBehavior.Cascade);





            builder.Property(e => e.FirstName)
                .HasMaxLength(50);
            builder.Property(e => e.LastName)
                .HasMaxLength(50);
            builder.Property(e => e.Mobile)
               .HasMaxLength(20);
            builder.Property(e => e.CountryCode)
              .HasMaxLength(20);
            builder.Property(propertyExpression: e => e.UserName)
              .HasMaxLength(100);
            builder.Property(e => e.NormalizedUserName)
              .HasMaxLength(100);
            builder.Property(propertyExpression: e => e.Email)
              .HasMaxLength(100);
            builder.Property(e => e.NormalizedEmail)
              .HasMaxLength(maxLength: 100);

            builder.Property(e => e.PasswordHash)
           .HasMaxLength(maxLength: 1000);

            builder.Property(e => e.SecurityStamp)
           .HasMaxLength(maxLength: 1000);
            builder.Property(e => e.ConcurrencyStamp)
           .HasMaxLength(maxLength: 1000);

            builder.Property(e => e.PhoneNumber)
           .HasMaxLength(maxLength: 20);



        }
    }
}
