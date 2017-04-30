using System.Linq;

namespace Sudokungfu.SudokuSolver.Techniques
{
    using Extensions;
    using FoundValues;
    using Model;
    using Sets;

    /// <summary>
    /// Class that represents a technique for values already in a set.
    /// </summary>
    public class SetTechnique : TechniqueBase
    {
        private const int COMPLEXITY = 1;
        private readonly ISudokuModel _foundValue;

        /// <summary>
        /// Model that will be displayed when this technique is clicked.
        /// </summary>
        public override ISudokuModel ClickableModel
        {
            get
            {
                return _foundValue;
            }
        }

        /// <summary>
        /// Creates a new <see cref="SetTechnique"/>.
        /// </summary>
        /// <param name="value">Value that was found.</param>
        /// <param name="set">Set that the value was found in.</param>
        public SetTechnique(FoundValueBase value, Set set) : base (COMPLEXITY)
        {
            _foundValue = value;

            var affectedIndexes = set.Indexes.Except(value.Index);

            _indexValueMap[value.Index] = value.Value.ToEnumerable();
            foreach (var index in affectedIndexes)
            {
                _indexValueMap[index] = Enumerable.Empty<int>();
            }

            _affectedIndexes.AddRange(affectedIndexes);
        }
    }
}
