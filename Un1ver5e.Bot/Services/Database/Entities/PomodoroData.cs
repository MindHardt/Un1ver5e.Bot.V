using Disqord;

namespace Un1ver5e.Bot.Services.Database.Entities
{
    public class PomodoroData
    {
        /// <summary>
        /// Gets the ID of the <see cref="IUser"/> which correlates to this <see cref="PomodoroData"/>.
        /// </summary>
        public ulong Id { get; set; }

        public TimeSpan Work { get; set; } = TimeSpan.FromMinutes(45);
        public TimeSpan ShortRest { get; set; } = TimeSpan.FromMinutes(5);
        public TimeSpan LongRest { get; set; } = TimeSpan.FromMinutes(15);
        public string Pattern { get; set; } = "WSWSWL";
    }
}
