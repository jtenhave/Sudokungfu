using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Techniques
{
    using Model;

    /// <summary>
    /// Class that represents a technique used to eliminate a value from a cell in a Sudoku.
    /// </summary>
    public class TechniqueBase : ISudokuModel
    {
        private readonly int _complexity;
        protected readonly IDictionary<int, IEnumerable<int>> _indexValueMap;
        protected readonly List<int> _affectedIndexes;

        #region ISudokuModel

        /// <summary>
        /// Indexes of cells that are part of the technique and the values that go in them.
        /// </summary>
        public IDictionary<int, IEnumerable<int>> IndexValueMap
        {
            get
            {
                return _indexValueMap;
            }
        }

        /// <summary>
        /// Not used by <see cref="TechniqueBase"/>.
        /// </summary>
        public virtual IEnumerable<ISudokuModel> Details
        {
            get
            {
                return Enumerable.Empty<ISudokuModel>();
            }
        }

        /// <summary>
        /// Indexes of cells that are affected by this technique.
        /// </summary>
        public IEnumerable<int> AffectedIndexes
        {
            get
            {
                return _affectedIndexes;
            }
        }

        /// <summary>
        /// Not used by <see cref="TechniqueBase"/>.
        /// </summary>
        public virtual ISudokuModel ClickableModel
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Complexity of the technique. 
        /// </summary>
        public int Complexity
        {
            get
            {
                if (Details != null && Details.Any())
                {
                    var complexity = Details.Max(t => t.Complexity);
                    if (complexity >= _complexity)
                    {
                        return complexity + 1;
                    }
                }

                return _complexity;
            }
        }

        #endregion

        /// <summary>
        /// Creates a new <see cref="TechniqueBase"/>.
        /// </summary>
        /// <param name="defaultComplexity">Default complexity of the model.</param>
        protected TechniqueBase(int defaultComplexity)
        {
            _complexity = defaultComplexity;
            _indexValueMap = new Dictionary<int, IEnumerable<int>>();
            _affectedIndexes = new List<int>();
        }
    }
}
