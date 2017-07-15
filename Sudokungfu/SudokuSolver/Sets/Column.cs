using System.Collections.Generic;

namespace Sudokungfu.SudokuSolver.Sets
{
    /// <summary>
    /// Class that represents a column of nine values in a Sudoku.
    /// </summary>
    public class Column : Set
    {
        /// <summary>
        /// Creates a new <see cref="Column"/>
        /// </summary>
        /// <param name="grid">The Sudoku grid to create the column from.</param>
        /// <param name="index">The index of the column.</param>
        public Column(IEnumerable<Cell> grid, int index) : base(grid, index)
        {
        }

        /// <summary>
        /// Returns true if a cell is in this column.
        /// </summary>
        protected override bool IsCellInSet(Cell cell)
        {
            return cell.Index % Constants.SET_SIZE == _index;
        }
    }
}
