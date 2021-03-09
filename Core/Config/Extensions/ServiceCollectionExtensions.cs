using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elvet.Core.Config.Extensions
{
    /// <summary>
    /// Extension methods for adding config to an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a <typeparamref name="TConfig" /> implementation to the <paramref name="services" /> with the given <paramref name="pathSections" />.
        /// </summary>
        /// <typeparam name="TConfig">The config implementation.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add to.</param>
        /// <param name="pathSections">The path to the config from the root.</param>
        /// <returns>The <paramref name="services" /> with the <see cref="IBotConfig" /> added to enable chaining.</returns>
        public static IServiceCollection AddConfig<TConfig>(this IServiceCollection services, params string[] pathSections)
            where TConfig : class =>
            AddConfig<TConfig, TConfig>(services, pathSections);

        /// <summary>
        /// Adds a <typeparamref name="TConfigImplementation" /> implementation of <typeparamref name="TConfig" /> to the <paramref name="services" /> with the given <paramref name="pathSections" />.
        /// </summary>
        /// <typeparam name="TConfig">The config type.</typeparam>
        /// <typeparam name="TConfigImplementation">The config type implementation.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add to.</param>
        /// <param name="pathSections">The path to the config from the root.</param>
        /// <returns>The <paramref name="services" /> with the <see cref="IBotConfig" /> added to enable chaining.</returns>
        public static IServiceCollection AddConfig<TConfig, TConfigImplementation>(this IServiceCollection services, params string[] pathSections)
            where TConfig : class
            where TConfigImplementation : class, TConfig =>
            services.AddTransient<TConfig>(innerServices =>
                innerServices
                    .GetRequiredService<IConfiguration>()
                    .GetRequiredSection<TConfigImplementation>(pathSections));

        /// <summary>
        /// Adds a validate-able <typeparamref name="TConfig" /> implementation to the <paramref name="services" /> with the given <paramref name="pathSections" />.
        /// </summary>
        /// <typeparam name="TConfig">The validate-able config implementation.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add to.</param>
        /// <param name="pathSections">The path to the config from the root.</param>
        /// <returns>The <paramref name="services" /> with the <see cref="IBotConfig" /> added to enable chaining.</returns>
        public static IServiceCollection AddValidatedConfig<TConfig>(this IServiceCollection services, params string[] pathSections)
            where TConfig : class, IValidateAble<TConfig> =>
            AddValidatedConfig<TConfig, TConfig>(services, pathSections);

        /// <summary>
        /// Adds a validate-able <typeparamref name="TConfigImplementation" /> implementation of <typeparamref name="TConfig" /> to the <paramref name="services" /> with the given <paramref name="pathSections" />.
        /// </summary>
        /// <typeparam name="TConfig">The config type.</typeparam>
        /// <typeparam name="TConfigImplementation">The validate-able config type implementation.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add to.</param>
        /// <param name="pathSections">The path to the config from the root.</param>
        /// <returns>The <paramref name="services" /> with the <see cref="IBotConfig" /> added to enable chaining.</returns>
        public static IServiceCollection AddValidatedConfig<TConfig, TConfigImplementation>(this IServiceCollection services, params string[] pathSections)
            where TConfig : class
            where TConfigImplementation : class, TConfig, IValidateAble<TConfigImplementation> =>
            services.AddTransient<TConfig>(innerServices =>
                innerServices
                    .GetRequiredService<IConfiguration>()
                    .GetRequiredSection<TConfigImplementation>(pathSections)
                    .Validate());
    }
}
