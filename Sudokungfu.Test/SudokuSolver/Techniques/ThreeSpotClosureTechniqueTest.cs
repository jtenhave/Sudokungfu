using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.Techniques
{
    using Model;
    using Sudokungfu.Extensions;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;
    using Sudokungfu.SudokuSolver.Techniques;

    /// <summary>
    /// Test class for <see cref="ThreeSpotClosureTechnique"/>.
    /// </summary>
    [TestClass]
    public class ThreeSpotClosureTechniqueTest : BaseTest
    {
        private List<Cell> _cells;
        private Box _box;
        private int _valueA;
        private int _valueB;
        private int _valueC;

        [TestInitialize]
        public void TestSetup()
        {
            _cells = GetAllCells().ToList();
            _box = new Box(_cells, 0);
            _valueA = 4;
            _valueB = 5;
            _valueC = 6;
        }


        [TestMethod]
        public void TestEliminateAffectedCells_TripleDoubleClosure()
        {
            var closureCells = SetupTripleDoubleClosure().ToList();

            ThreeSpotClosureTechnique.Apply(_cells, new Set[] { _box });

            foreach (var cell in closureCells)
            {
                Assert.AreEqual(2, cell.PossibleValues.Count());
            }

            Assert.IsTrue(closureCells[0].PossibleValues.Contains(_valueB));
            Assert.IsTrue(closureCells[0].PossibleValues.Contains(_valueC));
            Assert.IsTrue(closureCells[1].PossibleValues.Contains(_valueA));
            Assert.IsTrue(closureCells[1].PossibleValues.Contains(_valueC));
            Assert.IsTrue(closureCells[2].PossibleValues.Contains(_valueA));
            Assert.IsTrue(closureCells[2].PossibleValues.Contains(_valueB));
        }

        [TestMethod]
        public void TestEliminateAffectedCells_SingleTripleClosure()
        {
            var closureCells = SetupSingleTripleClosure();

            ThreeSpotClosureTechnique.Apply(_cells, new Set[] { _box });

            foreach (var cell in closureCells)
            {
                Assert.AreEqual(3, cell.PossibleValues.Count());
                Assert.IsTrue(cell.PossibleValues.Contains(_valueA));
                Assert.IsTrue(cell.PossibleValues.Contains(_valueB));
                Assert.IsTrue(cell.PossibleValues.Contains(_valueC));
            }
        }

        [TestMethod]
        public void TestEliminatesWithCorrectTechnique()
        {
            var closureCells = SetupSingleTripleClosure();

            ThreeSpotClosureTechnique.Apply(_cells, new Set[] { _box });

            ISudokuModel technique = null;
            foreach (var cell in closureCells)
            {
                foreach (var value in Constants.ALL_VALUES.Except(new int[] { _valueA, _valueB, _valueC }))
                {
                    Assert.IsTrue(cell.EliminationTechniques.ContainsKey(value));
                    Assert.AreEqual(1, cell.EliminationTechniques[value].Count());

                    var cellTechnique = cell.EliminationTechniques[value].First();
                    technique = technique ?? cellTechnique;

                    Assert.IsTrue(cellTechnique is ThreeSpotClosureTechnique);
                    Assert.AreSame(technique, cellTechnique);
                }
            }
        }

        [TestMethod]
        public void TestTechniqueProperties_AffectedIndexes()
        {
            var closureCells = SetupSingleTripleClosure();

            ThreeSpotClosureTechnique.Apply(_cells, new Set[] { _box });

            var technique = _cells[0].EliminationTechniques[7].First();
            Assert.IsTrue(technique is ThreeSpotClosureTechnique);

            AssertSetEqual(closureCells.Indexes(), technique.AffectedIndexes);
        }

        [TestMethod]
        public void TestTechniqueProperties_IndexValueMap_TripleDoubleClosure()
        {
            var closureCells = SetupTripleDoubleClosure();

            ThreeSpotClosureTechnique.Apply(_cells, new Set[] { _box });

            var technique = _cells[0].EliminationTechniques[7].First();
            Assert.IsTrue(technique is ThreeSpotClosureTechnique);

            var expectedValues1 = new int[] { _valueB, _valueC };
            var expectedValues2 = new int[] { _valueA, _valueC };
            var expectedValues3 = new int[] { _valueA, _valueB };

            AssertSetEqual(_box.Indexes, technique.IndexValueMap.Keys);

            AssertSetEqual(expectedValues1, technique.IndexValueMap[0]);
            AssertSetEqual(expectedValues2, technique.IndexValueMap[1]);
            AssertSetEqual(expectedValues3, technique.IndexValueMap[2]);
        }

        [TestMethod]
        public void TestTechniqueProperties_IndexValueMap_SingleTripleClosure()
        {
            var closureCells = SetupSingleTripleClosure();

            ThreeSpotClosureTechnique.Apply(_cells, new Set[] { _box });

            var technique = _cells[0].EliminationTechniques[7].First();
            Assert.IsTrue(technique is ThreeSpotClosureTechnique);

            var expectedValues = new int[] { _valueA, _valueB, _valueC };
            AssertSetEqual(_box.Indexes, technique.IndexValueMap.Keys);

            AssertSetEqual(expectedValues, technique.IndexValueMap[0]);
            AssertSetEqual(expectedValues, technique.IndexValueMap[1]);
            AssertSetEqual(expectedValues, technique.IndexValueMap[2]);
        }

        private IEnumerable<Cell> SetupTripleDoubleClosure()
        {
            var closureCells = new Cell[] { _cells[0], _cells[1], _cells[2] };
            var testTechnique = new TestTechnique(0);
            foreach (var cell in _box.Cells.Except(closureCells))
            {
                cell.EliminatePossibleValue(_valueA, testTechnique);
                cell.EliminatePossibleValue(_valueB, testTechnique);
                cell.EliminatePossibleValue(_valueC, testTechnique);
            }

            _cells[0].EliminatePossibleValue(_valueA, testTechnique);
            _cells[1].EliminatePossibleValue(_valueB, testTechnique);
            _cells[2].EliminatePossibleValue(_valueC, testTechnique);

            return closureCells;
        }

        private IEnumerable<Cell> SetupSingleTripleClosure()
        {
            var closureCells = new Cell[] { _cells[0], _cells[1], _cells[2] };
            var testTechnique = new TestTechnique(0);
            foreach (var cell in _box.Cells.Except(closureCells))
            {
                cell.EliminatePossibleValue(_valueA, testTechnique);
                cell.EliminatePossibleValue(_valueB, testTechnique);
                cell.EliminatePossibleValue(_valueC, testTechnique);
            }

            return closureCells;
        }
    }
}
