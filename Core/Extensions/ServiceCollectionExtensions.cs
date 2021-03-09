using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Elvet.Core.Commands;
using Elvet.Core.Config;
using Elvet.Core.Config.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Elvet.Core.Extensions
{
    /// <summary>
    /// Extension methods for adding core Elvet services to an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the core Elvet services to the <paramref name="services"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add to.</param>
        /// <returns>The <paramref name="services" /> with the services added to enable chaining.</returns>
        public static IServiceCollection AddElvetCore(this IServiceCollection services) =>
            services
                .AddValidatedConfig<IBotConfig, BotConfig>("Elvet", "Bot")
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<DiscordRestClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<ICommandHandler, CommandHandler>();
    }
}
