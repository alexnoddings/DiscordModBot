using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elvet.FridayNightLive.Data
{
    internal class SessionEntityConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder
                .HasKey(session => new { session.GuildId, session.Number})
                .IsClustered();

            builder
                .Property(s => s.Number)
                .ValueGeneratedOnAdd();

            builder
                .HasMany(session => session.Hosts)
                .WithOne()
                .IsRequired()
                .HasForeignKey(host => new {host.GuildId, host.SessionNumber})
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Navigation(session => session.Hosts)
                .AutoInclude();

            builder
                .HasMany(session => session.Winners)
                .WithOne()
                .IsRequired()
                .HasForeignKey(winner => new {winner.GuildId, winner.SessionNumber})
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Navigation(session => session.Winners)
                .AutoInclude();

            builder.ToTable("FnlSessions");
        }
    }
}
