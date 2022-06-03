using Disqord.Bot.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Un1ver5e.Bot.Services;
using Un1ver5e.Bot.Services.Dice;
using Un1ver5e.Bot.Services.Graphics;
using Microsoft.EntityFrameworkCore;
using Un1ver5e.Bot.Services.Database;
using Disqord.Gateway;

namespace Un1ver5e.Bot.Utilities
{
    public static class BotHostBuilder
    {
        public static IHost CreateHost(string[] args)
        {
            // All host configuration goes here.
            return new HostBuilder()
                .UseDefaultServiceProvider(config =>
                {
                    config.ValidateOnBuild = true;
                    config.ValidateScopes = true;
                })
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
                    config.AddJsonFile("Data/hostconfig.json");
                    config.AddCommandLine(args);
                })
                .ConfigureServices((context, services) =>
                {
                    services
                    .AddSingleton<Random>()
                    .AddSingleton<LoggingLevelSwitch>()

                    .AddSingleton<DefaultDiceService>()
                    .AddScoped<IDiceService, DefaultDiceService>()

                    .AddSingleton<ImageSharpGraphics>()
                    .AddScoped<IGraphics, ImageSharpGraphics>()

                    .AddSingleton<FolderPathService>()

                    .AddDbContext<ApplicationContext>(options =>
                    {
                        options.UseSqlite(context.Configuration["sqlite_connection"]);
                    });
                })
                .ConfigureDiscordBot((context, bot) =>
                {
                    IConfigurationSection config =
#if DEBUG
                    context.Configuration.GetSection("discord_config_debug");
#else
                    context.Configuration.GetSection("discord_config_release");
#endif

                    string splash = context.Configuration.GetSection("splashes").Get<string[]>().GetRandomElement();
                    string token = config["token"];
                    string[] prefixes = config.GetSection("prefixes").Get<string[]>();

                    bot.Activities = new LocalActivity[] { new(splash, Disqord.ActivityType.Watching) };
                    bot.Token = token;
                    bot.Prefixes = prefixes;
                    bot.Intents |= GatewayIntent.DirectMessages | GatewayIntent.DirectReactions;
                })
                .Build();
        }

    }
}
