using System.Threading;
using System.Threading.Tasks;

namespace SishIndustries.Discord.ModBot.Plugins
{
    public interface IModBotPlugin
    {
        public Task StartAsync(CancellationToken cancellationToken);
        public Task StopAsync(CancellationToken cancellationToken);
    }
}
