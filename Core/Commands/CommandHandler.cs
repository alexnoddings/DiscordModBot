using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Elvet.Core.Config;
using Elvet.Core.Plugins;
using Elvet.Core.Plugins.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Elvet.Core.Commands
{
    /// <inheritdoc cref="ICommandHandler" />
    internal class CommandHandler : ICommandHandler, IDisposable
    {
        private readonly ILogger<CommandHandler> _logger;
        private readonly IBotConfig _botConfig;
        private readonly CommandService _commandService;
        private readonly DiscordSocketClient _discordClient;
        private readonly IPluginRegister _pluginRegister;
        private readonly IServiceProvider _rootServices;
        private readonly IServiceScope _serviceScope;

        private bool _isInitialised = false;

        /// <summary>
        /// Initialises a new instance of the <see cref="CommandHandler" />.
        /// </summary>
        /// <param name="logger">The logger to use for this class.</param>
        /// <param name="botConfig">The Bot's <see cref="IBotConfig" />.</param>
        /// <param name="commandService">The <see cref="CommandService" /> to add modules to and execute commands on.</param>
        /// <param name="discordClient">The <see cref="DiscordSocketClient" /> to monitor command invocation.</param>
        /// <param name="pluginRegister">The <see cref="IPluginRegister" /> to get <see cref="IElvetPlugin" />s from.</param>
        /// <param name="rootServices">The root <see cref="IServiceProvider" /> to use for adding modules and executing commands.</param>
        public CommandHandler(ILogger<CommandHandler> logger, IBotConfig botConfig, CommandService commandService, DiscordSocketClient discordClient, IPluginRegister pluginRegister, IServiceProvider rootServices)
        {
            _logger = logger;
            _botConfig = botConfig;
            _commandService = commandService;
            _discordClient = discordClient;
            _pluginRegister = pluginRegister;
            _rootServices = rootServices;
            _serviceScope = rootServices.CreateScope();
        }

        /// <summary>
        /// Initialises the <see cref="CommandHandler"/>. This should only be performed once.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> that represents the handler starting.</returns>
        private Task InitialiseAsync(CancellationToken cancellationToken = default)
        {
            foreach (var pluginInfo in _pluginRegister.PluginsInstanceInfos)
            {
                foreach (var (valueType, readerType) in pluginInfo.TypeReaderTypes)
                {
                    var reader = _rootServices.GetRequiredService(readerType) as TypeReader;
                    _commandService.AddTypeReader(valueType, reader);
                }
            }

            _isInitialised = true;
            return Task.CompletedTask;
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            if (!_isInitialised)
                await InitialiseAsync(cancellationToken);

            var modules = _pluginRegister.PluginsInstanceInfos.SelectMany(pluginInfo => pluginInfo.ModuleTypes);
            await Task.WhenAll(modules.Select(moduleType => _commandService.AddModuleAsync(moduleType, _serviceScope.ServiceProvider)));

            _discordClient.MessageReceived += MessageReceivedAsync;
            _commandService.CommandExecuted += CommandExecutedAsync;
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            _discordClient.MessageReceived -= MessageReceivedAsync;
            _commandService.CommandExecuted -= CommandExecutedAsync;

            var modules = _commandService.Modules.ToList();
            await Task.WhenAll(modules.Select(module => _commandService.RemoveModuleAsync(module)));
        }

        private Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore messages not from users
            if (rawMessage is not SocketUserMessage message) return Task.CompletedTask;
            if (message.Source != MessageSource.User) return Task.CompletedTask;

            var argPos = 0;
            if (!message.HasStringPrefix(_botConfig.CommandPrefix, ref argPos, StringComparison.OrdinalIgnoreCase))
                return Task.CompletedTask;

            var commandContext = new SocketCommandContext(_discordClient, message);
            return _commandService.ExecuteAsync(commandContext, argPos, _serviceScope.ServiceProvider);
        }

        private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // Command is unspecified when there was a search failure (command not found)
            if (!command.IsSpecified) return;
            if (result.IsSuccess) return;

            await context.Message.AddReactionAsync(new Emoji("❌"));

            if (result is not ExecuteResult executeResult)
            {
                var trace = context.Message.Id;
                await context.Message.ReplyAsync($"An error occurred while processing your command. Trace: {trace}.");
                _logger.LogError("Non-" + nameof(ExecuteResult) + " result when executing {CommandName}: {ErrorType} - {ErrorReason}. Trace: {Trace}",
                    $"{command.Value.Module}.{command.Value.Name}", result.Error, result.ErrorReason, trace);
                return;
            }

            if (executeResult.Exception is not VisibleCommandException visibleException)
            {
                var trace = context.Message.Id;
                await context.Message.ReplyAsync($"An error occurred while processing your command. Trace: {trace}.");
                _logger.LogError("Non-" + nameof(VisibleCommandException) + " result when executing {CommandName}: {ErrorType}\n{ErrorReason}\nTrace: {Trace}",
                    $"{command.Value.Module}.{command.Value.Name}", result.Error, executeResult.Exception, trace);
                return;
            }

            await context.Message.ReplyAsync(visibleException.Message);
        }

        public void Dispose()
        {
            _serviceScope.Dispose();
        }
    }
}
