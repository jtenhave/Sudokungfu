using System.Linq;

namespace Sudokungfu.SudokuSolver.FoundValues
{
    using Extensions;
    using Sets;

    /// <summary>
    /// Class that represents a value found in a set.
    /// </summary>
    public class FoundInSetValue : FoundValueBase
    {
        /// <summary>
        /// Creates a new <see cref="FoundInSetValue"/>.
        /// </summary>
        /// <param name="cell">Cell where the value was found.</param>
        /// <param name="value">Value that was found.</param>
        /// <param name="set">Set the value was found in.</param>
        public FoundInSetValue(Cell cell, int value, Set set) : base (cell.Index, value)
        {
            var techniques = set.FindMinTechniques(cell.ToEnumerable(), value);
            _techniques.AddRange(techniques);

            foreach (var index in set.Indexes)
            {
                _indexValueMap[index] = Enumerable.Empty<int>();
            }

            _indexValueMap[cell.Index] = value.ToEnumerable();
        }
    }   
}
