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
        /// Creates a dictionary from an enumerable with all items as keys and one key with a value.
        /// </summary>
        /// <param name="key">Key with a value.</param>
        /// <param name="value">Value.</param>
        public static IDictionary<int, IEnumerable<int>> ToDictionary(this IEnumerable<int> enumerable, int key, int value)
        {
            return enumerable.ToDictionary(i => i, i => i == key ? value.ToEnumerable() : Enumerable.Empty<int>());
        }

        /// <summary>
        /// Produces a sequence by combining two sequences together like a zipper.
        /// </summary>
        /// <param name="first">First sequences.</param>
        /// <param name="second">Second sequence.</param>
        public static IEnumerable<T> Zipper<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            var firstEnumerator = first.GetEnumerator();
            var secondEnumerator = second.GetEnumerator();

            var firstHasValue = false;
            var secondHasValue = false;

            Func<bool> valueLeft = () =>
            {
                firstHasValue = firstEnumerator.MoveNext();
                secondHasValue = secondEnumerator.MoveNext();
                return firstHasValue || secondHasValue;
            };

            while (valueLeft())
            {
                if (firstHasValue)
                {
                    yield return firstEnumerator.Current;
                }

                if (secondHasValue)
                {
                    yield return secondEnumerator.Current;
                }
            }
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
