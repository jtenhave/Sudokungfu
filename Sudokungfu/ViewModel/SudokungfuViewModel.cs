using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly Dictionary<int, int> FONT_SIZE_MAP = new Dictionary<int, int>()
        {
            { 1, 36 },
            { 2, 26 },
            { 3, 16 },
        };

        private string _description = "";
        private int _shownValues;
        private int _givenValues;
        private bool _isSolving;
        private List<FoundValue> _sudoku;
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
        /// Gets the description for the current detail being displayed.
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
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
                var cell = new CellViewModel(i);
                Cells.Add(cell);
                cell.CellClicked += (sender, args) =>
                {
                    CellClicked(args.ClickedModel);
                };
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

                DrawDefault();
            }
        }

        /// <summary>
        /// Action to perform when enter command is invoked.
        /// </summary>
        private async Task EnterAction()
        {
            IsSolving = true;
            List<FoundValue> sudoku = null;

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
            _nextCommand.CanExecuteValue = true;
            _solveCommand.CanExecuteValue = true;

            _sudoku = sudoku;
            _givenValues = _shownValues = _sudoku.Count(v => !v.Details.Any());

            DrawSudoku();
        }

        /// <summary>
        /// Action to perform when next command is invoked.
        /// </summary>
        private void NextAction()
        {
            if (_shownValues < Constants.CELL_COUNT)
            {
                _nextCommand.CanExecuteValue = _solveCommand.CanExecuteValue = ++_shownValues < Constants.CELL_COUNT;
                _prevCommand.CanExecuteValue = _shownValues > _givenValues;

                DrawValue(_sudoku[_shownValues - 1]);
            }
        }

        /// <summary>
        /// Action to perform when previous command is invoked.
        /// </summary>
        private void PreviousAction()
        {
            if (_shownValues > _givenValues)
            {
                _prevCommand.CanExecuteValue = --_shownValues > _givenValues;
                _nextCommand.CanExecuteValue = _solveCommand.CanExecuteValue = true;

                DrawEmpty(_sudoku[_shownValues]);
            }
        }

        /// <summary>
        /// Action to perform when solve command is invoked.
        /// </summary>
        private void SolveAction()
        {
            while (_shownValues < Constants.CELL_COUNT)
            {
                NextAction();
            }
        }

        /// <summary>
        /// Action to perform when the close browser command is invoked.
        /// </summary>
        private void CloseAction()
        {
            _nextCommand.CanExecuteValue = _solveCommand.CanExecuteValue = _shownValues < Constants.CELL_COUNT;
            _prevCommand.CanExecuteValue = _shownValues > _givenValues;

             _closeCommand.CanExecuteValue = false;
             _backCommand.CanExecuteValue = false;
             _clickedModels.Clear();

            Description = "";

            DrawSudoku();
        }

        /// <summary>
        /// Action to perform when the back browser command is invoked.
        /// </summary>
        private void BackAction()
        {
            _clickedModels.Pop();
            _backCommand.CanExecuteValue = _clickedModels.Any();
            if (_clickedModels.Any())
            {
                DrawDetails(_clickedModels.Peek());
            }
            else
            {
                CloseAction();
            }
        }

        private void CellClicked(ISudokuModel model)
        {
            _nextCommand.CanExecuteValue = false;
            _prevCommand.CanExecuteValue = false;
            _solveCommand.CanExecuteValue = false;


            _clickedModels.Push(model);
            _closeCommand.CanExecuteValue = true;
            _backCommand.CanExecuteValue = true;

            DrawDetails(_clickedModels.Peek());
        }

        private void DrawDefault(Brush brush = null)
        {
            foreach (var cell in Cells)
            {
                cell.SetDefaultCellProperties();
                if (brush != null)
                {
                    cell.Background = brush;
                }
            }
        }

        private void DrawSudoku()
        {
            DrawDefault();
            for (int i = 0; i < _shownValues; i++)
            {
                DrawValue(_sudoku[i]);
            }
        }

        private void DrawValue(FoundValue foundValue)
        {
            var cellModel = Cells.Find(c => c.Index == foundValue.Cell.Index);
            cellModel.FoundValue = foundValue;
        }

        private void DrawEmpty(FoundValue foundValue)
        {
            var cellModel = Cells.Find(c => c.FoundValue == foundValue);
            cellModel.SetDefaultCellProperties();
        }

        private void DrawDetails(ISudokuModel model)
        {
            DrawDefault(Brushes.DarkGray);

            var affectedIndexes = model.Details.SelectMany(d => d.IndexValueMap.Keys.Concat(d.AffectedIndexes).Distinct());
            foreach (var index in affectedIndexes)
            {
                Cells.Find(c => c.Index == index).Background = Brushes.LightSalmon;
            }

            var setIndexes = model.IndexValueMap.Keys;
            foreach (var index in setIndexes)
            {
                Cells.Find(c => c.Index == index).Background = Brushes.Salmon;
            }

            foreach (var technique in model.Details)
            {
                var techniqueValueIndexes = technique.IndexValueMap.Keys.Where(key => technique.IndexValueMap[key].Any());
                foreach (var index in techniqueValueIndexes)
                {
                    var values = technique.IndexValueMap[index];
                    var cell = Cells.Find(c => c.Index == index);
                    cell.FontSize = GetFontSize(values.Count(), techniqueValueIndexes.Count() > 1);
                    cell.Value = string.Join("", values);
                    if (technique.ClickableModel.Details.Any())
                    {
                        cell.ClickableModel = technique.ClickableModel;
                    }
                    else
                    {
                        cell.Background = Brushes.DarkSalmon;
                    }
                }
            }

            var valueIndexes = model.IndexValueMap.Keys.Where(key => model.IndexValueMap[key].Any());
            foreach (var index in valueIndexes)
            {
                var values = model.IndexValueMap[index];
                var cell = Cells.Find(c => c.Index == index);
                cell.FontSize = GetFontSize(values.Count(), valueIndexes.Count() > 1);
                cell.Value = string.Join("", values);
                cell.Background = Brushes.LightGreen;              
            }

            Description = model.Description;
        }

        private int GetFontSize(int valueCount, bool hasMultipleCells)
        {
            if (valueCount == 1 && hasMultipleCells)
            {
                return FONT_SIZE_MAP[2];
            }

            return FONT_SIZE_MAP[valueCount];
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
