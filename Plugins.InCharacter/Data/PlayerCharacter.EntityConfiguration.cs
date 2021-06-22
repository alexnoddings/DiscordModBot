using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Elvet.InCharacter.Data
{
    internal class PlayerCharacterEntityConfiguration : IEntityTypeConfiguration<PlayerCharacter>
    {
        public void Configure(EntityTypeBuilder<PlayerCharacter> builder)
        {
            builder
                .HasKey(character => new { character.UserId, character.ChannelId })
                .IsClustered();

            builder.ToTable("IcPlayerCharacters");
        }
    }
}
