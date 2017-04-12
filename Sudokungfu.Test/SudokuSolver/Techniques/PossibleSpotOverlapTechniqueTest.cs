using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.Techniques
{
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
        [TestMethod]
        public void TestTwoOverlappingCells()
        {
            var testValue = 8;
            var cells = GetAllCells().ToList();
            var box = new Box(cells, 0);
            var row = new Row(cells, 0);
            var expectedCells = new List<Cell>() { cells[0], cells[2] };
            var expectedComplexity = 2;
            var expectedIndexValueMap = box.Indexes().ToDictionary(i => i, i => expectedCells.Indexes().Contains(i) ? testValue.ToEnumerable() : Enumerable.Empty<int>());
            var expectedAffectedIndexes = row.Cells.Except(box.Cells).Indexes();
            var testTechnique = new TestTechnique()
            {
                AffectedIndexes = box.Cells.Except(expectedCells).Indexes()
            };
            var expectedTechnique = new TestTechnique()
            {
                Complexity = expectedComplexity,
                IndexValueMap = expectedIndexValueMap,
                AffectedIndexes = expectedAffectedIndexes,
                Details = testTechnique.ToEnumerable(),
                ClickableModel = new TestTechnique()
                {
                    Complexity = expectedComplexity,
                    IndexValueMap = expectedIndexValueMap,
                    AffectedIndexes = expectedAffectedIndexes,
                    Details = testTechnique.ToEnumerable()
                }
            };

            EliminatePossibleValues(testTechnique, cells, testValue,  1, 9, 10, 11, 18, 19, 20);
            PossibleSpotOverlapTechnique.Apply(cells, new Set[] { box, row });

            Assert.IsTrue(row.PossibleSpots[testValue].SetEqual(expectedCells));
            foreach (var cell in row.Cells.Except(box.Cells))
            {
                AssertITechniqueEqual(expectedTechnique, cell.EliminationTechniques[testValue].First());
            }
        }

        [TestMethod]
        public void TestThreeOverlappingCells()
        {
            var testValue = 8;
            var cells = GetAllCells().ToList();
            var box = new Box(cells, 0);
            var row = new Row(cells, 0);
            var expectedCells = new List<Cell>() { cells[0], cells[1], cells[2] };
            var expectedComplexity = 2;
            var expectedIndexValueMap = box.Indexes().ToDictionary(i => i, i => expectedCells.Indexes().Contains(i) ? testValue.ToEnumerable() : Enumerable.Empty<int>());
            var expectedAffectedIndexes = row.Cells.Except(box.Cells).Indexes();
            var testTechnique = new TestTechnique()
            {
                AffectedIndexes = box.Cells.Except(expectedCells).Indexes()
            };
            var expectedTechnique = new TestTechnique()
            {
                Complexity = expectedComplexity,
                IndexValueMap = expectedIndexValueMap,
                AffectedIndexes = expectedAffectedIndexes,
                Details = testTechnique.ToEnumerable(),
                ClickableModel = new TestTechnique()
                {
                    Complexity = expectedComplexity,
                    IndexValueMap = expectedIndexValueMap,
                    AffectedIndexes = expectedAffectedIndexes,
                    Details = testTechnique.ToEnumerable()
                }
            };

            EliminatePossibleValues(testTechnique, cells, testValue, 9, 10, 11, 18, 19, 20);
            PossibleSpotOverlapTechnique.Apply(cells, new Set[] { box, row });

    
            Assert.IsTrue(row.PossibleSpots[testValue].SetEqual(expectedCells));
            foreach (var cell in row.Cells.Except(box.Cells))
            {
                AssertITechniqueEqual(expectedTechnique, cell.EliminationTechniques[testValue].First());
            }
        }

        [TestMethod]
        public void TestTwoOverlappingCellsWithOneNonOverlapping()
        {
            var testValue = 8;
            var cells = GetAllCells().ToList();
            var box = new Box(cells, 0);
            var row = new Row(cells, 0);

            var testTechnique = new TestTechnique();

            EliminatePossibleValues(testTechnique, cells, testValue, 1, 10, 11, 12, 13, 14);
            PossibleSpotOverlapTechnique.Apply(cells, new Set[] { box, row });

            var expectedCells = new List<Cell>() { cells[0], cells[2], cells[3], cells[4], cells[5], cells[6], cells[7], cells[8] };

            Assert.IsTrue(row.PossibleSpots[testValue].SequenceEqual(expectedCells));
        }

        [TestMethod]
        public void TestTechniqueNotAppliedToSourceSet()
        {
            var testValue = 8;
            var cells = GetAllCells().ToList();
            var box = new Box(cells, 0);
            var row = new Row(cells, 0);
            var expectedTechnique = new TestTechnique();

            EliminatePossibleValues(expectedTechnique, cells, testValue, 1, 9, 10, 11, 12, 13, 14);
            PossibleSpotOverlapTechnique.Apply(cells, new Set[] { box, row });

            var actualTechnique = cells[1].EliminationTechniques[testValue].FirstOrDefault();
            Assert.AreEqual(expectedTechnique, actualTechnique);
        }

        [TestMethod]
        public void TestTechniqueRequiresAtLeastTwoCells()
        {
            var testValue = 8;
            var cells = GetAllCells().ToList();
            var box = new Box(cells, 0);
            var row = new Row(cells, 0);
            var expectedCells = row.Cells.Except(cells[1], cells[2]);

            var testTechnique = new TestTechnique();

            EliminatePossibleValues(testTechnique, cells, testValue, 1, 2, 9, 10, 11, 18, 19, 20);
            PossibleSpotOverlapTechnique.Apply(cells, new Set[] { box, row });

            Assert.IsTrue(row.PossibleSpots[testValue].SetEqual(expectedCells));
        }
    }
}
