using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Techniques
{
    using Model;

    /// <summary>
    /// Class that represents a technique used to eliminate a value from a cell in a Sudoku.
    /// </summary>
    public class Technique : ISudokuModel
    {
        private List<Cell> _affectedCells;
        private Dictionary<Cell, IEnumerable<int>> _cellValueMap;
        private List<ISudokuModel> _techniques;
        private ISudokuModel _clickableModel;
        private List<int> _values;
        private int _complexity;

        #region ISudokuModel

        /// <summary>
        /// Indexes of cells that are part of the technique and the values that go in them.
        /// </summary>
        public IDictionary<int, IEnumerable<int>> IndexValueMap
        {
            get
            {
                return _cellValueMap.ToDictionary(c => c.Key.Index, c => c.Value);
            }
        }

        /// <summary>
        /// Techniques that make up this technique.
        /// </summary>
        public IEnumerable<ISudokuModel> Details
        {
            get
            {
                return _techniques;
            }
        }

        /// <summary>
        /// Indexes of cells that are affected by this technique.
        /// </summary>
        public IEnumerable<int> AffectedIndexes
        {
            get
            {
                return _affectedCells.Select(c => c.Index);
            }
        }

        /// <summary>
        /// Model that will be displayed when this technique is clicked.
        /// </summary>
        public ISudokuModel ClickableModel
        {
            get
            {
                return _clickableModel;
            }
            set
            {
                _clickableModel = value;
            }
        }

        /// <summary>
        /// Complexity of the technique. 
        /// </summary>
        public int Complexity
        {
            get
            {
                if (Techniques.Any())
                {
                    return _complexity + Techniques.Max(t => t.Complexity);
                }
                else
                {
                    return _complexity;
                }
            }
            set
            {
                _complexity = value;
            }
        }

        #endregion

        /// <summary>
        /// Cells that are part of the technique and the values that go in them.
        /// </summary>
        public IDictionary<Cell, IEnumerable<int>> CellValueMap
        {
            get
            {
                return _cellValueMap;
            }
        }

        /// <summary>
        /// Cells that are affected by this technique.
        /// </summary>
        public List<Cell> AffectedCells
        {
            get
            {
                return _affectedCells;
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
        /// Values affected by this technique.
        /// </summary>
        public List<int> Values
        {
            get
            {
                return _values;
            }
        }

        /// <summary>
        /// Creates a new <see cref="Technique"/>.
        /// </summary>
        public Technique()
        {
            _cellValueMap = new Dictionary<Cell, IEnumerable<int>>();
            _affectedCells = new List<Cell>();
            _techniques = new List<ISudokuModel>();
            _values = new List<int>();
            _clickableModel = this;
        }
    }
}
