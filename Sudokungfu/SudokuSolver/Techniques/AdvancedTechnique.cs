using System.Collections.Generic;

namespace Sudokungfu.SudokuSolver.Techniques
{
    using Sets;

    /// <summary>
    /// Class that represents an advanced technique for eliminating possible values from a cell in the Sudoku.
    /// </summary>
    public abstract class AdvancedTechnique : BasicEliminationTechnique, IEliminationTechnique
    {
        /// <summary>
        /// Applies the technique to the Sudoku.
        /// </summary>
        /// <param name="cells">Cells in the Sudoku.</param>
        /// <param name="sets">Sets of cells in the Sudoku.</param>
        public abstract void Apply(IEnumerable<Cell> cells, IEnumerable<Set> sets);
    }
}
