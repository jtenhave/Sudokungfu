using System.Collections.Generic;
using System.Windows.Controls;

namespace Sudokungfu.SudokuGrid
{
    /// <summary>
    /// Interaction logic for SudokuGrid.xaml
    /// </summary>
    public partial class SudokuGridView : UserControl
    {
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
    }
}
