using Disqord;
using Disqord.Bot;
using Qmmands;
using Un1ver5e.Bot.Utilities;
using System.Linq;

namespace Un1ver5e.Bot.Commands
{
    [Name("🔧 Создатель")]
    [RequireBotOwner]
    [Group("owner"), Description("Страшные вещи")]
    public class LogCommands : DiscordModuleBase
    {
        [Command("getlogs"), Description("Логи")]
        public DiscordCommandResult GetLogsCommand()
        {
            Stream logs = new FileStream($"{Logging.LogsFolderPath}/latest.log", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);

            LocalMessage msg = new()
            {
                Attachments = new List<LocalAttachment>() { new LocalAttachment(logs, "latest.log") },
            };

            return Reply(msg);
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

            Logging.SetLogLevel(actualLevel);

            return Reply("Успешно!".AsCodeBlock());
        }

        [Command("shutdown")]
        public void ShutDownCommand()
        {
            Environment.Exit(0); //TODO: create a proper shutdown
        }
    }
}
