using System;
using System.Collections.Generic;
using Discord.Commands;
using Discord.WebSocket;
using Elvet.Core;
using Elvet.Data;
using Elvet.Plugins.RequestRole;
using Elvet.Plugins.RoleBack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Elvet
{
    /// <summary>
    /// Entry point for the program.
    /// </summary>
    public static class Program
    {
        private const string EnvironmentVariablesPrefix = "ELVET_";

        /// <summary>
        /// Entry point for the program.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configuration =>
                {
                    configuration.AddEnvironmentVariables(EnvironmentVariablesPrefix);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(logging =>
                    {
                        logging.AddEventLog();
                        logging.AddConsole();
                    });

                    string? dbConnectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
                    if (string.IsNullOrWhiteSpace(dbConnectionString))
                        throw new InvalidOperationException("No connection string provided, please add a configuration item for ConnectionStrings:DefaultConnection");
                    services.AddDbContext<BotDbContext>(options =>
                    {
                        options.UseSqlServer(dbConnectionString);
                        if (hostContext.HostingEnvironment.IsDevelopment())
                            options.EnableSensitiveDataLogging();
                    });

                    // ToDo: set CommandServiceConfig.DefaultRunMode

                    services.AddHostedService<ModBotHostService>();

                    services.AddSingleton<DiscordSocketClient>();
                    services.AddSingleton<ModBot>();
                    services.AddSingleton<CommandService>();
                    services.AddSingleton<CommandHandler>();

                    RequestRolePlugin.ConfigureServices(services, hostContext.Configuration, hostContext.HostingEnvironment);
                    RoleBackPlugin.ConfigureServices(services, hostContext.Configuration, hostContext.HostingEnvironment);

                    services.AddTransient<IEnumerable<IModBotPlugin>>(serviceProvider => new IModBotPlugin[]
                    {
                        serviceProvider.GetRequiredService<RequestRolePlugin>(),
                        serviceProvider.GetRequiredService<RoleBackPlugin>(),
                    });
                });
    }
}
