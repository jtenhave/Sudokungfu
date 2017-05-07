using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.Sets
{
    using Sudokungfu.Extensions;
    using Sudokungfu.Model;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;

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

            EliminateValues(expectedValue, new TestModel(), 0, 1, 3, 4, 7);

            AssertSetEqual(expectedIndexes, _row.PossibleSpots[expectedValue].Indexes());
        }

        [TestMethod]
        public void TestFindMinTechniquesComplexityFilter()
        {
            var expectedValue = 6;
            var expectedTechniqueA = new TestModel()
            {
                Complexity = 0,
                AffectedIndexes = new int[] { 0, 1 }
            };

            var expectedTechniqueB = new TestModel()
            {
                Complexity = 1,
                AffectedIndexes = new int[] { 0, 1 }
            };

            EliminateValues(expectedValue, expectedTechniqueA, 0);
            EliminateValues(expectedValue, expectedTechniqueB, 1);

            var actualTechqniues = _row.FindMinTechniques(Enumerable.Empty<Cell>(), expectedValue);
            AssertSetEqual(expectedTechniqueA.ToEnumerable(), actualTechqniues);
        }

        [TestMethod]
        public void TestFindMinTechniquesUniqueIndexFilter()
        {
            var expectedValue = 6;
            var expectedTechniqueA = new TestModel()
            {
                Complexity = 0,
                AffectedIndexes = new int[] { 0, 1 }
            };

            var expectedTechniqueB = new TestModel()
            {
                Complexity = 0,
                AffectedIndexes = new int[] { 0, 1, 2 }
            };

            EliminateValues(expectedValue, expectedTechniqueA, 0);
            EliminateValues(expectedValue, expectedTechniqueB, 1);

            var actualTechqniues = _row.FindMinTechniques(Enumerable.Empty<Cell>(), expectedValue);
            AssertSetEqual(expectedTechniqueB.ToEnumerable(), actualTechqniues);
        }

        [TestMethod]
        public void TestFindMinTechniquesIndexCountFilter()
        {
            var expectedValue = 6;
            var expectedTechniqueA = new TestModel()
            {
                Complexity = 0,
                AffectedIndexes = new int[] { 0, 1 }
            };

            var expectedTechniqueB = new TestModel()
            {
                Complexity = 0,
                AffectedIndexes = new int[] { 0, 1, 2 }
            };

            var expectedTechniqueC = new TestModel()
            {
                Complexity = 0,
                AffectedIndexes = new int[] { 2 }
            };

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
            var expectedTechniqueA = new TestModel()
            {
                Complexity = 0,
                AffectedIndexes = new int[] { 0, 1 }
            };

            var expectedTechniqueB = new TestModel()
            {
                Complexity = 1,
                AffectedIndexes = new int[] { 0, 2, 3, 4, 5 }
            };

            var expectedTechniqueC = new TestModel()
            {
                Complexity = 2,
                AffectedIndexes = new int[] { 1, 6, 7, 8 }
            };
            var expectedTechniques = new TestModel[] { expectedTechniqueA, expectedTechniqueB, expectedTechniqueC };

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
            var expectedTechniqueA = new TestModel()
            {
                Complexity = 0,
                AffectedIndexes = new int[] { 0 }
            };

            var expectedTechniqueB = new TestModel()
            {
                Complexity = 1,
                AffectedIndexes = new int[] { 0, 1, 2, 4 }
            };

            EliminateValues(expectedValue, expectedTechniqueA, 0);
            EliminateValues(expectedValue, expectedTechniqueB, 1);

            var actualTechqniues = _row.FindMinTechniques(_cells[0].ToEnumerable(), expectedValue);
            AssertSetEqual(expectedTechniqueB.ToEnumerable(), actualTechqniues);
        }

        private void EliminateValues(int value, ISudokuModel technique, params int[] indexes)
        {
            var cells = _cells.ToList();
            foreach (int index in indexes)
            {
                cells[index].EliminatePossibleValue(value, technique);
            }
        }
    }
}
