using System;

namespace Elvet.Core.Config.Exceptions
{
    /// <summary>
    /// The base exception thrown when dealing with configuration.
    /// </summary>
    public abstract class ConfigurationException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ConfigurationException" />.
        /// </summary>
        protected ConfigurationException()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ConfigurationException" /> with a specified
        /// <paramref name="message" />.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        protected ConfigurationException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ConfigurationException" /> with a specified <paramref name="message" />
        /// and an inner <paramref name="innerException" />.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <c><see langword="null" /></c> if no <paramref name="innerException"/> is specified.</param>
        protected ConfigurationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
