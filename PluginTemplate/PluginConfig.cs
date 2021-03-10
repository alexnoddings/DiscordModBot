using Elvet.Core;
using Elvet.Core.Config.Exceptions;
using Elvet.Core.Plugins.Config;

namespace $safeprojectname$
{
    internal class $pluginname$Config : IPluginConfig, IConnectionStringConfig, IValidateAble<$pluginname$Config>
    {
        public bool Enabled { get; set; } = true;

        public string ConnectionString { get; set; } = string.Empty;

        public $pluginname$Config Validate()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
                throw new BadConfigurationException(nameof(ConnectionString), nameof($pluginname$Config), "cannot be null or whitespace.");

            return this;
        }
    }
}
