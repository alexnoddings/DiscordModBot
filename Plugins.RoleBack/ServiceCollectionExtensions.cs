using Elvet.Core.Config.Extensions;
using Elvet.Core.Plugins.Builder;
using Elvet.Core.Plugins.Extensions;
using Elvet.RoleBack.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Elvet.RoleBack
{
    /// <summary>
    /// Extension methods for adding the RoleBack plugin to an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the RoleBack plugin and it's required services to the <paramref name="pluginBuilder" />.
        /// </summary>
        /// <param name="pluginBuilder">The <see cref="IElvetPluginsBuilder" /> to add the plugin to.</param>
        /// <returns>The <paramref name="pluginBuilder" /> with the plugin added to enable chaining.</returns>
        public static IElvetPluginsBuilder AddRoleBackPlugin(this IElvetPluginsBuilder pluginBuilder)
        {
            pluginBuilder.Services
                .AddValidatedConfig<RoleBackConfig>("Elvet", "RoleBack")
                .AddDbContext<RoleBackDbContext>((services, options) => options.UseSqlServer(services.GetRequiredService<RoleBackConfig>().ConnectionString))
                .AddScoped<RoleBackService>();

            pluginBuilder
                .AddPlugin<RoleBackPlugin>();

            return pluginBuilder;
        }
    }
}
