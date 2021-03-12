namespace Elvet.Core.Config
{
    /// <summary>
    /// Bot-wide configuration options.
    /// </summary>
    public interface IBotConfig
    {
        /// <summary>
        /// The name of the Bot.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The application's Bot Token.
        /// </summary>
        public string Token { get; }

        /// <summary>
        /// The prefix before Bot commands.
        /// </summary>
        public string CommandPrefix { get; }

        /// <summary>
        /// The default connection string for plugins.
        /// </summary>
        public string DefaultConnectionString { get; }
    }
}
