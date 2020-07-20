using System.Threading;
using System.Threading.Tasks;

namespace Elvet.Core
{
    public interface IModBot
    {
        public Task StartAsync(CancellationToken cancellationToken);
        public Task StopAsync(CancellationToken cancellationToken);
    }
}
