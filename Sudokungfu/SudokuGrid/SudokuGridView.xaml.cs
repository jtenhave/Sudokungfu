using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Sudokungfu.SudokuGrid
{
    using SudokuSolver;

    /// <summary>
    /// Interaction logic for SudokuGrid.xaml
    /// </summary>
    public partial class SudokuGridView : UserControl
    {
        private int _shownValues;
        private int _givenValues;
        private List<FoundValue> _currentSudoku;

        /// <summary>
        /// The cells in the grid.
        /// </summary>
        public List<CellViewModel> Cells { get; set; }

        /// <summary>
        /// Sets the read only value of the cells in the grid.
        /// </summary>
        public bool IsReadOnly
        {
            set
            {
                Cells.ForEach(c => c.IsReadOnly = value);
            }
        }

        /// <summary>
        /// Creates a new <see cref="SudokuGridView"/>
        /// </summary>
        public SudokuGridView()
        {
            InitializeComponent();
            Cells = new List<CellViewModel>();

            // Initialize the cells.
            for (int i = 0; i < Constants.CELL_COUNT; i++)
            {
                Cells.Add(new CellViewModel(i));
            }

            DataContext = this;
        }

        /// <summary>
        /// Sets the current Sudoku for the grid.
        /// </summary>
        public void SetSudoku(IEnumerable<FoundValue> _foundValues)
        {
            _currentSudoku = _foundValues.ToList();
            _givenValues = _foundValues.Count(v => v.Techniques.Count() == 0);
            _shownValues = 0;

            while(_shownValues < _givenValues)
            {
                Next();
            }
        }

        /// <summary>
        /// Clears the current solved Sudoku.
        /// </summary>
        public void ClearSudoku()
        {
            _currentSudoku = null;
        }

        /// <summary>
        /// Shows all solved values in the grid.
        /// </summary>
        public void Solve()
        {
            while (_shownValues < Constants.CELL_COUNT && _currentSudoku != null)
            {
                Next();
            }
        }

        /// <summary>
        /// Shows the next solved value in the grid.
        /// </summary>
        public void Next()
        {
            if (_shownValues < Constants.CELL_COUNT && _currentSudoku != null)
            {
                var value = _currentSudoku[_shownValues];
                Cells[value.Index].Value = value.Value.ToString();
                if (_shownValues < _givenValues)
                {
                    Cells[value.Index].Background = Brushes.LightGray;
                }

                _shownValues++;
            }
        }

        /// <summary>
        /// Hides the last shown value in the grid.
        /// </summary>
        public void Previous()
        {
            if (_shownValues > _givenValues && _currentSudoku != null)
            {
                _shownValues--;

                var value = _currentSudoku[_shownValues];
                Cells[value.Index].Value = string.Empty;
            }
        }

        /// <summary>
        /// Event handler for an cell click.
        /// </summary>
        public void CellMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_currentSudoku != null)
            {
                var textBox = (Border)sender;
                var cellViewModel = textBox.DataContext as CellViewModel;

                var valueIndex = _currentSudoku.FindIndex(v => v.Index == cellViewModel.Index);
                if (valueIndex >= _givenValues && valueIndex < _shownValues)
                {
                    Cells.ForEach(c => c.SaveState());
                    var foundValue = _currentSudoku[valueIndex];

                    var techniqueIndexes = foundValue.Techniques.SelectMany(t => t.Indexes);
                    var techniqueCellModels = Cells.Where(c => techniqueIndexes.Contains(c.Index));
                    var techniqueValueMap = foundValue.Techniques.SelectMany(t => t.ValueMap);

                    // Set the background for all techniques used in finding this value.
                    foreach (var cell in techniqueCellModels)
                    {
                        cell.Background = Brushes.LightSalmon;
                        cell.Value = string.Empty;
                    }

                    // Set the values all techniques used in finding this value.
                    foreach (var value in techniqueValueMap)
                    {
                        foreach (var index in value.Value)
                        {
                            Cells[index].Value = value.Key.ToString();
                        }
                    }

                    // Set the background for cells used to find this value.
                    var methodCells = Cells.Where(c => foundValue.Indexes.Contains(c.Index));
                    foreach (var cell in methodCells)
                    {
                        cell.Background = Brushes.Salmon;
                    }

                    // Set the cell where the value was found.
                    Cells[foundValue.Index].Background = Brushes.LightGreen;
                    Cells[foundValue.Index].Value = foundValue.Value.ToString();
                }

                e.Handled = true;
            }
        }

        /// <summary>
        /// Event handler for an cell click.
        /// </summary>
        public void CellMouseUp(object sender, MouseButtonEventArgs e)
        {
            Cells.ForEach(c => c.RestoreState());
        }

        /// <summary>
        /// Event handler for an mouse leave event.
        /// </summary>
        private void CellMouseLeave(object sender, MouseEventArgs e)
        {
            Cells.ForEach(c => c.RestoreState());
        }
    }
}
