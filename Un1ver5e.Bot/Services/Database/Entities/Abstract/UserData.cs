using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un1ver5e.Bot.Services.Database.Entities.Abstract
{
    public abstract class UserData
    {
        /// <summary>
        /// The discord user Id converted to <see cref="ulong"/>.
        /// </summary>
        public ulong UserId { get; internal init; }
    }
}
