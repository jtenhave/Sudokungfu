using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver
{
    /// <summary>
    /// Class for Sudoku solver extensions.s
    /// </summary>
    public static class SolverExtensions
    {
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
