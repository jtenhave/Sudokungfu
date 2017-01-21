using System.Windows;

namespace Sudokungfu
{
    /// <summary>
    /// Interaction logic for SudokuWindow.xaml
    /// </summary>
    public partial class SudokuWindow : Window
    {
        public SudokuWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for a Clear button click
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
        }
    }
}
