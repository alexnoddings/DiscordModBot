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

        public BotConfig Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new BadConfigurationException("Invalid bot name. Cannot be null or whitespace.");

            if (string.IsNullOrWhiteSpace(Token))
                throw new BadConfigurationException("Invalid bot token. Cannot be null or whitespace.");

            return this;
        }
    }
}
