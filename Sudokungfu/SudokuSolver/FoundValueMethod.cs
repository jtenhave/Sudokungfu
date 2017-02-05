using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver
{
    using Sets;

    /// <summary>
    /// Enum that represents the type of method.
    /// </summary>
    public enum FoundValueMethodType
    {
        GIVEN,
        SET,
        SINGLE
    }

    /// <summary>
    /// Class that represents a method of finding a value in the Sudoku.
    /// </summary>
    public class FoundValueMethod
    {
        /// <summary>
        /// The type of the method.
        /// </summary>
        public FoundValueMethodType Type { get; private set; }

        /// <summary>
        /// The indexes of the set or the index of the only possible value.
        /// </summary>
        public IEnumerable<int> Indexes { get; private set; }

        /// <summary>
        /// Creates a new <see cref="FoundValueMethod"/>.
        /// </summary>
        private FoundValueMethod()
        {

        }

        /// <summary>
        /// Creates a new <see cref="FoundValueMethod"/> for a value that was given.
        /// </summary>
        public static FoundValueMethod CreateGivenMethod()
        {
            return new FoundValueMethod()
            {
                Type = FoundValueMethodType.GIVEN
            };
        }

        /// <summary>
        /// Creates a new <see cref="FoundValueMethod"/> for a value that was found in a set.
        /// </summary>
        public static FoundValueMethod CreateSetMethod(Set set)
        {
            return new FoundValueMethod()
            {
                Type = FoundValueMethodType.SET,
                Indexes = set.Cells.Select(c => c.Index)
            };
        }

        /// <summary>
        /// Creates a new <see cref="FoundValueMethod"/> for a value that was the only possible value in a cell.
        /// </summary>
        public static FoundValueMethod CreateSingleMethod(Cell cell)
        {
            return new FoundValueMethod()
            {
                Type = FoundValueMethodType.SET,
                Indexes = new List<int> { cell.Index }
            };
        }
    }
}
