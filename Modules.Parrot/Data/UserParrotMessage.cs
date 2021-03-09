using System;
using Discord;

namespace Elvet.Parrot.Data
{
    internal class UserParrotMessage
    {
        /// <summary>
        /// An identifier for this message.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The <see cref="IUser.Id"/> of the user to send the message to.
        /// </summary>
        public ulong UserId { get; set; }

        /// <summary>
        /// The body of the message to send.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// The time after which the message should be sent.
        /// </summary>
        public DateTime TriggerAt { get; set; }

        public UserParrotMessage()
        {
        }

        public UserParrotMessage(ulong userId, string message, DateTime triggerAt)
        {
            UserId = userId;
            Message = message;
            TriggerAt = triggerAt;
        }
    }
}
