using Microsoft.Extensions.Hosting;
using Un1ver5e.Bot.Utilities;

Un1ver5e.Bot.BoardGames.Core.Dice.CacheCommonDice();

using (var host = BotHostBuilder.CreateHost())
{
    host.Run();
}
