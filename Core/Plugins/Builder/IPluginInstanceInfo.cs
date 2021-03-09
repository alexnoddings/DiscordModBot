namespace Elvet.Core.Plugins.Builder
{
    /// <summary>
    /// Information describing a <see cref="IElvetPlugin"/>, alongside the instance of it.
    /// </summary>
    public interface IPluginInstanceInfo : IPluginInfo
    {
        /// <summary>
        /// The instance of the plugin of type <see cref="IPluginInfo.PluginType"/>.
        /// </summary>
        public IElvetPlugin Instance { get; }
    }
}
