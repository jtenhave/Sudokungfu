using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver
{
    using Model;

    /// <summary>
    /// Class that represents a value found in the Sudoku.
    /// </summary>
    public class FoundValue : ISudokuModel
    {
        private Cell _cell;
        private int _value;
        private List<ISudokuModel> _techniques;
        private Dictionary<Cell, IEnumerable<int>> _cellValueMap;
        private int? _compexity;

        #region ISudokuModel

        /// <summary>
        /// Not used by <see cref="FoundValue"/>.
        /// </summary>
        public IEnumerable<int> AffectedIndexes
        {
            get
            {
                return Enumerable.Empty<int>();
            }
        }

        /// <summary>
        /// Not used by <see cref="FoundValue"/>.
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
                if (_compexity.HasValue)
                {
                    return _compexity.Value;
                }

                if (Details.Any())
                {
                    return Details.Max(t => t.Complexity);
                }

                return 0;
            }

            set
            {
                _compexity = value;
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
                return _cellValueMap.ToDictionary(c => c.Key.Index, c => c.Value);
            }
        }

        /// <summary>
        /// Description of the found value.
        /// </summary>
        public string Description { get; set; }

        #endregion

        /// <summary>
        /// Cells that are part of the found value and the values that go in them.
        /// </summary>
        public IDictionary<Cell, IEnumerable<int>> CellValueMap
        {
            get
            {
                return _cellValueMap;
            }
        }

        /// <summary>
        /// Techniques that make up this technique.
        /// </summary>
        public List<ISudokuModel> Techniques
        {
            get
            {
                return _techniques;
            }
        }

        /// <summary>
        /// Cell that the value was found in.
        /// </summary>
        public Cell Cell
        {
            get
            {
                return _cell;
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

        /// <summary>
        /// Creates a new <see cref="FoundValue"/>.
        /// </summary>
        /// <param name="cell">Cell the value goes in.</param>
        /// <param name="value">Value.</param>
        public FoundValue(Cell cell, int value)
        {
            _cell = cell;
            _value = value;
            _techniques = new List<ISudokuModel>();
            _cellValueMap = new Dictionary<Cell, IEnumerable<int>>();
        }    
    }
}
