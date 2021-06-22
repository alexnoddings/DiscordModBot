using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Elvet.Core.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Elvet.InCharacter
{
    /// <summary>
    /// The InCharacter plugin.
    /// </summary>
    internal class InCharacterPlugin : ElvetPluginBase<InCharacterService, InCharacterConfig>
    {
        private readonly DiscordSocketClient _discordClient;

        /// <summary>
        /// Initialises a new instance of the <see cref="InCharacterPlugin" />.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger" />.</param>
        /// <param name="serviceScopeFactory">The <see cref="IServiceScopeFactory" /> used to enabled running scoped operations.</param>
        /// <param name="config">The <see cref="InCharacterConfig" />.</param>
        /// <param name="discordClient">The <see cref="DiscordSocketClient" /> to use.</param>
        public InCharacterPlugin(ILogger<InCharacterPlugin> logger, IServiceScopeFactory serviceScopeFactory, InCharacterConfig config, DiscordSocketClient discordClient)
            : base(logger, serviceScopeFactory, config)
        {
            _discordClient = discordClient;
        }

        protected override Task OnStartAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Starting InCharacter.");

            _discordClient.MessageReceived += OnMessageReceivedAsync;

            return Task.CompletedTask;
        }

        private Task OnMessageReceivedAsync(SocketMessage message) =>
            RunOnScopedServiceAsync(service => service.OnMessageReceivedAsync(message));

        protected override Task OnStopAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Stopping InCharacter.");

            _discordClient.MessageReceived -= OnMessageReceivedAsync;

            return Task.CompletedTask;
        }
    }
}
