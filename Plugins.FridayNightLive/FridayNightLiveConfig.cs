using Elvet.Core;
using Elvet.Core.Plugins.Config;

namespace Elvet.FridayNightLive
{
    internal class FridayNightLiveConfig : IPluginConfig, IConnectionStringConfig, IValidateAble<FridayNightLiveConfig>
    {
        public bool Enabled { get; set; } = true;

        public string? ConnectionString { get; set; }

        public FridayNightLiveConfig Validate() => this;
    }
}
