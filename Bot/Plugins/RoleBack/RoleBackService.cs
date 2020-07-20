using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using Elvet.Data;
using Elvet.Plugins.RoleBack.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Elvet.Plugins.RoleBack
{
    public class RoleBackService
    {
        private const int DefaultMaxReAddTimeMinutes = 120;
        private TimeSpan MaxReAddTime;
        private static RequestOptions DefaultRequestOptions = new RequestOptions {AuditLogReason = "[RoleBack] Adding user's previous roles back"};

        private BotDbContext DbContext { get; }

        public RoleBackService(BotDbContext dbContext, IConfiguration configuration)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            int maxReAddTimeMinutes = DefaultMaxReAddTimeMinutes;
            string maxReAddTimeMinutesStr = configuration["Plugins:RoleBack:MaxReAddTimeMinutes"];
            if (!string.IsNullOrWhiteSpace(maxReAddTimeMinutesStr))
            {
                bool parseSuccess = int.TryParse(maxReAddTimeMinutesStr, out maxReAddTimeMinutes);
                // maxReAddTimeMinutes will be reset to 0 if not successful
                if (!parseSuccess)
                    maxReAddTimeMinutes = DefaultMaxReAddTimeMinutes;
            }
            MaxReAddTime = TimeSpan.FromMinutes(maxReAddTimeMinutes);
        }

        public async Task OnUserLeftAsync(SocketGuildUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var previousRoleDetails = new PreviousRoleDetails {UserId = user.Id, GuildId = user.Guild.Id};

            DbContext.PreviousRoleDetails.Add(previousRoleDetails);

            var previousRoles =
                user.Roles
                    .Where(r => !r.IsEveryone && !r.IsManaged)
                    .Select(r => r.Id)
                    .Select(rId => new PreviousRole {DetailsId = previousRoleDetails.Id, RoleId = rId});

            DbContext.PreviousRoles.AddRange(previousRoles);

            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task OnUserJoinedAsync(SocketGuildUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var previousRoleDetails = await AsyncEnumerable.FirstOrDefaultAsync(DbContext.PreviousRoleDetails, prd => prd.UserId == user.Id && prd.GuildId == user.Guild.Id).ConfigureAwait(false);
            if (previousRoleDetails == null)
                return;

            TimeSpan timeSinceLeft = DateTime.Now - previousRoleDetails.Added;
            if (timeSinceLeft < MaxReAddTime)
            {
                SocketGuild guild = user.Guild;
                int highestBotRole = guild.CurrentUser.Roles.Max(r => r.Position);

                // server evaluation
                var roles = await
                    DbContext
                    .PreviousRoles
                    .AsQueryable()
                    .Where(pr => pr.DetailsId == previousRoleDetails.Id)
                    .ToListAsync().ConfigureAwait(false);

                // client evaluation
                var validRoles = roles
                    .Select(pr => guild.GetRole(pr.RoleId))
                    // Role may have been deleted
                    .Where(r => r != null)
                    // Ensure it is assignable
                    .Where(r => !r.IsEveryone && !r.IsManaged)
                    // Ensure the bot has the power to assign it
                    .Where(r => r.Position < highestBotRole);

                try
                {
                    foreach (var role in validRoles)
                        await user.AddRoleAsync(role, DefaultRequestOptions).ConfigureAwait(false);
                }
                catch (HttpException e) when (e.HttpCode == HttpStatusCode.Forbidden)
                {
                }
            }

            DbContext.PreviousRoleDetails.Remove(previousRoleDetails);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
