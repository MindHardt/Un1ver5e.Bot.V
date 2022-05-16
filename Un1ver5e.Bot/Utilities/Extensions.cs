using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un1ver5e.Bot.Utilities
{
    public static class Extensions
    {
        /// <summary>
        /// Shuffles the collection, making it random-ordered. This returns a lazy collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The original collection.</param>
        /// <returns></returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection, Random? randomOverride = null) =>
            collection.OrderBy((e) => (randomOverride ?? Random.Shared).Next());

        /// <summary>
        /// Gets random element of a <paramref name="collection"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T GetRandomElement<T>(this IList<T> collection, Random? randomOverride = null) =>
            collection.Shuffle(randomOverride).First();
    }
}
