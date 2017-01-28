using System.Collections.Generic;

namespace Sudokungfu.SudokuSolver
{
    /// <summary>
    /// Enum to represent the type of the result.
    /// </summary>
    public enum SudokuResultType
    {
        SUCCESS,
        INVALID,
        ERROR
    }

    /// <summary>
    /// Class to represent the result of solving a Sudoku.
    /// </summary>
    public class SolveResult
    {
        /// <summary>
        /// The type of the result.
        /// </summary>
        public SudokuResultType Type { get; set; }

        /// <summary>
        /// The list of found values.
        /// </summary>
        public IEnumerable<FoundValue> FoundValues { get; set; }

        /// <summary>
        /// The error message if one occured.
        /// </summary>
        public string Error { get; set; }
    }
}
