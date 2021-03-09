using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Elvet.Core.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Elvet.RoleBack
{
    /// <summary>
    /// The RoleBack plugin.
    /// </summary>
    internal class RoleBackPlugin : ElvetPluginBase<RoleBackService, RoleBackConfig>
    {
        private readonly DiscordSocketClient _discordClient;

        /// <summary>
        /// Initialises a new instance of the <see cref="RoleBackService" />.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger" />.</param>
        /// <param name="serviceScopeFactory">The <see cref="IServiceScopeFactory" /> used to enabled running scoped operations.</param>
        /// <param name="config">The <see cref="RoleBackConfig" />.</param>
        /// <param name="discordClient">The <see cref="DiscordSocketClient" /> to use.</param>
        public RoleBackPlugin(ILogger<RoleBackPlugin> logger, IServiceScopeFactory serviceScopeFactory, RoleBackConfig config, DiscordSocketClient discordClient)
            : base(logger, serviceScopeFactory, config)
        {
            _discordClient = discordClient;
        }

        protected override Task OnStartAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Starting RoleBack.");

            _discordClient.UserJoined += OnUserJoinedGuild;
            _discordClient.UserLeft += OnUserLeftGuild;

            return Task.CompletedTask;
        }

        protected override Task OnStopAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Stopping RoleBack.");

            _discordClient.UserJoined -= OnUserJoinedGuild;
            _discordClient.UserLeft -= OnUserLeftGuild;

            return Task.CompletedTask;
        }

        private Task OnUserJoinedGuild(SocketGuildUser guildUser) =>
            RunOnScopedServiceAsync(service => service.OnUserJoinedGuild(guildUser));

        private Task OnUserLeftGuild(SocketGuildUser guildUser) =>
            RunOnScopedServiceAsync(service => service.OnUserLeftGuild(guildUser));
    }
}
