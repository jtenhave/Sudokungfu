using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Sudokungfu.Test.SudokuSolver.Sets
{
    using Sudokungfu.Extensions;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;

    /// <summary>
    /// Test class for <see cref="Box"/>
    /// </summary>
    [TestClass]
    public class BoxTest : BaseTest
    {
        [TestMethod]
        public void TestBoxCellPicking()
        {
            var expectedIndexes = new List<List<int>> ()
            {
                new List<int>() { 0, 1, 2, 9, 10, 11, 18, 19, 20 },
                new List<int>() { 3, 4, 5, 12, 13, 14, 21, 22, 23 },
                new List<int>() { 6, 7, 8, 15, 16, 17, 24, 25, 26 },
                new List<int>() { 27, 28, 29, 36, 37, 38, 45, 46, 47 },
                new List<int>() { 30, 31, 32, 39, 40, 41, 48, 49, 50 },
                new List<int>() { 33, 34, 35, 42, 43, 44, 51, 52, 53 },
                new List<int>() { 54, 55, 56, 63, 64, 65, 72, 73, 74 },
                new List<int>() { 57, 58, 59, 66, 67, 68, 75, 76, 77 },
                new List<int>() { 60, 61, 62, 69, 70, 71, 78, 79, 80 }
            };

            var cells = GetAllCells();
            for (int i = 0; i < Constants.SET_SIZE; i++)
            {
                var box = new Box(cells, i);
                Assert.IsTrue(box.Indexes().SetEqual(expectedIndexes[i]));
            }
        }
    }
}
