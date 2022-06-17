using Disqord;

namespace Un1ver5e.Bot.Services.Database.Entities.Abstract
{
    /// <summary>
    /// Represents a single board game data as Win-Lose-Draw.
    /// </summary>
    public class TriGameData : BiGameData
    {
        public int Draw { get; set; } = 0;

        /// <inheritdoc cref="IGameData.TotalGames"/>
        public override int TotalGames => base.TotalGames + Draw;

        /// <inheritdoc cref="IGameData.AsEmbedField(string)"/>
        public new LocalEmbedField AsEmbedField(string name)
        {
            int total = TotalGames;
            total = total == 0 ? 1 : total; //prevent divide by zero

            return new()
            {
                Name = name,
                Value = string.Format(":trophy:{0} ({1}%)\n:flag_white:{2} ({3}%)\n:handshake:{4} ({5}%)",
                    Win, (Win * 100) / total,
                    Lose, (Lose * 100) / total,
                    Draw, (Draw * 100) / total)
            };
        }
    }
}
