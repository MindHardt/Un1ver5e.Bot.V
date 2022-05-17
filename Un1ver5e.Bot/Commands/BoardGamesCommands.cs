using Disqord;
using Disqord.Bot;
using Qmmands;
using Un1ver5e.Bot.BoardGames.Core;
using Un1ver5e.Bot.Utilities;

namespace Un1ver5e.Bot.Commands
{
    [Name("Настолки"), Description("Команды для настолок!")]
    public partial class BoardGamesCommands : DiscordModuleBase
    {
        [Command("throw", "dice"), Description("Бросает куб, заданный текстовым описанием.")]
        public DiscordCommandResult ThrowCommand(string query)
        {
            string reply = Dice.ThrowByQuery(query).ToString();

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
            string reply = string.Join('\n', Dice.GetCacheSnapshot().Keys).AsCodeBlock();

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
                        Value = Dice.AlwaysCacheDice ? ":green_circle:" : ":red_circle:"
                    }
                }
            };

            return Reply(embed);
        }
    }
}
