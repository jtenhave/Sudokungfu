using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Sudokungfu
{
    using SudokuSolver;

    /// <summary>
    /// Interaction logic for SudokuWindow.xaml
    /// </summary>
    public partial class SudokuWindow : Window
    {
        private const int ARTIFICIAL_DELAY = 3000;

        public SudokuWindow()
        {
            InitializeComponent();

            IsEnterEnabled = true;
            IsWaitingEnabled = false;
            IsSolveEnabled = false;
        }

        /// <summary>
        /// Whether controls 
        /// </summary>
        private bool IsWaitingEnabled
        {
            set
            {
                ClearButton.IsEnabled = !value;
                if (value)
                {
                    Cursor = Cursors.Wait;
                }
                else
                {
                    Cursor = Cursors.Arrow;
                }
            }
        }

        /// <summary>
        /// Whether the solve related controls are enabled.
        /// </summary>
        private bool IsSolveEnabled
        {
            set
            {
                PreviousButton.IsEnabled = value;
                NextButton.IsEnabled = value;
                SolveButton.IsEnabled = value;
            }
        }

        /// <summary>
        /// Whether the entering related controls are enabled.
        /// </summary>
        private bool IsEnterEnabled
        {
            set
            {
                SudokuGrid.IsReadOnly = !value;
                EnterButton.IsEnabled = value;
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
                foreach (var cell in SudokuGrid.Cells)
                {
                    cell.Value = string.Empty;
                }
            }

            IsSolveEnabled = false;
            IsEnterEnabled = true;
        }

        /// <summary>
        /// Event handler for an Enter button click.
        /// </summary>
        private async void EnterButtonClick(object sender, RoutedEventArgs e)
        {
            IsEnterEnabled = false;
            IsWaitingEnabled = true;

            var result = await SolveCurrent();
            IsWaitingEnabled = false;
            
            if (result.Type == SudokuResultType.SUCCESS)
            {
                IsSolveEnabled = true;
                SudokuGrid.SetSudoku(result.FoundValues);
            }
            else if (result.Type == SudokuResultType.INVALID)
            {
                MessageBox.Show(Properties.Resources.InvalidMessage, Properties.Resources.InvalidTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
                IsEnterEnabled = true;
            }
            else if (result.Type == SudokuResultType.ERROR)
            {
                IsEnterEnabled = true;
            }
        }

        /// <summary>
        /// Event handler for an Solve button click.
        /// </summary>
        private void SolveButtonClick(object sender, RoutedEventArgs e)
        {
            SudokuGrid.Solve();
        }

        /// <summary>
        /// Event handler for an Next button click.
        /// </summary>
        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            SudokuGrid.Next();
        }

        /// <summary>
        /// Event handler for an Previous button click.
        /// </summary>
        private void PreviousButtonClick(object sender, RoutedEventArgs e)
        {
            SudokuGrid.Previous();
        }

        /// <summary>
        /// Trys to solve the Sudoku based on the currently entered values.
        /// </summary>
        /// <returns>The <see cref="SolveResult"/></returns>
        private async Task<SolveResult> SolveCurrent()
        {
            return await Task.Run(async () =>
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                var initialValues = SudokuGrid.Cells.Select(c =>
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
        }
    }
}
