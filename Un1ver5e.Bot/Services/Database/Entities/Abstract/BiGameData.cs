using Disqord;

namespace Un1ver5e.Bot.Services.Database.Entities.Abstract
{
    /// <summary>
    /// Represents a single board game data as Win-Lose.
    /// </summary>
    public class BiGameData : UserData, IGameData
    {
        public int Win { get; set; } = 0;
        public int Lose { get; set; } = 0;

        /// <inheritdoc cref="IGameData.TotalGames"/>
        public virtual int TotalGames => Win + Lose;

        /// <summary>
        /// Gets a relation of wins to loses as percentage (0..100).
        /// </summary>
        /// <returns></returns>
        public virtual int GetWinratePercent() => (Win * 100);

        /// <inheritdoc cref="IGameData.AsEmbedField(string)"/>
        public LocalEmbedField AsEmbedField(string name)
        {
            int total = TotalGames;
            total = total == 0 ? 1 : total; //prevent divide by zero

            return new()
            {
                Name = name,
                Value = string.Format(":trophy:{0} ({1}%)\n:flag_white:{2} ({3}%)",
                    Win, (Win * 100) / total,
                    Lose, (Lose * 100) / total)
            };
        }
    }
}
