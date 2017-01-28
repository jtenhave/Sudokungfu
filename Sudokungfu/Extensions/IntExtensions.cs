
namespace Sudokungfu.Extensions
{
    /// <summary>
    /// Class for int extensions.
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        /// Returns true if an int is a valid sudoku index.
        /// </summary>
        public static bool IsSudokuIndex(this int value)
        {
            return value >= 0 && value < Constants.CELL_COUNT;
        }
    }
}
