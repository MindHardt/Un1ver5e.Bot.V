using Disqord;
using Disqord.Bot;
using Qmmands;
using System.Text.RegularExpressions;
using Un1ver5e.Bot.Services.Database;
using Un1ver5e.Bot.Services.Database.Entities;
using Un1ver5e.Bot.Utilities;
using Un1ver5e.Commands.Attributes;

namespace Un1ver5e.Bot.Commands
{
    [RequireDebug]
    [Name("🍅 Pomodoro")]
    [Group("pomodoro", "pmd"), Description("Таймеры помодоро")]
    public class PomodoroCommandModule : DiscordModuleBase
    {
        private readonly ApplicationContext dbCtx;

        public PomodoroCommandModule(ApplicationContext dbCtx)
        {
            this.dbCtx = dbCtx;
        }

        [Command("view")]
        public DiscordCommandResult ViewCommand()
        {
            PomodoroData data = dbCtx.GetPomodoro(Context.Author.Id.RawValue);

            IMember authorAsMember = (IMember)Context.Author!;

            string avatar = (authorAsMember ?? Context.Author).GetAvatarUrl();
            string name = authorAsMember is null ?
                Context.Author.Name :
                authorAsMember.GetDisplayName();

            LocalEmbed embed = new()
            {
                ThumbnailUrl = avatar,
                Title = $"Настройки pomodoro {name}",
                Fields =
                {
                    new()
                    {
                        Name = "Работа",
                        Value = data.Work.ToString().AsCodeBlock(),
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Короткий отдых",
                        Value = data.ShortRest.ToString().AsCodeBlock(),
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Длинный отдых",
                        Value = data.LongRest.ToString().AsCodeBlock(),
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Паттерн",
                        Value = data.Pattern.AsCodeBlock(),
                        IsInline = false
                    }
                }
            };

            return Reply(embed);
        }

        [Command("set")]
        public async ValueTask<DiscordCommandResult> SetCommand(string property, string value)
        {
            property = property.ToLower();

            string[] properties = { "work", "shortrest", "longrest", "pattern" };

            if (properties.Contains(property) == false)
            {
                LocalEmbed response = new LocalEmbed().WithTitle($"Недопустимое значение. Выберите что-то из:{string.Join('\n', properties).AsCodeBlock()}");
                return Reply(response);
            }

            value = value.ToUpper();

            if (property == "pattern")
            {
                Regex regex = new("[WSL]+");

                if (regex.IsMatch(value) == false)
                {
                    LocalEmbed response = new LocalEmbed().WithTitle($"Недопустимое значение. Паттерн должен состоять из символов W, S, L, означающих Work, Short и Long соответственно.");
                    return Reply(response);
                }

                PomodoroData data = dbCtx.GetPomodoro(Context.Author.Id.RawValue);
                data.Pattern = value;
                dbCtx.PomodoroData.Update(data);
                await dbCtx.SaveChangesAsync();

                return Reply(":white_check_mark:");
            }
            else
            {
                TimeSpan ts;

                if (TimeSpan.TryParse(value, out ts) == false)
                {
                    LocalEmbed response = new LocalEmbed().WithTitle($"Недопустимое значение. Не удалось преобразовать в формат времени.");
                    return Reply(response);
                }

                PomodoroData data = dbCtx.GetPomodoro(Context.Author.Id.RawValue);
                
                switch (property)
                {
                    case "work":
                        data.Work = ts;
                        break;
                    case "shortrest":
                        data.ShortRest = ts;
                        break;
                    case "longrest":
                        data.LongRest = ts;
                        break;
                }

                dbCtx.PomodoroData.Update(data);
                await dbCtx.SaveChangesAsync();

                return Reply(":white_check_mark:");
            }
        }
    }
}
