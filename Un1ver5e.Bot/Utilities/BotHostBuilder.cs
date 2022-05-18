using Disqord.Bot.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Un1ver5e.Bot.BoardGames.Core;

namespace Un1ver5e.Bot.Utilities
{
    internal static class BotHostBuilder
    {
        internal static IHost CreateHost()
        {
            // All host configuration goes here.
            return new HostBuilder()
                .UseSerilog((context, logger) =>
                {
                    logger.WriteTo.Logger(Logging.GetLogger());
                })
                .ConfigureDiscordBot((context, bot) =>
                {
                    bot.Activities = new Disqord.Gateway.LocalActivity[] { new(SplashReader.GetSplash(), Disqord.ActivityType.Watching) };
                    bot.Token = TokenReader.ReadToken();
                    bot.Prefixes = new[] { PrefixReader.ReadPrefix() };
                })
                .ConfigureServices((context, services) =>
                {
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
                })
                .Build();
        }
    }
}
