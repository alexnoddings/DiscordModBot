using Elvet.Core.Config.Exceptions;

namespace Elvet.Core.Config
{
    /// <summary>
    /// Validate-able <see cref="IBotConfig" /> implementation.
    /// </summary>
    internal class BotConfig : IBotConfig, IValidateAble<BotConfig>
    {
        public string Name { get; set; } = "Elvet";

        public string Token { get; set; } = string.Empty;

        public string CommandPrefix { get; set; } = ",,";

        public string DefaultConnectionString { get; set; } = string.Empty;

        public BotConfig Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new BadConfigurationException(nameof(Name), nameof(BotConfig), "cannot be null or whitespace.");

            if (string.IsNullOrWhiteSpace(Token))
                throw new BadConfigurationException(nameof(Token), nameof(BotConfig), "cannot be null or whitespace.");

            CommandPrefix ??= ",,";

            if (string.IsNullOrWhiteSpace(DefaultConnectionString))
                throw new BadConfigurationException(nameof(DefaultConnectionString), nameof(BotConfig), "cannot be null or whitespace.");

            return this;
        }
    }
}
