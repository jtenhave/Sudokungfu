using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Sudokungfu.ViewModel
{
    using Model;
    using SudokuSolver;

    /// <summary>
    /// Class that represents the view model for Sudokungfu.
    /// </summary>
    public class SudokungfuViewModel : INotifyPropertyChanged
    {
        private const int FONT_SIZE_DEFAULT = 36;

        private int _shownValues;
        private int _givenValues;
        private bool _isSolving;
        private bool _isEnabled;
        private List<ISudokuModel> _sudoku;
        private Stack<ISudokuModel> _models;

        private Func<bool> _clearConfirm;
        private Action _error;
        private Action _invalid;

        private DelegateCommand _clearCommand;
        private DelegateCommand _enterCommand;
        private DelegateCommand _nextCommand;
        private DelegateCommand _prevCommand;
        private DelegateCommand _solveCommand;

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

                    IsEnabled = !value;
                    _enterCommand.CanExecuteValue = !value;
                    _clearCommand.CanExecuteValue = !value;
                }
            }
        }

        /// <summary>
        /// Whether input is enabled for the Suokdu.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    OnPropertyChanged(nameof(IsEnabled));
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
        /// Creates a <see cref="SudokungfuViewModel"/>.
        /// </summary>
        /// <param name="clearConfirm">Action to confirm a clear.</param>
        /// <param name="invalid">Action to execute if a Sudoku is invalid.</param>
        /// <param name="error">Action to execute if an error occurs.</param>
        public SudokungfuViewModel(Func<bool> clearConfirm, Action invalid, Action error)
        {
            _shownValues = Constants.CELL_COUNT;
            _models = new Stack<ISudokuModel>();
            _isSolving = false;
            _isEnabled = true;

            _clearConfirm = clearConfirm;
            _invalid = invalid;
            _error = error;

            // Initialize the cell view models.
            for (int i = 0; i < Constants.CELL_COUNT; i++)
            {
                var cellViewModel = new CellViewModel(i);
                Cells.Add(new CellViewModel(i));
            }
            
            _clearCommand = DelegateCommand.Create(ClearAction);
            _enterCommand = DelegateCommand.CreateAsync(EnterAction);
            _nextCommand = DelegateCommand.Create(NextAction);
            _prevCommand = DelegateCommand.Create(PreviousAction);
            _solveCommand = DelegateCommand.Create(() => ShowValues(Constants.CELL_COUNT));
        }

        /// <summary>
        /// Action to perform when clear command is invoked.
        /// </summary>
        private void ClearAction()
        {
            if (_clearConfirm())
            {
                foreach (var cell in Cells)
                {
                    cell.SetDefaultCellProperties();
                }
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
            if (_shownValues < Constants.CELL_COUNT)
            {
                _shownValues++;
                RefreshCellViewModels();
            }
        }

        /// <summary>
        /// Action to perform when previous command is invoked.
        /// </summary>
        private void PreviousAction()
        {
            if (_shownValues > _givenValues)
            {
                _shownValues--;
                RefreshCellViewModels();
            }
        }

        private void ShowValues(int count)
        {
            while (_shownValues < count)
            {
                NextAction();
            }
        }

        private void RefreshCellViewModels()
        {
            if (_sudoku == null)
            {
                return;
            }

            if (_models.Any())
            {
                // TODO show details.
            }
            else
            {
                foreach (var cell in Cells)
                {
                    var cellModel = _sudoku.First(m => m.IndexValueMap.Keys.Contains(cell.Index) && m.IndexValueMap[cell.Index].Any());
                    if (_sudoku.IndexOf(cellModel) >= _shownValues)
                    {
                        cell.Background = Brushes.White;
                        cell.Value = string.Empty;
                    }
                    else
                    {
                        var value = cellModel.IndexValueMap[cell.Index].First().ToString();
                        cell.FontSize = FONT_SIZE_DEFAULT;
                        cell.FontStyle = FontStyles.Normal;
                        cell.Background = value != "0" && !cellModel.Details.Any() ? Brushes.LightGray : Brushes.White;
                        cell.Value = value;
                    }
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
