using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.FoundValues
{
    using Model;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Techniques;

    /// <summary>
    /// Test class for <see cref="FoundValueBase"/>.
    /// </summary>
    [TestClass]
    public class FoundValueTest
    {
        [TestMethod]
        public void TestDefaultComplexityForNoDetails()
        {
            var foundValue = new FoundValue(new Cell(0), 1);

            Assert.AreEqual(0, foundValue.Complexity);
        }

        [TestMethod]
        public void TestMaxDetailsComplexity()
        {
            var expectedComplexity = 5;
            var techniqueA = new Technique();
            techniqueA.Complexity = expectedComplexity;
            var techniqueB = new Technique();
            techniqueB.Complexity = 1;

            var foundValue = new FoundValue(new Cell(0), 1);
            foundValue.Techniques.Add(techniqueA);
            foundValue.Techniques.Add(techniqueB);

            Assert.AreEqual(expectedComplexity, foundValue.Complexity);
        }

        [TestMethod]
        public void TestSetComplexityOverridesMaxDetails()
        {
            var expectedComplexity = 2;
            var techniqueA = new Technique();
            techniqueA.Complexity = 5;
            var techniqueB = new Technique();
            techniqueB.Complexity = 1;

            var foundValue = new FoundValue(new Cell(0), 1);
            foundValue.Techniques.Add(techniqueA);
            foundValue.Techniques.Add(techniqueB);
            foundValue.Complexity = expectedComplexity;

            Assert.AreEqual(expectedComplexity, foundValue.Complexity);
        }
    }
}
