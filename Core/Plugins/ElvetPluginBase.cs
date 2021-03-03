using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Elvet.Core.Plugins
{
    /// <summary>
    /// Base <see cref="IElvetPlugin" /> implementation.
    /// </summary>
    /// <remarks>
    /// Primarily helps to run events from the singleton <see cref="IElvetPlugin" /> on a scoped service with <see cref="RunOnScopedServiceAsync{TService}" />.
    /// </remarks>
    public abstract class ElvetPluginBase : IElvetPlugin
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        protected ILogger<ElvetPluginBase> Logger { get; }
        protected abstract bool IsRunning { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ElvetPluginBase" />.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger" />.</param>
        /// <param name="serviceScopeFactory">The <see cref="IServiceScopeFactory" /> used to enabled running scoped operations.</param>
        protected ElvetPluginBase(ILogger<ElvetPluginBase> logger, IServiceScopeFactory serviceScopeFactory)
        {
            Logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public abstract IEnumerable<Type> GetModules();

        public abstract Task StartAsync(CancellationToken cancellationToken = default);

        public abstract Task StopAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Runs an operation on a scoped <typeparamref name="TService" />.
        /// </summary>
        /// <typeparam name="TService">The service to run on.</typeparam>
        /// <param name="expression">The expression to run on the <see cref="TService" />.</param>
        /// <returns>A <see cref="Task" /> that represents the <paramref name="expression" />'s execution.</returns>
            where TService : class
        {
            var methodIdentifier = GetMethodIdentifier(expression);

            if (!IsRunning)
                throw new InvalidOperationException($"Cannot run {methodIdentifier} when the plugin is not running.");

            var function = expression.Compile();
            using var scope = _serviceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<TService>();

            Logger.LogDebug("Invoking {InvocationTarget} on scoped service.", methodIdentifier);

            try
            {
                await function.Invoke(service);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error invoking {InvocationTarget}.", methodIdentifier);
            }
        }

        /// <summary>
        /// Identifies which method is being invoked by an <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TService">The service which is being ran on.</typeparam>
        /// <param name="expression">The expression being ran on the <see cref="TService" />.</param>
        /// <returns>A string representing the method being invoked by the <paramref name="expression" />.</returns>
        private static string GetMethodIdentifier<TService>(Expression<Func<TService, Task>> expression)
        {
            string? methodIdentifier;
            if (expression.Body is MethodCallExpression methodCall)
            {
                var reflectedType = methodCall.Method.ReflectedType?.Name ?? string.Empty;
                var method = methodCall.Method.Name ?? string.Empty;
                var parameters =
                    methodCall.Method?.GetParameters().Select(p => $"{p.ParameterType} {p.Name}") ??
                    Enumerable.Empty<string>();
                methodIdentifier = $"{reflectedType}.{method}({string.Join(", ", parameters)})";
            }
            else
            {
                methodIdentifier = $"Unknown method (not a {nameof(MethodCallExpression)})";
            }

            return methodIdentifier;
        }
    }
}
