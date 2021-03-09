using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elvet.Core.Plugins.Builder.Internal
{
    internal class PluginsBuilder : IElvetPluginsBuilder
    {
        public IServiceCollection Services { get; }
        public IConfiguration Configuration { get; }
        public IPluginCollection Plugins { get; } = new PluginCollection();

        public PluginsBuilder(IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            Services = services;
            Configuration = configuration;
        }
    }
}
