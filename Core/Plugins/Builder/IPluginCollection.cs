using System.Collections.Generic;

namespace Elvet.Core.Plugins.Builder
{
    /// <summary>
    /// Specifies the contract for a collection of <see cref="PluginDescriptor"/>s.
    /// </summary>
    public interface IPluginCollection : IList<PluginDescriptor>
    {
    }
}
