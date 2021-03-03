using System;
using System.Collections.Generic;

namespace Elvet.RoleBack.Data
{
    internal class UserGuildRoles
    {
        public ulong Guild { get; set; }
        public ulong User { get; set; }
        public List<ulong> Roles { get; set; } = new();
        public DateTime LeftUtc { get; set; } = DateTime.UtcNow;
    }
}
