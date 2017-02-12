using System.Collections.Generic;

namespace Sudokungfu
{
    /// <summary>
    /// Class for global constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The size of a set of values from 1 to 9.
        /// </summary>
        public const int SET_SIZE = 9;

        /// <summary>
        /// The number of cells in a Sudoku grid.
        /// </summary>
        public const int CELL_COUNT = 81;

        /// <summary>
        /// The size of the side of a box.
        /// </summary>
        public const int BOX_SIZE = 3;

        public static readonly IEnumerable<int> ALL_VALUES = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    }
}
