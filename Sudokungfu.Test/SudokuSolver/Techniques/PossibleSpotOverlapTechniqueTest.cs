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
    /// Test class for <see cref="PossibleSpotOverlapTechnique"/>.
    /// </summary>
    [TestClass]
    public class PossibleSpotOverlapTechniqueTest : BaseTest
    {
        private List<Cell> _cells;
        private Box _box;
        private Row _row;
        private int _value;

        [TestInitialize]
        public void TestSetup()
        {
            _cells = GetAllCells().ToList();
            _box = new Box(_cells, 0);
            _row = new Row(_cells, 0);
            _value = 4;
        }

        [TestMethod]
        public void TestNotOverlapping_FourCells()
        {
            SetupOverlapping(10, 11, 18, 19, 20);

            PossibleSpotOverlapTechnique.Apply(_cells, new Set[] { _box, _row });

            var affectedCells = _row.Cells.Except(_box.Cells);
            foreach (var cell in affectedCells)
            {
                Assert.IsTrue(cell.PossibleValues.Contains(_value));
            }
        }

        [TestMethod]
        public void TestNotOverlapping_OneCells()
        {
            SetupOverlapping(1, 2, 9, 10, 11, 18, 19, 20);

            PossibleSpotOverlapTechnique.Apply(_cells, new Set[] { _box, _row });

            var affectedCells = _row.Cells.Except(_box.Cells);
            foreach (var cell in affectedCells)
            {
                Assert.IsTrue(cell.PossibleValues.Contains(_value));
            }
        }

        [TestMethod]
        public void TestEliminateAffectedCells_TwoOverlapping()
        {
            SetupTwoOverlapping();

            PossibleSpotOverlapTechnique.Apply(_cells, new Set[] { _box, _row });

            var affectedCells = _row.Cells.Except(_box.Cells);
            foreach (var cell in affectedCells)
            {
                Assert.IsFalse(cell.PossibleValues.Contains(_value));
            }
        }

        [TestMethod]
        public void TestEliminateAffectedCells_ThreeOverlapping()
        {
            SetupThreeOverlapping();

            PossibleSpotOverlapTechnique.Apply(_cells, new Set[] { _box, _row });

            var affectedCells = _row.Cells.Except(_box.Cells);
            foreach (var cell in affectedCells)
            {
                Assert.IsFalse(cell.PossibleValues.Contains(_value));
            }
        }

        [TestMethod]
        public void TestDoesNotEliminateSourceSetCells()
        {
            SetupTwoOverlapping();

            PossibleSpotOverlapTechnique.Apply(_cells, new Set[] { _box, _row });

            var cell = _cells[1];
            Assert.IsTrue(cell.EliminationTechniques.ContainsKey(_value));
            Assert.AreEqual(1, cell.EliminationTechniques[_value].Count());
            Assert.IsFalse(cell.EliminationTechniques[_value].First() is PossibleSpotOverlapTechnique);
        }

        [TestMethod]
        public void TestEliminatesWithCorrectTechnique()
        {
            SetupTwoOverlapping();

            PossibleSpotOverlapTechnique.Apply(_cells, new Set[] { _box, _row });

            ISudokuModel technique = null;
            var affectedCells = _row.Cells.Except(_box.Cells);
            foreach (var cell in affectedCells)
            {
                Assert.IsTrue(cell.EliminationTechniques.ContainsKey(_value));
                Assert.AreEqual(1, cell.EliminationTechniques[_value].Count());

                var cellTechnique = cell.EliminationTechniques[_value].First();
                technique = technique ?? cellTechnique;

                Assert.IsTrue(cellTechnique is PossibleSpotOverlapTechnique);
                Assert.AreSame(technique, cellTechnique);
            }
        }

        [TestMethod]
        public void TestTechniqueProperties_AffectedIndexes()
        {
            SetupTwoOverlapping();

            PossibleSpotOverlapTechnique.Apply(_cells, new Set[] { _box, _row });

            var technique = _cells[4].EliminationTechniques[_value].First();
            Assert.IsTrue(technique is PossibleSpotOverlapTechnique);

            var expectedAffectedIndexes = _row.Cells.Except(_box.Cells).Indexes();
            AssertSetEqual(expectedAffectedIndexes, technique.AffectedIndexes);
        }

        [TestMethod]
        public void TestTechniqueProperties_IndexValueMap()
        {
            SetupTwoOverlapping();

            PossibleSpotOverlapTechnique.Apply(_cells, new Set[] { _box, _row });

            var technique = _cells[4].EliminationTechniques[_value].First();
            Assert.IsTrue(technique is PossibleSpotOverlapTechnique);

            AssertSetEqual(_box.Indexes, technique.IndexValueMap.Keys);
            foreach (var key in technique.IndexValueMap.Keys)
            {
                if (key == 0 || key == 2)
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

        private void SetupTwoOverlapping()
        {
            SetupOverlapping(1, 9, 10, 11, 18, 19, 20);
        }

        private void SetupThreeOverlapping()
        {
            SetupOverlapping(9, 10, 11, 18, 19, 20);
        }

        private void SetupOverlapping(params int[] indexes)
        {
            var testTechnique = new TestTechnique(0);
            foreach (var index in indexes)
            {
                _cells[index].EliminatePossibleValue(_value, testTechnique);
            }
        }
    }
}
