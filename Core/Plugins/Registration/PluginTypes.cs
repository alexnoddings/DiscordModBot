using System;
using System.Collections.Generic;

namespace Elvet.Core.Plugins.Registration
{
    /// <summary>
    /// Tracks the registered <see cref="IElvetPlugin" /> <see cref="Type" />s.
    /// </summary>
    internal class PluginTypes
    {
        private readonly HashSet<Type> _plugins = new();

        /// <summary>
        /// The registered <see cref="IElvetPlugin" /> <see cref="Type" />s
        /// </summary>
        public IEnumerable<Type> Types => _plugins;

        /// <summary>
        /// Registers a <typeparamref name="TPlugin" />.
        /// </summary>
        /// <typeparam name="TPlugin">The plugin to register.</typeparam>
        public void Register<TPlugin>() where TPlugin : IElvetPlugin =>
            _plugins.Add(typeof(TPlugin));
    }
}
