using Disqord;
using Disqord.Bot;
using Disqord.Extensions.Interactivity;
using Disqord.Gateway;
using Microsoft.Extensions.Hosting;
using Qmmands;
using Serilog.Core;
using Un1ver5e.Bot.Services;
using Un1ver5e.Bot.Services.Database;
using Un1ver5e.Bot.Utilities;

namespace Un1ver5e.Bot.Commands
{
    [Name("🔧 Создатель")]
    [RequireBotOwner]
    [Group("owner", "o"), Description("Страшные вещи")]
    public class OwnerCommands : DiscordModuleBase
    {
        private readonly IHost host;
        private readonly LoggingLevelSwitch logswitch;
        private readonly FolderPathService paths;
        private readonly ApplicationContext dbctx;

        public OwnerCommands(IHost host, LoggingLevelSwitch logswitch, Random random, FolderPathService paths, ApplicationContext dbctx)
        {
            this.host = host;
            this.logswitch = logswitch;
            this.paths = paths;
            this.dbctx = dbctx;
        }

        [Command("setloglevel")]
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

            logswitch.MinimumLevel = actualLevel;

            return Reply("Успешно!".AsCodeBlock());
        }

        [Command("shutdown")]
        public async ValueTask ShutDownCommand()
        {
            await Reply("Вы уверены что хотите продолжить?");

            InteractivityExtension inter = Context.Bot.GetInteractivity();
            Snowflake channelId = Context.ChannelId;
            TimeSpan timeout = TimeSpan.FromSeconds(30);

            MessageReceivedEventArgs args = await inter.WaitForMessageAsync(channelId, (e) =>
            {
                return e.AuthorId == Context.Author.Id && e.Message.Content.ToLower() == "да";
            }, timeout);

            if (args != null)
            {
                await Reply("Выключаюсь!");
                await host.StopAsync();
            }
            else
            {
                await Reply("Время вышло!");
            }
        }

        [Command("dbsnap"), RequirePrivate]
        public DiscordCommandResult DBSnapCommand()
        {
            Stream db = File.Open(paths["Database"] + "/mo.db", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            LocalMessage msg = new()
            {
                Attachments =
                {
                    new(db, $"snapshot_{DateTime.Now}.db")
                }
            };

            return Reply(msg);
        }

        [Command("temp")]
        public async ValueTask<DiscordCommandResult> TempCommand()
        {
            return Reply("lorem ipsum dolorem sit amet");
        }
    }
}
