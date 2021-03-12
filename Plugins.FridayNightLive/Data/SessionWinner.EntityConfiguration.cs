using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elvet.FridayNightLive.Data
{
    internal class SessionWinnerEntityConfiguration : IEntityTypeConfiguration<SessionWinner>
    {
        public void Configure(EntityTypeBuilder<SessionWinner> builder)
{
builder
                .HasKey(winner => new {winner.GuildId, winner.SessionNumber, winner.UserId})
                .IsClustered();

            builder.ToTable("FnlSessionWinners");
        }
    }
}
