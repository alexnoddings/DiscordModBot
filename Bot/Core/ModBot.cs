using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elvet.Core
{
    public class ModBot : IModBot
    {
        private const string TokenConfigKey = "Bot:Token";

        private IConfiguration Configuration { get; }
        private DiscordSocketClient DiscordClient { get; }
        private CommandHandler CommandHandler { get; }
        private IServiceProvider Services { get; }
        private IEnumerable<IModBotPlugin> Plugins => Services.GetRequiredService<IEnumerable<IModBotPlugin>>();

        public ModBot(IConfiguration configuration, DiscordSocketClient discordClient, CommandHandler commandHandler, IServiceProvider services)
        {
            Configuration = configuration;
            DiscordClient = discordClient;
            CommandHandler = commandHandler;
            Services = services;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string loginToken = Configuration[TokenConfigKey];
            if (string.IsNullOrWhiteSpace(loginToken))
                throw new InvalidOperationException($"No bot token provided, please add a configuration item for {TokenConfigKey}");

            await DiscordClient.LoginAsync(TokenType.Bot, loginToken).ConfigureAwait(false);
            await DiscordClient.StartAsync().ConfigureAwait(false);

            await CommandHandler.StartAsync().ConfigureAwait(false);

            await Task.WhenAll(Plugins.Select(p => p.StartAsync(cancellationToken))).ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.WhenAll(Plugins.Select(p => p.StopAsync(cancellationToken))).ConfigureAwait(false);

            await CommandHandler.StopAsync().ConfigureAwait(false);

            await DiscordClient.StopAsync().ConfigureAwait(false);
        }

    }
}
