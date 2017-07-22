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
    using Sudokungfu.SudokuSolver.Techniques.Advanced;

    /// <summary>
    /// Test class for <see cref="ThreeSpotClosureFactory"/>.
    /// </summary>
    [TestClass]
    public class ThreeSpotClosureFactoryTest : BaseTest
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

            var factory = new ThreeSpotClosureFactory(_cells, new Set[] { _box });
            factory.Apply();

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

            var factory = new ThreeSpotClosureFactory(_cells, new Set[] { _box });
            factory.Apply();

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

            var factory = new ThreeSpotClosureFactory(_cells, new Set[] { _box });
            factory.Apply();

            ISudokuModel technique = null;
            foreach (var cell in closureCells)
            {
                foreach (var value in Constants.ALL_VALUES.Except(new int[] { _valueA, _valueB, _valueC }))
                {
                    Assert.IsTrue(cell.Techniques.ContainsKey(value));
                    Assert.AreEqual(1, cell.Techniques[value].Count());

                    var cellTechnique = cell.Techniques[value].First();
                    technique = technique ?? cellTechnique;

                    Assert.AreEqual(ThreeSpotClosureFactory.COMPLEXITY, technique.Complexity);
                    Assert.AreSame(technique, cellTechnique);
                }
            }
        }

        [TestMethod]
        public void TestTechniqueProperties_AffectedIndexes()
        {
            var closureCells = SetupSingleTripleClosure();

            var factory = new ThreeSpotClosureFactory(_cells, new Set[] { _box });
            factory.Apply();

            var technique = _cells[0].Techniques[7].First();
            Assert.AreEqual(ThreeSpotClosureFactory.COMPLEXITY, technique.Complexity);

            AssertSetEqual(closureCells.Indexes(), technique.AffectedIndexes);
        }

        [TestMethod]
        public void TestTechniqueProperties_CellValueMap_TripleDoubleClosure()
        {
            var closureCells = SetupTripleDoubleClosure();

            var factory = new ThreeSpotClosureFactory(_cells, new Set[] { _box });
            factory.Apply();

            var technique = _cells[0].Techniques[7].First();
            Assert.AreEqual(ThreeSpotClosureFactory.COMPLEXITY, technique.Complexity);

            var expectedValues1 = new int[] { _valueB, _valueC };
            var expectedValues2 = new int[] { _valueA, _valueC };
            var expectedValues3 = new int[] { _valueA, _valueB };

            AssertSetEqual(_box.Cells, technique.CellValueMap.Keys);

            AssertSetEqual(expectedValues1, technique.IndexValueMap[0]);
            AssertSetEqual(expectedValues2, technique.IndexValueMap[1]);
            AssertSetEqual(expectedValues3, technique.IndexValueMap[2]);
        }

        [TestMethod]
        public void TestTechniqueProperties_CellValueMap_SingleTripleClosure()
        {
            var closureCells = SetupSingleTripleClosure();

            var factory = new ThreeSpotClosureFactory(_cells, new Set[] { _box });
            factory.Apply();

            var technique = _cells[0].Techniques[7].First();
            Assert.AreEqual(ThreeSpotClosureFactory.COMPLEXITY, technique.Complexity);

            var expectedValues = new int[] { _valueA, _valueB, _valueC };
            AssertSetEqual(_box.Cells, technique.CellValueMap.Keys);

            AssertSetEqual(expectedValues, technique.CellValueMap[_cells[0]]);
            AssertSetEqual(expectedValues, technique.CellValueMap[_cells[1]]);
            AssertSetEqual(expectedValues, technique.CellValueMap[_cells[2]]);
        }

        private IEnumerable<Cell> SetupTripleDoubleClosure()
        {
            var closureCells = SetupSingleTripleClosure().ToList();
            var testTechniqueA = new Technique();
            var testTechniqueB = new Technique();
            var testTechniqueC = new Technique();
            testTechniqueA.Values.Add(_valueA);
            testTechniqueB.Values.Add(_valueB);
            testTechniqueC.Values.Add(_valueC);
            testTechniqueA.Complexity = int.MaxValue;
            testTechniqueB.Complexity = int.MaxValue;
            testTechniqueC.Complexity = int.MaxValue;

            closureCells[0].ApplyTechnique(testTechniqueA);
            closureCells[1].ApplyTechnique(testTechniqueB);
            closureCells[2].ApplyTechnique(testTechniqueC);

            return closureCells;
        }

        private IEnumerable<Cell> SetupSingleTripleClosure()
        {
            var closureCells = new Cell[] { _cells[0], _cells[1], _cells[2] };
            var testTechnique = new Technique();
            testTechnique.Values.Add(_valueA);
            testTechnique.Values.Add(_valueB);
            testTechnique.Values.Add(_valueC);
            testTechnique.Complexity = int.MaxValue;
            foreach (var cell in _box.Cells.Except(closureCells))
            {
                cell.ApplyTechnique(testTechnique);
            }

            return closureCells;
        }
    }
}
