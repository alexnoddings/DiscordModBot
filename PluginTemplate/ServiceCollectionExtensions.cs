using Elvet.Core.Config.Extensions;
using Elvet.Core.Plugins.Builder;
using Elvet.Core.Plugins.Extensions;
using $safeprojectname$.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace $safeprojectname$
{
    /// <summary>
    /// Extension methods for adding the $pluginname$ plugin to an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the $pluginname$ plugin and it's required services to the <paramref name="pluginsBuilder" />.
        /// </summary>
        /// <param name="pluginsBuilder">The <see cref="IElvetPluginsBuilder" /> to add the plugin to.</param>
        /// <returns>The <paramref name="pluginsBuilder" /> with the plugin added to enable chaining.</returns>
        public static IElvetPluginsBuilder Add$pluginname$Plugin(this IElvetPluginsBuilder pluginsBuilder)
        {
            pluginsBuilder.Services
                .AddValidatedConfig<$pluginname$Config>("Elvet", "$pluginname$")
                .AddDbContext<$pluginname$DbContext>((services, options) => options.UseSqlServer(services.GetRequiredService<$pluginname$Config>().ConnectionString))
                .AddScoped<$pluginname$Service>();

            pluginsBuilder
                .AddPlugin<$pluginname$Plugin>()
                .AddModule<$pluginname$Module>();

            return pluginsBuilder;
        }
    }
}
