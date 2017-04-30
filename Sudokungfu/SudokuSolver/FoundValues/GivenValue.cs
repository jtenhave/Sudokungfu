
namespace Sudokungfu.SudokuSolver.FoundValues
{
    /// <summary>
    /// Class that represents a given value in the Sudoku.
    /// </summary>
    public class GivenValue : FoundValueBase
    {
        /// <summary>
        /// Creates a new <see cref="GivenValue"/>.
        /// </summary>
        /// <param name="index">Index of the given value.</param>
        /// <param name="value">Value that was given.</param>
        public GivenValue(int index, int value) : base(index, value)
        {

        }
    }
}
