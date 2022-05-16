using Disqord.Bot.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

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
                .Build();
        }
    }
}
