using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.Techniques.Advanced
{
    using Sudokungfu.Model;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;
    using Sudokungfu.SudokuSolver.Techniques;
    using Sudokungfu.SudokuSolver.Techniques.Advanced;

    /// <summary>
    /// Test class for <see cref="ThreeSpotOverlapFactory"/>.
    /// </summary>
    [TestClass]
    public class ThreeSpotOverlapFactoryTest : BaseTest
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

            var factory = new ThreeSpotOverlapFactory(_cells, new Set[] { _box, _row });
            factory.Apply();

            var affectedCells = _row.Cells.Except(_box.Cells);
            foreach (var cell in affectedCells)
            {
                Assert.IsTrue(cell.PossibleValues.Contains(_value));
            }
        }

        [TestMethod]
        public void TestNotOverlapping_TwoCells()
        {
            SetupOverlapping(1, 9, 10, 11, 18, 19, 20);

            var factory = new ThreeSpotOverlapFactory(_cells, new Set[] { _box, _row });
            factory.Apply();

            var affectedCells = _row.Cells.Except(_box.Cells);
            foreach (var cell in affectedCells)
            {
                Assert.IsTrue(cell.PossibleValues.Contains(_value));
            }
        }

        [TestMethod]
        public void TestEliminateAffectedCells_ThreeOverlapping()
        {
            SetupThreeOverlapping();

            var factory = new ThreeSpotOverlapFactory(_cells, new Set[] { _box, _row });
            factory.Apply();

            var affectedCells = _row.Cells.Except(_box.Cells);
            foreach (var cell in affectedCells)
            {
                Assert.IsFalse(cell.PossibleValues.Contains(_value));
            }
        }

        [TestMethod]
        public void TestEliminatesWithCorrectTechnique()
        {
            SetupThreeOverlapping();

            var factory = new ThreeSpotOverlapFactory(_cells, new Set[] { _box, _row });
            factory.Apply();

            ISudokuModel technique = null;
            var affectedCells = _row.Cells.Except(_box.Cells);
            foreach (var cell in affectedCells)
            {
                Assert.IsTrue(cell.Techniques.ContainsKey(_value));
                Assert.AreEqual(1, cell.Techniques[_value].Count());

                var cellTechnique = cell.Techniques[_value].First();
                technique = technique ?? cellTechnique;

                Assert.AreEqual(ThreeSpotOverlapFactory.COMPLEXITY, cellTechnique.Complexity);
                Assert.AreSame(technique, cellTechnique);
            }
        }

        [TestMethod]
        public void TestTechniqueProperties_AffectedIndexes()
        {
            SetupThreeOverlapping();

            var factory = new ThreeSpotOverlapFactory(_cells, new Set[] { _box, _row });
            factory.Apply();

            var technique = _cells[4].Techniques[_value].First();
            Assert.AreEqual(ThreeSpotOverlapFactory.COMPLEXITY, technique.Complexity);

            var expectedAffectedIndexes = _row.Cells.Except(_box.Cells).Indexes();
            AssertSetEqual(expectedAffectedIndexes, technique.AffectedIndexes);
        }

        [TestMethod]
        public void TestTechniqueProperties_CellValueMap()
        {
            SetupThreeOverlapping();

            var factory = new ThreeSpotOverlapFactory(_cells, new Set[] { _box, _row });
            factory.Apply();

            var technique = _cells[4].Techniques[_value].First();
            Assert.AreEqual(ThreeSpotOverlapFactory.COMPLEXITY, technique.Complexity);

            AssertSetEqual(_box.Cells, technique.CellValueMap.Keys);
            foreach (var key in technique.CellValueMap.Keys)
            {
                if (key == _cells[0] || key == _cells[1] || key == _cells[2])
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

        private void SetupThreeOverlapping()
        {
            SetupOverlapping(9, 10, 11, 18, 19, 20);
        }

        private void SetupOverlapping(params int[] indexes)
        {
            var testTechnique = new Technique();
            testTechnique.Values.Add(_value);
            testTechnique.Complexity = int.MaxValue;
            foreach (var index in indexes)
            {
                _cells[index].ApplyTechnique(testTechnique);
            }
        }
    }
}
