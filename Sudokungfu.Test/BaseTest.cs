using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

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
            AssertISudokuModelEqual(expected, actual);
        }

        /// <summary>
        /// Compares two <see cref="ISudokuModel"/>s and asserts they are equal.
        /// </summary>
        /// <param name="expected">Expected model.</param>
        /// <param name="actual">Actual model.</param>
        public static void AssertISudokuModelEqual(ISudokuModel expected, ISudokuModel actual)
        {
            if (expected.IndexValueMap == null)
            {
                Assert.IsNull(actual.IndexValueMap);
            }
            else
            {
                Assert.IsNotNull(actual.IndexValueMap);

                Assert.IsTrue(actual.IndexValueMap.Keys.SetEqual(expected.IndexValueMap.Keys));
                foreach (var index in expected.IndexValueMap.Keys)
                {
                    Assert.IsTrue(actual.IndexValueMap[index].SetEqual(expected.IndexValueMap[index]));
                }
            }

            if (expected.AffectedIndexes == null)
            {
                Assert.IsNull(actual.AffectedIndexes);
            }
            else
            {
                Assert.IsNotNull(actual.AffectedIndexes);
                Assert.IsTrue(actual.AffectedIndexes.SetEqual(expected.AffectedIndexes));
            }

            Assert.AreEqual(expected.IsInputEnabled, actual.IsInputEnabled);
            Assert.AreEqual(expected.IsSolving, actual.IsSolving);

            if (expected.Details == null)
            {
                Assert.IsNull(actual.Details);
            }
            else
            {
                Assert.IsNotNull(actual.Details);
                
                var expectedDetails = expected.Details.ToList();
                var actualDetails = actual.Details.ToList();

                Assert.AreEqual(expectedDetails.Count, actualDetails.Count);
                for (int i = 0; i < expectedDetails.Count; i++)
                {
                    AssertISudokuModelEqual(expectedDetails[i], actualDetails[i]);
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
