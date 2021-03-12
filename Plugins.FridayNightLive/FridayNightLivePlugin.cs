using System.Threading;
using System.Threading.Tasks;
using Elvet.Core.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Elvet.FridayNightLive
{
    /// <summary>
    /// The Friday Night Live plugin.
    /// </summary>
    internal class FridayNightLivePlugin : ElvetPluginBase<FridayNightLiveService, FridayNightLiveConfig>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="FridayNightLivePlugin" />.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger" />.</param>
        /// <param name="serviceScopeFactory">The <see cref="IServiceScopeFactory" /> used to enabled running scoped operations.</param>
        /// <param name="config">The <see cref="FridayNightLiveConfig" />.</param>
        public FridayNightLivePlugin(ILogger<FridayNightLivePlugin> logger, IServiceScopeFactory serviceScopeFactory, FridayNightLiveConfig config)
            : base(logger, serviceScopeFactory, config)
        {
        }

        protected override Task OnStartAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Starting FridayNightLive.");

            return Task.CompletedTask;
        }

        protected override Task OnStopAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Stopping FridayNightLive.");

            return Task.CompletedTask;
        }
    }
}
