using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Elvet.Core.Commands;
using Elvet.Core.Config;
using Elvet.Core.Extensions;
using Elvet.Core.Plugins.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Elvet.Host
{
    /// <summary>
    /// Long-running service which hosts the Bot.
    /// </summary>
    internal class BotService : IHostedService, IDisposable
    {
        private readonly ILogger<BotService> _logger;
        private readonly ILogger<DiscordSocketClient> _discordClientLogger;
        private readonly IBotConfig _config;
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly DiscordRestClient _discordRestClient;
        private readonly ICommandHandler _commandHandler;
        private readonly IPluginRegister _pluginRegister;

        /// <summary>
        /// Initialises a new instance of the <see cref="BotService" />.
        /// </summary>
        /// <param name="logger">The logger to use for this class.</param>
        /// <param name="discordClientLogger">The logger to use when logging events from the <paramref name="discordSocketClient" />.</param>
        /// <param name="config">The <see cref="IBotConfig" />.</param>
        /// <param name="discordSocketClient">The Websocket-based <see cref="DiscordSocketClient" />.</param>
        /// <param name="discordRestClient">The Rest-based <see cref="DiscordRestClient" />.</param>
        /// <param name="commandHandler">The <see cref="CommandHandler" />.</param>
        /// <param name="pluginRegister">The <see cref="IPluginRegister" />.</param>
        public BotService(ILogger<BotService> logger, ILogger<DiscordSocketClient> discordClientLogger,
            IBotConfig config, DiscordSocketClient discordSocketClient, DiscordRestClient discordRestClient,
            ICommandHandler commandHandler, IPluginRegister pluginRegister)
        {
            _logger = logger;
            _discordClientLogger = discordClientLogger;
            _config = config;
            _discordSocketClient = discordSocketClient;
            _discordSocketClient.Log += LogDiscordClientMessage;
            _discordRestClient = discordRestClient;
            _discordRestClient.Log += LogDiscordClientMessage;
            _commandHandler = commandHandler;
            _pluginRegister = pluginRegister;
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> that represents the service starting.</returns>
        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Starting Discord Socket Client.");
            await _discordSocketClient.LoginAsync(TokenType.Bot, _config.Token, true);
            await _discordSocketClient.StartAsync();

            _logger.LogInformation("Starting Discord Rest Client.");
            await _discordRestClient.LoginAsync(TokenType.Bot, _config.Token, true);

            _logger.LogInformation("Starting Plugins.");
            await Task.WhenAll(_pluginRegister.PluginsInstanceInfos.Select(pluginInfo => pluginInfo.Instance.StartAsync(cancellationToken)));

            _logger.LogInformation("Starting command handler.");
            await _commandHandler.StartAsync(cancellationToken);

            _logger.LogInformation("Bot started.");
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        /// <returns>A <see cref="Task" /> that represents the service stopping.</returns>
        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            await _discordSocketClient.SetStatusAsync(UserStatus.DoNotDisturb);

            _logger.LogInformation("Stopping command handler.");
            await _commandHandler.StopAsync(cancellationToken);

            _logger.LogInformation("Stopping Plugins.");
            await Task.WhenAll(_pluginRegister.PluginsInstanceInfos.Select(instanceInfo => instanceInfo.Instance.StopAsync(cancellationToken)));

            _logger.LogInformation("Stopping Discord Socket Client.");
            await _discordSocketClient.LogoutAsync();
            await _discordSocketClient.StopAsync();

            _logger.LogInformation("Stopping Discord Rest Client.");
            await _discordRestClient.LogoutAsync();

            _logger.LogInformation("Bot stopped.");
        }

        /// <summary>
        /// Logs <see cref="LogMessage" />s from a <see cref="BaseDiscordClient" />.
        /// </summary>
        /// <param name="message">The <see cref="LogMessage" /> to log.</param>
        /// <returns>A <see cref="Task" /> that represents the message being logged.</returns>
        private Task LogDiscordClientMessage(LogMessage message)
        {
            var level = message.Severity.ToLogLevel();
            var content = message.Message is null ? "[No log message]" : $"[{message.Source}] {message.Message}";
            _discordClientLogger.Log(level, message.Exception, content);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _discordSocketClient.Log -= LogDiscordClientMessage;
            _discordRestClient.Log -= LogDiscordClientMessage;
        }
    }
}
