using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SishIndustries.Discord.ModBot.Hosting
{
    public class ModBotHostService : IHostedService
    {
        private ILogger<ModBotHostService> Logger { get; }
        private Core.ModBot ModBot { get; }

        public ModBotHostService(ILogger<ModBotHostService> logger, Core.ModBot modBot)
        {
            Logger = logger;
            ModBot = modBot;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Starting bot");
            await ModBot.StartAsync(cancellationToken);
            Logger.LogInformation("Bot started");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Stopping bot");
            await ModBot.StopAsync(cancellationToken);
            Logger.LogInformation("Bot stopped");
        }
    }
}
