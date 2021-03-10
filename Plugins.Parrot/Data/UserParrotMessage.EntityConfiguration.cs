using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elvet.Parrot.Data
{
    internal class UserParrotMessageEntityConfiguration : IEntityTypeConfiguration<UserParrotMessage>
    {
        public void Configure(EntityTypeBuilder<UserParrotMessage> builder)
        {
            builder.HasKey(userParrotMessage => userParrotMessage.Id);
        }
    }
}
