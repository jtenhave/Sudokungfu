using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver
{
    using Sudokungfu.Extensions;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;
    using Sudokungfu.SudokuSolver.Techniques;

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

            var value = FoundValue.CreateGivenValue(testIndex, testValue);

            Assert.AreEqual(testIndex, value.Index);
            Assert.AreEqual(1, value.Indexes.Count());
            Assert.AreEqual(testIndex, value.Indexes.First());
            Assert.AreEqual(testValue, value.Value);
            Assert.AreEqual(0, value.Techniques.Count());
        }

        [TestMethod]
        public void TestCreateFoundInSetValue()
        {
            var testValue = 8;
            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(9), new Cell(10) };
            var box = new Box(cells, 0);

            var value = FoundValue.CreateFoundInSetValue(cells[0], testValue, box);

            Assert.AreEqual(cells[0].Index, value.Index);
            Assert.IsTrue(cells.Select(c => c.Index).SequenceEqual(value.Indexes));
            Assert.AreEqual(testValue, value.Value);
        }

        [TestMethod]
        public void TestFoundInSetValueHasAllOccupiedTechniques()
        {
            var testValue = 8;
            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(9), new Cell(10) };
            var box = new Box(cells, 0);
            var expectedTechniques = new ITechnique[]
            {
                BasicTechnique.CreateOccupiedTechnique(testValue, cells[1].Index),
                BasicTechnique.CreateOccupiedTechnique(testValue, cells[2].Index),
                BasicTechnique.CreateOccupiedTechnique(testValue, cells[3].Index)
            };

            cells[1].EliminatePossibleValue(testValue, expectedTechniques[0]);
            cells[2].EliminatePossibleValue(testValue, expectedTechniques[1]);
            cells[3].EliminatePossibleValue(testValue, expectedTechniques[2]);

            var value = FoundValue.CreateFoundInSetValue(cells[0], testValue, box);

            Assert.AreEqual(3, value.TechniqueCount);
            Assert.IsTrue(value.Techniques.Contains(expectedTechniques[0]));
            Assert.IsTrue(value.Techniques.Contains(expectedTechniques[1]));
            Assert.IsTrue(value.Techniques.Contains(expectedTechniques[2]));
        }

        [TestMethod]
        public void TestFoundInSetValueIgnoresExtraTechniquesWhenAllCellsOccupied()
        {
            var testValue = 8;
            var testRequiredIndex = 12;
            var testOptionalIndex = 27;
            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(9), new Cell(10), new Cell(12), new Cell(27) };
            var box = new Box(cells, 0);
            var row = new Row(cells, 1);
            var col = new Column(cells, 0);

            var occupiedTechA = BasicTechnique.CreateOccupiedTechnique(testValue, cells[1].Index);
            var occupiedTechB = BasicTechnique.CreateOccupiedTechnique(testValue, cells[2].Index);
            var occupiedTechC = BasicTechnique.CreateOccupiedTechnique(testValue, cells[3].Index);
            var requiredTech = BasicTechnique.CreateSetTechnique(testValue, testRequiredIndex, row.Indexes());
            var optionalTech = BasicTechnique.CreateSetTechnique(testValue, testOptionalIndex, col.Indexes());

            var expectedTechs = new List<ITechnique>() { occupiedTechA, occupiedTechB, occupiedTechC };

            cells[1].EliminatePossibleValue(testValue, occupiedTechA);
            cells[2].EliminatePossibleValue(testValue, requiredTech);
            cells[3].EliminatePossibleValue(testValue, requiredTech);
            cells[2].EliminatePossibleValue(testValue, optionalTech);
            cells[2].EliminatePossibleValue(testValue, occupiedTechB);
            cells[3].EliminatePossibleValue(testValue, occupiedTechC);

            var value = FoundValue.CreateFoundInSetValue(cells[0], testValue, box);

            Assert.IsTrue(expectedTechs.SetEqual(value.Techniques));
        }

        [TestMethod]
        public void TestFoundInSetValueIgnoresHigherComplexityTechniques()
        {
            var testValue = 8;
            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(9), new Cell(10), new Cell(12) };
            var box = new Box(cells, 0);
            var row = new Row(cells, 1);
            var lowAffectTech = BasicTechnique.CreateSetTechnique(testValue, 12, row.Indexes());
            var highAffectTech = TestTechnique.CreateTestTechnique(2, 1, 9, 10);
            var optionalTech = TestTechnique.CreateTestTechnique(1, 1);

            var expectedTechs = new List<ITechnique>() { optionalTech, lowAffectTech };

            cells[1].EliminatePossibleValue(testValue, highAffectTech);
            cells[2].EliminatePossibleValue(testValue, highAffectTech);
            cells[3].EliminatePossibleValue(testValue, highAffectTech);
            cells[1].EliminatePossibleValue(testValue, optionalTech);
            cells[2].EliminatePossibleValue(testValue, lowAffectTech);
            cells[3].EliminatePossibleValue(testValue, lowAffectTech);

            var value = FoundValue.CreateFoundInSetValue(cells[0], testValue, box);

            Assert.IsTrue(expectedTechs.SetEqual(value.Techniques));
        }

        [TestMethod]
        public void TestFoundInSetValueHasOnlyRequiredTechniques()
        {
            var testValue = 8;
            var testRequiredIndex = 12;
            var testOptionalIndex = 27;
            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(9), new Cell(10), new Cell(12), new Cell(27) };
            var box = new Box(cells, 0);
            var row = new Row(cells, 1);
            var col = new Column(cells, 0);

            var occupiedTech = BasicTechnique.CreateOccupiedTechnique(testValue, cells[1].Index);
            var requiredTech = BasicTechnique.CreateSetTechnique(testValue, testRequiredIndex, row.Indexes());
            var optionalTech = BasicTechnique.CreateSetTechnique(testValue, testOptionalIndex, col.Indexes());

            var expectedTechs = new List<ITechnique>() { occupiedTech, requiredTech };

            cells[1].EliminatePossibleValue(testValue, occupiedTech);
            cells[2].EliminatePossibleValue(testValue, requiredTech);
            cells[3].EliminatePossibleValue(testValue, requiredTech);
            cells[2].EliminatePossibleValue(testValue, optionalTech);

            var value = FoundValue.CreateFoundInSetValue(cells[0], testValue, box);

            Assert.IsTrue(expectedTechs.SetEqual(value.Techniques));
        }

        [TestMethod]
        public void TestFoundInSetValueLeftoverTechniquesTakenByCellsAffectedFirst()
        {
            var testValue = 8;
            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(9), new Cell(10), new Cell(18)};
            var box = new Box(cells, 0);

            var lowAffectTech = TestTechnique.CreateTestTechnique(1, 9, 10);           
            var highAffectTech = TestTechnique.CreateTestTechnique(2, 1, 9, 10, 18);
            var optionalTech = TestTechnique.CreateTestTechnique(2, 1, 18);

            var expectedTechs = new List<ITechnique>() { highAffectTech };

            cells[1].EliminatePossibleValue(testValue, highAffectTech);
            cells[2].EliminatePossibleValue(testValue, highAffectTech);
            cells[3].EliminatePossibleValue(testValue, highAffectTech);
            cells[4].EliminatePossibleValue(testValue, highAffectTech);
            cells[1].EliminatePossibleValue(testValue, optionalTech);
            cells[2].EliminatePossibleValue(testValue, lowAffectTech);
            cells[3].EliminatePossibleValue(testValue, lowAffectTech);
            cells[4].EliminatePossibleValue(testValue, optionalTech);

            var value = FoundValue.CreateFoundInSetValue(cells[0], testValue, box);

            Assert.IsTrue(expectedTechs.SetEqual(value.Techniques));
        }

        [TestMethod]
        public void TestFoundInSetValueLeftoverTechniquesTakenByComplexitySecond()
        {
            var testValue = 8;
            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(9), new Cell(10), new Cell(18) };
            var box = new Box(cells, 0);

            var lowComplexityTech = TestTechnique.CreateTestTechnique(1, 1, 9, 10, 18);
            var highComplexityTech = TestTechnique.CreateTestTechnique(2, 1, 9, 10, 18);

            var expectedTechs = new List<ITechnique>() { lowComplexityTech };

            EliminatePossibleValues(highComplexityTech, cells, testValue, 1, 2, 3, 4);
            EliminatePossibleValues(lowComplexityTech, cells, testValue, 1, 2, 3, 4);

            var value = FoundValue.CreateFoundInSetValue(cells[0], testValue, box);

            Assert.IsTrue(expectedTechs.SetEqual(value.Techniques));
        }

        [TestMethod]
        public void TestFoundInSetValueHandlesMutuallyExclusiveTechniques()
        {
            var testValue = 8;
            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(9), new Cell(10) };
            var box = new Box(cells, 0);
            var row = new Row(cells, 1);

            var techniqueA = TestTechnique.CreateTestTechnique(1, 1);
            var techniqueB = TestTechnique.CreateTestTechnique(1, 9);
            var techniqueC = TestTechnique.CreateTestTechnique(1, 1, 9);
            var techniqueD = TestTechnique.CreateTestTechnique(1, 10);
            var techniqueE = TestTechnique.CreateTestTechnique(1, 10);

            var expectedTechs = new List<ITechnique>() { techniqueC, techniqueD };

            cells[1].EliminatePossibleValue(testValue, techniqueA);
            cells[2].EliminatePossibleValue(testValue, techniqueB);
            cells[1].EliminatePossibleValue(testValue, techniqueC);
            cells[2].EliminatePossibleValue(testValue, techniqueC);
            cells[3].EliminatePossibleValue(testValue, techniqueD);
            cells[3].EliminatePossibleValue(testValue, techniqueE);

            var value = FoundValue.CreateFoundInSetValue(cells[0], testValue, box);

            Assert.IsTrue(expectedTechs.SequenceEqual(value.Techniques));
        }

        [TestMethod]
        public void TestCreateOnlyPossiblValue()
        {
            var testValue = 8;
            var cell = new Cell(34);
            var expectedTechs = new List<ITechnique>();

            foreach (var v in Constants.ALL_VALUES.Except(testValue))
            {
                var tech = BasicTechnique.CreateOccupiedTechnique(v, v);
                cell.EliminatePossibleValue(v, tech);
                expectedTechs.Add(tech);
            }

            var value = FoundValue.CreateOnlyPossiblValue(cell, testValue);

            Assert.AreEqual(cell.Index, value.Index);
            Assert.AreEqual(1, value.Indexes.Count());
            Assert.AreEqual(cell.Index, value.Indexes.First());
            Assert.AreEqual(testValue, value.Value);
            Assert.IsTrue(expectedTechs.SequenceEqual(value.Techniques));
        }
    }   
}
