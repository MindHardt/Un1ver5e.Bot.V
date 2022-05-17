using Disqord;
using Disqord.Bot;
using Qmmands;
using Un1ver5e.Bot.Utilities;
using Un1ver5e.Commands.Attributes;

namespace Un1ver5e.Commands
{
    [Name("Базовые команды")]
    public class BasicCommands : DiscordModuleBase
    {
        public static Random Random { private get; set; } = Random.Shared;

        [Command("splash"), Description("Сплеш из майнкрафта!")]
        public DiscordCommandResult SplashCommand()
        {
            return Reply(SplashReader.GetSplash());
        }

        [Command("roll"), Description("Случайное число от 1 до 100.")]
        public DiscordCommandResult RollCommand()
        {
            return RandomCommand(1, 100);
        }

        [Command("random", "rnd"), Description("Случайное число между двумя данными.")]
        public DiscordCommandResult RandomCommand(int lowerBound, int upperBound)
        {
            LocalEmbed embed = new()
            {
                Fields = new List<LocalEmbedField>()
                {
                    new LocalEmbedField()
                    {
                        Name = $"Результат вашего броска [{lowerBound}..{upperBound}]:",
                        Value = $"🎲 **{Random.Next(lowerBound, upperBound + 1)}**"
                    }
                }
            };

            return Reply(embed);
        }

        [RequireReferencedMessage]
        [Command("rate"), Description("Бот оценивает :sunglasses:")]
        public DiscordCommandResult RateCommand()
        {
            IUserMessage message = Context.Message.ReferencedMessage.Value;

            string[] rateOptions =
            {
                ":thumbsup: Крутяк",
                ":smile: Нормально-нормально",
                ":slight_smile: Покатит",
                ":confused: Ну такое",
                ":thumbsdown: Хрень",
                ":fire: Огонь!",
                ":japanese_ogre: Ы",
                ":scream: Абалдеть!!!",
                ":rage: Кринж",
                ":banana: ок",
                ":zero: 0/10",
                ":one: 1/10",
                ":two: 2/10",
                ":three: 3/10",
                ":four: 4/10",
                ":five: 5/10",
                ":six: 6/10",
                ":seven: 7/10",
                ":eight: 8/10",
                ":nine: 9/10",
                ":ten: 10/10",
                ":knife: РЕЗНЯ",
                $"<:german:971147037124984902> Нет.",
                ":radioactive: Бомба!",
                ":exclamation: !!!"
            };

            string rateMessage = rateOptions.GetRandomElement(new Random((int)message.Id.RawValue));

            LocalEmbed embed = new()
            {
                Footer = new LocalEmbedFooter()
                {
                    Text = "Все оценки бота случайны."
                },
                Fields = new List<LocalEmbedField>()
                {
                    new LocalEmbedField()
                    {
                        Name = "Оценка от бота:",
                        Value = rateMessage
                    }
                }
            };

            LocalMessage respond = new()
            {
                Embeds = new List<LocalEmbed>()
                {
                    embed
                },
            };

            return Reply(respond.WithReply(message.Id));
        }

        [Command("stealemoji", "steal"), Description("Ваш эмоджи теперь мой.")]
        public DiscordCommandResult StealEmojiCommand(IGuildEmoji emoji)
        {
            string url = emoji.GetUrl();

            LocalEmbed embed = new()
            {
                ImageUrl = url,
                Description = url
            };

            return Reply(embed);
        }

        [Command("avatar"), Description("Аватар")]
        public DiscordCommandResult AvatarCommand(IMember member)
        {
            string url = member.GetAvatarUrl();

            LocalEmbed embed = new()
            {
                ImageUrl = url,
                Description = url
            };

            return Reply(embed);
        }
    }

    [Name("Нейросетки!")]
    [Group("generate", "gen", "g"), Description("Нейронки делают фигню!")]
    public class GenerateCommands : DiscordModuleBase
    {
        [Command("cat"), Description("Котики!")]
        public DiscordCommandResult GenerateCatCommand()
        {
            return Reply(GetGeneratedPicture("https://thiscatdoesnotexist.com/"));
        }
        [Command("horse"), Description("Лошади!")]
        public DiscordCommandResult GenerateHorseCommand()
        {
            return Reply(GetGeneratedPicture("https://thishorsedoesnotexist.com/"));
        }
        [Command("art"), Description("Искусство!(?)")]
        public DiscordCommandResult GenerateArtCommand()
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

    [Name("Логи")]
    [RequireBotOwner]
    [Group("logs"), Description("Логи!")]
    public class LogCommands : DiscordModuleBase
    {
        [Command("get"), Description("Логи")]
        public DiscordCommandResult GetLogsCommand()
        {
            Stream logs = new FileStream($"{Logging.LogsFolderPath}/latest.log", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);

            LocalMessage msg = new LocalMessage()
            {
                Attachments = new List<LocalAttachment>() { new LocalAttachment(logs, "latest.log") },
            };

            return Reply(msg);
        }

        [Command("setlevel")]
        public DiscordCommandResult SetLevelCommand(string level)
        {
            Serilog.Events.LogEventLevel actualLevel = level.ToLower() switch
            {
                "verbose" => Serilog.Events.LogEventLevel.Verbose,
                "debug" => Serilog.Events.LogEventLevel.Debug,
                "info" => Serilog.Events.LogEventLevel.Information,
                "information" => Serilog.Events.LogEventLevel.Information,
                "warn" => Serilog.Events.LogEventLevel.Warning,
                "warning" => Serilog.Events.LogEventLevel.Warning,
                "error" => Serilog.Events.LogEventLevel.Error,
                _ => throw new ArgumentException("Недопустимый уровень логгирования.")
            };

            Logging.SetLogLevel(actualLevel);

            return Reply("Успешно!".AsCodeBlock());
        }
    }

}