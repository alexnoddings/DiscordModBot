using Microsoft.Extensions.DependencyInjection;

namespace Elvet.Core.Plugins.Registration
{
    /// <summary>
    /// Extension methods for adding plugins to an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an <see cref="IPluginRegister" /> to the <paramref name="services" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add to.</param>
        /// <returns>The <paramref name="services" /> with the <see cref="IPluginRegister" /> added to enable chaining.</returns>
        public static IServiceCollection AddPluginRegister(this IServiceCollection services) =>
            services
                .AddSingleton<IPluginRegister, PluginRegister>()
                .Configure<PluginTypes>(_ => { });

        /// <summary>
        /// Adds a <typeparamref name="TPlugin" /> to the <paramref name="services" />.
        /// </summary>
        /// <typeparam name="TPlugin">The plugin to add.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add to.</param>
        /// <returns>The <paramref name="services" /> with the <typeparamref name="TPlugin" /> added to enable chaining.</returns>
        public static IServiceCollection AddPlugin<TPlugin>(this IServiceCollection services)
            where TPlugin : class, IElvetPlugin =>
            services
                .AddSingleton<TPlugin>()
                .Configure<PluginTypes>(register => register.Register<TPlugin>());
    }
}
