using System;
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
    /// Test class for <see cref="PossibleSpotShadowTechnique"/>.
    /// </summary>
    [TestClass]
    public class PossibleSpotsShadowTechniqueTest : BaseTest
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

            PossibleSpotShadowTechnique.Apply(_cells, new Set[] { _rowA, _rowB });

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

            PossibleSpotShadowTechnique.Apply(_cells, new Set[] { _rowA, _rowB });

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

            PossibleSpotShadowTechnique.Apply(_cells, new Set[] { _rowA, _rowB });

            ISudokuModel technique = null;
            var affectedCells = _cells.Where(c => columns.Contains(Column(c)))
               .Except(_rowA.Cells)
               .Except(_rowB.Cells);
            foreach (var cell in affectedCells)
            {
                Assert.IsTrue(cell.EliminationTechniques.ContainsKey(_value));
                Assert.AreEqual(1, cell.EliminationTechniques[_value].Count());

                var cellTechnique = cell.EliminationTechniques[_value].First();
                technique = technique ?? cellTechnique;

                Assert.IsTrue(cellTechnique is PossibleSpotShadowTechnique);
                Assert.AreSame(technique, cellTechnique);
            }
        }

        [TestMethod]
        public void TestTechniqueProperties_AffectedIndexes()
        {
            var columns = SetupShadow();

            PossibleSpotShadowTechnique.Apply(_cells, new Set[] { _rowA, _rowB });

            var technique = _cells[11].EliminationTechniques[_value].First();
            Assert.IsTrue(technique is PossibleSpotShadowTechnique);

            var expectedAffectedIndexes = _cells.Where(c => columns.Contains(Column(c)))
                .Except(_rowA.Cells)
                .Except(_rowB.Cells)
                .Indexes();
            AssertSetEqual(expectedAffectedIndexes, technique.AffectedIndexes);
        }

        [TestMethod]
        public void TestTechniqueProperties_IndexValueMap()
        {
            var columns = SetupShadow();

            PossibleSpotShadowTechnique.Apply(_cells, new Set[] { _rowA, _rowB });

            var technique = _cells[11].EliminationTechniques[_value].First();
            Assert.IsTrue(technique is PossibleSpotShadowTechnique);

            AssertSetEqual(_rowA.Indexes.Concat(_rowB.Indexes), technique.IndexValueMap.Keys);
            foreach (var key in technique.IndexValueMap.Keys)
            {
                if (key == 2 || key == 7 || key == 38 || key == 43)
                {
                    Assert.AreEqual(1, technique.IndexValueMap[key].Count());
                    Assert.AreEqual(_value, technique.IndexValueMap[key].First());
                }
                else
                {
                    Assert.AreEqual(0, technique.IndexValueMap[key].Count());
                }
            }
        }

        private IEnumerable<int> SetupShadow()
        {
            var testTechnique = new TestTechnique(0);
            var columns = new List<int> { 2, 7 };

            foreach (var cell in _rowA.Cells.Concat(_rowB.Cells).Where(c => !columns.Contains(Column(c))))
            {
                cell.EliminatePossibleValue(_value, testTechnique);
            }

            return columns;
        }

        private static int Column(Cell cell)
        {
            return cell.Index % Constants.SET_SIZE;
        }
    }
}
