using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.Techniques.Advanced
{
    using Model;
    using Sudokungfu.Extensions;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;
    using Sudokungfu.SudokuSolver.Techniques;
    using Sudokungfu.SudokuSolver.Techniques.Advanced;

    /// <summary>
    /// Test class for <see cref="TwoSpotClosureFactory"/>.
    /// </summary>
    [TestClass]
    public class TwoSpotClosureFactoryTest : BaseTest
    {
        private List<Cell> _cells;
        private Box _box;
        private int _valueA;
        private int _valueB;

        [TestInitialize]
        public void TestSetup()
        {
            _cells = GetAllCells().ToList();
            _box = new Box(_cells, 0);
            _valueA = 4;
            _valueB = 5;
        }

        [TestMethod]
        public void TestEliminateAffectedCells()
        {
            var closureCells = SetupClosure();

            var factory = new TwoSpotClosureFactory(_cells, new Set[] { _box });
            factory.Apply();

            foreach (var cell in closureCells)
            {
                Assert.AreEqual(2, cell.PossibleValues.Count());
                Assert.IsTrue(cell.PossibleValues.Contains(_valueA));
                Assert.IsTrue(cell.PossibleValues.Contains(_valueB));
            }
        }

        [TestMethod]
        public void TestEliminatesWithCorrectTechnique()
        {
            var closureCells = SetupClosure();

            var factory = new TwoSpotClosureFactory(_cells, new Set[] { _box });
            factory.Apply();

            ISudokuModel technique = null;
            foreach (var cell in closureCells)
            {
                foreach (var value in Constants.ALL_VALUES.Except(new int[] { _valueA, _valueB }))
                {
                    Assert.IsTrue(cell.Techniques.ContainsKey(value));
                    Assert.AreEqual(1, cell.Techniques[value].Count());

                    var cellTechnique = cell.Techniques[value].First();
                    technique = technique ?? cellTechnique;

                    Assert.AreEqual(TwoSpotClosureFactory.COMPLEXITY, technique.Complexity);
                    Assert.AreSame(technique, cellTechnique);
                }              
            }
        }

        [TestMethod]
        public void TestTechniqueProperties_AffectedIndexes()
        {
            var closureCells = SetupClosure();

            var factory = new TwoSpotClosureFactory(_cells, new Set[] { _box });
            factory.Apply();

            var technique = _cells[0].Techniques[7].First();
            Assert.AreEqual(TwoSpotClosureFactory.COMPLEXITY, technique.Complexity);

            AssertSetEqual(closureCells.Indexes(), technique.AffectedIndexes);
        }
   
        [TestMethod]
        public void TestTechniqueProperties_IndexValueMap()
        {
            var closureCells = SetupClosure();

            var factory = new TwoSpotClosureFactory(_cells, new Set[] { _box });
            factory.Apply();

            var technique = _cells[0].Techniques[7].First();
            Assert.AreEqual(TwoSpotClosureFactory.COMPLEXITY, technique.Complexity);

            var expectedValues = new int[] { _valueA, _valueB };
            AssertSetEqual(_box.Indexes, technique.IndexValueMap.Keys);

            AssertSetEqual(expectedValues, technique.IndexValueMap[0]);
            AssertSetEqual(expectedValues, technique.IndexValueMap[1]);
        }

        private IEnumerable<Cell> SetupClosure()
        {
            var closureCells = new Cell[] { _cells[0], _cells[1] };
            var testTechnique = new Technique();
            testTechnique.Complexity = int.MaxValue;
            foreach (var cell in _box.Cells.Except(closureCells))
            {
                cell.ApplyTechnique(_valueA, testTechnique);
                cell.ApplyTechnique(_valueB, testTechnique);
            }

            return closureCells;
        }
    }
}
