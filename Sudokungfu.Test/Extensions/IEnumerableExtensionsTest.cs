using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.Extensions
{
    using Sudokungfu.Extensions;

    /// <summary>
    /// Test class for <see cref="IEnumerableExtensions"/>.
    /// </summary>
    [TestClass]
    public class IEnumerableExtensionsTest
    {
        [TestMethod]
        public void TestZipperSequencesSameSize()
        {
            var seqA = new List<int>() { 1, 3, 5, 7 };
            var seqB = new List<int>() { 2, 4, 6, 8 };
            var expectedSeq = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };

            var actualSeq = seqA.Zipper(seqB);
            Assert.IsTrue(actualSeq.SequenceEqual(expectedSeq));
        }

        [TestMethod]
        public void TestZipperFirstSequenceLarger()
        {
            var seqA = new List<int>() { 1, 3, 5, 7 };
            var seqB = new List<int>() { 2, 4 };
            var expectedSeq = new List<int> { 1, 2, 3, 4, 5, 7 };

            var actualSeq = seqA.Zipper(seqB);
            Assert.IsTrue(actualSeq.SequenceEqual(expectedSeq));
        }

        [TestMethod]
        public void TestZipperSecondSequenceLarger()
        {
            var seqA = new List<int>() { 1, 3 };
            var seqB = new List<int>() { 2, 4, 6, 8 };
            var expectedSeq = new List<int> { 1, 2, 3, 4, 6, 8 };

            var actualSeq = seqA.Zipper(seqB);
            Assert.IsTrue(actualSeq.SequenceEqual(expectedSeq));
        }

        [TestMethod]
        public void TestZipperFirstSequenceEmpty()
        {
            var seqA = new List<int>();
            var seqB = new List<int>() { 2, 4, 6, 8 };
            var expectedSeq = new List<int> { 2, 4, 6, 8 };

            var actualSeq = seqA.Zipper(seqB);
            Assert.IsTrue(actualSeq.SequenceEqual(expectedSeq));
        }

        [TestMethod]
        public void TestZipperSecondSequenceEmpty()
        {
            var seqA = new List<int>() { 1, 3, 5, 7 };
            var seqB = new List<int>();
            var expectedSeq = new List<int> { 1, 3, 5, 7 };

            var actualSeq = seqA.Zipper(seqB);
            Assert.IsTrue(actualSeq.SequenceEqual(expectedSeq));
        }
    }
}
