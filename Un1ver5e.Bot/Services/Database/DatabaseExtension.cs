using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Un1ver5e.Bot.Services.Database.Entities.Abstract;

namespace Un1ver5e.Bot.Services.Database
{
    public static class Extensions
    {
        /// <summary>
        /// Tries to find a <typeparamref name="T"/> object within <paramref name="dbctx"/>, if none is found then new <typeparamref name="T"/> with specified <paramref name="id"/> is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbset"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T FindOrCreate<T>(this DbContext dbctx, ulong id)
            where T: UserData, new() => dbctx.Find<T>(id) ?? new T() { UserId = id };


        /// <summary>
        /// Tries to find a <typeparamref name="T"/> object within <paramref name="dbctx"/>, if none is found then new <typeparamref name="T"/> with specified <paramref name="id"/> is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbset"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async ValueTask<T> FindOrCreateAsync<T>(this DbContext dbctx, ulong id)
            where T : UserData, new() => await dbctx.FindAsync<T>(id) ?? new T() { UserId = id };

    }
}
