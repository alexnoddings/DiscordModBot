using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elvet.FridayNightLive.Data
{
    internal class GuildConfigEntityConfiguration : IEntityTypeConfiguration<GuildConfig>
    {
        public void Configure(EntityTypeBuilder<GuildConfig> builder)
        {
            builder
                .HasKey(config => config.GuildId)
                .IsClustered();

            builder.ToTable("FnlGuildConfig");
        }
    }
}
