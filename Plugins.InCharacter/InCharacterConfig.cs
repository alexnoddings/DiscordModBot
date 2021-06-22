using Elvet.Core;
using Elvet.Core.Plugins.Config;

namespace Elvet.InCharacter
{
    internal class InCharacterConfig : IPluginConfig, IConnectionStringConfig, IValidateAble<InCharacterConfig>
    {
        public bool Enabled { get; set; } = true;

        public string? ConnectionString { get; set; } = string.Empty;

        public InCharacterConfig Validate() => this;
    }
}
