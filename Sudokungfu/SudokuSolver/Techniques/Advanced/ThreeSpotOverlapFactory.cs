using System.Collections.Generic;

namespace Sudokungfu.SudokuSolver.Techniques.Advanced
{
    using Sets;

    /// <summary>
    /// Advanced technique factory for the Three Spot Overlap technique.
    /// </summary>
    public class ThreeSpotOverlapFactory : TwoSpotOverlapFactory
    {
        public new const int COMPLEXITY = 3;

        protected override int OverlapSize
        {
            get
            {
                return 3;
            }
        }

        protected override int Complexity
        {
            get
            {
                return COMPLEXITY;
            }
        }

        /// <summary>
        /// Creates a new <see cref="ThreeSpotOverlapFactory"/>.
        /// </summary>
        /// <param name="cells">Cells in the Sudoku.</param>
        /// <param name="sets">Sets in the Sudoku.</param>
        public ThreeSpotOverlapFactory(IEnumerable<Cell> cells, IEnumerable<Set> sets) : base(cells, sets)
        {
        }
    }
}
