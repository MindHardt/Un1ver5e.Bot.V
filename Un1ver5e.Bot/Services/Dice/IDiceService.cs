using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un1ver5e.Bot.Services.Dice
{
    public interface IDiceService
    {
        public IThrowResult ThrowByQuery(string query);
    }
}
