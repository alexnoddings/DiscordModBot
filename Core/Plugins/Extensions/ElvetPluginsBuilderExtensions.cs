using System;
using System.Linq;
using Elvet.Core.Plugins.Builder;
using Elvet.Core.Plugins.Builder.Internal;

namespace Elvet.Core.Plugins.Extensions
{
    /// <summary>
    /// Extensions methods for adding <see cref="IElvetPlugin"/>s to an <see cref="IElvetPluginsBuilder"/>.
    /// </summary>
    public static class ElvetPluginsBuilderExtensions
    {
        /// <summary>
        /// Adds a plugin of type <typeparamref name="TPlugin"/> to the <paramref name="pluginsBuilder"/>.
        /// </summary>
        /// <typeparam name="TPlugin">The type of <see cref="IElvetPlugin"/> to add.</typeparam>
        /// <param name="pluginsBuilder">The <see cref="IElvetPluginsBuilder"/> to add to.</param>
        /// <returns>A <see cref="IPluginBuilder"/> which can be used to add to the plugin.</returns>
        public static IPluginBuilder AddPlugin<TPlugin>(this IElvetPluginsBuilder pluginsBuilder) =>
            pluginsBuilder.AddPlugin(typeof(TPlugin));

        /// <summary>
        /// Adds a plugin of type <paramref name="pluginType"/> to the <paramref name="pluginsBuilder"/>.
        /// </summary>
        /// <param name="pluginsBuilder">The <see cref="IElvetPluginsBuilder"/> to add to.</param>
        /// <param name="pluginType">The <see cref="Type"/> of <see cref="IElvetPlugin"/> to add.</param>
        /// <returns>A <see cref="IPluginBuilder"/> which can be used to add to the plugin.</returns>
        public static IPluginBuilder AddPlugin(this IElvetPluginsBuilder pluginsBuilder, Type pluginType)
        {
            if (pluginsBuilder is null)
                throw new ArgumentNullException(nameof(pluginType));

            if (pluginType is null)
                throw new ArgumentNullException(nameof(pluginType));

            if (pluginsBuilder.Plugins.Any(pluginDescriptor => pluginDescriptor.PluginType == pluginType))
                throw new ArgumentException($"Plugin {pluginType.Name} already added.", nameof(pluginType));

            var descriptor = new PluginDescriptor(pluginType);
            pluginsBuilder.Plugins.Add(descriptor);
            return new PluginBuilder(descriptor, pluginsBuilder);
        }
    }
}
