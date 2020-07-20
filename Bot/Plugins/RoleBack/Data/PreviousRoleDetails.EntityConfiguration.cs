using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elvet.Plugins.RoleBack.Data
{
    public class PreviousRoleDetailsEntityConfiguration : IEntityTypeConfiguration<PreviousRoleDetails>
    {
        public void Configure(EntityTypeBuilder<PreviousRoleDetails> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.HasKey(previousRoleDetails => previousRoleDetails.Id);

            builder
                .HasMany<PreviousRole>()
                .WithOne()
                .HasForeignKey(previousRole => previousRole.DetailsId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
