using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver
{
    using Extensions;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;

    /// <summary>
    /// Test class for <see cref="Cell"/>
    /// </summary>
    [TestClass]
    public class CellTest
    {
        [TestMethod]
        public void TestCellInitialize()
        {
            var expectedIndex = 45;
            var expectedValues = Constants.ALL_VALUES;

            var cell = new Cell(expectedIndex);

            Assert.AreEqual(expectedIndex, cell.Index);
            Assert.IsTrue(cell.PossibleValues.SequenceEqual(expectedValues));
            Assert.AreEqual(expectedValues.Count(), cell.EliminationTechniques.Count);

            foreach (var value in expectedValues)
            {
                Assert.IsTrue(cell.EliminationTechniques.ContainsKey(value));
                Assert.IsFalse(cell.EliminationTechniques[value].Any());
            }
        }

        [TestMethod]
        public void TestEliminatePossibleValue()
        {
            var testValue = 3;
            var expectedValues = Constants.ALL_VALUES.Except(testValue);

            var cell = new Cell(0);
            cell.EliminatePossibleValue(testValue, new TestEliminationTechnique(0));

            Assert.IsTrue(expectedValues.SequenceEqual(cell.PossibleValues));
        }

        [TestMethod]
        public void TestEliminationTechniqueAdded()
        {
            var testValue = 3;
            var expectedEliminationTechnique = new TestEliminationTechnique(0);
            var expectedValues = Constants.ALL_VALUES.Except(testValue);

            var cell = new Cell(0);
            cell.EliminatePossibleValue(testValue, expectedEliminationTechnique);

            Assert.AreEqual(1, cell.EliminationTechniques[testValue].Count());
            Assert.AreEqual(expectedEliminationTechnique, cell.EliminationTechniques[testValue].First());
        }

        [TestMethod]
        public void TestHigherComplexityEliminationTechniqueIgnored()
        {
            var testValue = 3;
            var expectedEliminationTechniqueA = new TestEliminationTechnique(0);
            var expectedEliminationTechniqueB = new TestEliminationTechnique(1);
            var expectedValues = Constants.ALL_VALUES.Except(testValue);

            var cell = new Cell(0);
            cell.EliminatePossibleValue(testValue, expectedEliminationTechniqueA);
            cell.EliminatePossibleValue(testValue, expectedEliminationTechniqueB);

            Assert.AreEqual(1, cell.EliminationTechniques[testValue].Count());
            Assert.AreEqual(expectedEliminationTechniqueA, cell.EliminationTechniques[testValue].First());
        }

        [TestMethod]
        public void TestLowerComplexityEliminationTechniqueOverwrites()
        {
            var testValue = 3;
            var expectedEliminationTechniqueA = new TestEliminationTechnique(0);
            var expectedEliminationTechniqueB = new TestEliminationTechnique(1);
            var expectedValues = Constants.ALL_VALUES.Except(testValue);

            var cell = new Cell(0);
            cell.EliminatePossibleValue(testValue, expectedEliminationTechniqueB);
            cell.EliminatePossibleValue(testValue, expectedEliminationTechniqueA);

            Assert.AreEqual(1, cell.EliminationTechniques[testValue].Count());
            Assert.AreEqual(expectedEliminationTechniqueA, cell.EliminationTechniques[testValue].First());
        }

        [TestMethod]
        public void TestEqualComplexityEliminationTechniqueIsAdded()
        {
            var testValue = 3;
            var expectedEliminationTechniqueA = new TestEliminationTechnique(0);
            var expectedEliminationTechniqueB = new TestEliminationTechnique(0);
            var expectedValues = Constants.ALL_VALUES.Except(testValue);

            var cell = new Cell(0);
            cell.EliminatePossibleValue(testValue, expectedEliminationTechniqueA);
            cell.EliminatePossibleValue(testValue, expectedEliminationTechniqueB);

            Assert.AreEqual(2, cell.EliminationTechniques[testValue].Count());
            Assert.AreEqual(expectedEliminationTechniqueA, cell.EliminationTechniques[testValue].First());
            Assert.AreEqual(expectedEliminationTechniqueB, cell.EliminationTechniques[testValue].Last());
        }

        [TestMethod]
        public void TestEliminationTechniqueNotAddedAfterCellFilled()
        {
            var testValue = 3;
            var expectedEliminationTechniqueA = new TestEliminationTechnique(1);
            var expectedEliminationTechniqueB = new TestEliminationTechnique(0);
         
            var cell = new Cell(0);
            foreach (var value in Constants.ALL_VALUES)
            {
                cell.EliminatePossibleValue(value, expectedEliminationTechniqueA);
            }

            cell.EliminatePossibleValue(testValue, expectedEliminationTechniqueB);

            Assert.AreEqual(1, cell.EliminationTechniques[testValue].Count());
            Assert.AreEqual(expectedEliminationTechniqueA, cell.EliminationTechniques[testValue].First());
        }

        [TestMethod]
        public void TestInsertValueEliminatesAllOtherValues()
        {
            var testValue = 3;
            var testIndex = 23;

            var cell = new Cell(testIndex);
            cell.InsertValue(testValue);

            foreach (var value in Constants.ALL_VALUES.Except(testValue))
            {
                Assert.AreEqual(1, cell.EliminationTechniques[value].Count());
                Assert.AreEqual(0, cell.EliminationTechniques[value].First().Complexity);
                Assert.AreEqual(1, cell.EliminationTechniques[value].First().Indexes.Count());
                Assert.AreEqual(testIndex, cell.EliminationTechniques[value].First().Indexes.First());
            }

            Assert.AreEqual(0, cell.EliminationTechniques[testValue].Count());
        }

        [TestMethod]
        public void TestInsertValuePreventsAddingEliminationTechnique()
        {
            var testValue = 3;
            var testIndex = 23;

            var cell = new Cell(testIndex);
            cell.InsertValue(5);

            var expectedEliminationTechnique = new TestEliminationTechnique(0);
            cell.EliminatePossibleValue(testValue, expectedEliminationTechnique);

            Assert.AreEqual(1, cell.EliminationTechniques[testValue].Count());
            Assert.AreEqual(0, cell.EliminationTechniques[testValue].First().Complexity);
            Assert.AreEqual(1, cell.EliminationTechniques[testValue].First().Indexes.Count());
            Assert.AreEqual(testIndex, cell.EliminationTechniques[testValue].First().Indexes.First());
        }

        [TestMethod]
        public void TestInsertValueEliminatesAllOtherValuesInMemberSets()
        {
            var testValue = 4;
            var expectedValues = Constants.ALL_VALUES.Except(testValue);
            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(9), new Cell(10) };

            var row = new Row(cells, 0);
            var col = new Column(cells, 0);
            var box = new Box(cells, 0);

            cells[0].InsertValue(testValue);

            Assert.IsFalse(row.Cells.First().PossibleValues.Any());
            Assert.IsTrue(expectedValues.SequenceEqual(row.Cells.Last().PossibleValues));
            Assert.IsFalse(col.Cells.First().PossibleValues.Any());
            Assert.IsTrue(expectedValues.SequenceEqual(col.Cells.Last().PossibleValues));
            Assert.IsFalse(box.Cells.First().PossibleValues.Any());
            Assert.IsTrue(expectedValues.SequenceEqual(box.Cells.Last().PossibleValues));
        }
    }
}
