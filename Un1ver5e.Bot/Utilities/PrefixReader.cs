namespace Un1ver5e.Bot.Utilities
{
    internal static class PrefixReader
    {
        /// <summary>
        /// Gets prefix corresponding to a current bots configuration.
        /// </summary>
        /// <returns></returns>
        public static string ReadPrefix()
        {
#if DEBUG
            return "mt ";
#else
            return "mo ";
#endif
        }
    }
}
