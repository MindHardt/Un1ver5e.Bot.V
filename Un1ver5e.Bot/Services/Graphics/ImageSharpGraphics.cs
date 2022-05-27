using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un1ver5e.Bot.Services.Graphics
{
    internal class ImageSharpGraphics : IGraphics
    {
        public async ValueTask<Stream> GetColorImage(int colorCode)
        {
            byte R = (byte)(colorCode >> 16);
            byte G = (byte)(colorCode >> 8);
            byte B = (byte)colorCode;

            Color color = Color.FromRgba(R, G, B, 0xFF);

            Image<Rgba32> image = new(1024, 1024, color);

            MemoryStream ms = new();
            await image.SaveAsync(ms, new PngEncoder());
            ms.Position = 0;

            return ms;
        }
    }
}
