using System;

namespace Elvet.Plugins.RoleBack.Data
{
    public class PreviousRoleDetails
    {
        public Guid Id { get; set; }
        public DateTime Added { get; set; } = DateTime.Now;
        public ulong UserId { get; set; }
        public ulong GuildId { get; set; }
    }
}
