using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.FoundValues
{
    using Model;

    /// <summary>
    /// Class that represents a value found in the Sudoku.
    /// </summary>
    public class FoundValueBase : ISudokuModel
    {
        private int _index;
        private int _value;
        protected readonly List<ISudokuModel> _techniques;
        protected readonly IDictionary<int, IEnumerable<int>> _indexValueMap;

        /// <summary>
        /// Creates a new <see cref="FoundValueBase"/>.
        /// </summary>
        /// <param name="index">Index of the value.</param>
        /// <param name="value">Value.</param>
        protected FoundValueBase(int index, int value)
        {
            _index = index;
            _value = value;
            _techniques = new List<ISudokuModel>();
            _indexValueMap = new Dictionary<int, IEnumerable<int>>();
        }

        /// <summary>
        /// Not used by <see cref="FoundValueBase"/>.
        /// </summary>
        public IEnumerable<int> AffectedIndexes
        {
            get
            {
                return Enumerable.Empty<int>();
            }
        }

        /// <summary>
        /// Not used by <see cref="FoundValueBase"/>.
        /// </summary>
        public ISudokuModel ClickableModel
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Complexity of the found value.
        /// </summary>
        public virtual int Complexity
        {
            get
            {
                if (Details.Any()) {
                    return Details.Max(t => t.Complexity);
                }

                return 0;
            }
        }

        /// <summary>
        /// Details that make up this found value.
        /// </summary>
        public IEnumerable<ISudokuModel> Details
        {
            get
            {
                return _techniques;
            }
        }

        /// <summary>
        /// Indexes of the cells that should be displayed and the values that go in them.
        /// </summary>
        public IDictionary<int, IEnumerable<int>> IndexValueMap
        {
            get
            {
                return _indexValueMap;
            }
        }

        /// <summary>
        /// Index of the value that was found.
        /// </summary>
        public int Index
        {
            get
            {
                return _index;
            }
        }

        /// <summary>
        /// Value that was found.
        /// </summary>
        public int Value
        {
            get
            {
                return _value;
            }
        }
    }
}
