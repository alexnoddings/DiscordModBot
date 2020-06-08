using System;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SishIndustries.Discord.ModBot.Core;
using SishIndustries.Discord.ModBot.Plugins.ReRole.Data;

namespace SishIndustries.Discord.ModBot.Plugins.ReRole
{
    public class ReRolePlugin : IModBotPlugin
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, string environment)
        {
            services.AddSingleton<ReRolePlugin>();
            services.AddSingleton<ReRoleModule>();

            services.AddScoped<ReRoleService>();

            string? dbConnectionString = configuration.GetConnectionString("ReRole") ?? configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(dbConnectionString))
                throw new InvalidOperationException("No connection string");

            services.AddDbContext<ReRoleDbContext>(
                options => options
                .UseSqlServer(dbConnectionString)
                .UseLazyLoadingProxies()
            );
        }

        private IServiceScopeFactory ScopeFactory { get; }
        private ReRoleModule Module { get; }
        private DiscordSocketClient DiscordClient { get; }

        public ReRolePlugin(IServiceScopeFactory scopeFactory, ReRoleModule module, DiscordSocketClient discordClient)
        {
            ScopeFactory = scopeFactory;
            Module = module;
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
            var service = scope.ServiceProvider.GetRequiredService<ReRoleService>();
            await service.OnUserJoinedAsync(user);
        }

        private async Task OnUserLeftAsync(SocketGuildUser user)
        {
            using var scope = ScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ReRoleService>();
            await service.OnUserLeftAsync(user);
        }
    }
}
