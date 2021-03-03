using System.Collections.Generic;

namespace Elvet.Core.Plugins.Registration
{
    /// <summary>
    /// A register of <see cref="IElvetPlugin" />s in use.
    /// </summary>
    public interface IPluginRegister
    {
        /// <summary>
        /// The <see cref="IElvetPlugin" />s registered.
        /// </summary>
        public IEnumerable<IElvetPlugin> Plugins { get; }
    }
}
