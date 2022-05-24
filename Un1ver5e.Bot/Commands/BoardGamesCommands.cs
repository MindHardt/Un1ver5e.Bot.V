using Disqord;
using Disqord.Bot;
using Qmmands;
using Un1ver5e.Bot.Services.Dice;

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
        [Command("challenge", "fight"), Description("Решаем дела по-мужски.")]
        public DiscordCommandResult Challenge(IMember opponent)
        {
            IMember author = (IMember)Context.Author;

            IThrowResult resultAuthor = service.ThrowByQuery("1d100");
            IThrowResult resultOpponent = service.ThrowByQuery("1d100");

            IMember? winner = Math.Sign(resultAuthor.GetCompleteSum().CompareTo(resultOpponent.GetCompleteSum())) switch
            {
                -1 => opponent,
                0 => null,
                1 => author,
                _ => throw new Exception("What the fuck")
            };

            string title = winner is not null ?
                $"Победил(а) {winner.Nick ?? winner.Name}!" :
                "Ничья!";

            LocalEmbed embed = new()
            {
                Fields = new LocalEmbedField[]
                {
                    new LocalEmbedField()
                    {
                        Name = author.Nick ?? author.Name,
                        Value = resultAuthor.GetCompleteSum().ToString(),
                        IsInline = true
                    },
                    new LocalEmbedField()
                    {
                        Name = opponent.Nick ?? opponent.Name,
                        Value = resultOpponent.GetCompleteSum().ToString(),
                        IsInline = true
                    }
                },
                Title = title,
                ThumbnailUrl = winner.GetGuildAvatarUrl()
            };

            return Reply(embed);
        }
    }
}
