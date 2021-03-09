using System;

namespace Elvet.Core.Plugins.Builder
{
    /// <summary>
    /// The exception thrown when dealing with bad types.
    /// </summary>
    public class BadTypeException : ArgumentException
    {
        public Type? ExpectedType { get; }
        public Type? ActualType { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="BadTypeException"/>.
        /// </summary>
        public BadTypeException()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BadTypeException"/>.
        /// </summary>
        /// <inheritdoc />
        public BadTypeException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BadTypeException"/>.
        /// </summary>
        /// <inheritdoc />
        public BadTypeException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BadTypeException"/>.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName">Which parameter the bad type is.</param>
        /// <param name="expectedType">Which type was expected.</param>
        /// <param name="actualType">Which type was received.</param>
        public BadTypeException(string? message, string? paramName, Type expectedType, Type actualType)
            : base($"Bad type. Expected {expectedType.Name}, but got {actualType.Name}. {message}.", paramName)
        {
            ExpectedType = expectedType;
            ActualType = actualType;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BadTypeException"/>.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName">Which parameter the bad type is.</param>
        /// <param name="expectedType">Which type was expected.</param>
        /// <param name="actualType">Which type was received.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public BadTypeException(string? message, string? paramName, Type expectedType, Type actualType, Exception? innerException)
            : base($"Bad type. Expected {expectedType.Name}, but got {actualType.Name}. {message}.", paramName, innerException)
        {
            ExpectedType = expectedType;
            ActualType = actualType;
        }
    }
}
