using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.FoundValues
{
    using Sudokungfu.Extensions;
    using Sudokungfu.SudokuSolver.FoundValues;

    /// <summary>
    /// Test class for <see cref="OnlyPossibleValue"/>.
    /// </summary>
    [TestClass]
    public class OnlyPossibleValueTest : BaseTest
    {
        [TestMethod]
        public void TestIndexValueMap()
        {
            var expectedKey = 0;
            var expectedValue = 4;
            var cell = GetAllCells().First();

            var foundValue = new OnlyPossibleValue(cell, expectedValue);

            AssertSetEqual(cell.Index.ToEnumerable(), foundValue.IndexValueMap.Keys);
            Assert.AreEqual(1, foundValue.IndexValueMap[expectedKey].Count());
            Assert.AreEqual(expectedValue, foundValue.IndexValueMap[expectedKey].First());
        }

        [TestMethod]
        public void TestDefaultComplexityForNoDetails()
        {
            var foundValue = new OnlyPossibleValue(GetAllCells().First(), 4);

            Assert.AreEqual(1, foundValue.Complexity);
        }
    }
}
