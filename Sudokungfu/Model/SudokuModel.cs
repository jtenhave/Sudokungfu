using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Sudokungfu.Model
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
    /// Class that represents the values in a Sudoku grid.
    /// </summary>
    public class SudokuModel : ISudokuModel
    {
        private IEnumerable<ISudokuModel> _details;
        private bool _isSolving;
        private bool _isInputEnabled;
        private SolveResult _result;

        #region ISudokuModel

        /// <summary>
        /// Not used by <see cref="SudokuModel"/>.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Not used by <see cref="SudokuModel"/>.
        /// </summary>
        public IEnumerable<int> AffectedIndexes
        {
            get
            {
                return Enumerable.Empty<int>();
            }
        }

        /// <summary>
        /// Details of the current values in the Sudoku
        /// </summary>
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

        /// <summary>
        /// Not used by <see cref="SudokuModel"/>.
        /// </summary>
        public IDictionary<int, IEnumerable<int>> IndexValueMap
        {
            get
            {
                return new Dictionary<int, IEnumerable<int>>();
            }
        }

        /// <summary>
        /// Whether the model is currently accepting input.
        /// </summary>
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

        /// <summary>
        /// Whether the model is currently solvers.
        /// </summary>
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
        /// Creates a new <see cref="SudokuModel"/>.
        /// </summary>
        public SudokuModel()
        {
            var details = new List<ISudokuModel>();

            for (int i = 0; i < Constants.CELL_COUNT; i++)
            {
                details.Add(new CellInputModel(i));
            }

            _details = details;
        }

        /// <summary>
        /// Result of last try for solving the Sudoku.
        /// </summary>
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

        /// <summary>
        /// Initializes the model.
        /// </summary>
        public void Initialize()
        {
            _isSolving = true;
            _isInputEnabled = false;
            _result = SolveResult.NONE;

            IsSolving = false;
            IsInputEnabled = true;
        }

        /// <summary>
        /// Solves the Sudoku.
        /// </summary>
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
    }
}
