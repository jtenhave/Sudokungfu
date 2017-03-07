using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.Techniques
{
    using Sudokungfu.Extensions;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;
    using Sudokungfu.SudokuSolver.Techniques;

    /// <summary>
    /// Test class for <see cref="ThreeSpotClosureTechnique"/>.
    /// </summary>
    [TestClass]
    public class ThreeSpotClosureTechniqueTest : BaseTest
    {
        [TestMethod]
        public void TestThreeValueClosureWithThreeTriples()
        {
            var testValueA = 3;
            var testValueB = 4;
            var testValueC = 5;
            var cells = GetAllCells().ToList();
            var box = new Box(cells, 0);
            var expectedCells = new Cell[] { cells[0], cells[1], cells[2] };
            var expectedValues = new int[] { testValueA, testValueB, testValueC };
            var expectedComplexity = 3;
            var expectedIndexValueMap = box.Indexes().ToDictionary(i => i, i => expectedCells.Indexes().Contains(i) ? expectedValues : Enumerable.Empty<int>());
            var expectedAffectedIndexes = expectedCells.Indexes();
            var expectedTechnique = new TestTechnique()
            {
                Complexity = expectedComplexity,
                IndexValueMap = expectedIndexValueMap,
                AffectedIndexes = expectedAffectedIndexes,
                ClickableModel = new TestTechnique()
                {
                    Complexity = expectedComplexity,
                    IndexValueMap = expectedIndexValueMap,
                    AffectedIndexes = expectedAffectedIndexes
                }
            };

            var testTechnique = new TestTechnique();
            foreach(var cell in box.Cells.Except(expectedCells))
            {
                cell.EliminatePossibleValue(testValueA, testTechnique);
                cell.EliminatePossibleValue(testValueB, testTechnique);
                cell.EliminatePossibleValue(testValueC, testTechnique);
            }

            ThreeSpotClosureTechnique.Apply(cells, new Set[] { box });

            foreach(var cell in expectedCells)
            {
                Assert.IsTrue(cell.PossibleValues.SetEqual(expectedValues));
                foreach (var value in Constants.ALL_VALUES.Except(expectedValues))
                {
                    AssertITechniqueEqual(expectedTechnique, cell.EliminationTechniques[value].First());
                }
            }
        }

        [TestMethod]
        public void TestThreeValueClosureWithThreePairs()
        {
            var testValueA = 1;
            var testValueB = 2;
            var testValueC = 3;
            var cells = GetAllCells().ToList();
            var box = new Row(cells, 0);
            var expectedCells = new Cell[] { cells[1], cells[2], cells[3] };
            var expectedValues = new int[] { testValueA, testValueB, testValueC };
            var expectedComplexity = 3;
            var expectedIndexValueMap = box.Indexes().ToDictionary(i => i, i => expectedCells.Indexes().Contains(i) ? expectedValues.Except(i) : Enumerable.Empty<int>());
            var expectedAffectedIndexes = expectedCells.Indexes();
            var expectedTechnique = new TestTechnique()
            {
                Complexity = expectedComplexity,
                IndexValueMap = expectedIndexValueMap,
                AffectedIndexes = expectedAffectedIndexes,
                ClickableModel = new TestTechnique()
                {
                    Complexity = expectedComplexity,
                    IndexValueMap = expectedIndexValueMap,
                    AffectedIndexes = expectedAffectedIndexes
                }
            };

            var testTechnique = new TestTechnique();
            foreach (var cell in box.Cells.Except(expectedCells))
            {
                cell.EliminatePossibleValue(testValueA, testTechnique);
                cell.EliminatePossibleValue(testValueB, testTechnique);
                cell.EliminatePossibleValue(testValueC, testTechnique);
            }

            cells[1].EliminatePossibleValue(testValueA, testTechnique);
            cells[2].EliminatePossibleValue(testValueB, testTechnique);
            cells[3].EliminatePossibleValue(testValueC, testTechnique);

            ThreeSpotClosureTechnique.Apply(cells, new Set[] { box });

            foreach (var cell in expectedCells)
            {
                Assert.IsTrue(cell.PossibleValues.SetEqual(expectedValues.Except(cell.Index)));
                foreach (var value in Constants.ALL_VALUES.Except(expectedValues))
                {
                    AssertITechniqueEqual(expectedTechnique, cell.EliminationTechniques[value].First());
                }
            }
        }
    }
}
