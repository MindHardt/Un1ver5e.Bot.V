using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un1ver5e.Bot.Services.Graphics
{
    public interface IGraphics
    {
        /// <summary>
        /// Gets an image of the 32-bit RGBA color specified by integer value.
        /// </summary>
        /// <param name="colorCode"></param>
        /// <returns></returns>
        public ValueTask<Stream> GetColorImage(int colorCode);
    }
}
