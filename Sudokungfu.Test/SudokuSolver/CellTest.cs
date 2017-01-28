using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver
{
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;

    /// <summary>
    /// Test class for <see cref="Cell"/>
    /// </summary>
    [TestClass]
    public class CellTest
    {
        /// <summary>
        /// Test that cell is initialized correctly.
        /// </summary>
        [TestMethod]
        public void TestCellInitialize()
        {
            var expectedIndex = 45;
            var expectedValues = new List<int>() { 1, 2, 4, 5, 6, 7, 8, 9 };
            var cell = new Cell(expectedIndex);

            Assert.AreEqual(expectedIndex, cell.Index);
            Assert.IsTrue(expectedValues.SequenceEqual(expectedValues));
        }

        /// <summary>
        /// Test eliminating a possible value.
        /// </summary>
        [TestMethod]
        public void TestEliminatePossibleValue()
        {
            var expectedValues = new List<int>() { 1, 2, 4, 5, 6, 7, 8, 9 };
            var cell = new Cell(0);
            cell.EliminatePossibleValue(3);

            Assert.IsTrue(expectedValues.SequenceEqual(cell.GetPossibleValues()));
        }

        /// <summary>
        /// Test eliminating a possible value that has already been eliminated.
        /// </summary>
        [TestMethod]
        public void TestAlreadyEliminatedPossibleValue()
        {
            var expectedValues = new List<int>() { 1, 2, 4, 5, 6, 7, 8, 9 };
            var cell = new Cell(0);
            cell.EliminatePossibleValue(3);
            cell.EliminatePossibleValue(3);

            Assert.IsTrue(expectedValues.SequenceEqual(cell.GetPossibleValues()));
        }

        /// <summary>
        /// Test inserting a value clear the possible values.
        /// </summary>
        [TestMethod]
        public void TestInsertValueClearsPossibleValues()
        {
            var cell = new Cell(0);
            cell.InsertValue(4);

            Assert.IsFalse(cell.GetPossibleValues().Any());
        }

        /// <summary>
        /// Test inserting a value eliminates possible values in all three sets.
        /// </summary>
        [TestMethod]
        public void TestInsertValueEliminatesPossiblesInSet()
        {
            var expectedValues = new List<int>() { 1, 2, 4, 5, 6, 7, 8, 9 };
            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(9), new Cell(10) };

            var row = new Row(cells, 0);
            var col = new Column(cells, 0);
            var box = new Box(cells, 0);

            cells[0].InsertValue(3);

            Assert.IsFalse(row.Cells.First().GetPossibleValues().Any());
            Assert.IsTrue(expectedValues.SequenceEqual(row.Cells.Last().GetPossibleValues()));
            Assert.IsFalse(col.Cells.First().GetPossibleValues().Any());
            Assert.IsTrue(expectedValues.SequenceEqual(col.Cells.Last().GetPossibleValues()));
            Assert.IsFalse(box.Cells.First().GetPossibleValues().Any());
            Assert.IsTrue(expectedValues.SequenceEqual(box.Cells.Last().GetPossibleValues()));
        }
    }
}
