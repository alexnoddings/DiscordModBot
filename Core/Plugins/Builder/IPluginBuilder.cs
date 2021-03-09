namespace Elvet.Core.Plugins.Builder
{
    /// <summary>
    /// A builder abstraction for configuring a <see cref="IElvetPlugin"/>.
    /// </summary>
    public interface IPluginBuilder
    {
        /// <summary>
        /// A <see cref="PluginDescriptor"/> for the <see cref="IElvetPlugin"/> being built.
        /// </summary>
        public PluginDescriptor PluginDescriptor { get; }

        /// <summary>
        /// The parent <see cref="IElvetPluginsBuilder"/> which is building this <see cref="IElvetPlugin"/>.
        /// </summary>
        public IElvetPluginsBuilder PluginsBuilder { get; }
    }
}
