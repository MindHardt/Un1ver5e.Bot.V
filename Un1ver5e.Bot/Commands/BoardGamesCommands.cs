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
        public BoardGamesCommands(IDiceService service)
        {
            this.service = service;
        }

        private readonly IDiceService service;

        [Command("throw", "dice"), Description("Бросает куб, заданный текстовым описанием.")]
        public DiscordCommandResult ThrowCommand(string query)
        {
            string reply = service.ThrowByQuery(query).ToString()!;

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

        [RequireGuild]
        [Command("challenge"), Description("Решаем дела по-мужски.")]
        public DiscordCommandResult Challenge(IMember mem)
        {
            
            return Reply();
        }
    }
}
