using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;

namespace Elvet.Core.Plugins
{
    /// <summary>
    /// A plugin for Elvet.
    /// </summary>
    public interface IElvetPlugin
    {
        /// <summary>
        /// Gets the <see cref="ModuleBase" /> <see cref="Type" />s for this plugin.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{Type}" /> of <see cref="ModuleBase" />s.</returns>
        public IEnumerable<Type> GetModules();

        /// <summary>
        /// Starts the plugin.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="Task" /> that represents the plugin starting.</returns>
        public Task StartAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Stops the plugin.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> used to cancel the operation.</param>
        /// <returns>A <see cref="Task" /> that represents the plugin stopping.</returns>
        public Task StopAsync(CancellationToken cancellationToken = default);
    }
}
