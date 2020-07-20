using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Elvet.Core
{
    public class ModBotHostService : IHostedService
    {
        private ILogger<ModBotHostService> Logger { get; }
        private ModBot ModBot { get; }

        public ModBotHostService(ILogger<ModBotHostService> logger, ModBot modBot)
        {
            Logger = logger;
            ModBot = modBot;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Starting bot");
            await ModBot.StartAsync(cancellationToken).ConfigureAwait(false);
            Logger.LogInformation("Bot started");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Stopping bot");
            await ModBot.StopAsync(cancellationToken).ConfigureAwait(false);
            Logger.LogInformation("Bot stopped");
        }
    }
}
