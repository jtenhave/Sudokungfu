using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.Sets
{
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;

    /// <summary>
    /// Test class for <see cref="Box"/>
    /// </summary>
    [TestClass]
    public class BoxTest
    {
        /// <summary>
        /// Test that boxes are created with the correct cells.
        /// </summary>
        [TestMethod]
        public void TestBoxCellPicking()
        {
            var expectedIndexes = new List<int> { 0, 1, 2, 9, 10, 11, 18, 19, 20, 3, 4, 5, 12, 13, 14, 21, 22, 23, 6, 7, 8, 15, 16, 17, 24, 25, 26, 27, 28, 29, 36, 37, 38, 45, 46, 47, 30, 31, 32, 39, 40, 41, 48, 49, 50, 33, 34, 35, 42, 43, 44, 51, 52, 53, 54, 55, 56, 63, 64, 65, 72, 73, 74, 57, 58, 59, 66, 67, 68, 75, 76, 77, 60, 61, 62, 69, 70, 71, 78, 79, 80 };
            var cells = new List<Cell>();
            for (int i = 0; i < Constants.CELL_COUNT; i++)
            {
                cells.Add(new Cell(i));
            }

            var boxes = new List<Box>();
            for (int i = 0; i < Constants.SET_SIZE; i++)
            {
                boxes.Add(new Box(cells, i));
            }

            var indexes = boxes.SelectMany(b => b.Cells).Select(c => c.Index);
            foreach (var box in boxes)
            {
                Assert.AreEqual(Constants.SET_SIZE, box.Cells.Count());
            }

            Assert.IsTrue(expectedIndexes.SequenceEqual(indexes));
        }
    }
}
