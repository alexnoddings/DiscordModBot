using System;
using Elvet.Core.Plugins.Builder;

namespace Elvet.Core.Plugins.Extensions
{
    /// <summary>
    /// Extension methods for adding to <see cref="PluginDescriptor"/>s.
    /// </summary>
    public static class PluginDescriptorExtensions
    {
        /// <summary>
        /// Adds a type reader of type <paramref name="valueType"/> which reads <paramref name="readerType"/>s to the <paramref name="pluginDescriptor"/>.
        /// </summary>
        /// <param name="pluginDescriptor">The <see cref="PluginDescriptor"/> to add to.</param>
        /// <param name="valueType">The <see cref="Type"/> which is read.</param>
        /// <param name="readerType">The <see cref="Type"/> which does the reading.</param>
        public static void AddTypeReader(this PluginDescriptor pluginDescriptor, Type valueType, Type readerType)
        {
            if (pluginDescriptor is null)
                throw new ArgumentNullException(nameof(pluginDescriptor));

            if (!pluginDescriptor.TryAddTypeReader(valueType, readerType))
                throw new InvalidOperationException($"TypeReader for {valueType.Name} already added to plugin {pluginDescriptor.PluginType.Name}.");
        }

        /// <summary>
        /// Adds a module of type <paramref name="moduleType"/> to the <paramref name="pluginDescriptor"/>.
        /// </summary>
        /// <param name="pluginDescriptor">The <see cref="PluginDescriptor"/> to add to.</param>
        /// <param name="moduleType">The <see cref="Type"/> of the module.</param>
        public static void AddModule(this PluginDescriptor pluginDescriptor, Type moduleType)
        {
            if (pluginDescriptor is null)
                throw new ArgumentNullException(nameof(pluginDescriptor));

            if (!pluginDescriptor.TryAddModule(moduleType))
                throw new InvalidOperationException($"Module {moduleType.Name} already added to plugin {pluginDescriptor.PluginType.Name}.");
        }
    }
}
