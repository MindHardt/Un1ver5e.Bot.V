using Disqord;
using Disqord.Bot;
using Microsoft.Extensions.Configuration;
using Qmmands;
using System.Diagnostics;
using Un1ver5e.Bot.Services;
using Un1ver5e.Bot.Utilities;
using Un1ver5e.Commands.Attributes;

namespace Un1ver5e.Commands
{
    [Name("⚙️ Базовые команды")]
    public class BasicCommands : DiscordModuleBase
    {
        private readonly Random random;
        private readonly IConfiguration config;
        private readonly DatabaseService databaseService;

        public BasicCommands(Random random, IConfiguration config, DatabaseService databaseService)
        {
            this.random = random;
            this.config = config;
            this.databaseService = databaseService;
        }

        [Command("splash"), Description("Сплеш из майнкрафта!")]
        public DiscordCommandResult SplashCommand()
        {
            return Reply(config.GetSection("splashes").Get<string[]>().GetRandomElement());
        }

        [Command("roll"), Description("Случайное число от 1 до 100")]
        public DiscordCommandResult RollCommand()
        {
            return RandomCommand(1, 100);
        }

        [Command("random", "rnd"), Description("Случайное число между двумя данными")]
        public DiscordCommandResult RandomCommand(int lowerBound, int upperBound)
        {
            LocalEmbed embed = new()
            {
                Fields = new List<LocalEmbedField>()
                {
                    new LocalEmbedField()
                    {
                        Name = $"Результат вашего броска [{lowerBound}..{upperBound}]:",
                        Value = $"🎲 **{random.Next(lowerBound, upperBound + 1)}**"
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
                Description = rateMessage
            };

            LocalMessage respond = new LocalMessage()
                .WithEmbeds(embed)
                .WithReply(Context.Message.ReferencedMessage.Value.Id);

            return Response(respond);
        }

        [Command("stealemoji", "steal"), Description("Ваш эмоджи теперь мой")]
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

        [Command("stats"), Description("Состояние бота")]
        public async ValueTask<DiscordCommandResult> StatsCommand()
        {
            string doubleFormatter = "###,###,###,###.00"; //Precision up to 2 digits

            string launchTimestamp = $"<t:{new DateTimeOffset(Process.GetCurrentProcess().StartTime).ToUnixTimeSeconds()}:R>";
            string heapSize = $"{(GC.GetTotalMemory(true) / 1048576.0).ToString(doubleFormatter)} MBs";
            TimeSpan dbLatency = await databaseService.GetPing();
            TimeSpan socketLatency = DateTimeOffset.UtcNow - Context.Message.CreatedAt();

            LocalEmbed embed = new()
            {
                Fields = new List<LocalEmbedField>()
                {
                    new LocalEmbedField()
                    {
                        Name = "Бот запущен",
                        Value = launchTimestamp,
                        IsInline = true
                    },
                    new LocalEmbedField()
                    {
                        Name = "Объем хипа",
                        Value = heapSize,
                        IsInline = false
                    },
                    new LocalEmbedField()
                    {
                        Name = "Пинг сокета",
                        Value = $"{socketLatency.TotalMilliseconds.ToString(doubleFormatter)} мс",
                        IsInline = true
                    },
                    new LocalEmbedField()
                    {
                        Name = "Пинг базы данных",
                        Value = $"{dbLatency.TotalMilliseconds.ToString(doubleFormatter)} мс",
                        IsInline = true
                    },
                }
            };

            return Reply(embed);
        }


        [Command("test"), RequireGuild()]
        public DiscordCommandResult TestCommand()
        {
            return Reply("Сись");
        }
    }

    [Name("🤖 Нейросетки!")]
    [Group("generate", "gen", "g"), Description("Нейронки делают фигню!")]
    public class GenerateCommands : DiscordModuleBase
    {
        [Command("cat"), Description("Котики!")]
        public async ValueTask<DiscordCommandResult> GenerateCatCommand()
        {
            return Reply(await GetGeneratedPicture("https://thiscatdoesnotexist.com/"));
        }
        [Command("horse"), Description("Лошади!")]
        public async ValueTask<DiscordCommandResult> GenerateHorseCommand()
        {
            return Reply(await GetGeneratedPicture("https://thishorsedoesnotexist.com/"));
        }
        [Command("art"), Description("Искусство!(?)")]
        public async ValueTask<DiscordCommandResult> GenerateArtCommand()
        {
            return Reply(await GetGeneratedPicture("https://thisartworkdoesnotexist.com/"));
        }

        private static async ValueTask<LocalMessage> GetGeneratedPicture(string url)
        {
            using HttpClient client = new();
            Stream pic = await client.GetStreamAsync(url);

            return new LocalMessage()
            {
                Attachments = new List<LocalAttachment>() { new LocalAttachment(pic, "generated.jpg") },
                Content = $"Источник: ||{url}||"
            };
        }
    }
}