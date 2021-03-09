using System.Collections.Generic;
using System.Linq;

namespace Elvet.Core.Plugins.Builder.Internal
{
    internal class PluginRegister : IPluginRegister
    {
        private readonly List<IPluginInstanceInfo> _pluginInstanceInstanceInfos;

        public IEnumerable<IPluginInstanceInfo> PluginsInstanceInfos => _pluginInstanceInstanceInfos;

        public PluginRegister(IEnumerable<IPluginInstanceInfo> pluginInstanceInstanceInfos)
        {
            _pluginInstanceInstanceInfos = pluginInstanceInstanceInfos.ToList();
        }
    }
}
