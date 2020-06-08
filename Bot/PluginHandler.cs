using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SishIndustries.Discord.ModBot.Core;
using SishIndustries.Discord.ModBot.Plugins.ReRole;

namespace SishIndustries.Discord.ModBot
{
    public static class PluginHandler
    {
        public static IServiceCollection AddPlugins(this IServiceCollection services)
        {
            ReRolePlugin.ConfigureServices(services);

            return services;
        }

        internal static IList<IModBotPlugin> GetPlugins(this IServiceProvider services)
        {
            return new List<IModBotPlugin>
            {
                services.GetRequiredService<ReRolePlugin>()
            };
        }
    }
}
