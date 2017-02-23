using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

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
        private const int FONT_SIZE_DEFAULT = 36;

        private string _value;
        private Brush _background;
        private bool _isReadOnly;
        private Cursor _cursor;
        private FontStyle _fontStyle;
        private int _fontSize;

        private CellViewModel _savedState;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The index of the cell. 
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Gets or sets the value of the cell.
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
        /// Gets or sets background color of the cell.
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
        /// Gets or sets the read only value of the cell.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return _isReadOnly;
            }

            set
            {
                if (_isReadOnly != value)
                {
                    _isReadOnly = value;
                    Cursor = _isReadOnly ? Cursors.Arrow : Cursors.IBeam;

                    OnPropertyChanged(nameof(IsReadOnly));
                }
            }
        }

        /// <summary>
        /// Gets or sets the cursor of the cell.
        /// </summary>
        public Cursor Cursor
        {
            get
            {
                return _cursor;
            }

            set
            {
                if (_cursor != value)
                {
                    _cursor = value;
                    OnPropertyChanged(nameof(Cursor));
                }
            }
        }

        public FontStyle FontStyle
        {
            get
            {
                return _fontStyle;
            }

            set
            {
                if (_fontStyle != value)
                {
                    _fontStyle = value;
                    OnPropertyChanged(nameof(FontStyle));
                }
            }
        }

        public int FontSize
        {
            get
            {
                return _fontSize;
            }

            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    OnPropertyChanged(nameof(FontSize));
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
            Value = string.Empty;
            FontSize = FONT_SIZE_DEFAULT;
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

        /// <summary>
        /// Saves the visual state of the cell.
        /// </summary>
        public void SaveState()
        {
            _savedState = new CellViewModel(this.Index);
            _savedState._background = _background;
            _savedState._value = _value;
            _savedState._isReadOnly = _isReadOnly;
            _savedState._cursor = _cursor;
            _savedState._fontSize = _fontSize;
            _savedState._fontStyle = _fontStyle;
        }

        /// <summary>
        /// Restores the visual state of the cell.
        /// </summary>
        public void RestoreState()
        {
            if (_savedState != null)
            {
                Background = _savedState._background;
                Value = _savedState._value;
                IsReadOnly = _savedState._isReadOnly;
                Cursor = _savedState._cursor;
                FontStyle = _savedState._fontStyle;
                FontSize = _savedState._fontSize;

                _savedState = null;
            }
        }
    }
}
