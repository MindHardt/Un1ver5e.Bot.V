using Microsoft.Extensions.Hosting;

using (var host = Un1ver5e.Bot.Utilities.BotHostBuilder.CreateHost(args))
{
    host.Run();
}
