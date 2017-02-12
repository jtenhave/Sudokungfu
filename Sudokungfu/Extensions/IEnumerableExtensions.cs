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

        /// <summary>
        /// Returns items from a sequence as long as a specified condition is true.
        /// </summary>
        /// <param name="enumerable">Enumerable to take items from</param>
        /// <param name="predicate">Function that returns true if item should be taken. Takes current item and taken items as input.</param>
        public static IEnumerable<T> TakeWhile<T>(this IEnumerable<T> enumerable, Func<T, IEnumerable<T>, bool> predicate)
        {
            var takenItems = new List<T>();
            foreach (var item in enumerable)
            {
                if (predicate(item, takenItems))
                {
                    takenItems.Add(item);
                }
                else
                {
                    break;
                }
            }

            return takenItems;
        }
    }
}
