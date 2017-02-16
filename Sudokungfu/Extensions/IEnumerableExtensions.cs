using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Extensions
{
    /// <summary>
    /// Class for <see cref="IEnumerable{T}"/> extensions.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Iterates over each element and applies a function.
        /// </summary>
        /// <param name="func">Function to apply to each element.</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> func)
        {
            foreach (var item in enumerable)
            {
                func(item);
            }
        }

        /// <summary>
        /// Produces the set difference of an enumerable and an item in the enumerable.
        /// </summary>
        /// <param name="enumerable">Enumerable of the original set.</param>
        /// <param name="item">Item to exclude from the set.</param>
        /// <param name="items">More items to exclude from the set.</param>
        public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, T value, params T[] items)
        {
            return enumerable.Except(value.ToEnumerable().Concat(items));
        }

        /// <summary>
        /// Produces a single item enumerable out of an item.
        /// </summary>
        /// <param name="item">Item to create enumerable from.</param>
        public static IEnumerable<T> ToEnumerable<T>(this T item)
        {
            return new T[] { item };
        }
    }
}
