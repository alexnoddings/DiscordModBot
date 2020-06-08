using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using SishIndustries.Discord.ModBot.Plugins.ReRole.Data;

namespace SishIndustries.Discord.ModBot.Plugins.ReRole
{
    public class ReRoleService
    {
        private static TimeSpan MaxReAddTime = TimeSpan.FromMinutes(120);
        private static RequestOptions DefaultRequestOptions = new RequestOptions {AuditLogReason = "[ReRole] Adding user's previous roles back"};

        private ReRoleDbContext DbContext { get; }

        public ReRoleService(ReRoleDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task OnUserLeftAsync(SocketGuildUser user)
        {
            var previousRoleDetails = 
                DbContext.PreviousRoleDetails.Add(new PreviousRoleDetails(user.Id, user.Guild.Id));

            var roles =
                user.Roles
                    .Where(r => !r.IsEveryone && !r.IsManaged)
                    .Select(r => r.Id)
                    .Select(rid => new PreviousRole{RoleId = rid})
                    .ToList();


            if (roles.Count == 0)
                return;

            previousRoleDetails.Entity.PreviousRoles.AddRange(roles);

            await DbContext.SaveChangesAsync();
        }

        public async Task OnUserJoinedAsync(SocketGuildUser user)
        {
            var previousRoleDetails = await DbContext.PreviousRoleDetails.FirstOrDefaultAsync(prd => prd.UserId == user.Id && prd.GuildId == user.Guild.Id);

            if (previousRoleDetails == null)
                return;

            var timeSinceLeft = DateTime.Now - previousRoleDetails.Added;
            if (timeSinceLeft < MaxReAddTime)
            {
                var guild = user.Guild;
                var highestBotRole = guild.CurrentUser.Roles.Max(r => r.Position);
                var roles =
                    previousRoleDetails.PreviousRoles
                        .Select(pr => guild.GetRole(pr.RoleId))
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

            DbContext.PreviousRoleDetails.Remove(previousRoleDetails);
            await DbContext.SaveChangesAsync();
        }
    }
}
