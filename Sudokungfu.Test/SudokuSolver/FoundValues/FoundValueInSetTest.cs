using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.FoundValues
{
    using Sudokungfu.Extensions;
    using Sudokungfu.SudokuSolver.FoundValues;
    using Sudokungfu.SudokuSolver.Sets;

    /// <summary>
    /// Test class for <see cref="FoundInSetValue"/>.
    /// </summary>
    [TestClass]
    public class FoundValueInSetTest : BaseTest
    {
        [TestMethod]
        public void TestIndexValueMap()
        {
            var expectedKey = 0;
            var expectedValue = 4;
            var box = new Box(GetAllCells(), 0);

            var foundValue = new FoundInSetValue(box.Cells.First(), expectedValue, box);

            AssertSetEqual(box.Indexes, foundValue.IndexValueMap.Keys);
            foreach (var key in foundValue.IndexValueMap.Keys.Except(expectedKey))
            {
                Assert.AreEqual(0, foundValue.IndexValueMap[key].Count());
            }
            Assert.AreEqual(1, foundValue.IndexValueMap[expectedKey].Count());
            Assert.AreEqual(expectedValue, foundValue.IndexValueMap[expectedKey].First());
        }
    }
}
