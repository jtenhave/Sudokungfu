using System.Collections.Generic;

namespace Sudokungfu.SudokuSolver.Sets
{
    /// <summary>
    /// Class that represents a box of nine values in a Sudoku.
    /// </summary>
    public class Box : Set
    {
        private int _boxRowSize;
        private int _boxRow;
        private int _boxCol;

        /// <summary>
        /// The number of cells in a row of boxes.
        /// </summary>
        private int BoxRowSize
        {
            get
            {
                if (_boxRowSize == 0)
                {
                    _boxRowSize = Constants.BOX_SIZE * Constants.SET_SIZE;
                }

                return _boxRowSize;
            }
        }

        /// <summary>
        /// The row (of boxes) that the box is in.
        /// </summary>
        private int BoxRow
        {
            get
            {
                if (_boxRow == 0)
                {
                    _boxRow = Index / Constants.BOX_SIZE;
                }

                return _boxRow;
            }
        }

        /// <summary>
        /// The column (of boxes) that the box is in.
        /// </summary>
        private int BoxCol
        {
            get
            {
                if (_boxCol == 0)
                {
                    _boxCol = Index % Constants.BOX_SIZE;
                }

                return _boxCol;
            }
        }

        /// <summary>
        /// Creates a new <see cref="Box"/>
        /// </summary>
        /// <param name="grid">The Sudoku grid to create the box from.</param>
        /// <param name="index">The index of the box.</param>
        public Box(IEnumerable<Cell> grid, int index) : base(grid, index)
        {
        }

        /// <summary>
        /// Returns true if a cell is in this box.
        /// </summary>
        protected override bool IsCellInSet(Cell cell)
        {
            return cell.Index / BoxRowSize == BoxRow && (cell.Index / Constants.BOX_SIZE) % Constants.BOX_SIZE == BoxCol;
        }
    }
}
