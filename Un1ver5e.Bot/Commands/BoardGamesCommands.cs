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
        public DiscordCommandResult Challenge(IMember opponent, string dice = "1d100")
        {
            IMember author = (IMember)Context.Author;

            if (opponent.IsBot)     return Reply(new LocalEmbed().WithDescription("Не трогай шайтан-машину!"));
            if (author == opponent) return Reply(new LocalEmbed().WithDescription("Не делай этого с собой! Тебе есть ради чего жить!"));

            IThrowResult resultAuthor = service.ThrowByQuery(dice);
            IThrowResult resultOpponent = service.ThrowByQuery(dice);

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

            string thumbnail = (winner ?? (IMember)Context.Bot.CurrentUser).GetGuildAvatarUrl();

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
                ThumbnailUrl = thumbnail
            };

            return Reply(embed);
        }
    }
}
