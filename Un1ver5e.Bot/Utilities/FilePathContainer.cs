using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un1ver5e.Bot.Utilities
{
    public class PathContainer
    {
        private string logsFolder = "Logs";
        private string dataFolder = "Data";
        private string galleryFolder = "Data/Gallery";

        public string LogsFolder
        {
            get => logsFolder; init
            {
                logsFolder = Directory.CreateDirectory(value ?? throw new ArgumentNullException($"{nameof(value)} was null.")).FullName;
            }
        }
        public string DataFolder
        {
            get => dataFolder; init
            {
                dataFolder = Directory.CreateDirectory(value ?? throw new ArgumentNullException($"{nameof(value)} was null.")).FullName;
            }
        }
        public string GalleryFolder
        {
            get => galleryFolder; init
            {
                galleryFolder = Directory.CreateDirectory(value ?? throw new ArgumentNullException($"{nameof(value)} was null.")).FullName;
            }
        }

        public string this[string foldername]
        {
            get => foldername.ToLower() switch
            {
                "logs" => LogsFolder,
                "data" => DataFolder,
                "gallery" => GalleryFolder,
                _ => throw new DirectoryNotFoundException()
            };
        }
    }
}
