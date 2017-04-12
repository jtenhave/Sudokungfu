using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver
{
    using Sudokungfu.Extensions;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;

    /// <summary>
    /// Test class for <see cref="FoundValue"/>.
    /// </summary>
    [TestClass]
    public class FoundValueTest : BaseTest
    {
        [TestMethod]
        public void TestCreateGivenValue()
        {
            var testIndex = 45;
            var testValue = 8;
            var expectedValue = new TestSudokuModel()
            {
                IndexValueMap = testIndex.ToDictionary(testValue)
            };

            var actualValue = FoundValue.CreateGivenValue(testIndex, testValue);

            AssertISudokuModelEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestCreateFoundInSetValue()
        {
            var testValue = 8;
            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(9), new Cell(10) };
            var expectedCell = cells[0];
            var box = new Box(cells, 0);
            var expectedIndexValueMap = cells.Indexes().ToDictionary(expectedCell.Index, testValue);
            var expectedValue = new TestSudokuModel()
            {
                IndexValueMap = expectedIndexValueMap,
                ClickableModel = new TestSudokuModel()
                {
                    IndexValueMap = expectedIndexValueMap
                }
            };

            var actualValue = FoundValue.CreateFoundInSetValue(expectedCell, testValue, box);

            AssertISudokuModelEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestCreateOnlyPossiblValue()
        {
            var testValue = 8;
            var cell = new Cell(34);
            var expectedIndexValueMap = cell.Index.ToDictionary(testValue);
            var expectedValue = new TestSudokuModel()
            {
                IndexValueMap = expectedIndexValueMap,
                ClickableModel = new TestSudokuModel()
                {
                    IndexValueMap = expectedIndexValueMap
                }
            };

            var actualValue = FoundValue.CreateOnlyPossiblValue(cell, testValue);

            AssertISudokuModelEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestFoundSetValueOnlySelectsUsableTechniquess()
        {
            var testValue = 8;
            var cells = new List<Cell>() { new Cell(0), new Cell(1) };
            var box = new Box(cells, 0);
            var unusableTechnique = new TestTechnique()
            {
                AffectedIndexes = new List<int>() { 23 },
                Complexity = 0
            };
            var expectedTechnique = new TestTechnique()
            {
                AffectedIndexes = new List<int>() { 1 },
                Complexity = 0
            };
            
            cells[1].EliminatePossibleValue(testValue, unusableTechnique);
            cells[1].EliminatePossibleValue(testValue, expectedTechnique);
            var value = FoundValue.CreateFoundInSetValue(cells[0], testValue, box);

            Assert.AreEqual(1, value.TechniqueCount);
            AssertITechniqueEqual(expectedTechnique, value.Techniques.First());
        }

        [TestMethod]
        public void TestFoundSetValueSelectsTechniquesByComplexityFirst()
        {
            var testValue = 8;
            var cells = new List<Cell>() { new Cell(0), new Cell(1) };
            var box = new Box(cells, 0);
            var unusedTechnique = new TestTechnique()
            {
                AffectedIndexes = new List<int>() { 1 },
                Complexity = 1
            };
            var expectedTechnique = new TestTechnique()
            {
                AffectedIndexes = new List<int>() { 1 },
                Complexity = 0
            };

            cells[1].EliminatePossibleValue(testValue, unusedTechnique);
            cells[1].EliminatePossibleValue(testValue, expectedTechnique);
            var value = FoundValue.CreateFoundInSetValue(cells[0], testValue, box);

            Assert.AreEqual(1, value.TechniqueCount);
            AssertITechniqueEqual(expectedTechnique, value.Techniques.First());
        }

        [TestMethod]
        public void TestFoundSetValueSelectsTechniquesByUniquenessSecond()
        {
            var testValue = 8;
            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(2), new Cell(3) };
            var row = new Row(cells, 0);
            var expectedTechniqueA = new TestTechnique()
            {
                AffectedIndexes = new List<int>() { 1, 3 },
                Complexity = 0
            };
            var unusedTechnique = new TestTechnique()
            {
                AffectedIndexes = new List<int>() { 1, 3, 5, 6, 7 },
                Complexity = 0
            };
            var expectedTechniqueB = new TestTechnique()
            {
                AffectedIndexes = new List<int>() { 1, 2 },
                Complexity = 0
            };

            cells[1].EliminatePossibleValue(testValue, expectedTechniqueA);
            cells[1].EliminatePossibleValue(testValue, unusedTechnique);
            cells[1].EliminatePossibleValue(testValue, expectedTechniqueB);
            var value = FoundValue.CreateFoundInSetValue(cells[0], testValue, row);

            Assert.AreEqual(2, value.TechniqueCount);
            AssertITechniqueEqual(expectedTechniqueB, value.Techniques.First());
            AssertITechniqueEqual(expectedTechniqueA, value.Techniques.Last());
        }

        [TestMethod]
        public void TestFoundSetValueSelectsTechniquesByAffectedIndexesThird()
        {
            var testValue = 8;
            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(2), new Cell(3) };
            var row = new Row(cells, 0);
            var unusedTechniqueA = new TestTechnique()
            {
                AffectedIndexes = new List<int>() { 1, 3 },
                Complexity = 0
            };
            var unusedTechniqueB = new TestTechnique()
            {
                AffectedIndexes = new List<int>() { 1, 2 },
                Complexity = 0
            };
            var expectedTechnique = new TestTechnique()
            {
                AffectedIndexes = new List<int>() { 1, 2, 3 },
                Complexity = 0
            };

            cells[1].EliminatePossibleValue(testValue, unusedTechniqueA);
            cells[1].EliminatePossibleValue(testValue, unusedTechniqueB);
            cells[1].EliminatePossibleValue(testValue, expectedTechnique);
            var value = FoundValue.CreateFoundInSetValue(cells[0], testValue, row);

            Assert.AreEqual(1, value.TechniqueCount);
            AssertITechniqueEqual(expectedTechnique, value.Techniques.First());
        }

        [TestMethod]
        public void TestFoundSetValueSelectsTechniquesWhileTechniquesRequired()
        {
            var testValue = 8;
            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(2) };
            var row = new Row(cells, 0);
            var expectedTechnique = new TestTechnique()
            {
                AffectedIndexes = new List<int>() { 1, 2 },
                Complexity = 0
            };
            var unusedTechnique = new TestTechnique()
            {
                AffectedIndexes = new List<int>() { 1, 2 },
                Complexity = 0
            };

            cells[1].EliminatePossibleValue(testValue, expectedTechnique);
            cells[1].EliminatePossibleValue(testValue, unusedTechnique);
            var value = FoundValue.CreateFoundInSetValue(cells[0], testValue, row);

            Assert.AreEqual(1, value.TechniqueCount);
            AssertITechniqueEqual(expectedTechnique, value.Techniques.First());
        }
    }   
}
