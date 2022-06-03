using Disqord;
using Disqord.Bot;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Un1ver5e.Bot.Commands.Attributes;
using Un1ver5e.Bot.Services.Database;
using Un1ver5e.Bot.Services.Database.Entities;
using Un1ver5e.Bot.Utilities;

namespace Un1ver5e.Bot.Commands
{
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
            ulong id = Context.Author.Id.RawValue;

            PomodoroData data = dbCtx.PomodoroData.Where(data => data.Id == id).FirstOrDefault()!;

            if (data is null)
            {
                data = new PomodoroData() { Id = id };
            }

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
                        Value = data.PomodoroWork.ToString().AsCodeBlock(),
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Короткий отдых",
                        Value = data.PomodoroShortRest.ToString().AsCodeBlock(),
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Длинный отдых",
                        Value = data.PomodoroLongRest.ToString().AsCodeBlock(),
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
        public async ValueTask<DiscordCommandResult> SetCommand()
        {
            
        }
    }
}
