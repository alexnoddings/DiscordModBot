using System;
using Elvet.Core.Plugins;
using Elvet.Core.Plugins.Config;
using Microsoft.Extensions.DependencyInjection;

namespace Elvet.Core.Config.Extensions
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Gets a <typeparamref name="TPluginConfig"/> from the <paramref name="services"/>.
        /// </summary>
        /// <typeparam name="TPluginConfig">The plugin config type.</typeparam>
        /// <param name="services">The <see cref="IServiceProvider"/> to get the <typeparamref name="TPluginConfig"/> from.</param>
        /// <returns>The <typeparamref name="TPluginConfig"/>.</returns>
        public static TPluginConfig GetPluginConfig<TPluginConfig>(this IServiceProvider services)
            where TPluginConfig : class, IPluginConfig =>
            services.GetRequiredService<TPluginConfig>();

        /// <summary>
        /// Gets the connection string for a <typeparamref name="TConnectionStringConfig"/>.
        /// </summary>
        /// <typeparam name="TConnectionStringConfig">The plugin connection string config type.</typeparam>
        /// <param name="services">The <see cref="IServiceProvider"/> to get the <typeparamref name="TConnectionStringConfig"/>'s connection string from.</param>
        /// <returns>The <see cref="IConnectionStringConfig.ConnectionString"/> from the <typeparamref name="TConnectionStringConfig"/>.</returns>
        public static string GetConnectionString<TConnectionStringConfig>(this IServiceProvider services)
            where TConnectionStringConfig : class, IPluginConfig, IConnectionStringConfig =>
            services.GetPluginConfig<TConnectionStringConfig>().ConnectionString;
    }
}
