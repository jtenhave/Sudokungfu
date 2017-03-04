using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver
{
    using Sets;

    /// <summary>
    /// Class for Sudoku solver extensions.s
    /// </summary>
    public static class SolverExtensions
    {
        /// <summary>
        /// Returns the indexes of the cells in a set.
        /// </summary>
        /// <param name="set">Set.</param>
        public static IEnumerable<int> Indexes(this Set set)
        {
            return set.Cells.Indexes();
        }

        /// <summary>
        /// Returns the indexes of a sequence of cells.
        /// </summary>
        /// <param name="cells">Cells.</param>
        public static IEnumerable<int> Indexes(this IEnumerable<Cell> cells)
        {
            return cells.Select(c => c.Index);
        }
    }
}
