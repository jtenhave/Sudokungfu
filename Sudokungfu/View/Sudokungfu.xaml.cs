using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sudokungfu.View
{
    using Model;


    /// <summary>
    /// Interaction logic for SudokuWindow.xaml
    /// </summary>
    public partial class Sudokungfu : Window
    {
        private SudokuModel _model;

        /// <summary>
        /// The cells in the grid.
        /// </summary>
        public List<CellViewModel> Cells { get; set; } = new List<CellViewModel>();

        public Sudokungfu()
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
            _model.AllValues();
        }

        /// <summary>
        /// Event handler for an Next button click.
        /// </summary>
        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            _model.NextValue();
        }

        /// <summary>
        /// Event handler for an Previous button click.
        /// </summary>
        private void PreviousButtonClick(object sender, RoutedEventArgs e)
        {
            _model.PreviousValue();
        }

        private void OnModelChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "IsSolving")
            {
                Cursor = _model.IsSolving ? Cursors.Wait : Cursors.Arrow;
                ClearButton.IsEnabled = !_model.IsSolving;
            }
            else if (args.PropertyName == "IsInputEnabled")
            {
                EnterButton.IsEnabled = _model.IsInputEnabled;
            }
            else if (args.PropertyName == "SolveResult")
            {
                if (_model.SolveResult == SolveResult.NONE)
                {
                    NextButton.IsEnabled = false;
                    PreviousButton.IsEnabled = false;
                    SolveButton.IsEnabled = false;
                }
                if (_model.SolveResult == SolveResult.INVALID)
                {
                    MessageBox.Show(Properties.Resources.InvalidMessage, Properties.Resources.InvalidTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (_model.SolveResult == SolveResult.ERROR)
                {
                    MessageBox.Show(Properties.Resources.ErrorMessage, Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (args.PropertyName == "Details")
            {
                if (_model.SolveResult == SolveResult.SUCCESS)
                {
                    NextButton.IsEnabled = _model.Details.Count() < Constants.CELL_COUNT;
                    PreviousButton.IsEnabled = _model.Details.Any(d => d.Details.Any());
                    SolveButton.IsEnabled = _model.Details.Count() != Constants.CELL_COUNT;
                }
            }
        }
    }
}
