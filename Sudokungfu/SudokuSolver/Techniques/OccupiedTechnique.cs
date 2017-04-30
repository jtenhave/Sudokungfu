
namespace Sudokungfu.SudokuSolver.Techniques
{
    using Extensions;
    using FoundValues;
    using Model;

    /// <summary>
    /// Class that represents a technique for cells that already occupied.
    /// </summary>
    public class OccupiedTechnique : TechniqueBase
    {
        private const int COMPLEXITY = 0;
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
        /// Creates a new <see cref="OccupiedTechnique"/>.
        /// </summary>
        /// <param name="value">Value that was found.</param>
        public OccupiedTechnique(FoundValueBase value) : base (COMPLEXITY)
        {
            _foundValue = value;
            _indexValueMap[value.Index] = value.Value.ToEnumerable();
        }
    }
}
