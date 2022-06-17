using Disqord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un1ver5e.Bot.Services.Database.Entities.Abstract
{
    public interface IGameData
    {
        /// <summary>
        /// Gets the count of all the games played.
        /// </summary>
        public int TotalGames { get; }

        /// <summary>
        /// Formats this <see cref="IGameData"/> into a <see cref="LocalEmbedField"/> used for displaying stats.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract LocalEmbedField AsEmbedField(string name);
    }
}
