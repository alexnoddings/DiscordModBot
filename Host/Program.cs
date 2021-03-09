using Elvet.Core.Extensions;
using Elvet.Core.Plugins.Extensions;
using Elvet.Parrot;
using Elvet.RoleBack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hosting = Microsoft.Extensions.Hosting.Host;

namespace Elvet.Host
{
    /// <summary>
    /// Entry point for the Host.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Entry point for the program.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args) =>
            CreateHostBuilder(args)
                .Build()
                .Run();

        /// <summary>
        /// Creates the <see cref="IHostBuilder" />.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <returns>The <see cref="IHostBuilder" /> ready to build.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Hosting.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(ConfigureHost)
                .ConfigureServices(ConfigureServices);

        /// <summary>
        /// Configures the <paramref name="configuration" />.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfigurationBuilder" /> to configure.</param>
        private static void ConfigureHost(IConfigurationBuilder configuration)
        {
            configuration.AddEnvironmentVariables("ELVET_");
        }

        /// <summary>
        /// Configures services for the <see cref="IHost" />.
        /// </summary>
        /// <param name="host">The <see cref="HostBuilderContext" /> to use when configuring services.</param>
        /// <param name="services">The <see cref="IServiceCollection" /> to add to.</param>
        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddHostedService<BotService>();

            services
                .AddElvetCore()
                .AddPlugins(host.Configuration, pluginsBuilder =>
                    pluginsBuilder
                        .AddRoleBackPlugin()
                        .AddParrotPlugin()
                );
        }
    }
}
