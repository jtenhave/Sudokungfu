using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.Techniques
{
    using Sudokungfu.SudokuSolver.Techniques;
    using Sudokungfu.SudokuSolver.Sets;
    using Sudokungfu.Extensions;

    /// <summary>
    /// Test class for <see cref="SetTechniqueTest"/>.
    /// </summary>
    [TestClass]
    public class SetTechniqueTest : BaseTest
    {
        [TestMethod]
        public void TestClickableModel()
        {
            var expectedClickableModel = new TestFoundValue();
            var technique = new SetTechnique(expectedClickableModel, new TestSet());

            Assert.AreSame(expectedClickableModel, technique.ClickableModel);
        }

        [TestMethod]
        public void TestIndexValueMap()
        {
            var expectedKey = 9;
            var expectedValue = 4;
            var box = new Box(GetAllCells(), 0);

            var technique = new SetTechnique(new TestFoundValue(expectedKey, expectedValue), box);

            AssertSetEqual(box.Indexes, technique.IndexValueMap.Keys);
            foreach (var key in technique.IndexValueMap.Keys.Except(expectedKey))
            {
                Assert.AreEqual(0, technique.IndexValueMap[key].Count());
            }
            Assert.AreEqual(1, technique.IndexValueMap[expectedKey].Count());
            Assert.AreEqual(expectedValue, technique.IndexValueMap[expectedKey].First());
        }
    }
}
