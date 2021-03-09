using System;
using System.Linq;
using Elvet.Core.Config.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Elvet.Core.Config.Extensions
{
    /// <summary>
    /// Extension methods for getting configuration from an <see cref="IConfiguration" />.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Gets a <typeparamref name="TConfig" /> from the <paramref name="configuration" /> with the given <paramref name="pathSections" />.
        /// </summary>
        /// <typeparam name="TConfig">The configuration.</typeparam>
        /// <param name="configuration">The <see cref="IConfiguration" /> to get from.</param>
        /// <param name="pathSections">The path of the config from the root of the <paramref name="configuration" />.</param>
        /// <returns>The <typeparamref name="TConfig" /> from the <paramref name="configuration" />.</returns>
        /// <exception cref="MissingConfigurationException">Thrown when the <paramref name="pathSections" /> did not contain a <typeparamref name="TConfig" />.</exception>
        public static TConfig GetRequiredSection<TConfig>(this IConfiguration configuration, params string[] pathSections)
            where TConfig : class
        {
            var configSection = configuration ?? throw new ArgumentNullException(nameof(configuration));
            configSection = pathSections.Aggregate(configSection, (current, sectionKey) => current.GetSection(sectionKey));

            var config = configSection.Get<TConfig>();
            if (config is not null)
                return config;

            var path = string.Join(" > ", pathSections);
            throw new MissingConfigurationException($"Required configuration is missing or empty: {path}");
        }
    }
}
