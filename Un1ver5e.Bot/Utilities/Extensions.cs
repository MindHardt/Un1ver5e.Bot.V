using Disqord;

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

        /// <summary>
        /// Formats string as a Discord Codeblock
        /// </summary>
        /// <param name="orig">The original string.</param>
        /// <returns></returns>
        public static string AsCodeBlock(this string original, string lang = "") => $"```{lang}\n{original}```";
    }
}
