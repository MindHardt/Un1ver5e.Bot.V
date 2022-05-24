using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un1ver5e.Bot.Services.Database
{
    public class UserData : IDatabaseEntity
    {
        public UserData(ulong discordID)
        {
            DiscordID = discordID;
        }
        public ulong DiscordID { get; set; }
        public PomodoroConfiguration Pomodoro { get; set; } = new();



        public async ValueTask Pull(DatabaseService dbService)
        {
            throw new NotImplementedException();
        }

        public async ValueTask Push(DatabaseService dbService)
        {
            using NpgsqlConnection conn = dbService.GetConnection();
            await conn.OpenAsync();

            using NpgsqlCommand cmd = conn.CreateCommand();
        }
    }

    public class PomodoroConfiguration
    {
        public TimeSpan WorkTime { get; set; } = TimeSpan.FromMinutes(25);
        public TimeSpan ShortRestTime { get; set; } = TimeSpan.FromMinutes(5);
        public TimeSpan LongRestTime { get; set; } = TimeSpan.FromMinutes(30);
        public string Pattern { get; set; } = "WSWSWSWL";
    }
}
