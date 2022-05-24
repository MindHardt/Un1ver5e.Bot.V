using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un1ver5e.Bot.Services.Dice
{
    public class DefaultDice : IDice
    {
        /// <summary>
        /// Represents the count of the thrown dice.
        /// </summary>
        public int Count { get; init; }
        /// <summary>
        /// Represents the maximum value that can appear on dice.
        /// </summary>
        public int MaxValue { get; init; }

        /// <summary>
        /// Throws this <see cref="DefaultDice"/> and returns the results.
        /// </summary>
        /// <returns>A collection of integers, the size of collection is equal to <see cref="Count"/>.</returns>
        public IEnumerable<int> GetResults(Random random) => new int[Count].Select(r => random.Next(1, MaxValue + 1));

        public DefaultDice(int maxValue, int count = 1)
        {
            if (maxValue < 2 || count < 1) throw new ArgumentException("Неверные значения при создании дайсов.");

            MaxValue = maxValue;
            Count = count;
        }

        public IThrowResult Throw(Random random, int modifyer = 0)
        {
            IEnumerable<int> results = GetResults(random);

            return new DefaultThrowResult(this, results, modifyer);
        }

        public override string ToString()
        {
            return $"{Count}d{MaxValue}";
        }
    }
}
