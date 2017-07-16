using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sudokungfu.Test.SudokuSolver.Techniques
{
    using Sudokungfu.SudokuSolver.Techniques;

    /// <summary>
    /// Test class for <see cref="Technique"/>.
    /// </summary>
    [TestClass]
    public class TechniqueTest : BaseTest
    {
        [TestMethod]
        public void TestDefaultComplexityForNoDetails()
        {
            var expectedComplexity = 23;
            var technique = new Technique();
            technique.Complexity = expectedComplexity;

            Assert.AreEqual(expectedComplexity, technique.Complexity);
        }

        [TestMethod]
        public void TestAddedComplexityForDetails()
        {
            var expectedComplexity = 7;
            var techniqueA = new Technique();
            techniqueA.Complexity = 2;
            var techniqueB = new Technique();
            techniqueB.Complexity = 3;

            var technique = new Technique();
            technique.Complexity = 4;
            technique.Techniques.Add(techniqueA);
            technique.Techniques.Add(techniqueB);

            Assert.AreEqual(expectedComplexity, technique.Complexity);
        }
    }
}
