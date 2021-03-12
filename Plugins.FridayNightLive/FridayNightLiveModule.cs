using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Elvet.Core.Commands;
using Elvet.Core.Commands.Preconditions;
using Elvet.Core.Extensions;
using Elvet.FridayNightLive.Data;
using Elvet.FridayNightLive.Preconditions;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace Elvet.FridayNightLive
{
    [Name("FridayNightLive")]
    [Alias("fnl")]
    internal class FridayNightLiveModule : ElvetModuleBase
    {
        private const string ConfigureRolesInfo = "Configure host and winner roles with the `config` command.";

        private readonly FridayNightLiveDbContext _dbContext;

        public FridayNightLiveModule(FridayNightLiveDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Command("EndQuiz")]
        [Alias("end")]
        [RequireContext(ContextType.Guild)]
        [RequireAdminOrFnlHost]
        public async Task EndSession(string activity, params IGuildUser[] newWinners)
        {
            var guildConfig = await _dbContext.GuildConfigs.FindAsync(Context.Guild.Id);
            if (guildConfig is null)
            {
                guildConfig = new GuildConfig {GuildId = Context.Guild.Id};
                _dbContext.GuildConfigs.Add(guildConfig);
                await _dbContext.SaveChangesAsync();
                throw new VisibleCommandException("No FNL config setup for this guild. " + ConfigureRolesInfo);
            }

            if (guildConfig.HostRoleId == 0)
                throw new VisibleCommandException($"No host role configured. " + ConfigureRolesInfo);
            var hostRole = Context.Guild.GetRole(guildConfig.HostRoleId);
            if (hostRole is null)
                throw new VisibleCommandException($"Could not resolve host role <@{guildConfig.HostRoleId}>. " + ConfigureRolesInfo);

            if (guildConfig.WinnerRoleId == 0)
                throw new VisibleCommandException($"No winner role configured. " + ConfigureRolesInfo);
            var winnerRole = Context.Guild.GetRole(guildConfig.WinnerRoleId);
            if (winnerRole is null)
                throw new VisibleCommandException($"Could not resolve winner role <@{guildConfig.WinnerRoleId}>. " + ConfigureRolesInfo);

            // If no activity is provided, the activity field will be filled with the first winner's ID.
            if (Regex.IsMatch(activity, "<@[0-9]{18}>"))
            {
                await ReplyAsync("Must provide an activity description.");
                await MarkError();
            }

            var hosts = hostRole.Members.ToList();
            var oldWinners = winnerRole.Members.ToList();

            // Add this session to the db
            var session = new Session
            {
                GuildId = Context.Guild.Id,
                Activity = activity,
                Date = DateTime.UtcNow,
                Hosts = hosts.Select(user => new SessionHost {UserId = user.Id}).ToList(),
                Winners = newWinners.Select(user => new SessionWinner { UserId = user.Id }).ToList()
            };
            _dbContext.Add(session);
            await _dbContext.SaveChangesAsync();

            // remove old hosts
            foreach (var user in hosts.Where(Context.Guild.CurrentUser.CanTarget))
                await user.RemoveRoleAsync(hostRole);

            // remove old winners
            foreach (var user in oldWinners.Where(Context.Guild.CurrentUser.CanTarget))
                await user.RemoveRoleAsync(winnerRole);

            // add new winners
            foreach (var user in newWinners.Cast<SocketGuildUser>().Where(Context.Guild.CurrentUser.CanTarget))
                await user.AddRoleAsync(winnerRole);

            // Display current winner(s) and the previous host(s)
            var descriptionBuilder = new StringBuilder();
            descriptionBuilder.Append("Current ");
            descriptionBuilder.Append("Winner".ToQuantity(newWinners.Length, ShowQuantityAs.None));
            descriptionBuilder.Append(": ");
            descriptionBuilder.Append(newWinners.Select(user => user.Mention).Humanize("and"));
            descriptionBuilder.Append('\n');
            descriptionBuilder.Append("Previous ");
            descriptionBuilder.Append("Host".ToQuantity(hosts.Count, ShowQuantityAs.None));
            descriptionBuilder.Append(": ");
            descriptionBuilder.Append(hosts.Select(user => user.Mention).Humanize("and"));

            var embed = new EmbedBuilder();
            embed
                .WithColor(255, 92, 232)
                .WithTitle($"Friday Night Live #{session.Number}: *{activity}*")
                .WithDescription(descriptionBuilder.ToString())
                .WithFooter("Last updated")
                .WithCurrentTimestamp();

            if (guildConfig.ThumbnailUrl is not null)
                embed = embed.WithThumbnailUrl(guildConfig.ThumbnailUrl);

            // Queryable of (user id, number of wins)
            var userWins =
                from sessionWinner in _dbContext.Winners.AsQueryable()
                where sessionWinner.GuildId == Context.Guild.Id
                group sessionWinner by sessionWinner.UserId
                into userWin
                select new {UserId = userWin.Key, WinCount = userWin.Count()};

            // Async enumerable of (number of wins, list of user ids)
            var groupedWins =
                from userWin in userWins.AsAsyncEnumerable()
                group userWin by userWin.WinCount
                into groupedWin
                select new {WinCount = groupedWin.Key, UserIds = groupedWin.Select(userWin => userWin.UserId)};

            await foreach (var group in groupedWins.OrderByDescending(groupedWin => groupedWin.WinCount))
            {
                var winCount = "win".ToQuantity(group.WinCount, ShowQuantityAs.Words).ApplyCase(LetterCasing.Title);
                var userMentions = (await group.UserIds.ToListAsync()).Humanize(id => $"<@{id}>", "and");
                embed.AddField(winCount, userMentions);
            }

            if (guildConfig.LeaderboardMessageId == 0)
            {
                guildConfig.LeaderboardMessageId = (await SendAsync(embed: embed.Build())).Id;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                var leaderboardMessage = await Context.Channel.GetMessageAsync(guildConfig.LeaderboardMessageId);
                if (leaderboardMessage is not RestUserMessage userMessage)
                {
                    guildConfig.LeaderboardMessageId = (await SendAsync(embed: embed.Build())).Id;
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    await userMessage.ModifyAsync(message => { message.Embed = embed.Build(); });
                }
            }

            await Context.Message.DeleteAsync(new RequestOptions {AuditLogReason = "Removing processed command."});
        }

        [Command("Configure")]
        [Alias("conf")]
        [RequireContext(ContextType.Guild)]
        [RequireAdmin]
        public async Task Configure(IRole hostRole, IRole winnerRole, string? thumbnailUrl = null)
        {
            var guildConfig = await _dbContext.GuildConfigs.FindAsync(Context.Guild.Id);
            if (guildConfig is null)
            {
                guildConfig = new GuildConfig { GuildId = Context.Guild.Id, HostRoleId = hostRole.Id, WinnerRoleId = winnerRole.Id, ThumbnailUrl = thumbnailUrl };
                _dbContext.GuildConfigs.Add(guildConfig);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                guildConfig.ThumbnailUrl = thumbnailUrl ?? guildConfig.ThumbnailUrl;
                guildConfig.HostRoleId = hostRole.Id;
                guildConfig.WinnerRoleId = winnerRole.Id;
                await _dbContext.SaveChangesAsync();
            }
            
            await Context.Message.DeleteAsync(new RequestOptions {AuditLogReason = "Removing processed command."});
        }
    }
}
