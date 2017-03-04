using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.Techniques
{
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
        [TestMethod]
        public void TestTwoSpotClosure()
        {
            var testValueA = 3;
            var testValueB = 4;
            var cells = GetAllCells().ToList();
            var box = new Box(cells, 0);
            var expectedCells = new Cell[] { cells[0], cells[1] };
            var expectedValues = new int[] { testValueA, testValueB };
            var expectedTechnique = new TestTechnique()
            {
                Complexity = 2,
                IndexValueMap = box.Indexes().ToDictionary(i => i, i => expectedCells.Indexes().Contains(i) ? expectedValues : Enumerable.Empty<int>())
            };

            var testTechnique = new TestTechnique();
            var cellsRemaining = box.Cells.Except(expectedCells);
            foreach (var cell in cellsRemaining)
            {
                cell.EliminatePossibleValue(testValueA, testTechnique);
                cell.EliminatePossibleValue(testValueB, testTechnique);
            }

            TwoSpotClosureTechnique.Apply(cells, new Set[] { box });

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
        public void TestTwoSpotClosureRequiresTwoValues()
        {
            var testValueA = 3;
            var testValueB = 4;
            var cells = GetAllCells().ToList();
            var box = new Box(cells, 0);

            var testTechnique = new TestTechnique();
            cells[1].EliminatePossibleValue(testValueA, testTechnique);
            for (int i = 2; i < cells.Count; i++)
            {
                cells[i].EliminatePossibleValue(testValueA, testTechnique);
                cells[i].EliminatePossibleValue(testValueB, testTechnique);
            }

            TwoSpotClosureTechnique.Apply(cells, new Set[] { box });

            Assert.AreEqual(9, cells[0].PossibleValues.Count());
            Assert.AreEqual(8, cells[1].PossibleValues.Count());
        }
    }
}
