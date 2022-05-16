using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un1ver5e.Bot.Utilities
{
    internal class Logging
    {
        private static LoggingLevelSwitch loggingLevelSwitch = new(LogEventLevel.Information);
        /// <summary>
        /// Gets default <see cref="LoggerConfiguration"/> used for MO.
        /// </summary>
        /// <returns></returns>
        internal static Logger GetLogger()
        {
            return new LoggerConfiguration()
                .MinimumLevel.ControlledBy(loggingLevelSwitch)
                .WriteTo.Console()
                .WriteTo.File($"{LogsFolderPath}/latest.log", shared: true)
                .CreateLogger();
        }

        private static DirectoryInfo logsDirectory = Directory.CreateDirectory(Statics.DataFolderPath + "/logs");
        /// <summary>
        /// A path to logs folder.
        /// </summary>
        internal static string LogsFolderPath => logsDirectory.FullName;
        /// <summary>
        /// Sets logging level to <paramref name="level"/>.
        /// </summary>
        /// <param name="level"></param>
        internal static void SetLogLevel(LogEventLevel level) => loggingLevelSwitch.MinimumLevel = level;
    }
}
