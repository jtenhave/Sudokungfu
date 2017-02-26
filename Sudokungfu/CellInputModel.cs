using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudokungfu
{
    using Extensions;

    public class CellInputModel : ISudokuModel
    {
        private IDictionary<int, IEnumerable<int>> _indexValueMap;

        public event PropertyChangedEventHandler PropertyChanged;

        public CellInputModel(int index)
        {
            _indexValueMap = new Dictionary<int, IEnumerable<int>>();
            _indexValueMap[index] = 0.ToEnumerable();
        }

        public IEnumerable<int> AffectedIndexes
        {
            get
            {
                return Enumerable.Empty<int>();
            }
        }

        public IEnumerable<ISudokuModel> Details
        {
            get
            {
                return new List<ISudokuModel>();
            }
        }

        public IDictionary<int, IEnumerable<int>> IndexValueMap
        {
            get
            {
                return _indexValueMap;
            }
        }

        public bool IsInputEnabled
        {
            get
            {
                return false;
            }
        }

        public bool IsSolving
        {
            get
            {
                return false;
            }
        }
    }
}
