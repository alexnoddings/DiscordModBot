using Discord;
using Microsoft.Extensions.Logging;

namespace Elvet.Core.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="LogSeverity" />.
    /// </summary>
    public static class LogSeverityExtensions
    {
        /// <summary>
        /// Converts a <see cref="LogSeverity" /> to a <see cref="LogLevel" />.
        /// </summary>
        /// <param name="severity">The <see cref="LogSeverity" />.</param>
        /// <returns>The equal level <see cref="LogLevel" />.</returns>
        public static LogLevel ToLogLevel(this LogSeverity severity) =>
            severity switch
            {
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Error => LogLevel.Error,
                LogSeverity.Warning => LogLevel.Warning,
                LogSeverity.Info => LogLevel.Information,
                // These mappings are intentional.
                // LogSeverity's Debug is the most detailed level, whereas LogLevel's Trace is the most detailed level.
                LogSeverity.Verbose => LogLevel.Debug,
                LogSeverity.Debug => LogLevel.Trace,
                // Use Information level by default.
                _ => LogLevel.Information
            };
    }
}
