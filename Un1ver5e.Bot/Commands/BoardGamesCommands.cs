using Disqord;
using Disqord.Bot;
using Qmmands;
using Un1ver5e.Bot.Services;
using Un1ver5e.Bot.Services.Dice;
using Un1ver5e.Bot.Utilities;

namespace Un1ver5e.Bot.Commands
{
    [Name("🎲 Настолки"), Description("Команды для настолок!")]
    public partial class BoardGamesCommands : DiscordModuleBase
    {
        public BoardGamesCommands(DiceService service)
        {
            this.service = service;
        }

        private readonly DiceService service;

        [Command("throw", "dice"), Description("Бросает куб, заданный текстовым описанием.")]
        public DiscordCommandResult ThrowCommand(string query)
        {
            string reply = service.ThrowByQuery(query).ToString();

            LocalEmbed embed = new()
            {
                Fields = new List<LocalEmbedField>()
                {
                    new LocalEmbedField()
                    {
                        Name = "Результат вашего броска:",
                        Value = reply
                    }
                }
            };

            return Reply(embed);
        }

        [Command("listdice"), Description("Показывает кешированные кубы.")]
        public DiscordCommandResult ListDiceCommand()
        {
            string reply = string.Join('\n', service.GetCacheSnapshot().Keys).AsCodeBlock();

            LocalEmbed embed = new()
            {
                Fields = new List<LocalEmbedField>()
                {
                    new LocalEmbedField()
                    {
                        Name = "Список кешированных кубов:",
                        Value = reply
                    },
                    new LocalEmbedField()
                    {
                        Name = "Кеширование кубов:",
                        Value = service.AlwaysCacheDice.AsEmoji().ToString()
                    }
                }
            };

            return Reply(embed);
        }

        [RequireBotOwner]
        [Command("cachedice"), Description("Переключает кеширование кубов.")]
        public DiscordCommandResult CacheDiceCommand()
        {
            service.AlwaysCacheDice = !service.AlwaysCacheDice;//Switching

            LocalEmbed embed = new()
            {
                Description = $"Кеширование кубов: {service.AlwaysCacheDice.AsEmoji()}"
            };

            return Reply(embed);
        }


    }
}
