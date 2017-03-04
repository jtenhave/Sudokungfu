
namespace Sudokungfu.SudokuSolver.Techniques
{
    using Model;

    /// <summary>
    /// Interface that represents a technique used to eliminate a possible value from a cell in the Sudoku.
    /// </summary>
    public interface ITechnique : ISudokuModel
    {
        /// <summary>
        /// Complexity of the technique. 
        /// </summary>
        /// <remarks>
        /// Basic techniques will have a lower complexity and advanced techniques will have a higher complexity.
        /// </remarks>
        int Complexity { get; }
    }
}
