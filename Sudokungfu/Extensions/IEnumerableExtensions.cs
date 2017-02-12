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
        /// Produces the set difference of an enumerable and a value.
        /// </summary>
        /// <param name="enumerable">Enumerable of the original set.</param>
        /// <param name="value">Value to exclude from the set.</param>
        public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, T value)
        {
            return enumerable.Except(value.ToEnumerable());
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
