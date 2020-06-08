using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace SishIndustries.Discord.ModBot.Plugins.ReRole
{
    public class ReRolePlugin : IModBotPlugin
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ReRoleService>();
            services.AddSingleton<ReRoleModule>();
        }

        private ReRoleModule Module { get; }
        private ReRoleService Service { get; }
        private DiscordSocketClient DiscordClient { get; }

        public ReRolePlugin(ReRoleModule module, ReRoleService service, DiscordSocketClient discordClient)
        {
            Module = module;
            Service = service;
            DiscordClient = discordClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            DiscordClient.UserLeft += Service.OnUserLeftAsync;
            DiscordClient.UserJoined += Service.OnUserJoinedAsync;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            DiscordClient.UserLeft -= Service.OnUserLeftAsync;
            DiscordClient.UserJoined -= Service.OnUserJoinedAsync;
        }
    }
}
