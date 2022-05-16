using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un1ver5e.Bot.Utilities
{
    internal static class SplashReader
    {
        private static readonly string[] s_splashes = File.ReadAllLines($"{Statics.DataFolderPath}/Splashes.txt");
        /// <summary>
        /// Gets random splash.
        /// </summary>
        /// <returns></returns>
        internal static string GetSplash()
        {
            return s_splashes.GetRandomElement();
        }
    }
}
