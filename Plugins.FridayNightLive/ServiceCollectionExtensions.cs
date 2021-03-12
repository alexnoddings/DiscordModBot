using Elvet.Core.Config.Extensions;
using Elvet.Core.Plugins.Builder;
using Elvet.Core.Plugins.Extensions;
using Elvet.FridayNightLive.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Elvet.FridayNightLive
{
    /// <summary>
    /// Extension methods for adding the FridayNightLive plugin to an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the FridayNightLive plugin and it's required services to the <paramref name="pluginsBuilder" />.
        /// </summary>
        /// <param name="pluginsBuilder">The <see cref="IElvetPluginsBuilder" /> to add the plugin to.</param>
        /// <returns>The <paramref name="pluginsBuilder" /> with the plugin added to enable chaining.</returns>
        public static IElvetPluginsBuilder AddFridayNightLivePlugin(this IElvetPluginsBuilder pluginsBuilder)
        {
            pluginsBuilder.Services
                .AddValidatedConfig<FridayNightLiveConfig>("Elvet", "FridayNightLive")
                .AddDbContext<FridayNightLiveDbContext>((services, options) => options.UseSqlServer(services.GetConnectionString<FridayNightLiveConfig>()))
                .AddScoped<FridayNightLiveService>();

            pluginsBuilder
                .AddPlugin<FridayNightLivePlugin>()
                .AddModule<FridayNightLiveModule>();

            return pluginsBuilder;
        }
    }
}
