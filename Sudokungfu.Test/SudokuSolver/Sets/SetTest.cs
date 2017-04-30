using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.Sets
{
    using Sudokungfu.SudokuSolver.Sets;

    /// <summary>
    /// Test class for <see cref="Set"/>
    /// </summary>
    [TestClass]
    public class SetTest : BaseTest
    {
        /*[TestMethod]
        public void TestPossibleSpots()
        {
            var testValue = 6;
            var cells = GetAllCells().ToList();
            var row = new Row(cells, 0);
            var expectedIndexes = new List<int>() { 2, 5, 6, 8 };

            var technique = new TestModel();
            EliminatePossibleValues(technique, cells, testValue, 0, 1, 3, 4, 7);

            Assert.IsTrue(expectedIndexes.SequenceEqual(row.PossibleSpots[testValue].Select(c => c.Index)));
        }*/
    }
}
