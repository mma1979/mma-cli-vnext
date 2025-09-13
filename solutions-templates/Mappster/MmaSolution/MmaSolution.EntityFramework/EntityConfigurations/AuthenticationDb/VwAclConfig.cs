using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MmaSolution.Core.Database.Views;

namespace MmaSolution.EntityFramework.EntityConfigurations.AuthenticationDb
{
    internal class VwAclConfig: IEntityTypeConfiguration<VwAcl>
    {
        private readonly string _schema;

        public VwAclConfig(string schema="dbo")
        {
            _schema = schema;
        }

        public void Configure(EntityTypeBuilder<VwAcl> builder)
        {
           builder.ToView("VW_ACLs", _schema);
        }
    }
}
