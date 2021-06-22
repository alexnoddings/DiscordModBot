using Elvet.Core.Config.Extensions;
using Elvet.Core.Plugins.Builder;
using Elvet.Core.Plugins.Extensions;
using Elvet.InCharacter.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Elvet.InCharacter
{
    /// <summary>
    /// Extension methods for adding the InCharacter plugin to an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the InCharacter plugin and it's required services to the <paramref name="pluginsBuilder" />.
        /// </summary>
        /// <param name="pluginsBuilder">The <see cref="IElvetPluginsBuilder" /> to add the plugin to.</param>
        /// <returns>The <paramref name="pluginsBuilder" /> with the plugin added to enable chaining.</returns>
        public static IElvetPluginsBuilder AddInCharacterPlugin(this IElvetPluginsBuilder pluginsBuilder)
        {
            pluginsBuilder.Services
                .AddValidatedConfig<InCharacterConfig>("Elvet", "InCharacter")
                .AddDbContext<InCharacterDbContext>((services, options) => options.UseSqlServer(services.GetConnectionString<InCharacterConfig>()))
                .AddScoped<InCharacterService>();

            pluginsBuilder
                .AddPlugin<InCharacterPlugin>()
                .AddModule<InCharacterModule>();

            return pluginsBuilder;
        }
    }
}
