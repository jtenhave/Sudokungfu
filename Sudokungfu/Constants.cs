using System.Collections.Generic;
using System.Windows;

namespace Sudokungfu
{
    /// <summary>
    /// Class for global constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Size of a set of values from 1 to 9.
        /// </summary>
        public const int SET_SIZE = 9;

        /// <summary>
        /// Number of cells in a Sudoku grid.
        /// </summary>
        public const int CELL_COUNT = 81;

        /// <summary>
        /// Size of the side of a box.
        /// </summary>
        public const int BOX_SIZE = 3;

        /// <summary>
        /// All Sudoku values.
        /// </summary>
        public static readonly IEnumerable<int> ALL_VALUES = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        /// <summary>
        /// Default font size for Sudoku cells.
        /// </summary>
        public const int DEFAULT_FONT_SIZE = 36;

        /// <summary>
        /// Default font style for Sudoku cells.
        /// </summary>
        public static readonly FontStyle DEFAULT_FONT_STYLE = FontStyles.Normal;
    }
}
