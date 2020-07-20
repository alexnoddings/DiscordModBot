using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elvet.Plugins.RoleBack.Data
{
    public class PreviousRoleEntityConfiguration : IEntityTypeConfiguration<PreviousRole>
    {
        public void Configure(EntityTypeBuilder<PreviousRole> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.HasKey(previousRole => previousRole.Id);
        }
    }
}
