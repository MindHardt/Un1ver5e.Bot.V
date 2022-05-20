using Disqord.Bot.Hosting;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;

namespace Un1ver5e.Bot.BoardGames.Core
{
    public class DiceThrower
    {
        /// <summary>
        /// The random object used to create throws. Defaults to <see cref="Random.Shared"/>.
        /// </summary>
        public Random Randomizer { get; set; } = Random.Shared;
        /// <summary>
        /// Defines whether all the created Dice should be cached. Cache helps retrieving <see cref="Dice"/> via <see cref="Dice.FromText(string)"/> method faster. Defaults to <see cref="true"/>.
        /// </summary>
        public bool AlwaysCacheDice { get; set; } = true;
        /// <summary>
        /// Defines values that are present in dice cache from the start.
        /// </summary>
        public IEnumerable<string> CacheBase
        {
            init
            {
                foreach (string dice in value)
                {
                    if (CheckValidDiceText(dice))
                        CacheDice(ParseText(dice));
                }
            }
        }

        /// <summary>
        /// Creates <see cref="Dice"/> from its text form, allowing modifyer (i.e. 2d6+3) and throws it.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>A result of the throw.</returns>
        /// <exception cref="ArgumentException"></exception>
        public ThrowResult ThrowByQuery(string text)
        {
            Regex regex = new("(?<Dice>\\d*[Dd]\\d+)(?<Modifyer>[+-]\\d+)?");

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

            TryCacheDice(dice);

            return dice.Throw(Randomizer, modifyer);
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

            bool countCorrect;
            bool maxValueCorrect;
            int count;

            if (string.IsNullOrEmpty(numbers[0]))
            {
                countCorrect = true;
                count = 1;
            }
            else
            {
                countCorrect = int.TryParse(numbers[0], out count);
            }
            maxValueCorrect = int.TryParse(numbers[1], out int maxValue);

            if (countCorrect == false || maxValueCorrect == false) throw new ArgumentException("Некорректный текст дайса", nameof(text));

            return new Dice(maxValue, count);
        }

        //THE UNDERLYING PART IS RESPONSIBLE FOR DICE CACHE
        private Dictionary<string, Dice> Cache { get; } = new Dictionary<string, Dice>();

        /// <summary>
        /// Tries to cache <paramref name="dice"/>. The result depends on <see cref="AlwaysCacheDice"/>.
        /// </summary>
        /// <param name="dice"></param>
        public void TryCacheDice(Dice dice)
        {
            if (AlwaysCacheDice) CacheDice(dice);
        }

        /// <summary>
        /// Adds this <paramref name="dice"/> to <see cref="Cache"/>.
        /// </summary>
        /// <param name="dice"></param>
        public void CacheDice(Dice dice)
        {
            Cache[dice.ToString()!] = dice;
        }

        /// <summary>
        /// Get current state of <see cref="Dice"/> cache.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyDictionary<string, Dice> GetCacheSnapshot() => Cache;
    }
}
