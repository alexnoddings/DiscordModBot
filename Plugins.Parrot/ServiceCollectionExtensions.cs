using Elvet.Core.Config.Extensions;
using Elvet.Core.Plugins.Builder;
using Elvet.Core.Plugins.Extensions;
using Elvet.Parrot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Elvet.Parrot
{
    /// <summary>
    /// Extension methods for adding the Parrot plugin to an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the Parrot plugin and it's required services to the <paramref name="pluginsBuilder" />.
        /// </summary>
        /// <param name="pluginsBuilder">The <see cref="IElvetPluginsBuilder" /> to add the plugin to.</param>
        /// <returns>The <paramref name="pluginsBuilder" /> with the plugin added to enable chaining.</returns>
        public static IElvetPluginsBuilder AddParrotPlugin(this IElvetPluginsBuilder pluginsBuilder)
        {
            pluginsBuilder.Services
                .AddValidatedConfig<ParrotConfig>("Elvet", "Parrot")
                .AddDbContext<ParrotDbContext>((services, options) => options.UseSqlServer(services.GetRequiredService<ParrotConfig>().ConnectionString))
                .AddScoped<ParrotService>();

            pluginsBuilder
                .AddPlugin<ParrotPlugin>()
                .AddModule<ParrotModule>();

            return pluginsBuilder;
        }
    }
}
