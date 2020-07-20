using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Elvet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Elvet.Plugins.RoleBack
{
    public class RoleBackPlugin : IModBotPlugin
    {
        [SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "Standard for all plugins to use the same parameters.")]
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Standard for all plugins to use the same parameters.")]
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddSingleton<RoleBackPlugin>();
            services.AddSingleton<RoleBackModule>();

            services.AddScoped<RoleBackService>();
        }

        private IServiceScopeFactory ScopeFactory { get; }
        private RoleBackModule RoleBackModule { get; }
        private DiscordSocketClient DiscordClient { get; }

        public RoleBackPlugin(IServiceScopeFactory scopeFactory, RoleBackModule backModule, DiscordSocketClient discordClient)
        {
            ScopeFactory = scopeFactory;
            RoleBackModule = backModule;
            DiscordClient = discordClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            DiscordClient.UserJoined += OnUserJoinedAsync;
            DiscordClient.UserLeft += OnUserLeftAsync;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            DiscordClient.UserJoined -= OnUserJoinedAsync;
            DiscordClient.UserLeft -= OnUserLeftAsync;
        }

        private async Task OnUserJoinedAsync(SocketGuildUser user)
        {
            using var scope = ScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<RoleBackService>();
            await service.OnUserJoinedAsync(user).ConfigureAwait(false);
        }

        private async Task OnUserLeftAsync(SocketGuildUser user)
        {
            using var scope = ScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<RoleBackService>();
            await service.OnUserLeftAsync(user).ConfigureAwait(false);
        }
    }
}
