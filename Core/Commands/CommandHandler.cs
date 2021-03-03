using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Elvet.Core.Plugins;
using Elvet.Core.Plugins.Registration;

namespace Elvet.Core.Commands
{
    /// <inheritdoc cref="ICommandHandler" />
    internal class CommandHandler : ICommandHandler
    {
        private readonly CommandService _commandService;
        private readonly DiscordSocketClient _discordClient;
        private readonly IPluginRegister _pluginRegister;
        private readonly IServiceProvider _services;

        /// <summary>
        /// Initialises a new instance of the <see cref="CommandHandler" />.
        /// </summary>
        /// <param name="commandService">The <see cref="CommandService" /> to add modules to and execute commands on.</param>
        /// <param name="discordClient">The <see cref="DiscordSocketClient" /> to monitor command invocation.</param>
        /// <param name="pluginRegister">The <see cref="IPluginRegister" /> to get <see cref="IElvetPlugin" />s from.</param>
        /// <param name="services">The <see cref="IServiceProvider" /> to use for adding modules and executing commands.</param>
        public CommandHandler(CommandService commandService, DiscordSocketClient discordClient, IPluginRegister pluginRegister, IServiceProvider services)
        {
            _commandService = commandService;
            _discordClient = discordClient;
            _pluginRegister = pluginRegister;
            _services = services;
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            var modules = _pluginRegister.Plugins.SelectMany(plugin => plugin.GetModules());
            await Task.WhenAll(modules.Select(moduleType => _commandService.AddModuleAsync(moduleType, _services)));

            _discordClient.MessageReceived += MessageReceivedAsync;
            _commandService.CommandExecuted += CommandServiceExecutedAsync;
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            _discordClient.MessageReceived -= MessageReceivedAsync;
            _commandService.CommandExecuted -= CommandServiceExecutedAsync;

            var modules = _commandService.Modules.ToList();
            await Task.WhenAll(modules.Select(module => _commandService.RemoveModuleAsync(module)));
        }

        private Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore messages not from users
            if (rawMessage is not SocketUserMessage message) return Task.CompletedTask;
            if (message.Source != MessageSource.User) return Task.CompletedTask;

            var argPos = 0;
            if (!message.HasStringPrefix(",,", ref argPos, StringComparison.OrdinalIgnoreCase))
                return Task.CompletedTask;

            var commandContext = new SocketCommandContext(_discordClient, message);
            return _commandService.ExecuteAsync(commandContext, argPos, _services);
        }

        private static async Task CommandServiceExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // Command is unspecified when there was a search failure (command not found)
            if (!command.IsSpecified) return;
            if (result.IsSuccess) return;

            await context.Message.AddReactionAsync(new Emoji("❌"));
            await context.Message.ReplyAsync($"Error: {result}");
        }
    }
}
