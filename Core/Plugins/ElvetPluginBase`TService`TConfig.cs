using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elvet.Core.Plugins.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Elvet.Core.Plugins
{
    /// <summary>
    /// Base <see cref="IElvetPlugin" /> implementation with an associated <typeparamref name="TService" />.
    /// </summary>
    /// <typeparam name="TService">The service to use by default when running a scoped operation.</typeparam>
    /// <typeparam name="TConfig">The config to use for this plugin.</typeparam>
    /// <inheritdoc />
    public abstract class ElvetPluginBase<TService, TConfig> : ElvetPluginBase
        where TService : class
        where TConfig : class, IPluginConfig
    {
        protected TConfig Config { get; }

        private bool _isRunning = false;
        protected override bool IsRunning => _isRunning;

        /// <summary>
        /// Initialises a new instance of the <see cref="ElvetPluginBase{TService, TConfig}" />.
        /// </summary>
        /// <inheritdoc />
        protected ElvetPluginBase(ILogger<ElvetPluginBase<TService, TConfig>> logger, IServiceScopeFactory serviceScopeFactory, TConfig config)
            : base(logger, serviceScopeFactory)
        {
            Config = config;
        }

        /// <summary>
        /// Called by <see cref="StartAsync"/> if <see cref="IPluginConfig.Enabled"/> is <see langword="true"/> for the <typeparamref name="TConfig"/>, and <see cref="IsRunning"/> is <see langword="false"/>.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="Task" /> that represents the plugin starting.</returns>
        protected virtual Task OnStartAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public sealed override async Task StartAsync(CancellationToken cancellationToken = default)
        {
            if (_isRunning)
                throw new InvalidOperationException("Can't start plugin as it is already running.");

            if (!Config.Enabled)
            {
                Logger.LogInformation("Plugin not enabled; skipping start.");
                return;
            }

            await OnStartAsync(cancellationToken);

            _isRunning = true;
        }

        /// <summary>
        /// Called by <see cref="StopAsync"/> if <see cref="IsRunning"/> is <see langword="true"/>.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="Task" /> that represents the plugin stopping.</returns>
        protected virtual Task OnStopAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public sealed override async Task StopAsync(CancellationToken cancellationToken = default)
        {
            if (!_isRunning)
            {
                Logger.LogInformation("Plugin not running; skipping stop.");
                return;
            }

            await OnStopAsync(cancellationToken);

            _isRunning = false;
        }

        /// <summary>
        /// Runs an operation on a scoped <typeparamref name="TService" />.
        /// </summary>
        /// <inheritdoc cref="ElvetPluginBase.RunOnScopedServiceAsync{TService}" />
        protected Task RunOnScopedServiceAsync(Expression<Func<TService, Task>> expression) =>
            RunOnScopedServiceAsync<TService>(expression);
    }
}
