using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un1ver5e.Bot
{
    internal static class Statics
    {
        private static DirectoryInfo dataDirectory = Directory.CreateDirectory("Data");
        /// <summary>
        /// A path to Data folder.
        /// </summary>
        internal static string DataFolderPath => dataDirectory.FullName;
    }
}
