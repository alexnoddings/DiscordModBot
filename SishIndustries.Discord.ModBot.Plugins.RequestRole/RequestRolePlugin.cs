using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SishIndustries.Discord.ModBot.Core;

namespace SishIndustries.Discord.ModBot.Plugins.RequestRole
{
    public class RequestRolePlugin : IModBotPlugin
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, string environment)
        {
            services.AddSingleton<RequestRolePlugin>();
            services.AddSingleton<RequestRoleModule>();

            services.AddScoped<RequestRoleService>();
        }

        private RequestRoleModule Module { get; }
        private RequestRoleService Service { get; }
        private DiscordSocketClient DiscordClient { get; }

        public RequestRolePlugin(RequestRoleModule module, RequestRoleService service, DiscordSocketClient discordClient)
        {
            Module = module;
            Service = service;
            DiscordClient = discordClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }
    }
}
