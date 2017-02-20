using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Sudokungfu.Test.SudokuSolver.Sets
{
    using Sudokungfu.Extensions;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;

    /// <summary>
    /// Test class for <see cref="Column"/>
    /// </summary>
    [TestClass]
    public class ColumnTest : BaseTest
    {
        [TestMethod]
        public void TestColumnCellPicking()
        {
            var expectedIndexes = new List<List<int>>()
            {
                new List<int>() { 0, 9, 18, 27, 36, 45, 54, 63, 72 },
                new List<int>() { 1, 10, 19, 28, 37, 46, 55, 64, 73 },
                new List<int>() { 2, 11, 20, 29, 38, 47, 56, 65, 74 },
                new List<int>() { 3, 12, 21, 30, 39, 48, 57, 66, 75 },
                new List<int>() { 4, 13, 22, 31, 40, 49, 58, 67, 76 },
                new List<int>() { 5, 14, 23, 32, 41, 50, 59, 68, 77 },
                new List<int>() { 6, 15, 24, 33, 42, 51, 60, 69, 78 },
                new List<int>() { 7, 16, 25, 34, 43, 52, 61, 70, 79 },
                new List<int>() { 8, 17, 26, 35, 44, 53, 62, 71, 80 }
            };

            var cells = GetAllCells();
            for (int i = 0; i < Constants.SET_SIZE; i++)
            {
                var column = new Column(cells, i);
                Assert.IsTrue(column.Indexes().SetEqual(expectedIndexes[i]));
            }
        }
    }
}
