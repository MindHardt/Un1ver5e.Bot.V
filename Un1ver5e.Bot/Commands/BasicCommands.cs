using Disqord;
using Disqord.Bot;
using Qmmands;
using Un1ver5e.Bot.Utilities;

namespace Un1ver5e.Commands
{
    public class BasicCommandsModule : DiscordModuleBase
    {
        [Command("splash"), Description("Сплеш из майнкрафта!")]
        public DiscordCommandResult Splash()
        {
            return Reply(SplashReader.GetSplash());
        }

        [Group("generate", "gen", "g"), Description("Нейронки делают фигню!")]
        public class GenerateCommands : DiscordModuleBase
        {
            [Command("cat"), Description("Котики!")]
            public DiscordCommandResult Cat()
            {
                return Reply(GetGeneratedPicture("https://thiscatdoesnotexist.com/"));
            }
            [Command("horse"), Description("Лошади!")]
            public DiscordCommandResult Horse()
            {
                return Reply(GetGeneratedPicture("https://thishorsedoesnotexist.com/"));
            }
            [Command("art"), Description("Искусство!(?)")]
            public DiscordCommandResult Art()
            {
                return Reply(GetGeneratedPicture("https://thisartworkdoesnotexist.com/"));
            }

            private static LocalMessage GetGeneratedPicture(string url)
            {
                using HttpClient client = new();
                Stream pic = client.GetStreamAsync(url).GetAwaiter().GetResult();

                return new LocalMessage()
                {
                    Attachments = new List<LocalAttachment>() { new LocalAttachment(pic, "generated.jpg") },
                    Content = $"Источник: ||{url}||"
                };
            }
        }

        [Group("logs")]
        public class LogCommands : DiscordModuleBase
        {
            [Command("get"), Description("Логи")]
            public DiscordCommandResult Get()
            {
                Stream logs = new FileStream($"{Logging.LogsFolderPath}/latest.log", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);

                LocalMessage msg =  new LocalMessage()
                {
                    Attachments = new List<LocalAttachment>() { new LocalAttachment(logs, "latest.log") },
                };

                return Reply(msg);
            }
        }
    }
}