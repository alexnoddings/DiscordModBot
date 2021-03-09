using System;
using Discord.Commands;
using Elvet.Core.Plugins.Builder;

namespace Elvet.Core.Plugins.Extensions
{
    /// <summary>
    /// Extension methods for adding to an <see cref="IPluginBuilder"/>.
    /// </summary>
    public static class PluginBuilderExtensions
    {
        /// <summary>
        /// Adds a type reader <typeparamref name="TReader"/> which reads <typeparamref name="TValue"/>s to the <paramref name="pluginBuilder"/>.
        /// </summary>
        /// <typeparam name="TValue">The type which is read.</typeparam>
        /// <typeparam name="TReader">The type which does the reading.</typeparam>
        /// <param name="pluginBuilder">The <see cref="IPluginBuilder"/> to add to.</param>
        /// <returns>The <paramref name="pluginBuilder"/> with the type reader added to enable chaining.</returns>
        public static IPluginBuilder AddTypeReader<TValue, TReader>(this IPluginBuilder pluginBuilder) where TReader : TypeReader =>
            pluginBuilder.AddTypeReader(typeof(TValue), typeof(TReader));

        /// <summary>
        /// Adds a type reader of type <paramref name="valueType"/> which reads <paramref name="readerType"/>s to the <paramref name="pluginBuilder"/>.
        /// </summary>
        /// <param name="pluginBuilder">The <see cref="IPluginBuilder"/> to add to.</param>
        /// <param name="valueType">The <see cref="Type"/> which is read.</param>
        /// <param name="readerType">The <see cref="Type"/> which does the reading.</param>
        /// <returns>The <paramref name="pluginBuilder"/> with the type reader added to enable chaining.</returns>
        public static IPluginBuilder AddTypeReader(this IPluginBuilder pluginBuilder, Type valueType, Type readerType)
        {
            pluginBuilder.PluginDescriptor.AddTypeReader(valueType, readerType);
            return pluginBuilder;
        }

        /// <summary>
        /// Adds a module of type <typeparamref name="TModule"/> to the <paramref name="pluginBuilder"/>.
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <param name="pluginBuilder">The <see cref="IPluginBuilder"/> to add to.</param>
        /// <returns>The <paramref name="pluginBuilder"/> with the module added to enable chaining.</returns>
        public static IPluginBuilder AddModule<TModule>(this IPluginBuilder pluginBuilder) where TModule : ModuleBase<SocketCommandContext> =>
            pluginBuilder.AddModule(typeof(TModule));

        /// <summary>
        /// Adds a module of type <paramref name="moduleType"/> to the <paramref name="pluginBuilder"/>.
        /// </summary>
        /// <param name="pluginBuilder">The <see cref="IPluginBuilder"/> to add to.</param>
        /// <param name="moduleType">The <see cref="Type"/> of the module.</param>
        /// <returns>The <paramref name="pluginBuilder"/> with the module added to enable chaining.</returns>
        public static IPluginBuilder AddModule(this IPluginBuilder pluginBuilder, Type moduleType)
        {
            pluginBuilder.PluginDescriptor.AddModule(moduleType);
            return pluginBuilder;
        }
    }
}
