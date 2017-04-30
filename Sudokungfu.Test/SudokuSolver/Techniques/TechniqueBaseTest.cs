using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.Techniques
{
    using Model;

    /// <summary>
    /// Test class for <see cref="TechniqueBase"/>.
    /// </summary>
    [TestClass]
    public class TechniqueBaseTest : BaseTest
    {
        [TestMethod]
        public void TestInitializeIndexValueMap()
        {
            var technique = new TestTechnique(0);

            Assert.IsNotNull(technique.IndexValueMap);
            Assert.AreEqual(0, technique.IndexValueMap.Count);
        }

        [TestMethod]
        public void TestInitializeAffectedIndexes()
        {
            var technique = new TestTechnique(0);

            Assert.IsNotNull(technique.AffectedIndexes);
            Assert.AreEqual(0, technique.AffectedIndexes.Count());
        }

        [TestMethod]
        public void TestClickableModel()
        {
            var technique = new TestTechnique(0);

            Assert.AreSame(technique, technique.ClickableModel);
        }

        [TestMethod]
        public void TestDefaultComplexityForNoDetails()
        {
            var expectedComplexity = 23;
            var technique = new TestTechnique(expectedComplexity);

            Assert.AreEqual(expectedComplexity, technique.Complexity);
        }

        [TestMethod]
        public void TestHigherMaxDetailsComplexity()
        {
            var expectedComplexity = 5;
            var techniqueA = new TestTechnique(expectedComplexity);
            var techniqueB = new TestTechnique(1);

            var technique = new TestTechnique(3) {
                Techniques = new List<ISudokuModel> { techniqueA, techniqueB }
            };

            Assert.AreEqual(expectedComplexity + 1, technique.Complexity);
        }

        [TestMethod]
        public void TestLowerMaxDetailsComplexity()
        {
            var expectedComplexity = 5;
            var techniqueA = new TestTechnique(1);
            var techniqueB = new TestTechnique(3);

            var technique = new TestTechnique(expectedComplexity)
            {
                Techniques = new List<ISudokuModel> { techniqueA, techniqueB }
            };

            Assert.AreEqual(expectedComplexity, technique.Complexity);
        }
    }
}
