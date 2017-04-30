
namespace Sudokungfu.SudokuSolver.FoundValues
{
    using Extensions;

    /// <summary>
    /// Class that represents a value that was found because it was the only value that could go in a cell.
    /// </summary>
    public class OnlyPossibleValue : FoundValueBase
    {
        /// <summary>
        /// Creates a new <see cref="OnlyPossibleValue"/>.
        /// </summary>
        /// <param name="index">Index of the value.</param>
        /// <param name="value">Value.</param>
        public OnlyPossibleValue(Cell cell, int value) : base (cell.Index, value)
        {
            var techniques = cell.FindMinTechniques(value);
            _techniques.AddRange(techniques);

            _indexValueMap[cell.Index] = value.ToEnumerable();
        }

        /// <summary>
        /// Complexity of the found value.
        /// </summary>
        public override int Complexity
        {
            get
            {
                return base.Complexity + 1;
            }
        }
    }
}
