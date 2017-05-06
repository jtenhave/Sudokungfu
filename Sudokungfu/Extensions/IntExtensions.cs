using System.Collections.Generic;

namespace Sudokungfu.Extensions
{
    /// <summary>
    /// Class for int extensions.
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        /// Returns true if an int is a valid Sudoku index.
        /// </summary>
        public static bool IsSudokuIndex(this int index)
        {
            return index >= 0 && index < Constants.CELL_COUNT;
        }

        /// <summary>
        /// Returns true if an int is valid Sudoku value.
        /// </summary>
        public static bool IsSudokuValue(this int value)
        {
            return value >= 0 && value <= Constants.SET_SIZE;
        }
    }
}
