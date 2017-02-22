using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Sudokungfu.Test
{
    using Sudokungfu.Extensions;
    using Sudokungfu.SudokuSolver;
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
        /// Compares two <see cref="ITechnique"/> and asserts they are equal.
        /// </summary>
        /// <param name="expected">Expected technique.</param>
        /// <param name="actual">Actual technique.</param>
        public static void AssertITechniqueEqual(ITechnique expected, ITechnique actual)
        {
            Assert.AreEqual(expected.Complexity, actual.Complexity);
            Assert.AreEqual(expected.UsesFoundValues, actual.UsesFoundValues);

            if (expected.IndexValueMap == null)
            {
                Assert.IsNull(actual.IndexValueMap);
            }
            else
            {
                Assert.IsNotNull(actual.IndexValueMap);
                Assert.IsNotNull(actual.IndexValueMap.Values);

                Assert.IsTrue(actual.IndexValueMap.Keys.SetEqual(expected.IndexValueMap.Keys));

                foreach (var index in expected.IndexValueMap.Keys)
                {
                    Assert.IsTrue(actual.IndexValueMap[index].SetEqual(expected.IndexValueMap[index]));
                }
            }
        }

        /// <summary>
        /// Eliminates a value from a group of cells using a technique.
        /// </summary>
        /// <param name="technique">Technique to use.</param>
        /// <param name="cells">Cells in the Sudoku.</param>
        /// <param name="value">Value to eliminate.</param>
        /// <param name="indexes">Indexes of the cells to eliminate the value from.</param>
        public static void EliminatePossibleValues(ITechnique technique, List<Cell> cells, int value, params int[] indexes)
        {
            foreach (var i in indexes)
            {
                cells[i].EliminatePossibleValue(value, technique);
            }
        }
    }
}
