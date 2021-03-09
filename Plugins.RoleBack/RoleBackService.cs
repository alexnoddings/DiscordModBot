using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Elvet.RoleBack.Data;

namespace Elvet.RoleBack
{
    /// <summary>
    /// The scoped service which handles RoleBack events.
    /// </summary>
    internal class RoleBackService
    {
        private static readonly RequestOptions DefaultAddRolesRequestOptions =
            new() {AuditLogReason = "[RoleBack] Adding previous user's roles."};


        private readonly RoleBackDbContext _dbContext;
        private readonly RoleBackConfig _config;

        /// <summary>
        /// Initialises a new instance of the <see cref="RoleBackService" />.
        /// </summary>
        /// <param name="dbContext">The <see cref="RoleBackDbContext" /> to get data from.</param>
        /// <param name="config">The <see cref="RoleBackConfig" /> to get configuration from.</param>
        public RoleBackService(RoleBackDbContext dbContext, RoleBackConfig config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        /// <summary>
        /// Called when a <see cref="SocketGuildUser" /> joins a <see cref="IGuild" />.
        /// </summary>
        /// <param name="guildUser">The <see cref="SocketGuildUser" />.</param>
        /// <returns>A <see cref="Task" /> that represents the service handling the event.</returns>
        public async Task OnUserJoinedGuild(SocketGuildUser guildUser)
        {
            // Don't track roles for bots
            if (guildUser.IsBot) return;

            // Find existing user guild roles
            var userGuildRoles = await _dbContext.UserGuildRoles.FindAsync(guildUser.Id, guildUser.Guild.Id);

            if (userGuildRoles is null)
                return;

            // Remove from the db
            _dbContext.Remove(userGuildRoles);
            await _dbContext.SaveChangesAsync();

            // Don't re-add roles if they left longer ago than the RolesValidFor TimeSpan allows
            if (userGuildRoles.LeftUtc + _config.RolesValidFor < DateTime.UtcNow)
                return;

            // Will fail to add roles if they have a higher position than the bot's highest role
            var highestBotRolePosition = guildUser.Guild.CurrentUser.Roles.Max(role => role.Position);

            var roles =
                guildUser
                    .Guild
                    .Roles
                    .Where(role => userGuildRoles.Roles.Contains(role.Id)
                                   && role.Position < highestBotRolePosition);
            await guildUser.AddRolesAsync(roles, DefaultAddRolesRequestOptions);
        }

        /// <summary>
        /// Called when a <see cref="SocketGuildUser" /> leaves a <see cref="IGuild" />.
        /// </summary>
        /// <param name="guildUser">The <see cref="SocketGuildUser" />.</param>
        /// <returns>A <see cref="Task" /> that represents the service handling the event.</returns>
        public async Task OnUserLeftGuild(SocketGuildUser guildUser)
        {
            // Don't track roles for bots
            if (guildUser.IsBot) return;

            // Don't add an entry if the user had no roles
            var roles = guildUser.Roles.Where(role => !role.IsEveryone && !role.IsManaged)
                .Select(role => role.Id).ToList();
            if (roles.Count == 0) return;

            // Don't retain roles if the user was kicked/banned with the norole flag
            if (await HasNoRolesFlag(guildUser))
                return;

            // Find existing user guild roles
            var userGuildRoles = await _dbContext.UserGuildRoles.FindAsync(guildUser.Id, guildUser.Guild.Id);
            if (userGuildRoles is null)
            {
                // Add new user guild roles
                userGuildRoles = new UserGuildRoles {User = guildUser.Id, Guild = guildUser.Guild.Id, Roles = roles};
                _dbContext.UserGuildRoles.Add(userGuildRoles);
            }
            else
            {
                // Update existing user guild roles
                userGuildRoles.Roles = roles;
            }

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Checks if a user is flagged to not have their roles retained.
        /// </summary>
        /// <param name="guildUser">The <see cref="SocketGuildUser" /> to check.</param>
        /// <returns>A boolean indicating if the user is flagged to not have their roles retained.</returns>
        private static async Task<bool> HasNoRolesFlag(SocketGuildUser guildUser)
        {
            // Accounts for a delay in the audit log entry creation and this code executing
            var timeFrame = DateTimeOffset.UtcNow - TimeSpan.FromSeconds(10);

            var lastKicks = await guildUser.Guild.GetAuditLogsAsync(4, actionType: ActionType.Kick).ToListAsync();
            var lastBans = await guildUser.Guild.GetAuditLogsAsync(4, actionType: ActionType.Ban).ToListAsync();
            var logEntries = lastKicks.Union(lastBans).SelectMany(collection => collection);

            var hasNoRolesFlag = logEntries
                .Any(entry =>
                    // The entry must have happened recently
                    entry.CreatedAt > timeFrame
                    // The reason must have contained the "norole" flag
                    && entry.Reason.Contains("norole")
                    // The entry must be a kick or ban targetted as the user
                    && (entry.Data is KickAuditLogData kick && kick.Target.Id == guildUser.Id
                        || entry.Data is BanAuditLogData ban && ban.Target.Id == guildUser.Id));

            // User should be allowed to keep roles if no flag was found
            return hasNoRolesFlag;
        }
    }
}
