using System.Collections.Generic;

namespace Sudokungfu.SudokuSolver.Sets
{
    /// <summary>
    /// Class that represents a row of nine values in a Sudoku.
    /// </summary>
    public class Row : Set
    {
        /// <summary>
        /// Type of the set.
        /// </summary>
        public override string Type
        {
            get
            {
                return "row";
            }
        }

        /// <summary>
        /// Creates a new <see cref="Row"/>
        /// </summary>
        /// <param name="grid">The Sudoku grid to create the row from.</param>
        /// <param name="index">The index of the row.</param>
        public Row(IEnumerable<Cell> grid, int index) : base(grid, index)
        {
        }

        /// <summary>
        /// Returns true if a cell is in this row.
        /// </summary>
        protected override bool IsCellInSet(Cell cell)
        {
            return cell.Index / Constants.SET_SIZE == _index;
        }
    }
}
