using System;

namespace SishIndustries.Discord.ModBot.Plugins.ReRole.Data
{
    public class PreviousRole
    {
        public Guid Id { get; set; }
        public Guid DetailsId { get; set; }
        public ulong RoleId { get; set; }

        public virtual PreviousRoleDetails? Details { get; set; }
    }
}
