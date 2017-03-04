using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;

namespace Sudokungfu.Test.SudokuGrid
{
    using View;

    /// <summary>
    /// Test class for <see cref="CellBorderConverter"/>
    /// </summary>
    [TestClass]
    public class CellBorderConverterTest
    {
        /// <summary>
        /// Test class that reprsents a WPF border value.
        /// </summary>
        private class BorderValues
        {
            public int Right;
            public int Top;
            public int Left;
            public int Bottom;

            /// <summary>
            /// Creates a new <see cref="BorderValues"/> from a WPF border string.
            /// </summary>
            public static BorderValues FromString(string border)
            {
                var borderValues = new BorderValues();

                var borderSplit = border.Split(' ');
                Assert.AreEqual(4, borderSplit.Count());
                Assert.IsTrue(int.TryParse(borderSplit[0], out borderValues.Left));
                Assert.IsTrue(int.TryParse(borderSplit[1], out borderValues.Top));
                Assert.IsTrue(int.TryParse(borderSplit[2], out borderValues.Right));
                Assert.IsTrue(int.TryParse(borderSplit[3], out borderValues.Bottom));

                return borderValues;
            }
        }

        private static CellBorderConverter _cellBorderConverter;

        /// <summary>
        /// Run before all tests in the class.
        /// </summary>
        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            _cellBorderConverter = new CellBorderConverter();
        }

        /// <summary>
        /// Test with argument that is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullArgument()
        {
            _cellBorderConverter.Convert(null, null, null, null);
        }

        /// <summary>
        /// Test with argument that is not an int.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestWrongTypeArgument()
        {
            _cellBorderConverter.Convert("test", null, null, null);
        }

        /// <summary>
        /// Test with argument below the expected range.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestArgumentOutOfRangeBelow()
        {
            _cellBorderConverter.Convert(-1, null, null, null);
        }

        /// <summary>
        /// Test with argument above the expected range.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestArgumentOutOfRangeAbove()
        {
            _cellBorderConverter.Convert(Constants.CELL_COUNT, null, null, null);
        }

        /// <summary>
        /// Test that the correct borders are generated when creating a Sudoku grid. This test works by
        /// generating the borders for all cells in the grid and encoding the border values to characters.
        /// The expected sequence of characters was generated from a grid with correct borders based on 
        /// visual inspection.
        /// </summary>
        [TestMethod]
        public void TestBorders()
        {
            var expectedEncodedSequence = "ccbbacbbaccbacbbacbbaccbacbbacbbaccbcabbaabbaacbaabbaabbaacbaabbaabbaacbcabcaabcaaccaabcaabcaaccaabcaabcaacccabbaabbaacbaabbaabbaacbaabbaabbaacbcabbaabbaacbaabbaabbaacbaabbaabbaacbcabcaabcaaccaabcaabcaaccaabcaabcaacccabbaabbaacbaabbaabbaacbaabbaabbaacbcabbaabbaacbaabbaabbaacbaabbaabbaacbcabcaabcaaccaabcaabcaaccaabcaabcaacc";
            var actualEncodedSequence = new StringBuilder();
            for (int i = 0; i < Constants.CELL_COUNT; i++)
            {
                var borderValues = GetBorderValues(i);
                actualEncodedSequence.Append(EncodeBorderValue(borderValues.Left));
                actualEncodedSequence.Append(EncodeBorderValue(borderValues.Top));
                actualEncodedSequence.Append(EncodeBorderValue(borderValues.Right));
                actualEncodedSequence.Append(EncodeBorderValue(borderValues.Bottom));
            }

            Assert.AreEqual(expectedEncodedSequence, actualEncodedSequence.ToString());
        }

        /// <summary>
        /// Generates a <see cref="BorderValues"/> based on a cell index.
        /// </summary>
        /// <param name="index">The cell index.</param>
        /// <returns>The <see cref="BorderValues"/></returns>
        private static BorderValues GetBorderValues(int index)
        {
            var wpfBorder = _cellBorderConverter.Convert(index, null, null, null);
            return BorderValues.FromString(wpfBorder.ToString());
        }

        /// <summary>
        /// Encode a border value to a character.
        /// </summary>
        /// <param name="value">The border value.</param>
        /// <returns>The encoded character.</returns>
        private static string EncodeBorderValue(int value)
        {
            if (value == CellBorderConverter.NO_WIDTH)
            {
                return "a";
            }

            if (value == CellBorderConverter.THIN_WIDTH)
            {
                return "b";
            }

            if (value == CellBorderConverter.THICK_WIDTH)
            {
                return "c";
            }

            return "d";
        }
    }
}
