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
        /// <param name="item">Item to exclude from the set.</param>
        /// <param name="items">More items to exclude from the set.</param>
        public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, T item, params T[] items)
        {
            return enumerable.Except(item.ToEnumerable().Concat(items));
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
        /// Compares two sequences and returns true if they are equal sets.
        /// </summary>
        /// <param name="first">First sequence.</param>
        /// <param name="second">Second sequence.</param>
        public static bool SetEqual<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            var secondList = second.ToList();
            if (first.Count() != second.Count())
            {
                return false;
            }

            foreach (var item in first)
            {
                if (secondList.Contains(item))
                {
                    secondList.Remove(item);
                }
                else
                {
                    return false;
                }
            }

            return !secondList.Any();
        }
    }
}
