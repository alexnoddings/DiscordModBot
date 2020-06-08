using System;
using System.Collections.Generic;

namespace SishIndustries.Discord.ModBot.Plugins.ReRole.Data
{
    public class PreviousRoleDetails
    {
        public Guid Id { get; set; }
        public DateTime Added { get; set; } = DateTime.Now;
        public ulong UserId { get; set; }
        public ulong GuildId { get; set; }

        public virtual List<PreviousRole> PreviousRoles { get; set; } = new List<PreviousRole>();

        public PreviousRoleDetails()
        {
        }

        public PreviousRoleDetails(ulong userId, ulong guildId)
            : this()
        {
            UserId = userId;
            GuildId = guildId;
        }

        public PreviousRoleDetails(DateTime added, ulong userId, ulong guildId)
            : this(userId, guildId)
        {
            Added = added;
        }
    }
}
