using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.Techniques
{
    using Sudokungfu.SudokuSolver.Techniques;

    /// <summary>
    /// Test class for <see cref="OccupiedTechnqiueTest"/>.
    /// </summary>
    [TestClass]
    public class OccupiedTechnqiueTest : BaseTest
    {
        [TestMethod]
        public void TestClickableModel()
        {
            var expectedClickableModel = new TestFoundValue();
            var technique = new OccupiedTechnique(expectedClickableModel);

            Assert.AreSame(expectedClickableModel, technique.ClickableModel);
        }

        [TestMethod]
        public void TestIndexValueMap()
        {
            var expectedKey = 34;
            var expectedValue = 4;

            var technique = new OccupiedTechnique(new TestFoundValue(expectedKey, expectedValue));

            Assert.AreEqual(1, technique.IndexValueMap.Count());
            Assert.IsTrue(technique.IndexValueMap.ContainsKey(expectedKey));
            Assert.AreEqual(1, technique.IndexValueMap[expectedKey].Count());
            Assert.AreEqual(expectedValue, technique.IndexValueMap[expectedKey].First());
        }
    }
}
