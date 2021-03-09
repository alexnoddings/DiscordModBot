using System;
using System.Collections.Generic;
using Discord;

namespace Elvet.RoleBack.Data
{
    internal class UserGuildRoles
    {
        /// <summary>
        /// The <see cref="IGuild.Id"/> of the guild the roles are valid for.
        /// </summary>
        public ulong Guild { get; set; }

        /// <summary>
        /// The <see cref="IUser.Id"/> of the user who the roles belong to.
        /// </summary>
        public ulong User { get; set; }

        /// <summary>
        /// A list of <see cref="IRole.Id"/>s of the roles the user has.
        /// </summary>
        public List<ulong> Roles { get; set; } = new();

        /// <summary>
        /// What time the user left the guild. Used to calculate if they are still entitled to roles.
        /// </summary>
        public DateTime LeftUtc { get; set; } = DateTime.UtcNow;
    }
}
