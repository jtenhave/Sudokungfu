
namespace Sudokungfu.SudokuSolver
{
    /// <summary>
    /// Class that represents a found value in the Sudoku.
    /// </summary>
    public class FoundValue
    {
        /// <summary>
        /// The index of the value.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// The value.
        /// </summary>
        public int Value { get; set; }
    }
}
