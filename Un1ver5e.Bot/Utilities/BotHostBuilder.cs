﻿using Disqord.Bot.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Un1ver5e.Bot.BoardGames.Core;
using Serilog.Core;
using Microsoft.Extensions.Configuration;

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
                    string filePath = $"{services.GetRequiredService<FolderPathProvider>()["Logs"]}/Log-.log";

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
                    services.AddSingleton(new LoggingLevelSwitch());
                    services.AddSingleton(Random.Shared);
                    services.AddSingleton(new DiceThrower()
                    {
                        AlwaysCacheDice = true,
                        Randomizer = Random.Shared,
                        CacheBase = new string[]
                        {
                            "1d2", "1d3", "1d4", "1d6", "1d8", "1d10", "1d12", "1d20", "1d100", "2d6"
                        }
                    });
                    services.AddSingleton(new FolderPathProvider()
                    {
                        Paths = new string[]
                        {
                            "Logs", "Data", "Data/Gallery"
                        }
                    });
                })
                .ConfigureDiscordBot((context, bot) =>
                {
                    string splash = context.Configuration.GetSection("splashes").Get<string[]>().GetRandomElement();
                    string token = context.Configuration["token"];
                    string[] prefixes = context.Configuration.GetSection("prefixes").Get<string[]>();

                    bot.Activities = new Disqord.Gateway.LocalActivity[] { new(splash, Disqord.ActivityType.Watching) };
                    bot.Token = token;
                    bot.Prefixes = prefixes;
                })
                .Build();
        }

    }
}
