using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test
{
    using Model;
    using Sudokungfu.Extensions;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.FoundValues;
    using Sudokungfu.SudokuSolver.Sets;
    using Sudokungfu.SudokuSolver.Techniques;

    /// <summary>
    /// Base test class for all other tests.
    /// </summary>
    public class BaseTest
    {
        /// <summary>
        /// Gets all 81 cells.
        /// </summary>
        public static IEnumerable<Cell> GetAllCells()
        {
            var cells = new List<Cell>();
            for (int i = 0; i < Constants.CELL_COUNT; i++)
            {
                cells.Add(new Cell(i));
            }

            return cells;
        }

        /// <summary>
        /// Asserts two sequences are equal sets.
        /// </summary>
        /// <param name="first">First sequence.</param>
        /// <param name="second">Second sequence.</param>
        public static void AssertSetEqual<T>(IEnumerable<T> first, IEnumerable<T> second)
        {
            var secondList = second.ToList();
            Assert.AreEqual(first.Count(), second.Count(), "length");

            foreach (var item in first)
            {
                if (secondList.Contains(item))
                {
                    secondList.Remove(item);
                }
                else
                {
                    Assert.Fail($"Second enumerable does not contain element: {item}");
                }
            }

            if (secondList.Any())
            {
                Assert.Fail($"Second enumerable contained {secondList.Count} unexpected elements: {string.Join(", ", secondList)}");
            }
        }

        /// <summary>
        /// Compares two <see cref="ISudokuModel"/>s and asserts they are equal.
        /// </summary>
        /// <param name="expected">Expected model.</param>
        /// <param name="actual">Actual model.</param>
        public static void AssertISudokuModelEqual(ISudokuModel expected, ISudokuModel actual, bool isClickableModel = false)
        {
            if (expected == null)
            {
                Assert.IsNull(actual);
                return;
            }

            Assert.IsNotNull(actual);

            if (expected.IndexValueMap == null)
            {
                Assert.IsNull(actual.IndexValueMap);
            }
            else
            {
                Assert.IsNotNull(actual.IndexValueMap);

                Assert.IsTrue(actual.IndexValueMap.Keys.SetEqual(expected.IndexValueMap.Keys));
                foreach (var index in expected.IndexValueMap.Keys)
                {
                    Assert.IsTrue(actual.IndexValueMap[index].SetEqual(expected.IndexValueMap[index]));
                }
            }

            if (expected.AffectedIndexes == null)
            {
                Assert.IsNull(actual.AffectedIndexes);
            }
            else
            {
                Assert.IsNotNull(actual.AffectedIndexes);
                Assert.IsTrue(actual.AffectedIndexes.SetEqual(expected.AffectedIndexes));
            }

            if (expected.Details == null)
            {
                Assert.IsNull(actual.Details);
            }
            else
            {
                Assert.IsNotNull(actual.Details);
                
                var expectedDetails = expected.Details.ToList();
                var actualDetails = actual.Details.ToList();

                Assert.AreEqual(expectedDetails.Count, actualDetails.Count);
                for (int i = 0; i < expectedDetails.Count; i++)
                {
                    AssertISudokuModelEqual(expectedDetails[i], actualDetails[i]);
                }
            }

            if (!isClickableModel)
            {
                if (expected.ClickableModel == null)
                {
                    Assert.IsNull(actual.ClickableModel);
                }
                else
                {
                    AssertISudokuModelEqual(expected.ClickableModel, actual.ClickableModel, true);
                }
            }  
        }

       /* /// <summary>
        /// Eliminates a value from a group of cells using a technique.
        /// </summary>
        /// <param name="technique">Technique to use.</param>
        /// <param name="cells">Cells in the Sudoku.</param>
        /// <param name="value">Value to eliminate.</param>
        /// <param name="indexes">Indexes of the cells to eliminate the value from.</param>
        public static void EliminatePossibleValues(ITechnique technique, List<Cell> cells, int value, params int[] indexes)
        {
            foreach (var i in indexes)
            {
                cells[i].EliminatePossibleValue(value, technique);
            }
        }*/
    }

    /// <summary>
    ///  Implementation of <see cref="ISudokuModel"/> for testing purposes.
    /// </summary>
    internal class TestModel : ISudokuModel
    {
        public IDictionary<int, IEnumerable<int>> IndexValueMap { get; set; }

        public IEnumerable<ISudokuModel> Details { get; set; }

        public IEnumerable<int> AffectedIndexes { get; set; }

        public ISudokuModel ClickableModel { get; set; }

        public int Complexity { get; set; }
    }

    /// <summary>
    ///  Sub-class of <see cref="FoundValueBase"/> for testing purposes.
    /// </summary>
    internal class TestFoundValue : FoundValueBase
    {
        public TestFoundValue() : base(0, 0)
        {
        }

        public TestFoundValue(int index, int value) : base (index, value)
        {
        }

        public IEnumerable<ISudokuModel> Techniques
        {
            set
            {
                _techniques.AddRange(value);
            }
        }
    }

    /// <summary>
    ///  Sub-class of <see cref="Set"/> for testing purposes.
    /// </summary>
    internal class TestSet : Set
    {
        public TestSet() : base(BaseTest.GetAllCells(), 0)
        {

        }

        protected override bool IsCellInSet(Cell cell)
        {
            return false;
        }
    }

    /// <summary>
    ///  Sub-class of <see cref="TechniqueBase"/> for testing purposes.
    /// </summary>
    internal class TestTechnique : TechniqueBase
    {
        public TestTechnique(int defaultComplexity) : base(defaultComplexity)
        {

        }

        public IEnumerable<ISudokuModel> Techniques { get; set; }

        public override IEnumerable<ISudokuModel> Details
        {
            get
            {
                return Techniques;
            }
        }
    }
}
