using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Elvet.Core.Commands;
using Elvet.Parrot.Data;

namespace Elvet.Parrot
{
    internal class ParrotModule : ElvetModuleBase
    {
        private readonly ParrotConfig _parrotConfig;
        private readonly ParrotDbContext _parrotDbContext;

        protected override IEmote SuccessReactionEmote { get; } = new Emoji("🦜");

        public ParrotModule(ParrotConfig config, ParrotDbContext parrotDbContext)
        {
            _parrotConfig = config;
            _parrotDbContext = parrotDbContext;
        }

        [Command("Parrot")]
        [Alias("Echo", "RemindMe")]
        [RequireContext(ContextType.DM, ErrorMessage = "Reminders can only be created in DMs.")]
        public async Task CreateReminder(TimeSpan sendAfter, [Remainder] string message)
        {
            var userId = Context.User.Id;
            var userMessageCount = await _parrotDbContext.UserMessages.CountAsync(msg => msg.UserId == userId);
            if (userMessageCount > _parrotConfig.MaxReminders)
            {
                await MarkError();
                await ReplyAsync($"Hit maximum queued messages ({_parrotConfig.MaxReminders}).");
                return;
            }

            if (message.Length > _parrotConfig.MaxMessageSize)
            {
                await MarkError();
                await ReplyAsync($"Message too long ({message.Length}). Max size is {_parrotConfig.MaxMessageSize} characters.");
                return;
            }

            if (sendAfter > _parrotConfig.MaxTriggerTime)
            {
                await MarkError();
                await ReplyAsync($"Reminder time too far into the future. Maximum time is {_parrotConfig.MaxTriggerTime}.");
                return;
            }

            var userMessage = new UserParrotMessage(userId, message, DateTime.UtcNow + sendAfter);
            _parrotDbContext.UserMessages.Add(userMessage);
            await _parrotDbContext.SaveChangesAsync();

            await MarkSuccessful();
        }
    }
}
