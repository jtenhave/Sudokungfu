using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Sudokungfu.Test.SudokuSolver.Techniques
{
    using Sudokungfu.Extensions;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;
    using Sudokungfu.SudokuSolver.Techniques;

    /// <summary>
    /// Test class for <see cref="BasicTechnique"/>.
    /// </summary>
    [TestClass]
    public class BasicTechniqueTest : BaseTest
    {
        [TestMethod]
        public void TestCreateOccupiedTechnique()
        {
            var testValue = 3;
            var testIndex = 76;
            var expectedTechnique = new TestTechnique()
            {
                Complexity = 0,
                IndexValueMap = new Dictionary<int, IEnumerable<int>>()
                {
                    [testIndex] = testValue.ToEnumerable()
                },
                AffectedIndexes = testIndex.ToEnumerable()
            };

            var actualTechnique = BasicTechnique.CreateOccupiedTechnique(testValue, testIndex);

            AssertITechniqueEqual(expectedTechnique, actualTechnique);
        }

        [TestMethod]
        public void TestCreateSetTechnique()
        {
            var testValue = 3;
            var testIndex = 9;
            var cells = new Cell[] { new Cell(0), new Cell(1), new Cell(9), new Cell(10) };
            var box = new Box(cells, 0);
            var expectedTechnique = new TestTechnique()
            {
                Complexity = 1,
                IndexValueMap = cells.Indexes().ToDictionary(testIndex, testValue),
                AffectedIndexes = cells.Indexes().Except(testIndex)
            };

            var actualTechnique = BasicTechnique.CreateSetTechnique(testValue, testIndex, box.Indexes());

            AssertITechniqueEqual(expectedTechnique, actualTechnique);
        }
    }
}
