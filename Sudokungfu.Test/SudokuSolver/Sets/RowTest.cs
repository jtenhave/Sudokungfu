using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Sudokungfu.Test.SudokuSolver.Sets
{
    using Sudokungfu.Extensions;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;

    /// <summary>
    /// Test class for <see cref="Row"/>
    /// </summary>
    [TestClass]
    public class RowTest : BaseTest
    {
        [TestMethod]
        public void TestRowCellPicking()
        {
            var expectedIndexes = new List<List<int>>()
            {
                new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 },
                new List<int>() { 9, 10, 11, 12, 13, 14, 15, 16, 17, },
                new List<int>() { 18, 19, 20, 21, 22, 23, 24, 25, 26 },
                new List<int>() { 27, 28, 29, 30, 31, 32, 33, 34, 35 },
                new List<int>() { 36, 37, 38, 39, 40, 41, 42, 43, 44 },
                new List<int>() { 45,46, 47, 48, 49, 50, 51, 52, 53 },
                new List<int>() { 54, 55, 56, 57, 58, 59, 60, 61, 62 },
                new List<int>() { 63, 64, 65, 66, 67, 68, 69, 70, 71 },
                new List<int>() { 72, 73, 74, 75, 76, 77, 78, 79, 80 }
            };

            var cells = GetAllCells();
            for (int i = 0; i < Constants.SET_SIZE; i++)
            {
                var row = new Row(cells, i);
                Assert.IsTrue(row.Indexes.SetEqual(expectedIndexes[i]));
            }
        }
    }
}
