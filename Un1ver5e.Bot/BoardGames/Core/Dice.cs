using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un1ver5e.Bot.BoardGames.Core
{
    public class Dice
    {
        /// <summary>
        /// The random object used to create throws. Defaults to <see cref="Random.Shared"/>.
        /// </summary>
        public static Random Randomizer { private get; set; } = Random.Shared;
        /// <summary>
        /// Defines whether all the created Dice should be cached. Cache helps retrieving <see cref="Dice"/> via <see cref="Dice.FromText(string)"/> method faster.
        /// </summary>
        public static bool AlwaysCacheDice { private get; set; } = true;
        /// <summary>
        /// Represents the count of the thrown dice.
        /// </summary>
        public int Count { get; init; }
        /// <summary>
        /// Represents the maximum value that can appear on dice.
        /// </summary>
        public int MaxValue { get; init; }

        /// <summary>
        /// Throws this <see cref="Dice"/> and returns the results.
        /// </summary>
        /// <returns>A collection of integers, the size of collection is equal to <see cref="Count"/>.</returns>
        public IEnumerable<int> GetResults(Random random) => new int[Count].Select(r => random.Next(1, MaxValue + 1));

        public Dice(int maxValue, int count = 1)
        {
            if (maxValue < 2 || count < 1) throw new ArgumentException("Неверные значения при создании дайсов.");

            MaxValue = maxValue;
            Count = count;
        }

        public ThrowResult Throw(Random random, int modifyer = 0)
        {
            IEnumerable<int> results = GetResults(random);

            return new ThrowResult(this, results, modifyer);
        }

        public override string ToString()
        {
            return $"{Count}d{MaxValue}";
        }
    }
}
