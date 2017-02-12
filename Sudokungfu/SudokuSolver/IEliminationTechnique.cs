using System.Collections.Generic;

namespace Sudokungfu.SudokuSolver
{
    public interface IEliminationTechnique
    {
        /// <summary>
        /// Complexity of the technique. 
        /// </summary>
        /// <remarks>
        /// Basic techniques will have a lower complexity and advanced techniques will have a higher complexity.
        /// </remarks>
        int Complexity { get; }

        /// <summary>
        /// Indexes of cells that should be shown when this technique is displayed.
        /// </summary>
        IEnumerable<int> Indexes { get; }

        /// <summary>
        /// Values, and the indexes of the cells they should be in, that should be shown when this technique is displayed.
        /// </summary>
        IDictionary<int, IEnumerable<int>> ValueMap { get;  }

    }
}
