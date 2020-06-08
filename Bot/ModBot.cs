using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SishIndustries.Discord.ModBot.Plugins;
using SishIndustries.Discord.ModBot.Plugins.ReRole;

namespace SishIndustries.Discord.ModBot
{
    public class ModBot
    {
        private IConfiguration Configuration { get; set; }
        private DiscordSocketClient DiscordClient { get; set; }
        private IServiceProvider Services { get; set; }

        private IList<IModBotPlugin> Plugins { get; }

        public ModBot(IConfiguration configuration, DiscordSocketClient discordClient, IServiceProvider services)
        {
            Configuration = configuration;
            DiscordClient = discordClient;
            Services = services;

            Plugins = new List<IModBotPlugin>
            {
                Services.GetRequiredService<ReRolePlugin>()
            };
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var loginToken = Configuration["Bot:Token"];
            if (string.IsNullOrWhiteSpace(loginToken))
                throw new InvalidOperationException("no token");

            await DiscordClient.LoginAsync(TokenType.Bot, loginToken);
            await DiscordClient.StartAsync();

            await Task.WhenAll(Plugins.Select(p => p.StartAsync(cancellationToken)));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.WhenAll(Plugins.Select(p => p.StopAsync(cancellationToken)));

            await DiscordClient.StopAsync();
        }
    }
}
