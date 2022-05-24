using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un1ver5e.Bot.Services.Database
{
    public interface IDatabaseEntity
    {
        /// <summary>
        /// Tries to update current <see cref="IDatabaseEntity"/> using <paramref name="dbService"/>.
        /// </summary>
        /// <param name="dbService"></param>
        /// <returns></returns>
        public ValueTask Pull(DatabaseService dbService);

        /// <summary>
        /// Saves current <see cref="IDatabaseEntity"/> using <paramref name="dbService"/>.
        /// </summary>
        /// <param name="dbService"></param>
        /// <returns></returns>
        public ValueTask Push(DatabaseService dbService);
    }
}
