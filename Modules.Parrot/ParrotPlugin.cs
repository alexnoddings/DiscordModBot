using System;
using System.Threading;
using System.Threading.Tasks;
using Elvet.Core.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Elvet.Parrot
{
    /// <summary>
    /// The Parrot plugin.
    /// </summary>
    internal class ParrotPlugin : ElvetPluginBase<ParrotService, ParrotConfig>
    {
        private CancellationTokenSource _runMessagesCancellationTokenSource = new();
        private Task _runMessagesTask = Task.CompletedTask;

        /// <summary>
        /// Initialises a new instance of the <see cref="ParrotPlugin" />.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger" />.</param>
        /// <param name="serviceScopeFactory">The <see cref="IServiceScopeFactory" /> used to enabled running scoped operations.</param>
        /// <param name="config">The <see cref="ParrotConfig" />.</param>
        public ParrotPlugin(ILogger<ParrotPlugin> logger, IServiceScopeFactory serviceScopeFactory, ParrotConfig config)
            : base(logger, serviceScopeFactory, config)
        {
        }

        protected override async Task OnStartAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Starting Parrot.");

            _runMessagesCancellationTokenSource.Cancel(false);
            try
            {
                await _runMessagesTask;
            }
            catch (TaskCanceledException)
            {
            }

            _runMessagesCancellationTokenSource = new CancellationTokenSource();
            _runMessagesTask = Task.Run(() => RunMessageCheck(_runMessagesCancellationTokenSource.Token), cancellationToken);
        }

        protected override async Task OnStopAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Stopping Parrot.");

            _runMessagesCancellationTokenSource.Cancel(false);
            try
            {
                await _runMessagesTask;
            }
            catch (TaskCanceledException)
            {
            }
        }

        private async Task RunMessageCheck(CancellationToken cancellationToken)
        {
            var sleepTime = TimeSpan.FromSeconds(30);

            // Avoid running immediately at startup
            await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                await RunOnScopedServiceAsync(service => service.SendOutstandingMessages(cancellationToken));
                await Task.Delay(sleepTime, cancellationToken);
            }
        }
    }
}
