using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Elvet.Core.Commands.Preconditions
{
    public sealed class RequireAdmin : PreconditionAttribute
    {
        public override string ErrorMessage { get; set; } = "You must be an administrator to run this command.";

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            PreconditionResult result;
            if (context.User is not IGuildUser guildUser)
                result = PreconditionResult.FromError("Command must be used in a guild.");
            else if (guildUser.GuildPermissions.Administrator)
                result = PreconditionResult.FromSuccess();
            else
                result = PreconditionResult.FromError(ErrorMessage);

            return Task.FromResult(result);
        }
    }
}
