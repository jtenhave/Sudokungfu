using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.FoundValues
{
    using Model;

    /// <summary>
    /// Test class for <see cref="FoundValueBase"/>.
    /// </summary>
    [TestClass]
    public class FoundValueBaseTest
    {
        [TestMethod]
        public void TestInitializeIndexValueMap()
        {
            var foundValue = new TestFoundValue();

            Assert.IsNotNull(foundValue.IndexValueMap);
            Assert.AreEqual(0, foundValue.IndexValueMap.Count);
        }

        [TestMethod]
        public void TestInitializeAffectedIndexes()
        {
            var foundValue = new TestFoundValue();

            Assert.IsNotNull(foundValue.AffectedIndexes);
            Assert.AreEqual(0, foundValue.AffectedIndexes.Count());
        }

        [TestMethod]
        public void TestClickableModel()
        {
            var foundValue = new TestFoundValue();

            Assert.AreSame(foundValue, foundValue.ClickableModel);
        }

        [TestMethod]
        public void TestDefaultComplexityForNoDetails()
        {
            var foundValue = new TestFoundValue();

            Assert.AreEqual(0, foundValue.Complexity);
        }

        [TestMethod]
        public void TestMaxDetailsComplexity()
        {
            var expectedComplexity = 5;
            var techniqueA = new TestTechnique(expectedComplexity);
            var techniqueB = new TestTechnique(1);

            var foundValue = new TestFoundValue()
            {
                Techniques = new List<ISudokuModel> { techniqueA, techniqueB }
            };

            Assert.AreEqual(expectedComplexity, foundValue.Complexity);
        }
    }
}
