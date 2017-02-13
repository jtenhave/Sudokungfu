using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver
{
    using Extensions;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;

    /// <summary>
    /// Test class for <see cref="FoundValue"/>.
    /// </summary>
    [TestClass]
    public class FoundValueTest
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

            cells[1].EliminatePossibleValue(testValue, BasicEliminationTechnique.CreateOccupiedTechnique(testValue, cells[1].Index));
            cells[2].EliminatePossibleValue(testValue, BasicEliminationTechnique.CreateOccupiedTechnique(testValue, cells[2].Index));
            cells[3].EliminatePossibleValue(testValue, BasicEliminationTechnique.CreateOccupiedTechnique(testValue, cells[3].Index));

            var value = FoundValue.CreateFoundInSetValue(cells[0], testValue, box);

            Assert.IsTrue(cells.SelectMany(c => c.EliminationTechniques[testValue]).SequenceEqual(value.Techniques));
        }

        [TestMethod]
        public void TestFoundInSetValueIgnoresExtraTechniquesWhenAllCellsOccupied()
        {
            var testValue = 8;
            var testRequiredIndex = 12;
            var testOptionalIndex = 27;

            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(9), new Cell(10) };
            var box = new Box(cells, 0);
            var row = new Row(cells, 1);
            var col = new Column(cells, 0);

            var occupiedTechA = BasicEliminationTechnique.CreateOccupiedTechnique(testValue, cells[1].Index);
            var occupiedTechB = BasicEliminationTechnique.CreateOccupiedTechnique(testValue, cells[2].Index);
            var occupiedTechC = BasicEliminationTechnique.CreateOccupiedTechnique(testValue, cells[3].Index);
            var requiredTech = BasicEliminationTechnique.CreateSetTechnique(testValue, testRequiredIndex, row);
            var optionalTech = BasicEliminationTechnique.CreateSetTechnique(testValue, testOptionalIndex, col);

            var expectedTechs = new List<IEliminationTechnique>() { occupiedTechA, occupiedTechB, occupiedTechC };

            cells[1].EliminatePossibleValue(testValue, occupiedTechA);
            cells[2].EliminatePossibleValue(testValue, requiredTech);
            cells[3].EliminatePossibleValue(testValue, requiredTech);
            cells[2].EliminatePossibleValue(testValue, optionalTech);
            cells[2].EliminatePossibleValue(testValue, occupiedTechB);
            cells[3].EliminatePossibleValue(testValue, occupiedTechC);

            var value = FoundValue.CreateFoundInSetValue(cells[0], testValue, box);

            Assert.IsTrue(expectedTechs.SequenceEqual(value.Techniques));
        }

        [TestMethod]
        public void TestFoundInSetValueIgnoresHigherComplexityTechniques()
        {
            var testValue = 8;

            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(9), new Cell(10) };
            var box = new Box(cells, 0);
            var row = new Row(cells, 1);

            var lowAffectTech = BasicEliminationTechnique.CreateSetTechnique(testValue, 12, row);
            var highAffectTech = new TestEliminationTechnique(2)
            {
                Indexes = new List<int> { 1, 9, 10 }
            };

            var optionalTech = new TestEliminationTechnique(1)
            {
                Indexes = new List<int> { 1 }
            };

            var expectedTechs = new List<IEliminationTechnique>() { optionalTech, lowAffectTech };

            cells[1].EliminatePossibleValue(testValue, highAffectTech);
            cells[2].EliminatePossibleValue(testValue, highAffectTech);
            cells[3].EliminatePossibleValue(testValue, highAffectTech);
            cells[1].EliminatePossibleValue(testValue, optionalTech);
            cells[2].EliminatePossibleValue(testValue, lowAffectTech);
            cells[3].EliminatePossibleValue(testValue, lowAffectTech);

            var value = FoundValue.CreateFoundInSetValue(cells[0], testValue, box);

            Assert.IsTrue(expectedTechs.SequenceEqual(value.Techniques));
        }

        [TestMethod]
        public void TestFoundInSetValueHasOnlyRequiredTechniques()
        {
            var testValue = 8;
            var testRequiredIndex = 12;
            var testOptionalIndex = 27;

            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(9), new Cell(10) };
            var box = new Box(cells, 0);
            var row = new Row(cells, 1);
            var col = new Column(cells, 0);

            var occupiedTech = BasicEliminationTechnique.CreateOccupiedTechnique(testValue, cells[1].Index);
            var requiredTech = BasicEliminationTechnique.CreateSetTechnique(testValue, testRequiredIndex, row);
            var optionalTech = BasicEliminationTechnique.CreateSetTechnique(testValue, testOptionalIndex, col);

            var expectedTechs = new List<IEliminationTechnique>() { occupiedTech, requiredTech };

            cells[1].EliminatePossibleValue(testValue, occupiedTech);
            cells[2].EliminatePossibleValue(testValue, requiredTech);
            cells[3].EliminatePossibleValue(testValue, requiredTech);
            cells[2].EliminatePossibleValue(testValue, optionalTech);

            var value = FoundValue.CreateFoundInSetValue(cells[0], testValue, box);

            Assert.IsTrue(expectedTechs.SequenceEqual(value.Techniques));
        }

        [TestMethod]
        public void TestFoundInSetValueLeftoverTechniquesTakenByCellsAffectedFirst()
        {
            var testValue = 8;

            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(9), new Cell(10), new Cell(18)};
            var box = new Box(cells, 0);

            var lowAffectTech = new TestEliminationTechnique(1)
            {
                Indexes = new List<int> { 9, 10}
            };

            var highAffectTech = new TestEliminationTechnique(2)
            {
                Indexes = new List<int> { 1, 9, 10, 18 }
            };

            var optionalTech = new TestEliminationTechnique(2)
            {
                Indexes = new List<int> { 1, 18 }
            };

            var expectedTechs = new List<IEliminationTechnique>() { highAffectTech };

            cells[1].EliminatePossibleValue(testValue, highAffectTech);
            cells[2].EliminatePossibleValue(testValue, highAffectTech);
            cells[3].EliminatePossibleValue(testValue, highAffectTech);
            cells[4].EliminatePossibleValue(testValue, highAffectTech);
            cells[1].EliminatePossibleValue(testValue, optionalTech);
            cells[2].EliminatePossibleValue(testValue, lowAffectTech);
            cells[3].EliminatePossibleValue(testValue, lowAffectTech);
            cells[4].EliminatePossibleValue(testValue, optionalTech);

            var value = FoundValue.CreateFoundInSetValue(cells[0], testValue, box);

            Assert.IsTrue(expectedTechs.SequenceEqual(value.Techniques));
        }

        [TestMethod]
        public void TestFoundInSetValueLeftoverTechniquesTakenByComplexitySecond()
        {
            var testValue = 8;

            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(9), new Cell(10), new Cell(18) };
            var box = new Box(cells, 0);

            var lowComplexityTech = new TestEliminationTechnique(1)
            {
                Indexes = new List<int> { 1, 9, 10, 18 }
            };

            var highComplexityTech = new TestEliminationTechnique(2)
            {
                Indexes = new List<int> { 1, 9, 10, 18 }
            };

            var expectedTechs = new List<IEliminationTechnique>() { lowComplexityTech };

            cells[1].EliminatePossibleValue(testValue, highComplexityTech);
            cells[2].EliminatePossibleValue(testValue, highComplexityTech);
            cells[3].EliminatePossibleValue(testValue, highComplexityTech);
            cells[4].EliminatePossibleValue(testValue, highComplexityTech);
            cells[1].EliminatePossibleValue(testValue, lowComplexityTech);
            cells[2].EliminatePossibleValue(testValue, lowComplexityTech);
            cells[3].EliminatePossibleValue(testValue, lowComplexityTech);
            cells[4].EliminatePossibleValue(testValue, lowComplexityTech);

            var value = FoundValue.CreateFoundInSetValue(cells[0], testValue, box);

            Assert.IsTrue(expectedTechs.SequenceEqual(value.Techniques));
        }

        [TestMethod]
        public void TestFoundInSetValueHandlesMutuallyExclusiveTechniques()
        {
            var testValue = 8;

            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(9), new Cell(10) };
            var box = new Box(cells, 0);
            var row = new Row(cells, 1);

            var techniqueA  = new TestEliminationTechnique(1)
            {
                Indexes = new List<int> { 1 }
            };

            var techniqueB = new TestEliminationTechnique(1)
            {
                Indexes = new List<int> { 9 }
            };

            var techniqueC = new TestEliminationTechnique(1)
            {
                Indexes = new List<int> { 1, 9 }
            };

            var techniqueD = new TestEliminationTechnique(1)
            {
                Indexes = new List<int> { 10 }
            };

            var techniqueE = new TestEliminationTechnique(1)
            {
                Indexes = new List<int> { 10 }
            };

            var expectedTechs = new List<IEliminationTechnique>() { techniqueC, techniqueD };

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
            var expectedTechs = new List<IEliminationTechnique>();

            foreach (var v in Constants.ALL_VALUES.Except(testValue))
            {
                var tech = BasicEliminationTechnique.CreateOccupiedTechnique(v, v);
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
