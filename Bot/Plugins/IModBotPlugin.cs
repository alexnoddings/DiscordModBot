using System.Threading;
using System.Threading.Tasks;

namespace SishIndustries.Discord.ModBot.Core.Plugins
{
    public interface IModBotPlugin
    {
        public Task StartAsync(CancellationToken cancellationToken);
        public Task StopAsync(CancellationToken cancellationToken);
    }
}
