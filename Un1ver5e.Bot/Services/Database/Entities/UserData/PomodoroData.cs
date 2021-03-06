using Disqord;
using Un1ver5e.Bot.Services.Database.Entities.Abstract;

namespace Un1ver5e.Bot.Services.Database.Entities
{
    public class PomodoroData : UserData
    {
        public TimeSpan Work { get; set; } = TimeSpan.FromMinutes(45);
        public TimeSpan ShortRest { get; set; } = TimeSpan.FromMinutes(5);
        public TimeSpan LongRest { get; set; } = TimeSpan.FromMinutes(15);
        public string Pattern { get; set; } = "WSWSWL";
    }
}
