using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un1ver5e.Bot.BoardGames.Catan.Entities
{
    public class ResourcePack
    {
        private int[] resources = new int[5];
        public int this[Resource resource] => resources[((int)resource)];

        /// <summary>
        /// Defines whether pack has <paramref name="amount"/> of <paramref name="resource"/>.
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool Has(Resource resource, int amount) =>
            amount > 0 ?
            resources[((int)resource)] >= amount :
            throw new ArgumentException("Must be above zero!", nameof(amount));

        /// <summary>
        /// Adds <paramref name="amount"/> resources to the pack.
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="amount"></param>
        /// <returns>The amount of resources after the operation.</returns>
        public int Add(Resource resource, int amount) => 
            amount > 0 ?
            resources[((int)resource)] += amount :
            throw new ArgumentException("Must be above zero!", nameof(amount));

        /// <summary>
        /// Takes <paramref name="amount"/> of <paramref name="resource"/> from this stack.
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="amount"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>The amount of resources after the operation.</returns>
        public int Take(Resource resource, int amount) =>
            Has(resource, amount) ?
            resources[((int)resource)] -= amount :
            throw new ArgumentException("Must be above zero!", nameof(amount));
    }
    /// <summary>
    /// Represents a specific resource type inside a <see cref="ResourcePack"/>. This is used as an index, so do not combine them.
    /// </summary>
    public enum Resource
    {
        Clay = 0,
        Lumber = 1,
        Ore = 2,
        Sheep = 3,
        Wheat = 4
    }
}
