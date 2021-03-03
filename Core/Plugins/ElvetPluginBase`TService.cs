using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Elvet.Core.Plugins
{
    /// <summary>
    /// Base <see cref="IElvetPlugin" /> implementation with an associated <typeparamref name="TService" />.
    /// </summary>
    /// <typeparam name="TService">The service to use by default when running a scoped operation.</typeparam>
    /// <inheritdoc />
    public abstract class ElvetPluginBase<TService> : ElvetPluginBase where TService : class
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ElvetPluginBase{TService}" />.
        /// </summary>
        /// <inheritdoc />
        protected ElvetPluginBase(ILogger<ElvetPluginBase<TService>> logger, IServiceScopeFactory serviceScopeFactory)
            : base(logger, serviceScopeFactory)
        {
        }

        /// <summary>
        /// Runs an operation on a scoped <typeparamref name="TService" />.
        /// </summary>
        /// <inheritdoc cref="ElvetPluginBase.RunOnScopedServiceAsync{TService}" />
        protected Task RunOnScopedServiceAsync(Expression<Func<TService, Task>> expression) =>
            RunOnScopedServiceAsync<TService>(expression);
    }
}
