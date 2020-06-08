using System;
using System.IO;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SishIndustries.Discord.ModBot.Hosting
{
    public class Program
    {
        private const string EnvironmentVariablesPrefix = "SI_BOT_";
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configuration =>
                {
                    configuration.SetBasePath(Directory.GetCurrentDirectory());
                    configuration.AddJsonFile("appsettings.json", optional: true);

                    var environment = 
                        Environment.GetEnvironmentVariable($"{EnvironmentVariablesPrefix}ENVIRONMENT")?.ToLowerInvariant();
                    if (!string.IsNullOrWhiteSpace(environment))
                        configuration.AddJsonFile($"appsettings.{environment}.json", optional: true);

                    configuration.AddEnvironmentVariables(EnvironmentVariablesPrefix);
                    configuration.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(logging =>
                    {
                        logging.AddEventLog();
                        logging.AddConsole();
                    });

                    services.AddHostedService<ModBotHostService>();

                    services.AddSingleton<DiscordSocketClient>();
                    services.AddSingleton<ModBot>();

                    services.AddPlugins(hostContext.Configuration, hostContext.HostingEnvironment.EnvironmentName);
                });
    }
}
