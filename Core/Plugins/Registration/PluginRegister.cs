using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Elvet.Core.Plugins.Registration
{
    /// <inheritdoc cref="IPluginRegister" />
    internal class PluginRegister : IPluginRegister
    {
        private readonly List<IElvetPlugin> _plugins;

        public IEnumerable<IElvetPlugin> Plugins => _plugins;

        /// <summary>
        /// Initialises a new instance of the <see cref="PluginRegister" />.
        /// </summary>
        /// <param name="pluginTypesOptions">An <see cref="IOptions{TOptions}" /> for <see cref="PluginTypes" />.</param>
        /// <param name="services">The <see cref="IServiceProvider" /> to get <see cref="IElvetPlugin" />s from.</param>
        public PluginRegister(IOptions<PluginTypes> pluginTypesOptions, IServiceProvider services)
        {
            _plugins =
                pluginTypesOptions
                    .Value
                    // Get the registered Types
                    .Types
                    // Get the types from the IServiceProvider
                    .Select(services.GetRequiredService)
                    // Cast them from object to IElvetPlugin
                    .Cast<IElvetPlugin>()
                    // Convert to a list
                    .ToList();
        }
    }
}
