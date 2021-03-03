using System;

namespace Elvet.Core.Config.Exceptions
{
    /// <summary>
    /// The exception thrown when dealing with missing configuration.
    /// </summary>
    public class MissingConfigurationException : ConfigurationException
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="BadConfigurationException" />.
        /// </summary>
        public MissingConfigurationException()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BadConfigurationException" />.
        /// </summary>
        /// <inheritdoc />
        public MissingConfigurationException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BadConfigurationException" />.
        /// </summary>
        /// <inheritdoc />
        public MissingConfigurationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
