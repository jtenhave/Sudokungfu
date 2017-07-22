using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.Sets
{
    using Sudokungfu.Extensions;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;
    using Sudokungfu.SudokuSolver.Techniques;

    /// <summary>
    /// Test class for <see cref="Set"/>.
    /// </summary>
    [TestClass]
    public class SetTest : BaseTest
    {
        private List<Cell> _cells;
        private Row _row;

        [TestInitialize]
        public void TestSetup()
        {
            _cells = GetAllCells().ToList();
            _row = new Row(_cells, 0);
        }

        [TestMethod]
        public void TestPossibleSpots()
        {
            var expectedValue = 6;
            var expectedIndexes = new List<int>() { 2, 5, 6, 8 };

            EliminateValues(expectedValue, new Technique(), 0, 1, 3, 4, 7);

            AssertSetEqual(expectedIndexes, _row.PossibleSpots[expectedValue].Indexes());
        }

        [TestMethod]
        public void TestFindMinTechniquesComplexityFilter()
        {
            var expectedValue = 6;
            var expectedTechniqueA = new Technique()
            {
                Complexity = 0
            };
            expectedTechniqueA.AffectedCells.AddRange(new Cell[] { _cells[0], _cells[1] });

            var expectedTechniqueB = new Technique()
            {
                Complexity = 1
            };
            expectedTechniqueB.AffectedCells.AddRange(new Cell[] { _cells[0], _cells[1] });

            EliminateValues(expectedValue, expectedTechniqueA, 0);
            EliminateValues(expectedValue, expectedTechniqueB, 1);

            var actualTechqniues = _row.FindMinTechniques(Enumerable.Empty<Cell>(), expectedValue);
            AssertSetEqual(expectedTechniqueA.ToEnumerable(), actualTechqniues);
        }

        [TestMethod]
        public void TestFindMinTechniquesUniqueIndexFilter()
        {
            var expectedValue = 6;
            var expectedTechniqueA = new Technique()
            {
                Complexity = 0
            };
            expectedTechniqueA.AffectedCells.AddRange(new Cell[] { _cells[0], _cells[1] });

            var expectedTechniqueB = new Technique()
            {
                Complexity = 0
            };
            expectedTechniqueB.AffectedCells.AddRange(new Cell[] { _cells[0], _cells[1], _cells[2] });

            EliminateValues(expectedValue, expectedTechniqueA, 0);
            EliminateValues(expectedValue, expectedTechniqueB, 1);

            var actualTechqniues = _row.FindMinTechniques(Enumerable.Empty<Cell>(), expectedValue);
            AssertSetEqual(expectedTechniqueB.ToEnumerable(), actualTechqniues);
        }

        [TestMethod]
        public void TestFindMinTechniquesIndexCountFilter()
        {
            var expectedValue = 6;
            var expectedTechniqueA = new Technique()
            {
                Complexity = 0
            };
            expectedTechniqueA.AffectedCells.AddRange(new Cell[] { _cells[0], _cells[1] });

            var expectedTechniqueB = new Technique()
            {
                Complexity = 0
            };
            expectedTechniqueB.AffectedCells.AddRange(new Cell[] { _cells[0], _cells[1], _cells[2] });

            var expectedTechniqueC = new Technique()
            {
                Complexity = 0
            };
            expectedTechniqueC.AffectedCells.AddRange(new Cell[] { _cells[2] });

            EliminateValues(expectedValue, expectedTechniqueA, 0);
            EliminateValues(expectedValue, expectedTechniqueB, 1);
            EliminateValues(expectedValue, expectedTechniqueC, 2);

            var actualTechqniues = _row.FindMinTechniques(Enumerable.Empty<Cell>(), expectedValue);
            AssertSetEqual(expectedTechniqueB.ToEnumerable(), actualTechqniues);
        }

        [TestMethod]
        public void TestFindMinTechniquesCoversAllIndexes()
        {
            var expectedValue = 6;
            var expectedTechniqueA = new Technique()
            {
                Complexity = 0
            };
            expectedTechniqueA.AffectedCells.AddRange(new Cell[] { _cells[0], _cells[1] });

            var expectedTechniqueB = new Technique()
            {
                Complexity = 1
            };
            expectedTechniqueB.AffectedCells.AddRange(new Cell[] { _cells[0], _cells[2], _cells[3], _cells[4], _cells[5] });

            var expectedTechniqueC = new Technique()
            {
                Complexity = 2
            };
            expectedTechniqueC.AffectedCells.AddRange(new Cell[] { _cells[1], _cells[6], _cells[7], _cells[8] });

            var expectedTechniques = new Technique[] { expectedTechniqueA, expectedTechniqueB, expectedTechniqueC };

            EliminateValues(expectedValue, expectedTechniqueA, 0);
            EliminateValues(expectedValue, expectedTechniqueB, 1);
            EliminateValues(expectedValue, expectedTechniqueC, 2);

            var actualTechqniues = _row.FindMinTechniques(Enumerable.Empty<Cell>(), expectedValue);
            AssertSetEqual(expectedTechniques, actualTechqniues);
        }

        [TestMethod]
        public void TestFindMinTechniquesIgnoresIndexesInProvidedCells()
        {
            var expectedValue = 6;
            var expectedTechniqueA = new Technique()
            {
                Complexity = 0
            };
            expectedTechniqueA.AffectedCells.Add(_cells[0]);

            var expectedTechniqueB = new Technique()
            {
                Complexity = 1
            };
            expectedTechniqueB.AffectedCells.AddRange(new Cell[] { _cells[0], _cells[1], _cells[2], _cells[4] });

            EliminateValues(expectedValue, expectedTechniqueA, 0);
            EliminateValues(expectedValue, expectedTechniqueB, 1);

            var actualTechqniues = _row.FindMinTechniques(_cells[0].ToEnumerable(), expectedValue);
            AssertSetEqual(expectedTechniqueB.ToEnumerable(), actualTechqniues);
        }

        private void EliminateValues(int value, Technique technique, params int[] indexes)
        {
            var cells = _cells.ToList();
            technique.Values.Add(value);
            foreach (int index in indexes)
            {
                cells[index].ApplyTechnique(technique);
            }
        }
    }
}
