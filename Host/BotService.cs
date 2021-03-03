using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Elvet.Core.Commands;
using Elvet.Core.Config;
using Elvet.Core.Extensions;
using Elvet.Core.Plugins.Registration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Elvet.Host
{
    /// <summary>
    /// Long-running service which hosts the Bot.
    /// </summary>
    public class BotService : IHostedService
    {
        private readonly ILogger<BotService> _logger;
        private readonly ILogger<DiscordSocketClient> _discordClientLogger;
        private readonly IBotConfig _config;
        private readonly DiscordSocketClient _discordClient;
        private readonly ICommandHandler _commandHandler;
        private readonly IPluginRegister _pluginRegister;

        /// <summary>
        /// Initialises a new instance of the <see cref="BotService" />.
        /// </summary>
        /// <param name="logger">The logger to use for this class.</param>
        /// <param name="discordClientLogger">The logger to use when logging events from the <paramref name="discordClient" />.</param>
        /// <param name="config">The <see cref="IBotConfig" />.</param>
        /// <param name="discordClient">The Websocket-based <see cref="DiscordSocketClient" />.</param>
        /// <param name="commandHandler">The <see cref="CommandHandler" />.</param>
        /// <param name="pluginRegister">The <see cref="IPluginRegister" />.</param>
        public BotService(ILogger<BotService> logger, ILogger<DiscordSocketClient> discordClientLogger,
            IBotConfig config, DiscordSocketClient discordClient, ICommandHandler commandHandler,
            IPluginRegister pluginRegister)
        {
            _logger = logger;
            _discordClientLogger = discordClientLogger;
            _config = config;
            _discordClient = discordClient;
            _commandHandler = commandHandler;
            _pluginRegister = pluginRegister;
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> that represents the service starting.</returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _discordClient.Log += LogDiscordClientMessage;

            _logger.LogInformation("Starting Discord Client.");
            await _discordClient.LoginAsync(TokenType.Bot, _config.Token, true);
            await _discordClient.StartAsync();
            await _discordClient.SetStatusAsync(UserStatus.DoNotDisturb);

            _logger.LogInformation("Starting Plugins.");
            await Task.WhenAll(_pluginRegister.Plugins.Select(plugin => plugin.StartAsync(cancellationToken)));

            _logger.LogInformation("Starting command handler.");
            await _commandHandler.StartAsync();

            await _discordClient.SetStatusAsync(UserStatus.Online);
            _logger.LogInformation("Bot started.");
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        /// <returns>A <see cref="Task" /> that represents the service stopping.</returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping command handler.");
            await _commandHandler.StopAsync();

            _logger.LogInformation("Stopping Plugins.");
            await Task.WhenAll(_pluginRegister.Plugins.Select(module => module.StopAsync(cancellationToken)));

            _logger.LogInformation("Stopping Discord Client.");
            await _discordClient.LogoutAsync();
            await _discordClient.StopAsync();

            _discordClient.Log -= LogDiscordClientMessage;

            _logger.LogInformation("Bot stopped.");
        }

        /// <summary>
        /// Logs <see cref="LogMessage" />s from the <see cref="DiscordSocketClient" />.
        /// </summary>
        /// <param name="message">The <see cref="LogMessage" /> to log.</param>
        /// <returns>A <see cref="Task" /> that represents the message being logged.</returns>
        private Task LogDiscordClientMessage(LogMessage message)
        {
            var level = message.Severity.ToLogLevel();
            _discordClientLogger.Log(level, message.Exception, message.Message, message.Source);
            return Task.CompletedTask;
        }
    }
}
