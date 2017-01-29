using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
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
            _givenValues = _foundValues.Count(v => v.Method?.Type == FoundValueMethodType.GIVEN);
            _shownValues = 0;

            while(_shownValues < _givenValues)
            {
                Next();
            }
        }

        /// <summary>
        /// Shows all solved values in the grid.
        /// </summary>
        public void Solve()
        {
            while (_shownValues < Constants.CELL_COUNT)
            {
                Next();
            }
        }

        /// <summary>
        /// Shows the next solved value in the grid.
        /// </summary>
        public void Next()
        {
            if (_shownValues < Constants.CELL_COUNT)
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
            if (_shownValues > _givenValues)
            {
                _shownValues--;

                var value = _currentSudoku[_shownValues];
                Cells[value.Index].Value = string.Empty;
            }
        }
    }
}
