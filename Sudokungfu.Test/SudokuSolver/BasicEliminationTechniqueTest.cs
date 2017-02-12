using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver
{
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;

    /// <summary>
    /// Test class for <see cref="BasicEliminationTechnique"/>.
    /// </summary>
    [TestClass]
    public class BasicEliminationTechniqueTest
    {
        [TestMethod]
        public void TestCreateOccupiedTechnique()
        {
            var testValue = 3;
            var testIndex = 76;

            var technique = BasicEliminationTechnique.CreateOccupiedTechnique(testValue, testIndex);

            Assert.AreEqual(0, technique.Complexity);
            Assert.AreEqual(1, technique.Indexes.Count());
            Assert.AreEqual(testIndex, technique.Indexes.First());
            Assert.AreEqual(1, technique.ValueMap.Count);
            Assert.AreEqual(testValue, technique.ValueMap.First().Key);
            Assert.AreEqual(1, technique.ValueMap.First().Value.Count());
            Assert.AreEqual(testIndex, technique.ValueMap.First().Value.First());
        }

        [TestMethod]
        public void TestCreateSetTechnique()
        {
            var testValue = 3;
            var testIndex = 76;

            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(9), new Cell(10) };
            var box = new Box(cells, 0);

            var technique = BasicEliminationTechnique.CreateSetTechnique(testValue, testIndex, box);

            Assert.AreEqual(1, technique.Complexity);
            Assert.AreEqual(cells.Count, technique.Indexes.Count());
            Assert.IsTrue(technique.Indexes.SequenceEqual(cells.Select(c => c.Index)));
            Assert.AreEqual(1, technique.ValueMap.Count);
            Assert.AreEqual(testValue, technique.ValueMap.First().Key);
            Assert.AreEqual(1, technique.ValueMap.First().Value.Count());
            Assert.AreEqual(testIndex, technique.ValueMap.First().Value.First());
        }
    }
}
