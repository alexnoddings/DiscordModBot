using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Elvet.Core.Commands
{
    /// <summary>
    /// Base class for Elvet modules.
    /// </summary>
    public abstract class ElvetModuleBase : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// The emote used by <see cref="MarkSuccessful"/>.
        /// </summary>
        protected virtual IEmote SuccessReactionEmote { get; } = new Emoji("✅");

        /// <summary>
        /// The emote used by <see cref="MarkError"/>.
        /// </summary>
        protected virtual IEmote ErrorReactionEmote { get; } = new Emoji("❌");

        /// <summary>
        /// Denotes the message which invoked the current command as having been executed successfully.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the reaction being added.</returns>
        protected Task MarkSuccessful() =>
            Context.Message.AddReactionAsync(SuccessReactionEmote);

        /// <summary>
        /// Denotes the message which invoked the current command as having had an error occur during execution.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the reaction being added.</returns>
        protected Task MarkError() =>
            Context.Message.AddReactionAsync(ErrorReactionEmote);

        /// <summary>
        /// Replies to the message which invoked the current command.
        /// </summary>
        /// <inheritdoc cref="ModuleBase{T}.ReplyAsync"/>
        protected override Task<IUserMessage> ReplyAsync(string? message = null, bool isTts = false, Embed? embed = null,
            RequestOptions? options = null, AllowedMentions? allowedMentions = null, MessageReference? messageReference = null) =>
            Context.Message.ReplyAsync(message, isTts, embed, allowedMentions, options);

        /// <inheritdoc cref="ModuleBase{T}.ReplyAsync"/>
        protected Task<IUserMessage> SendAsync(string? message = null, bool isTts = false, Embed? embed = null,
            RequestOptions? options = null, AllowedMentions? allowedMentions = null, MessageReference? messageReference = null) =>
            base.ReplyAsync(message, isTts, embed, options, allowedMentions, messageReference);
    }
}
