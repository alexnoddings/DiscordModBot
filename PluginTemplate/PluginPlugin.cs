using System.Threading;
using System.Threading.Tasks;
using Elvet.Core.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace $safeprojectname$
{
    /// <summary>
    /// The $pluginname$ plugin.
    /// </summary>
    internal class $pluginname$Plugin : ElvetPluginBase<$pluginname$Service, $pluginname$Config>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="$pluginname$Plugin" />.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger" />.</param>
        /// <param name="serviceScopeFactory">The <see cref="IServiceScopeFactory" /> used to enabled running scoped operations.</param>
        /// <param name="config">The <see cref="$pluginname$Config" />.</param>
        public $pluginname$Plugin(ILogger<$pluginname$Plugin> logger, IServiceScopeFactory serviceScopeFactory, $pluginname$Config config)
            : base(logger, serviceScopeFactory, config)
        {
        }

        protected override Task OnStartAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Starting $pluginname$.");

            return Task.CompletedTask;
        }

        protected override Task OnStopAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Stopping $pluginname$.");

            return Task.CompletedTask;
        }
    }
}
