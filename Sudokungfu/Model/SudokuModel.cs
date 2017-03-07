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
        private int _shownValues;
        private int _givenValues;

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
                return _details.Take(_shownValues);
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
        /// Not used by <see cref="SudokuModel"/>.
        /// </summary>
        public ISudokuModel ClickableModel
        {
            get
            {
                return null;
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
            _shownValues = Constants.CELL_COUNT;
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
                _result = value;
                OnPropertyChanged(nameof(SolveResult));
                
            }
        }

        /// <summary>
        /// Initializes the model.
        /// </summary>
        public void Initialize()
        {
            _isSolving = true;
            _isInputEnabled = false;

            SolveResult = SolveResult.NONE;
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
                        SolveResult = SolveResult.SUCCESS;

                        _details = result;
                        _givenValues = _details.Count(v => v.Details.Count() == 0);
                        _shownValues = 0;

                        ShowValues(_givenValues);

                        OnPropertyChanged(nameof(Details));
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
        /// Shows the next value in the Sudoku.
        /// </summary>
        public void NextValue()
        {
            if (_shownValues < Constants.CELL_COUNT)
            {
                _shownValues++;
                OnPropertyChanged(nameof(Details));
            }
        }

        /// <summary>
        /// Hides the current value in the Sudoku.
        /// </summary>
        public void PreviousValue()
        {
           if (_shownValues > _givenValues)
           {
               _shownValues--;
                OnPropertyChanged(nameof(Details));
            }
        }

        /// <summary>
        /// Shows all values in the Sudoku.
        /// </summary>
        public void AllValues()
        {
            ShowValues(Constants.CELL_COUNT);
            OnPropertyChanged(nameof(Details));
        }

        private void ShowValues(int count)
        {
            while (_shownValues < count)
            {
                NextValue();
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
