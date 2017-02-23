using System.Collections.Generic;

namespace Sudokungfu.SudokuSolver.Techniques
{
    /// <summary>
    /// Interface that represents a technique used to eliminate a possible value from a cell in the Sudoku.
    /// </summary>
    public interface ITechnique
    {
        /// <summary>
        /// Complexity of the technique. 
        /// </summary>
        /// <remarks>
        /// Basic techniques will have a lower complexity and advanced techniques will have a higher complexity.
        /// </remarks>
        int Complexity { get; }

        /// <summary>
        /// Indexes of the cells that are part of the technique and the values that go in them.
        /// </summary>
        IDictionary<int, IEnumerable<int>> IndexValueMap { get; }

        /// <summary>
        /// Indexes of cells that had values eliminanated by this technique.
        /// </summary>
        IEnumerable<int> AffectedIndexes { get; }

        /// <summary>
        /// Returns true if the technique uses values that have already been found in the Sudoku.
        /// </summary>
        bool UsesFoundValues { get; }
    }
}
