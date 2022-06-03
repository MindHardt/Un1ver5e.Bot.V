using Disqord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un1ver5e.Bot.BoardGames.Catan.Entities
{
    public class CatanPlayer
    {
        public IMember User { get; }

        /// <summary>
        /// The resources of a player.
        /// </summary>
        public ResourcePack Resources { get; } = new();

        public CatanPlayer(IMember user)
        {
            User = user;
        }

        public static CatanPlayer CreateRandom(IMember user)
        {
            CatanPlayer player = new(user);
            Random random = new Random();

            foreach (int index in Enumerable.Range(0, 5))
            {
                int amount = random.Next(0, 5);
                if (amount > 0) player.Resources.Add((Resource)index, amount);
            };

            return player;
        }
    }
}
