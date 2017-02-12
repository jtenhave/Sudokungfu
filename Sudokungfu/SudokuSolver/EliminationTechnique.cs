using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver
{
    using Sets;
    using Extensions;
    
    /// <summary>
    /// Class that represents the basic techniques for eliminating a possile value from a Sudoku cell.
    /// </summary>
    public class BasicEliminationTechnique : IEliminationTechnique
    {
        /// <summary>
        /// Complexity of the technique. 
        /// </summary>
        /// <remarks>
        /// Basic techniques will have a lower complexity and advanced techniques will have a higher complexity.
        /// </remarks>
        public int Complexity { get; private set; }

        /// <summary>
        /// Indexes of cells that should be shown when this technique is displayed.
        /// </summary>
        public IEnumerable<int> Indexes { get; private set; }

        /// <summary>
        /// Values, and the indexes of the cells they should be in, that should be shown when this technique is displayed.
        /// </summary>
        public IDictionary<int, IEnumerable<int>> ValueMap { get; private set; }

        private BasicEliminationTechnique()
        {           
        }

        /// <summary>
        /// Creates a <see cref="BasicEliminationTechnique"/> that represents a possible value being eliminated from a cell
        /// because that cell already contains a value.
        /// </summary>
        /// <param name="value">Value that the cell contains.</param>
        /// <param name="index">Index of the cell.</param>
        public static BasicEliminationTechnique CreateOccupiedTechnique(int value, int index)
        {
            return new BasicEliminationTechnique()
            {
                Complexity = 0,
                Indexes = index.ToEnumerable(),
                ValueMap = new Dictionary<int, IEnumerable<int>>() { [value] = index.ToEnumerable() },
            };
        }

        /// <summary>
        /// Creates a <see cref="BasicEliminationTechnique"/> that represents a possible value being eliminated from a cell
        /// because one of that cells member sets already contains the value.
        /// </summary>
        /// <param name="value">Value that already exists in one of the member sets.</param>
        /// <param name="index">Index of the cell where the value is.</param>
        /// <param name="set">Member set that contains the value.</param>
 
        public static BasicEliminationTechnique CreateSetTechnique(int value, int index, Set set)
        {
            return new BasicEliminationTechnique()
            {
                Complexity = 1,
                Indexes = set.Cells.Select(c => c.Index),
                ValueMap = new Dictionary<int, IEnumerable<int>>() { [value] = index.ToEnumerable() },
            };
        }
    }
}
