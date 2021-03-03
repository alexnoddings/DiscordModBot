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
    }
}
