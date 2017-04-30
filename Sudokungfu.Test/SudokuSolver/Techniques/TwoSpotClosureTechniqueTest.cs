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
    /// Test class for <see cref="TwoSpotClosureTechnique"/>.
    /// </summary>
    [TestClass]
    public class TwoSpotClosureTechniqueTest : BaseTest
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

            TwoSpotClosureTechnique.Apply(_cells, new Set[] { _box });

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

            TwoSpotClosureTechnique.Apply(_cells, new Set[] { _box });

            ISudokuModel technique = null;
            foreach (var cell in closureCells)
            {
                foreach (var value in Constants.ALL_VALUES.Except(new int[] { _valueA, _valueB }))
                {
                    Assert.IsTrue(cell.EliminationTechniques.ContainsKey(value));
                    Assert.AreEqual(1, cell.EliminationTechniques[value].Count());

                    var cellTechnique = cell.EliminationTechniques[value].First();
                    technique = technique ?? cellTechnique;

                    Assert.IsTrue(cellTechnique is TwoSpotClosureTechnique);
                    Assert.AreSame(technique, cellTechnique);
                }              
            }
        }

        [TestMethod]
        public void TestTechniqueProperties_AffectedIndexes()
        {
            var closureCells = SetupClosure();

            TwoSpotClosureTechnique.Apply(_cells, new Set[] { _box });

            var technique = _cells[0].EliminationTechniques[7].First();
            Assert.IsTrue(technique is TwoSpotClosureTechnique);

            AssertSetEqual(closureCells.Indexes(), technique.AffectedIndexes);
        }
   
        [TestMethod]
        public void TestTechniqueProperties_IndexValueMap()
        {
            var closureCells = SetupClosure();

            TwoSpotClosureTechnique.Apply(_cells, new Set[] { _box });

            var technique = _cells[0].EliminationTechniques[7].First();
            Assert.IsTrue(technique is TwoSpotClosureTechnique);

            var expectedValues = new int[] { _valueA, _valueB };
            AssertSetEqual(_box.Indexes, technique.IndexValueMap.Keys);

            AssertSetEqual(expectedValues, technique.IndexValueMap[0]);
            AssertSetEqual(expectedValues, technique.IndexValueMap[1]);
        }

        private IEnumerable<Cell> SetupClosure()
        {
            var closureCells = new Cell[] { _cells[0], _cells[1] };
            var testTechnique = new TestTechnique(0);
            foreach (var cell in _box.Cells.Except(closureCells))
            {
                cell.EliminatePossibleValue(_valueA, testTechnique);
                cell.EliminatePossibleValue(_valueB, testTechnique);
            }

            return closureCells;
        }
    }
}
