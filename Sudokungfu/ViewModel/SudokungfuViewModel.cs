using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sudokungfu.ViewModel
{
    using Model;
    using SudokuSolver;

    /// <summary>
    /// Class that represents the view model for Sudokungfu.
    /// </summary>
    public class SudokungfuViewModel : INotifyPropertyChanged, ISudokungfuViewModel
    {
        private int _shownValues;
        private int _givenValues;
        private bool _isSolving;
        private List<ISudokuModel> _sudoku;
        private Stack<ISudokuModel> _clickedModels;

        private Func<bool> _clearConfirm;
        private Action _error;
        private Action _invalid;

        private DelegateCommand _clearCommand;
        private DelegateCommand _enterCommand;
        private DelegateCommand _nextCommand;
        private DelegateCommand _prevCommand;
        private DelegateCommand _solveCommand;
        private DelegateCommand _backCommand;
        private DelegateCommand _closeCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Cells in the grid.
        /// </summary>
        public List<CellViewModel> Cells { get; set; } = new List<CellViewModel>();

        /// <summary>
        /// Whether the sudoku is currently being solved.
        /// </summary>
        public bool IsSolving
        {
            get
            {
                return _isSolving;
            }
            set
            {
                if (value != _isSolving)
                {
                    _isSolving = value;
                    OnPropertyChanged(nameof(IsSolving));

                    _enterCommand.CanExecuteValue = !value;
                    _clearCommand.CanExecuteValue = !value;
                }
            }
        }

        /// <summary>
        /// Gets the enter command.
        /// </summary>
        public ICommand EnterCommand
        {
            get
            {
                return _enterCommand;
            }
        }

        /// <summary>
        /// Gets the clear command.
        /// </summary>
        public ICommand ClearCommand
        {
            get
            {
                return _clearCommand;
            }
        }

        /// <summary>
        /// Gets the next command.
        /// </summary>
        public ICommand NextCommand
        {
            get
            {
                return _nextCommand;
            }
        }

        /// <summary>
        /// Gets the previous command.
        /// </summary>
        public ICommand PreviousCommand
        {
            get
            {
                return _prevCommand;
            }
        }

        /// <summary>
        /// Gets the solve command.
        /// </summary>
        public ICommand SolveCommand
        {
            get
            {
                return _solveCommand;
            }
        }

        /// <summary>
        /// Gets the back command.
        /// </summary>
        public ICommand BackCommand
        {
            get
            {
                return _backCommand;
            }
        }

        /// <summary>
        /// Gets the close command.
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand;
            }
        }

        /// <summary>
        /// Gets the number of values currently shown.
        /// </summary>
        public int ShownValues {
            get
            {
                return _shownValues;
            }
        }

        /// <summary>
        /// Creates a <see cref="SudokungfuViewModel"/>.
        /// </summary>
        /// <param name="clearConfirm">Action to confirm a clear.</param>
        /// <param name="invalid">Action to execute if a Sudoku is invalid.</param>
        /// <param name="error">Action to execute if an error occurs.</param>
        public SudokungfuViewModel(Func<bool> clearConfirm, Action invalid, Action error)
        {
            _shownValues = Constants.CELL_COUNT;
            _clickedModels = new Stack<ISudokuModel>();
            _isSolving = false;

            _clearConfirm = clearConfirm;
            _invalid = invalid;
            _error = error;

            // Initialize the cell view models.
            for (int i = 0; i < Constants.CELL_COUNT; i++)
            {
                Cells.Add(new CellViewModel(i, this));
            }
            
            _clearCommand = DelegateCommand.Create(ClearAction);
            _enterCommand = DelegateCommand.CreateAsync(EnterAction);
            _nextCommand = DelegateCommand.Create(NextAction, false);
            _prevCommand = DelegateCommand.Create(PreviousAction, false);
            _solveCommand = DelegateCommand.Create(SolveAction , false);
            _closeCommand = DelegateCommand.Create(CloseAction, false);
            _backCommand = DelegateCommand.Create(BackAction, false);
        }

        /// <summary>
        /// Action to perform when clear command is invoked.
        /// </summary>
        private void ClearAction()
        {
            if (_clearConfirm())
            {
                _sudoku = null;
                _enterCommand.CanExecuteValue = true;
                _nextCommand.CanExecuteValue = _prevCommand.CanExecuteValue = _solveCommand.CanExecuteValue = false;

                RefreshCellViewModels();
            }
        }

        /// <summary>
        /// Action to perform when enter command is invoked.
        /// </summary>
        private async Task EnterAction()
        {
            IsSolving = true;
            List<ISudokuModel> sudoku = null;

            try
            {
                sudoku = await Solver.Solve(Cells.Select(c => c.ValueAsInt));
            }
            catch (Exception)
            {
                _error();
                return;
            }
            finally
            {
                IsSolving = false;
            }

            if (sudoku == null)
            {
                _invalid();
                return;
            }

            _enterCommand.CanExecuteValue = false;

            _sudoku = sudoku;
            _givenValues = _sudoku.Count(v => v.Details.Count() == 0);
            _shownValues = 0;

            ShowValues(_givenValues);
        }

        /// <summary>
        /// Action to perform when next command is invoked.
        /// </summary>
        private void NextAction()
        {
            if (_shownValues < Constants.CELL_COUNT && !_clickedModels.Any())
            {
                _nextCommand.CanExecuteValue = _solveCommand.CanExecuteValue = ++_shownValues < Constants.CELL_COUNT;
                _prevCommand.CanExecuteValue = _shownValues > _givenValues;
                RefreshCellViewModels();
            }
        }

        /// <summary>
        /// Action to perform when previous command is invoked.
        /// </summary>
        private void PreviousAction()
        {
            if (_shownValues > _givenValues && !_clickedModels.Any())
            {
                _prevCommand.CanExecuteValue = --_shownValues > _givenValues;
                _nextCommand.CanExecuteValue = _solveCommand.CanExecuteValue = true;

                RefreshCellViewModels();
            }
        }

        /// <summary>
        /// Action to perform when solve command is invoked.
        /// </summary>
        private void SolveAction()
        {
            if (!_clickedModels.Any())
            {
                ShowValues(Constants.CELL_COUNT);
            }
        }

        /// <summary>
        /// Action to perform when the close browser command is invoked.
        /// </summary>
        private void CloseAction()
        {
            _closeCommand.CanExecuteValue = false;
            _backCommand.CanExecuteValue = false;
            _clickedModels.Clear();
            RefreshCellViewModels();
        }

        /// <summary>
        /// Action to perform when the back browser command is invoked.
        /// </summary>
        private void BackAction()
        {
            _clickedModels.Pop();
            _backCommand.CanExecuteValue = _clickedModels.Any();
            RefreshCellViewModels();
        }

        private void ShowValues(int count)
        {
            while (_shownValues < count)
            {
                NextAction();
            }
        }

        public void CellClicked(ISudokuModel model)
        {
            _clickedModels.Push(model);
            _closeCommand.CanExecuteValue = true;
            _backCommand.CanExecuteValue = true;
            RefreshCellViewModels();
        }

        private void RefreshCellViewModels()
        {
            foreach (var cell in Cells)
            {
                if (_clickedModels.Any())
                {
                    cell.SetCellProperties(_clickedModels.Peek());
                }
                else
                {
                    cell.SetCellProperties(_sudoku);
                }
            }
        }

        /// <summary>
        /// Notifies listeners of the PropertyChanged event that a property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
