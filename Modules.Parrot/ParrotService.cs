using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.Rest;
using Elvet.Parrot.Data;
using Microsoft.EntityFrameworkCore;

namespace Elvet.Parrot
{
    /// <summary>
    /// The scoped service which handles Parrot events.
    /// </summary>
    internal class ParrotService
    {
        private readonly ParrotDbContext _dbContext;
        private readonly DiscordRestClient _restClient;

        /// <summary>
        /// Initialises a new instance of the <see cref="ParrotService" />.
        /// </summary>
        /// <param name="dbContext">The <see cref="ParrotDbContext" /> to get data from.</param>
        /// <param name="restClient">The <see cref="DiscordRestClient" /> to use to interact with Discord.</param>
        public ParrotService(ParrotDbContext dbContext, DiscordRestClient restClient)
        {
            _dbContext = dbContext;
            _restClient = restClient;
        }

        /// <summary>
        /// Polls the database for any <see cref="UserParrotMessage"/>s which are due to be delivered.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> used to cancel the operation. Any unsent messages will be saved til next execution.</param>
        /// <returns>A <see cref="Task" /> that represents the messages being sent.</returns>
        public async Task SendOutstandingMessages(CancellationToken cancellationToken)
        {
            var messages = await _dbContext.UserMessages.AsQueryable().Where(msg => msg.TriggerAt < DateTime.UtcNow).ToListAsync(cancellationToken);
            foreach (var message in messages)
            {
                if (cancellationToken.IsCancellationRequested) return;

                var user = await _restClient.GetUserAsync(message.UserId);
                if (user is null)
                {
                    // Can't send a message to a user which doesn't exist any more.
                    _dbContext.Remove(message);
                    await _dbContext.SaveChangesAsync();
                }

                try
                {
                    await user.SendMessageAsync("[Parrot] " + message.Message);
                }
                catch (HttpException e) when (e.Message.Contains("cannot send messages to this user", StringComparison.OrdinalIgnoreCase))
                {
                    // Occurs when the bot is no longer able to send messages to a user. This could happen because the user blocks the bot intentionally,
                    // or could happen by accident if they no longer share a guild and have their privacy settings high.
                }
                finally
                {
                    // The message is removed regardless of whether the user was contacted. Saving them could cause a flood of messages to the user
                    // if they rejoin a guild with this bot in, and will have an unnecessary overhead for the bot to process every cycle.
                    _dbContext.Remove(message);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
