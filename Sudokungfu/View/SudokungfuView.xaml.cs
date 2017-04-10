using System.Windows;

namespace Sudokungfu.View
{
    using ViewModel;

    /// <summary>
    /// Interaction logic for SudokungfuView.xaml
    /// </summary>
    public partial class SudokungfuView : Window
    {
        private SudokungfuViewModel _viewModel;

        public SudokungfuView()
        {
            InitializeComponent();
            DataContext = _viewModel = new SudokungfuViewModel(ClearConfirm, Invalid, Error);
        }

        /// <summary>
        /// Display a diaglog for confirmation of clearing the Sudoku.
        /// </summary>
        private bool ClearConfirm()
        {
            var result = MessageBox.Show(Properties.Resources.ClearMessage, Properties.Resources.ClearTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
            return result == MessageBoxResult.OK;
        }

        /// <summary>
        /// Display a diaglog indicating an invalid Sudoku.
        /// </summary>
        private void Invalid()
        {
            MessageBox.Show(Properties.Resources.InvalidMessage, Properties.Resources.InvalidTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Display a diaglog indicating an error occured.
        /// </summary>
        private void Error()
        {
            MessageBox.Show(Properties.Resources.ErrorMessage, Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
