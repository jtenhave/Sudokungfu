using System;

namespace Sudokungfu.SudokuGrid
{
    using Extensions;

    /// <summary>
    /// Class for the view model of a cell in the Sudoku grid.
    /// </summary>
    /// <remarks>
    /// Cells are indexed horizontally (e.g. The first row in the grid is indexed 0-8).
    /// </remarks>
    public class CellViewModel
    {
        /// <summary>
        /// The index of the cell. 
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// The value of the cell.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Creates a new <see cref="CellViewModel"/>
        /// </summary>
        /// <param name="index">The index of the cell.</param>
        public CellViewModel(int index)
        {
            if (!index.IsSudokuIndex())
            {
                throw new ArgumentOutOfRangeException("index must be between 0 and 80");
            }

            Index = index;
            Value = index.ToString();
        }
    }
}
