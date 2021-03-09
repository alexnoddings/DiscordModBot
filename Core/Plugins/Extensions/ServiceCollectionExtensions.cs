using System;
using System.Collections.Generic;
using Elvet.Core.Plugins.Builder;
using Elvet.Core.Plugins.Builder.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elvet.Core.Plugins.Extensions
{
    /// <summary>
    /// Extension methods for adding plugins to an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds plugins to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add to.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> used.</param>
        /// <param name="configurePlugins">An <see cref="Action"/> which adds <see cref="IElvetPlugin"/>s to an <see cref="IElvetPluginsBuilder"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddPlugins(this IServiceCollection services, IConfiguration configuration, Action<IElvetPluginsBuilder> configurePlugins)
        {
            var builder = new PluginsBuilder(services, configuration);
            configurePlugins(builder);

            foreach (var pluginInfo in builder.Plugins)
            {
                services.AddSingleton(pluginInfo.PluginType);
                foreach (var (valueType, readerType) in pluginInfo.TypeReaderTypes)
                {
                    services.AddSingleton(readerType);
                }
            }

            services.AddSingleton<IPluginCollection>(builder.Plugins);
            services.AddSingleton<IPluginRegister>(services =>
            {
                var plugins = services.GetRequiredService<IPluginCollection>();

                var pluginInstanceInfos = new List<PluginInstanceInfo>();
                foreach (var pluginDescriptor in plugins)
                {
                    if (services.GetRequiredService(pluginDescriptor.PluginType) is not IElvetPlugin instance)
                        throw new InvalidOperationException($"Plugin type {pluginDescriptor.PluginType} is not assignable to {nameof(IElvetPlugin)}.");

                    pluginInstanceInfos.Add(new PluginInstanceInfo(instance, pluginDescriptor.TypeReaderTypes, pluginDescriptor.ModuleTypes));
                }

                return new PluginRegister(pluginInstanceInfos);
            });

            return services;
        }
    }
}
