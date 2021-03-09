using System;

namespace Elvet.Core.Commands
{
    /// <summary>
    /// The exception thrown from a command which will be relayed back to the user.
    /// The <see cref="VisibleCommandException.InnerException"/> will not be visible,
    /// only the <see cref="VisibleCommandException.Message"/>.
    /// </summary>
    public class VisibleCommandException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="VisibleCommandException"/>.
        /// </summary>
        public VisibleCommandException()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="VisibleCommandException"/>.
        /// </summary>
        /// <inheritdoc />
        public VisibleCommandException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="VisibleCommandException"/>.
        /// </summary>
        /// <inheritdoc />
        public VisibleCommandException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
