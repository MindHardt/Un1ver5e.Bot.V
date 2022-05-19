using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Un1ver5e.Bot.Utilities
{
    public class FolderPathProvider
    {

        public IEnumerable<string> Paths
        {
            init
            {
                foreach (string path in value)
                {
                    DirectoryInfo dir = Directory.CreateDirectory(path);

                    paths.Add(dir.Name.ToLower(), dir.FullName);
                }
            }
        }

        private Dictionary<string, string> paths = new();

        public string this[string query]
        {
            get
            {
                Regex regex = new($".*{query.ToLower()}.*");

                string key = paths.Keys.Where(k => regex.IsMatch(k)).Single();

                return paths[key];
            }
        }
    }
}
