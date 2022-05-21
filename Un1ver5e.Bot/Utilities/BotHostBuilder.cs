using Disqord.Bot.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Un1ver5e.Bot.BoardGames.Core;
using Serilog.Core;
using Microsoft.Extensions.Configuration;
using Un1ver5e.Bot.Database;

namespace Un1ver5e.Bot.Utilities
{
    public static class BotHostBuilder
    {
        public static IHost CreateHost(string[] args)
        {
            // All host configuration goes here.
            return new HostBuilder()
                .UseSerilog((context, services, logger) =>
                {
                    string filePath = $"{services.GetRequiredService<FolderPathService>()["Logs"]}/Log-.log";

                    logger
                    .MinimumLevel.ControlledBy(services.GetRequiredService<LoggingLevelSwitch>())
                    .WriteTo.Console()
                    .WriteTo.File(filePath, rollingInterval: RollingInterval.Day, shared: true);
                })
                .ConfigureHostConfiguration(config =>
                {
                    config.SetBasePath(Environment.CurrentDirectory);
                    config.AddJsonFile("hostconfig.json");
                    config.AddCommandLine(args);
                })
                .ConfigureServices((context, services) =>
                {
                    //TODO: Make all singleton services use config && name them properly.
                    services.AddSingleton<Random>();
                    services.AddSingleton<LoggingLevelSwitch>();
                    services.AddSingleton<DiceService>();
                    services.AddSingleton<FolderPathService>();
                    services.AddSingleton<DatabaseService>();
                })
                .ConfigureDiscordBot((context, bot) =>
                {
                    string splash = context.Configuration.GetSection("splashes").Get<string[]>().GetRandomElement();
                    string token = context.Configuration["discord_token"];
                    string[] prefixes = context.Configuration.GetSection("prefixes").Get<string[]>();

                    bot.Activities = new Disqord.Gateway.LocalActivity[] { new(splash, Disqord.ActivityType.Watching) };
                    bot.Token = token;
                    bot.Prefixes = prefixes;
                })
                .Build();
        }

    }
}
