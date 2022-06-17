using Disqord;

namespace Un1ver5e.Bot.Services.Database.Entities.Abstract
{
    public abstract class BoardGameData : UserData
    {
        public int Win { get; set; } = 0;
        public int Lose { get; set; } = 0;
        public int Draw { get; set; } = 0;

        /// <summary>
        /// Gets a relation of wins to loses as percentage (0..100). Draws are ignored.
        /// </summary>
        /// <returns></returns>
        public int GetWinratePercent() => (Win * 100) / Lose;

        /// <summary>
        /// Gets a sum of <see cref="Win"/>, <see cref="Lose"/> and <see cref="Draw"/> properties.
        /// </summary>
        /// <returns></returns>
        public int GetTotal() => Win + Lose + Draw;

        /// <summary>
        /// Formats this <see cref="BoardGameData"/> into a <see cref="LocalEmbedField"/> used for displaying stats.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual LocalEmbedField GetEmbedField(string name)
        {
            int total = GetTotal();
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
