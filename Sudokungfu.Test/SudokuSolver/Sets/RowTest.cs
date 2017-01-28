using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.Sets
{
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;

    /// <summary>
    /// Test class for <see cref="Row"/>
    /// </summary>
    [TestClass]
    public class RowTest
    {
        /// <summary>
        /// Test that rows are created with the correct cells.
        /// </summary>
        [TestMethod]
        public void TestRowCellPicking()
        {
            var expectedIndexes = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80 };
            var cells = new List<Cell>();
            for (int i = 0; i < Constants.CELL_COUNT; i++)
            {
                cells.Add(new Cell(i));
            }

            var rowes = new List<Row>();
            for (int i = 0; i < Constants.SET_SIZE; i++)
            {
                rowes.Add(new Row(cells, i));
            }

            var indexes = rowes.SelectMany(b => b.Cells).Select(c => c.Index);
            foreach (var row in rowes)
            {
                Assert.AreEqual(Constants.SET_SIZE, row.Cells.Count());
            }

            Assert.IsTrue(expectedIndexes.SequenceEqual(indexes));
        }
    }
}
