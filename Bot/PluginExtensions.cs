using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SishIndustries.Discord.ModBot.Core;
using SishIndustries.Discord.ModBot.Plugins.RequestRole;
using SishIndustries.Discord.ModBot.Plugins.ReRole;

namespace SishIndustries.Discord.ModBot
{
    public static class PluginExtensions
    {
        public static IServiceCollection AddPlugins(this IServiceCollection services, IConfiguration configuration, string environment)
        {
            RequestRolePlugin.ConfigureServices(services, configuration, environment);
            ReRolePlugin.ConfigureServices(services, configuration, environment);

            return services;
        }

        internal static IList<IModBotPlugin> GetPlugins(this IServiceProvider services)
        {
            return new List<IModBotPlugin>
            {
                services.GetRequiredService<RequestRolePlugin>(),
                services.GetRequiredService<ReRolePlugin>()
            };
        }
    }
}
