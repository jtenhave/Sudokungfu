using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test
{
    using Model;
    using Sudokungfu.Extensions;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;
    using Sudokungfu.SudokuSolver.Techniques;

    /// <summary>
    /// Base test class for all other tests.
    /// </summary>
    public class BaseTest
    {
        /// <summary>
        /// Gets all 81 cells.
        /// </summary>
        public static IEnumerable<Cell> GetAllCells()
        {
            var cells = new List<Cell>();
            for (int i = 0; i < Constants.CELL_COUNT; i++)
            {
                cells.Add(new Cell(i));
            }

            return cells;
        }

        /// <summary>
        /// Asserts two sequences are equal sets.
        /// </summary>
        /// <param name="first">First sequence.</param>
        /// <param name="second">Second sequence.</param>
        public static void AssertSetEqual<T>(IEnumerable<T> first, IEnumerable<T> second)
        {
            var secondList = second.ToList();
            Assert.AreEqual(first.Count(), second.Count(), "length");

            foreach (var item in first)
            {
                if (secondList.Contains(item))
                {
                    secondList.Remove(item);
                }
                else
                {
                    Assert.Fail($"Second enumerable does not contain element: {item}");
                }
            }

            if (secondList.Any())
            {
                Assert.Fail($"Second enumerable contained {secondList.Count} unexpected elements: {string.Join(", ", secondList)}");
            }
        }
    }
}
