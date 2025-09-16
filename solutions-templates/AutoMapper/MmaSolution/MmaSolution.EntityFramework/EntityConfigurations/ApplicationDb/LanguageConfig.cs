using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MmaSolution.EntityFramework.EntityConfigurations.ApplicationDb;

public class LanguageConfig : IEntityTypeConfiguration<Language>
{
    private readonly string _schema;
    public LanguageConfig(string schema = "dbo")
    {
        _schema = schema;
    }
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("Languages", _schema);

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.HasMany(e => e.Resources)
            .WithOne(e => e.Language)
            .HasForeignKey(e => e.LanguageId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasData(new
        {
            Id = 1,
            LanguageName = "العربية",
            LanguageCode = "ar"
        },
        new
        {
            Id = 2,
            LanguageName = "English",
            LanguageCode = "en"
        });

    }
}