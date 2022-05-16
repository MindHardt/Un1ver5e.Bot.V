namespace Un1ver5e.Bot.Utilities
{
    internal static class TokenReader
    {
        /// <summary>
        /// Gets token corresponding to a current bots configuration.
        /// </summary>
        /// <returns></returns>
        public static string ReadToken()
        {
#if DEBUG
            string tokenFilePath = $"{Statics.DataFolderPath}/debug.token";
#else
            string tokenFilePath = $"{Statics.DataFolderPath}/release.token"; 
#endif
            if (File.Exists(tokenFilePath) == false) throw new FileNotFoundException($"Could not find file '{tokenFilePath}'");

            return File.ReadAllText(tokenFilePath);
        }
    }
}
