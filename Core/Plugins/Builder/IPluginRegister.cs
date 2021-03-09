using System.Collections.Generic;

namespace Elvet.Core.Plugins.Builder
{
    /// <summary>
    /// A register of <see cref="IElvetPlugin"/>s.
    /// </summary>
    public interface IPluginRegister
    {
        /// <summary>
        /// An enumerable of <see cref="IPluginInstanceInfo"/>s for registered <see cref="IElvetPlugin"/>s.
        /// </summary>
        public IEnumerable<IPluginInstanceInfo> PluginsInstanceInfos{ get; }
    }
}
