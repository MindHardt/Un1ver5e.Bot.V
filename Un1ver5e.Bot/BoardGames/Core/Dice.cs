using System.Text.RegularExpressions;

namespace Un1ver5e.Bot.BoardGames.Core
{
    /// <summary>
    /// Represents a fixed set of dice (i.e. 1d4, 1d6, 1d8, 2d6...)
    /// </summary>
    public class Dice
    {
        /// <summary>
        /// The random object used to create throws. Defaults to <see cref="Random.Shared"/>.
        /// </summary>
        public static Random Randomizer { private get; set; } = Random.Shared;
        /// <summary>
        /// Defines whether all the created Dice should be cached. Cache helps retrieving <see cref="Dice"/> via <see cref="Dice.FromText(string)"/> method faster.
        /// </summary>
        public static bool AlwaysCacheDice { get; set; } = true;
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
        public IEnumerable<int> GetResults() => new int[Count].Select(r => Randomizer.Next(1, MaxValue + 1));

        public Dice(int maxValue, int count = 1)
        {
            if (maxValue < 2 || count < 1) throw new ArgumentException("Неверные значения при создании дайсов.");

            MaxValue = maxValue;
            Count = count;

            TryCache();
        }

        public CompleteThrowResult Throw(int modifyer = 0)
        {
            IEnumerable<int> results = GetResults();

            return new CompleteThrowResult(this, results, modifyer);
        }

        /// <summary>
        /// Creates a <see cref="Dice"/> from its text representation, i.e. 2d6, 1d8... This methods tries to retrieve them from cache first.
        /// </summary>
        /// <param name="text">The text representation of the dice.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Dice FromText(string text)
        {
            if (s_cache.ContainsKey(text)) return s_cache[text];

            if (CheckValidDiceText(text) == false) throw new ArgumentException("Некорректный текст дайса", nameof(text));

            return ParseText(text);
        }

        public static CompleteThrowResult ThrowByQuery(string text)
        {
            Regex regex = new("(?<Dice>\\d+[Dd]\\d+)(?<Modifyer>[+-]\\d+)?");

            if (regex.IsMatch(text) == false) throw new ArgumentException("Некорректный запрос!");

            Match match = regex.Match(text);

            int modifyer = 0;
            string modifyerString = match.Groups["Modifyer"].Value;
            if (string.IsNullOrWhiteSpace(modifyerString) == false)
            {
                modifyerString = modifyerString.First() == '+' ? modifyerString[1..] : modifyerString;
                modifyer = int.Parse(modifyerString);
            }

            Dice dice = ParseText(match.Groups["Dice"].Value);

            return dice.Throw(modifyer);
        }

        /// <summary>
        /// Checks whether given dice string is a valid dice.
        /// </summary>
        /// <param name="text"></param>
        /// <exception cref="ArgumentException"></exception>
        private static bool CheckValidDiceText(string text)
        {
            Regex diceRegex = new("\\d*[Dd]\\d+");
            return diceRegex.IsMatch(text);
        }
        /// <summary>
        /// Parses correct text into a <see cref="Dice"/> object.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static Dice ParseText(string text)
        {
            string[] numbers = text.ToLower().Split('d');
            int count;
            int maxValue;

            bool countCorrect;
            bool maxValueCorrect;

            countCorrect = int.TryParse(numbers[0], out count);
            maxValueCorrect = int.TryParse(numbers[1], out maxValue);

            if (countCorrect == false || maxValueCorrect == false) throw new ArgumentException("Некорректный текст дайса", nameof(text));

            return new Dice(maxValue, count);
        }

        public override string ToString()
        {
            return $"{Count}d{MaxValue}";
        }

        //THE UNDERLYING PART IS RESPONSIBLE FOR DICE CACHE

        private static Dictionary<string, Dice> s_cache { get; } = new Dictionary<string, Dice>();

        /// <summary>
        /// Caches common <see cref="Dice"/>. The list includes: 
        /// <para>1d2, 1d3, 1d4, 1d6, 1d8, 1d10, 1d12, 1d20, 1d100, 2d6.</para>
        /// They get cached regardless of <see cref="AlwaysCacheDice"/> rule.
        /// </summary>
        public static void CacheCommonDice()
        {
            s_cache["1d2"] = new Dice(2);
            s_cache["1d3"] = new Dice(3);
            s_cache["1d4"] = new Dice(4);
            s_cache["1d6"] = new Dice(6);
            s_cache["1d8"] = new Dice(8);
            s_cache["1d10"] = new Dice(10);
            s_cache["1d12"] = new Dice(12);
            s_cache["1d20"] = new Dice(20);
            s_cache["1d100"] = new Dice(100);
            s_cache["2d6"] = new Dice(6, 2);
        }
        /// <summary>
        /// Gets cached dice.
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyDictionary<string, Dice> GetCacheSnapshot() => s_cache;
        private void TryCache()
        {
            if (AlwaysCacheDice == false) return;

            string name = ToString();

            s_cache[name] = this;
        }
    }
}
