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
    /// Test class for <see cref="PossibleSpotShadowFactory"/>.
    /// </summary>
    [TestClass]
    public class PossibleSpotShadowTechniqueTest : BaseTest
    {
        private List<Cell> _cells;
        private Row _rowA;
        private Row _rowB;
        private int _value;

        [TestInitialize]
        public void TestSetup()
        {
            _cells = GetAllCells().ToList();
            _rowA = new Row(_cells, 0);
            _rowB = new Row(_cells, 4);
            _value = 5;
        }

        [TestMethod]
        public void TestEliminateAffectedCells()
        {
            var columns = SetupShadow();

            var factory = new PossibleSpotShadowFactory(_cells, new Set[] { _rowA, _rowB });
            factory.Apply();

            var affectedCells = _cells.Where(c => columns.Contains(Column(c)))
                .Except(_rowA.Cells)
                .Except(_rowB.Cells);

            foreach (var cell in affectedCells)
            {
                Assert.IsFalse(cell.PossibleValues.Contains(_value));
            }
        }

        [TestMethod]
        public void TestDoesNotEliminateSourceSetCells()
        {
            var columns = SetupShadow();

            var factory = new PossibleSpotShadowFactory(_cells, new Set[] { _rowA, _rowB });
            factory.Apply();

            var unAffectedCells = _cells.Where(c => columns.Contains(Column(c)))
                .Intersect(_rowA.Cells.Concat(_rowB.Cells));

            foreach (var cell in unAffectedCells)
            {
                Assert.IsTrue(cell.PossibleValues.Contains(_value));
         
            }
        }

        [TestMethod]
        public void TestEliminatesWithCorrectTechnique()
        {
            var columns = SetupShadow();

            var factory = new PossibleSpotShadowFactory(_cells, new Set[] { _rowA, _rowB });
            factory.Apply();

            ISudokuModel technique = null;
            var affectedCells = _cells.Where(c => columns.Contains(Column(c)))
               .Except(_rowA.Cells)
               .Except(_rowB.Cells);
            foreach (var cell in affectedCells)
            {
                Assert.IsTrue(cell.Techniques.ContainsKey(_value));
                Assert.AreEqual(1, cell.Techniques[_value].Count());

                var cellTechnique = cell.Techniques[_value].First();
                technique = technique ?? cellTechnique;

                Assert.AreEqual(PossibleSpotShadowFactory.COMPLEXITY, technique.Complexity);
                Assert.AreSame(technique, cellTechnique);
            }
        }

        [TestMethod]
        public void TestTechniqueProperties_AffectedIndexes()
        {
            var columns = SetupShadow();

            var factory = new PossibleSpotShadowFactory(_cells, new Set[] { _rowA, _rowB });
            factory.Apply();

            var technique = _cells[11].Techniques[_value].First();
            Assert.AreEqual(PossibleSpotShadowFactory.COMPLEXITY, technique.Complexity);

            var expectedAffectedIndexes = _cells.Where(c => columns.Contains(Column(c)))
                .Except(_rowA.Cells)
                .Except(_rowB.Cells)
                .Indexes();
            AssertSetEqual(expectedAffectedIndexes, technique.AffectedIndexes);
        }

        [TestMethod]
        public void TestTechniqueProperties_CellValueMap()
        {
            var columns = SetupShadow();

            var factory = new PossibleSpotShadowFactory(_cells, new Set[] { _rowA, _rowB });
            factory.Apply();

            var technique = _cells[11].Techniques[_value].First();
            Assert.AreEqual(PossibleSpotShadowFactory.COMPLEXITY, technique.Complexity);

            AssertSetEqual(_rowA.Cells.Concat(_rowB.Cells), technique.CellValueMap.Keys);
            foreach (var key in technique.CellValueMap.Keys)
            {
                if (key == _cells[2] || key == _cells[7] || key == _cells[38] || key == _cells[43])
                {
                    Assert.AreEqual(1, technique.CellValueMap[key].Count());
                    Assert.AreEqual(_value, technique.CellValueMap[key].First());
                }
                else
                {
                    Assert.AreEqual(0, technique.CellValueMap[key].Count());
                }
            }
        }

        private IEnumerable<int> SetupShadow()
        {
            var testTechnique = new Technique();
            testTechnique.Values.Add(_value);
            testTechnique.Complexity = int.MaxValue;
            var columns = new List<int> { 2, 7 };

            foreach (var cell in _rowA.Cells.Concat(_rowB.Cells).Where(c => !columns.Contains(Column(c))))
            {
                cell.ApplyTechnique(testTechnique);
            }

            return columns;
        }

        private static int Column(Cell cell)
        {
            return cell.Index % Constants.SET_SIZE;
        }
    }
}
