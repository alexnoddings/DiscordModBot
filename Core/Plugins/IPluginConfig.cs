namespace Elvet.Core.Plugins
{
    /// <summary>
    /// Base configuration for an <see cref="IElvetPlugin"/>.
    /// </summary>
    public interface IPluginConfig
    {
        /// <summary>
        /// Whether or not the plugin is enabled.
        /// </summary>
        public bool Enabled { get; set; }
    }
}
