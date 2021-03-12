using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elvet.FridayNightLive.Data
{
    internal class SessionHostEntityConfiguration : IEntityTypeConfiguration<SessionHost>
    {
        public void Configure(EntityTypeBuilder<SessionHost> builder)
        {
            builder
                .HasKey(host => new {host.GuildId, host.SessionNumber, host.UserId})
                .IsClustered();

            builder.ToTable("FnlSessionHosts");
        }
    }
}
