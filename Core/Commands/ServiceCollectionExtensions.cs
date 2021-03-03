using Microsoft.Extensions.DependencyInjection;

namespace Elvet.Core.Commands
{
    /// <summary>
    /// Extension methods for adding a <see cref="ICommandHandler" /> implementation to an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an <see cref="ICommandHandler" /> implementation to the <paramref name="services" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add to.</param>
        /// <returns>The <paramref name="services" /> with the <see cref="ICommandHandler" /> added to enable chaining.</returns>
        public static IServiceCollection AddCommandHandler(this IServiceCollection services) =>
            services.AddSingleton<ICommandHandler, CommandHandler>();
    }
}
