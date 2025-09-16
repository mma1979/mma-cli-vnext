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





            builder.Property(e => e.FullName)
                .HasMaxLength(200);
           
            builder.Property( e => e.UserName)
              .HasMaxLength(100);
            builder.Property(e => e.NormalizedUserName)
              .HasMaxLength(100);
            builder.Property( e => e.Email)
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

            builder.HasData(new
            {
                Id = new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"),
                FullName = "Super Admin",
                TwoFactorMethod = TwoFactorMethods.Email,
                CreatedDate = new DateTime(2025, 7, 23),
                IsDeleted = false,
                UserName = "admin@local.com",
                NormalizedUserName = "ADMIN@LOCAL.COM",
                Email = "admin@local.com",
                NormalizedEmail = "ADMIN@LOCAL.COM",
                EmailConfirmed = true,
                PasswordHash = "D01B2782115F692E0E0D52FC64EFE727F52DDA8CB03703898F1D182BD2517251-73073B1B83DFA10409FA469853F87F71",
                SecurityStamp = "LR2TCJY3QTYEMAG27JLB57NGL6H27HTW",
                ConcurrencyStamp = "d4f06dc9-8e7f-46ed-85e7-c11f4545470c",
                PhoneNumber = "+201008983687",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = true,
                LockoutEnabled = true,
                AccessFailedCount = 0
            });



        }
    }
}
