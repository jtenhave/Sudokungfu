using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Sudokungfu.Model
{
    using Extensions;

    /// <summary>
    /// Class that represents an input model for a cell.
    /// </summary>
    public class CellInputModel : ISudokuModel
    {
        private IDictionary<int, IEnumerable<int>> _indexValueMap;

        #region ISudokuModel

        /// <summary>
        /// Not used by <see cref="CellInputModel"/>.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Not used by <see cref="CellInputModel"/>.
        /// </summary>
        public IEnumerable<int> AffectedIndexes
        {
            get
            {
                return Enumerable.Empty<int>();
            }
        }

        /// <summary>
        /// Not used by <see cref="CellInputModel"/>.
        /// </summary>
        public IEnumerable<ISudokuModel> Details
        {
            get
            {
                return new List<ISudokuModel>();
            }
        }

        /// <summary>
        /// Indexes of the cell and the value that it currently holds.
        /// </summary>
        public IDictionary<int, IEnumerable<int>> IndexValueMap
        {
            get
            {
                return _indexValueMap;
            }
        }

        /// <summary>
        /// Not used by <see cref="CellInputModel"/>.
        /// </summary>
        public bool IsInputEnabled
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Not used by <see cref="CellInputModel"/>.
        /// </summary>
        public bool IsSolving
        {
            get
            {
                return false;
            }
        }

        #endregion

        /// <summary>
        /// Creates a new <see cref="CellInputModel"/>
        /// </summary>
        /// <param name="index"></param>
        public CellInputModel(int index)
        {
            _indexValueMap = new Dictionary<int, IEnumerable<int>>();
            _indexValueMap[index] = 0.ToEnumerable();
        }
    }
}
