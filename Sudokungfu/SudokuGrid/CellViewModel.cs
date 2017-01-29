using System;
using System.ComponentModel;
using System.Windows.Media;

namespace Sudokungfu.SudokuGrid
{
    using Extensions;

    /// <summary>
    /// Class for the view model of a cell in the Sudoku grid.
    /// </summary>
    /// <remarks>
    /// Cells are indexed horizontally (e.g. The first row in the grid is indexed 0-8).
    /// </remarks>
    public class CellViewModel : INotifyPropertyChanged
    {
        private string _value;
        private Brush _background;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The index of the cell. 
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// The value of the cell.
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }

            set
            {
                int i;
                if (string.IsNullOrWhiteSpace(value) || (int.TryParse(value, out i) && i >= 1 && i <= 9))
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        /// <summary>
        /// The background color of the cell.
        /// </summary>
        public Brush Background
        {
            get
            {
                return _background;
            }

            set
            {
                if (_background != value)
                {
                    _background = value;
                    OnPropertyChanged(nameof(Background));
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="CellViewModel"/>
        /// </summary>
        /// <param name="index">The index of the cell.</param>
        public CellViewModel(int index)
        {
            if (!index.IsSudokuIndex())
            {
                throw new ArgumentOutOfRangeException("index must be between 0 and 80");
            }

            Index = index;
            Background = Brushes.White;
        }

        /// <summary>
        /// Notifies listeners of the PropertyChanged event that a property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
