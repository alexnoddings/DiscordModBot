using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.WebSocket;

namespace SishIndustries.Discord.ModBot.Core.Plugins.ReRole
{
    public class ReRoleService
    {
        private class PreviousRoles
        {
            public DateTime Added { get; }
            public ulong UserId { get; }
            public ulong GuildId { get; }
            public IList<ulong> RoleIds { get; }

            public PreviousRoles(DateTime added, ulong userId, ulong guildId, IList<ulong> roleIds)
            {
                Added = added;
                UserId = userId;
                GuildId = guildId;
                RoleIds = roleIds;
            }

            public PreviousRoles(ulong userId, ulong guildId, IList<ulong> roleIds)
                : this(DateTime.Now, userId, guildId, roleIds)
            {
            }
        }

        private static TimeSpan MaxReAddTime = TimeSpan.FromMinutes(120);
        private static RequestOptions DefaultRequestOptions = new RequestOptions {AuditLogReason = "[ReRole] Adding user's previous roles back"};

        private IList<PreviousRoles> Roles { get; } = new List<PreviousRoles>();

        public async Task OnUserLeftAsync(SocketGuildUser user)
        {
            var roleIds = 
                user.Roles
                    .Where(r => !r.IsEveryone && !r.IsManaged)
                    .Select(r => r.Id)
                    .ToList();
            var previousRoles = new PreviousRoles(user.Id, user.Guild.Id, roleIds);
            Roles.Add(previousRoles);
        }

        public async Task OnUserJoinedAsync(SocketGuildUser user)
        {
            var previousRoles = Roles.FirstOrDefault(pr => pr.UserId == user.Id && pr.GuildId == user.Guild.Id);
            if (previousRoles == null)
                return;

            Roles.Remove(previousRoles);
            if (DateTime.Now - previousRoles.Added > MaxReAddTime)
                return;

            var guild = user.Guild;
            var highestBotRole = guild.CurrentUser.Roles.Max(r => r.Position);
            var roles =
                previousRoles.RoleIds
                    .Select(rid => guild.GetRole(rid))
                    // Role may have been deleted
                    .Where(r => r != null)
                    // Ensure it is assignable
                    .Where(r => !r.IsEveryone && !r.IsManaged)
                    // Ensure the bot has the power to assign it
                    .Where(r => r.Position < highestBotRole);

            try
            {
                await user.AddRolesAsync(roles, DefaultRequestOptions);
            }
            catch (HttpException e) when (e.HttpCode == HttpStatusCode.Forbidden)
            {
                // Send error
            }
        }
    }
}
