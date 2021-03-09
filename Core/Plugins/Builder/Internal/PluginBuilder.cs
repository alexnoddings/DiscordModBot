using System;

namespace Elvet.Core.Plugins.Builder.Internal
{
    internal class PluginBuilder : IPluginBuilder
    {
        public PluginDescriptor PluginDescriptor { get; }
        public IElvetPluginsBuilder PluginsBuilder { get; }

        public PluginBuilder(PluginDescriptor pluginDescriptor, IElvetPluginsBuilder pluginsBuilder)
        {
            if (pluginDescriptor is null)
                throw new ArgumentNullException(nameof(pluginDescriptor));

            if (pluginsBuilder is null)
                throw new ArgumentNullException(nameof(pluginsBuilder));

            PluginDescriptor = pluginDescriptor;
            PluginsBuilder = pluginsBuilder;
        }
    }
}
