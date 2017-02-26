using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;

namespace Sudokungfu
{
    using SudokuSolver;
    using SudokuGrid;
    using System.ComponentModel;

    /// <summary>
    /// Interaction logic for SudokuWindow.xaml
    /// </summary>
    public partial class SudokuWindow : Window
    {
        private const int ARTIFICIAL_DELAY = 3000;

        private SudokuModel _model;

        /// <summary>
        /// The cells in the grid.
        /// </summary>
        public List<CellViewModel> Cells { get; set; } = new List<CellViewModel>();

        public SudokuWindow()
        {
            InitializeComponent();

            // Initialize the cell view models.
            for (int i = 0; i < Constants.CELL_COUNT; i++)
            {
                var cellViewModel = new CellViewModel(i);
                Cells.Add(new CellViewModel(i));
            }

            InitializeModel();

            DataContext = this;
        }

        private void InitializeModel()
        {
            _model = new SudokuModel();
            _model.PropertyChanged += OnModelChanged;
            _model.Initialize();

            foreach (var cellViewModel in Cells)
            {
                cellViewModel.SetSudokuModel(_model);
            }
        }

        /// <summary>
        /// Event handler for a Clear button click.
        /// </summary>
        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(Properties.Resources.ClearMessage, Properties.Resources.ClearTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                InitializeModel();
            }
        }

        /// <summary>
        /// Event handler for an Enter button click.
        /// </summary>
        private async void EnterButtonClick(object sender, RoutedEventArgs e)
        {
            await _model.Solve();
        }

        /// <summary>
        /// Event handler for an Solve button click.
        /// </summary>
        private void SolveButtonClick(object sender, RoutedEventArgs e)
        {
            /*while (_shownValues < Constants.CELL_COUNT && _currentSudoku != null)
            {
                Next();
            }*/
        }

        /// <summary>
        /// Event handler for an Next button click.
        /// </summary>
        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            /*if (_shownValues < Constants.CELL_COUNT && _currentSudoku != null)
            {
                var value = _currentSudoku[_shownValues];
                Cells[value.Index].Value = value.Value.ToString();
                if (_shownValues < _givenValues)
                {
                    // Cells[value.Index].Background = Brushes.LightGray;
                }

                _shownValues++;
            }*/
        }

        /// <summary>
        /// Event handler for an Previous button click.
        /// </summary>
        private void PreviousButtonClick(object sender, RoutedEventArgs e)
        {
            /*if (_shownValues > _givenValues && _currentSudoku != null)
            {
                _shownValues--;

                var value = _currentSudoku[_shownValues];
                Cells[value.Index].Value = string.Empty;
            }*/
        }

        /// <summary>
        /// Sets the current Sudoku for the grid.
        /// </summary>
        /*public void SetSudoku(IEnumerable<FoundValue> _foundValues)
        {
            _currentSudoku = _foundValues.ToList();
            _givenValues = _foundValues.Count(v => v.Techniques.Count() == 0);
            _shownValues = 0;

            while (_shownValues < _givenValues)
            {
                Next();
            }
        }*/

        /// <summary>
        /// Trys to solve the Sudoku based on the currently entered values.
        /// </summary>
        /// <returns>The <see cref="SudokuSolver.SolveResult"/></returns>
        /*private async Task<SolveResult> SolveCurrent()
        {
            return await Task.Run(async () =>
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                var initialValues = Cells.Select(c =>
                {
                    var value = 0;
                    int.TryParse(c.Value, out value);

                    return value;
                }).ToArray();

                var solveResult = Solver.Solve(initialValues);

                stopWatch.Stop();

                if (stopWatch.ElapsedMilliseconds < ARTIFICIAL_DELAY)
                {
                    await Task.Delay(ARTIFICIAL_DELAY - (int)stopWatch.ElapsedMilliseconds);
                }

                return solveResult;
            });
        }*/

        private void OnModelChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "IsSolving")
            {
                Cursor = _model.IsSolving ? Cursors.Wait : Cursors.Arrow;
                ClearButton.IsEnabled = !_model.IsSolving;
            }
            else if (args.PropertyName == "IsInputEnabled")
            {
                EnterButton.IsEnabled = _model.IsInputEnabled && !_model.IsSolving;
            }
            else if (args.PropertyName == "SolveResult")
            {
                if (_model.SolveResult == SolveResult.INVALID)
                {
                    MessageBox.Show(Properties.Resources.InvalidMessage, Properties.Resources.InvalidTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (_model.SolveResult == SolveResult.ERROR)
                {
                    MessageBox.Show(Properties.Resources.ErrorMessage, Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
