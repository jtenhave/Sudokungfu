using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Sets
{
    using Extensions;

    /// <summary>
    /// Class that represents a set of nine values in a Sudoku.
    /// </summary>
    public abstract class Set
    {
        /// <summary>
        /// The cells in this set.
        /// </summary>
        public IEnumerable<Cell> Cells { get; private set; }

        /// <summary>
        /// The index of the set.
        /// </summary>
        protected int Index { get; private set; }

        /// <summary>
        /// Creates a new <see cref="Set"/>
        /// </summary>
        /// <param name="grid">The Sudoku grid to create the set from.</param>
        /// <param name="index">The index of the set.</param>
        public Set(IEnumerable<Cell> grid, int index)
        {
            Index = index;
            Cells = grid.Where(IsCellInSet).ToList();
            Cells.ForEach(c => c.Sets.Add(this));
        }

        /// <summary>
        /// Gets a dictionary of values to the cells in the set where the value could go.
        /// </summary>
        public IDictionary<int, IEnumerable<Cell>> GetValuePossibleSpots()
        {
            return Cells
                    .SelectMany(c => c.PossibleValues)
                    .Distinct()
                    .ToDictionary(i => i, i => Cells.Where(c => c.PossibleValues.Contains(i)));
        }

        /// <summary>
        /// Returns true if a cell is in this set.
        /// </summary>
        protected abstract bool IsCellInSet(Cell cell);
    }
}
