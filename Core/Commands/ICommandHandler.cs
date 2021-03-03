using System.Threading;
using System.Threading.Tasks;

namespace Elvet.Core.Commands
{
    /// <summary>
    /// Handles executing commands.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Starts the <see cref="ICommandHandler" />.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="Task" /> that represents the handler starting.</returns>
        public Task StartAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Stops the <see cref="ICommandHandler" />.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="Task" /> that represents the handler Stopping.</returns>
        public Task StopAsync(CancellationToken cancellationToken = default);
    }
}
