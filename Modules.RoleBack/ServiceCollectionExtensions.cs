using Elvet.Core.Config.Extensions;
using Elvet.Core.Plugins.Registration;
using Elvet.RoleBack.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elvet.RoleBack
{
    /// <summary>
    /// Extension methods for adding the RoleBack plugin to an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the RoleBack plugin and it's required services to the <paramref name="services" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the plugin to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> to use when configuring the plugin.</param>
        /// <returns>The <paramref name="services" /> with the plugin added to enable chaining.</returns>
        public static IServiceCollection AddRoleBackPlugin(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddValidatedConfig<RoleBackConfig>("Elvet", "RoleBack")
                .AddDbContext<RoleBackDbContext>(options => options.UseSqlServer(configuration.GetRequiredSection<RoleBackConfig>("Elvet", "RoleBack").Validate().ConnectionString))
                .AddScoped<RoleBackService>()
                .AddPlugin<RoleBackPlugin>();
    }
}
