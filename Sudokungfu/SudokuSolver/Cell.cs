using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver
{
    using Extensions;
    using Sets;

    /// <summary>
    /// Class that represents a cell in the Sudoku grid.
    /// </summary>
    public class Cell
    {
        private HashSet<int> _possibleValues;

        /// <summary>
        /// The index of the cell.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// The sets that this cell belongs to.
        /// </summary>
        public List<Set> Sets { get; private set; }
        
        /// <summary>
        /// Creates a new <see cref="Cell"/>.
        /// </summary>
        /// <param name="index">The index of the cell.</param>
        public Cell(int index)
        {
            _possibleValues = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Index = index;
            Sets = new List<Set>();
        }

        /// <summary>
        /// Eliminates a possible value from this cell.
        /// </summary>
        /// <param name="value">The value to eliminate.</param>
        public void EliminatePossibleValue(int value)
        {
            _possibleValues.Remove(value);
        }

        /// <summary>
        /// Gets the values that can possibly go in this cell.
        /// </summary>
        public IEnumerable<int> GetPossibleValues()
        {
            return _possibleValues;
        }

        /// <summary>
        /// Inserts a value into this cell.
        /// </summary>
        /// <param name="value">The value to insert.</param>
        public void InsertValue(int value)
        {
            _possibleValues.Clear();
            Sets.SelectMany(s => s.Cells).Where(c => c != this).ForEach(c => c.EliminatePossibleValue(value));
        }
    }
}
