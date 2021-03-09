using System;

namespace Elvet.Core.Config.Exceptions
{
    /// <summary>
    /// The exception thrown when dealing with bad configuration.
    /// </summary>
    public class BadConfigurationException : ConfigurationException
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="BadConfigurationException" />.
        /// </summary>
        public BadConfigurationException()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BadConfigurationException" />.
        /// </summary>
        /// <inheritdoc />
        public BadConfigurationException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BadConfigurationException" />.
        /// </summary>
        /// <inheritdoc />
        public BadConfigurationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BadConfigurationException" />.
        /// </summary>
        /// <param name="configItem">The name of the bad configuration item.</param>
        /// <param name="configName">The name of the configuration type.</param>
        /// <param name="message">Why the configuration is bad.</param>
        public BadConfigurationException(string configItem, string configName, string message) : base($"Invalid {configItem} on {configName}: {message}")
        {
        }
    }
}
