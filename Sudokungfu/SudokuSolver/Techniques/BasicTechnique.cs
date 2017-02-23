using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Techniques
{
    using Extensions;

    /// <summary>
    /// Class that represents the basic techniques for eliminating a possile value from a Sudoku cell.
    /// </summary>
    public class BasicTechnique : ITechnique
    {
        /// <summary>
        /// Complexity of the technique. 
        /// </summary>
        /// <remarks>
        /// Basic techniques will have a lower complexity and advanced techniques will have a higher complexity.
        /// </remarks>
        public int Complexity { get; protected set; }

        /// <summary>
        /// Indexes of the cells that are part of the technique and the values that go in them.
        /// </summary>
        public IDictionary<int, IEnumerable<int>> IndexValueMap { get; protected set; }

        /// <summary>
        /// Indexes of cells that had values eliminanated by this technique.
        /// </summary>
        public IEnumerable<int> AffectedIndexes { get; protected set; }

        /// <summary>
        /// Returns true if the technique uses values that have already been found in the Sudoku.
        /// </summary>
        public bool UsesFoundValues { get; protected set; }

        protected BasicTechnique()
        {           
        }

        /// <summary>
        /// Creates a <see cref="BasicTechnique"/> that represents a possible value being eliminated from a cell
        /// because that cell already contains a value.
        /// </summary>
        /// <param name="value">Value that the cell contains.</param>
        /// <param name="index">Index of the cell.</param>
        public static BasicTechnique CreateOccupiedTechnique(int value, int index)
        {
            return new BasicTechnique()
            {
                Complexity = 0,
                IndexValueMap = new Dictionary<int, IEnumerable<int>>()
                {
                    [index] = value.ToEnumerable()
                },
                AffectedIndexes = index.ToEnumerable(),
                UsesFoundValues = true
            };
        }

        /// <summary>
        /// Creates a <see cref="BasicTechnique"/> that represents a possible value being eliminated from a cell
        /// because one of that cells member sets already contains the value.
        /// </summary>
        /// <param name="value">Value that already exists in one of the member sets.</param>
        /// <param name="index">Index of the cell that holds the value.</param>
        /// <param name="setIndexes">Indxes of the member set cells.</param>

        public static BasicTechnique CreateSetTechnique(int value, int index, IEnumerable<int> setIndexes)
        {
            var technique = new BasicTechnique()
            {
                Complexity = 1,
                IndexValueMap = setIndexes.ToDictionary(i => i, i =>
                {
                    if (i == index)
                    {
                        return value.ToEnumerable();
                    }

                    return Enumerable.Empty<int>();
                }),
                AffectedIndexes = setIndexes.Except(index),
                UsesFoundValues = true
            };

            return technique;
        }
    }
}
