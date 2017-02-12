using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.Sets
{
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;

    /// <summary>
    /// Test class for <see cref="Set"/>
    /// </summary>
    [TestClass]
    public class SetTest
    {
        [TestMethod]
        public void TestGetPossibleValueSpots()
        {
            var testValue = 6;
            var expectedIndexes = new List<int>() { 2, 5, 6, 8 };
            var cells = new List<Cell>();
            for (int i = 0; i < Constants.SET_SIZE; i++)
            {
                cells.Add(new Cell(i));
            }

            var row = new Row(cells, 0);

            cells[0].EliminatePossibleValue(testValue, new TestEliminationTechnique(0));
            cells[1].EliminatePossibleValue(testValue, new TestEliminationTechnique(0));
            cells[3].EliminatePossibleValue(testValue, new TestEliminationTechnique(0));
            cells[4].EliminatePossibleValue(testValue, new TestEliminationTechnique(0));
            cells[7].EliminatePossibleValue(testValue, new TestEliminationTechnique(0));

            Assert.IsTrue(expectedIndexes.SequenceEqual(row.GetValuePossibleSpots()[testValue].Select(c => c.Index)));
        }
    }
}
