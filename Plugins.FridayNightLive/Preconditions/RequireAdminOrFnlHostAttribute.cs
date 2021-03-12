using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Elvet.FridayNightLive.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Elvet.FridayNightLive.Preconditions
{
    internal class RequireAdminOrFnlHostAttribute : PreconditionAttribute
    {
        public override string ErrorMessage { get; set; } = "You must be the current FNL host (or an admin) to use this command.";

        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (context.User is not IGuildUser guildUser)
                return PreconditionResult.FromError("Command must be used in a guild.");

            if (guildUser.GuildPermissions.Administrator)
                return PreconditionResult.FromSuccess();

            var dbContext = services.GetRequiredService<FridayNightLiveDbContext>();
            var guildConfig =  await dbContext.GuildConfigs.FindAsync(context.Guild.Id);
            if (guildConfig is not null && guildUser.RoleIds.Contains(guildConfig.HostRoleId))
                return PreconditionResult.FromSuccess();

            return PreconditionResult.FromError(ErrorMessage);
        }
    }
}
