using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudokungfu
{
    using SudokuSolver;

    /// <summary>
    /// Enum to represent the type of the result.
    /// </summary>
    public enum SolveResult
    {
        NONE,
        SUCCESS,
        INVALID,
        ERROR
    }

    /// <summary>
    /// 
    /// </summary>
    public class SudokuModel : ISudokuModel
    {
        private IEnumerable<ISudokuModel> _details;
        private bool _isSolving;
        private bool _isInputEnabled;
        private SolveResult _result;

        public event PropertyChangedEventHandler PropertyChanged;

        public SudokuModel()
        {
            var details = new List<ISudokuModel>();

            for(int i = 0; i < Constants.CELL_COUNT; i++)
            {
                details.Add(new CellInputModel(i));
            }

            _details = details;
        }

        #region ISudokuModel

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
                return _details;
            }

            private set
            {
                if (value != _details)
                {
                    _details = value;
                    OnPropertyChanged(nameof(Details));
                }
            }
        }

        public IDictionary<int, IEnumerable<int>> IndexValueMap
        {
            get
            {
                return new Dictionary<int, IEnumerable<int>>();
            }
        }

        public bool IsInputEnabled
        {
            get
            {
                return _isInputEnabled;
            }

            set
            {
                if (value != _isInputEnabled)
                {
                    _isInputEnabled = value;
                    OnPropertyChanged(nameof(IsInputEnabled));
                }
            }
        }

        public bool IsSolving
        {
            get
            {
                return _isSolving;
            }

            private set
            {
                if (value != _isSolving)
                {
                    _isSolving = value;
                    OnPropertyChanged(nameof(IsSolving));
                }
            }
        }

        #endregion

        /// <summary>
        /// Notifies listeners of the PropertyChanged event that a property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public SolveResult SolveResult
        {
            get
            {
                return _result;
            }

            set
            {
                if (value != _result)
                {
                    _result = value;
                    OnPropertyChanged(nameof(SolveResult));
                }
            }
        }

        public void Initialize()
        {
            _isSolving = true;
            _isInputEnabled = false;
            _result = SolveResult.NONE;

            IsSolving = false;
            IsInputEnabled = true;
        }

        public async Task Solve()
        {
            if (SolveResult != SolveResult.SUCCESS)
            {
                IsSolving = true;
                IsInputEnabled = false;

                try
                {
                    var result = await Solver.Solve(Details.Select(d => d.IndexValueMap.Values.First().First()));
                    if (result == null)
                    {
                        IsInputEnabled = true;
                        SolveResult = SolveResult.INVALID;
                    }
                    else
                    {
                        Details = result;
                    }
                }
                catch (Exception)
                {
                    IsInputEnabled = true;
                    SolveResult = SolveResult.ERROR;
                }

                IsSolving = false;
            }
        }
    }
}
