﻿using Disqord;

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
        public static T GetRandomElement<T>(this IEnumerable<T> collection, Random? randomOverride = null) =>
            collection.Shuffle(randomOverride).First();

        /// <summary>
        /// Formats string as a Discord Codeblock
        /// </summary>
        /// <param name="orig">The original string.</param>
        /// <returns></returns>
        public static string AsCodeBlock(this string original, string lang = "") => $"```{lang}\n{original}```";

        /// <summary>
        /// Returns either :green_circle: or :red_circle: emoji depending on <paramref name="value"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static LocalEmoji AsEmoji(this bool value) => value ? LocalEmoji.FromString(":green_circle:") : LocalEmoji.FromString(":red_circle:");
    }
}
