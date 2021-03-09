using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elvet.Core.Plugins.Builder
{
    /// <summary>
    /// A builder abstraction for adding <see cref="IElvetPlugin"/>s.
    /// </summary>
    public interface IElvetPluginsBuilder
    {
        /// <summary>
        /// The <see cref="IServiceCollection"/> being built by the host.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// The <see cref="IConfiguration"/> loaded by the host.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// The <see cref="IPluginCollection"/> to add plugins to.
        /// </summary>
        public IPluginCollection Plugins { get; }
    }
}
