using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Linq;

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
        private ISudokuModel _model;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the index of the cell. 
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Gets the value of the cell.
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }

            set
            {
                int i = 0;
                if (string.IsNullOrWhiteSpace(value) || (int.TryParse(value, out i) && i.IsSudokuValue()))
                {
                    _value = i == 0 ? string.Empty : value;
                    OnPropertyChanged(nameof(Value));

                    if (_model.IsInputEnabled)
                    {
                        CellModel.IndexValueMap[Index] = i.ToEnumerable();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the background color of the cell.
        /// </summary>
        public Brush Background
        {
            get
            {
                return _background;
            }

            private set
            {
                if (_background != value)
                {
                    _background = value;
                    OnPropertyChanged(nameof(Background));
                }
            }
        }

        /// <summary>
        /// Gets the read only value of the cell.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return _isReadOnly;
            }

            private set
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
        /// Gets the cursor of the cell.
        /// </summary>
        public Cursor Cursor
        {
            get
            {
                return _cursor;
            }

            private set
            {
                if (_cursor != value)
                {
                    _cursor = value;
                    OnPropertyChanged(nameof(Cursor));
                }
            }
        }

        /// <summary>
        /// Gets the font style of the cell.
        /// </summary>
        public FontStyle FontStyle
        {
            get
            {
                return _fontStyle;
            }

            private set
            {
                if (_fontStyle != value)
                {
                    _fontStyle = value;
                    OnPropertyChanged(nameof(FontStyle));
                }
            }
        }

        /// <summary>
        /// Gets the font size of cell.
        /// </summary>
        public int FontSize
        {
            get
            {
                return _fontSize;
            }

            private set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    OnPropertyChanged(nameof(FontSize));
                }
            }
        }

        private ISudokuModel CellModel
        {
            get
            {
                return _model.Details.FirstOrDefault(d => d.IndexValueMap.ContainsKey(Index));
            }
        }

        /// <summary>
        /// Creates a new <see cref="CellViewModel"/>
        /// </summary>
        /// <param name="index">Index of the cell.</param>
        public CellViewModel(int index)
        {
            if (!index.IsSudokuIndex())
            {
                throw new ArgumentOutOfRangeException("index must be between 0 and 80");
            }

            Index = index;
        }

        public void SetSudokuModel(ISudokuModel model)
        {
            _model = model;
            _model.PropertyChanged += OnModelChanged;

            RefreshCell();
        }

        private void OnModelChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "IsSolving" || args.PropertyName == "IsInputEnabled")
            {
                IsReadOnly = !_model.IsInputEnabled;
                Cursor = _model.IsSolving ? Cursors.Wait : _model.IsInputEnabled ? Cursors.IBeam : Cursors.Arrow;
            }
            else if (args.PropertyName == "Details")
            {
                RefreshCell();
            }
        }

        private void RefreshCell()
        {
            if (_model.IndexValueMap.Any())
            {
                // TODO show details.
            }
            else
            {
                var value = CellModel.IndexValueMap[Index].First().ToString();
                FontSize = FONT_SIZE_DEFAULT;
                FontStyle = FontStyles.Normal;
                Background = value != "0" && !CellModel.Details.Any() ? Brushes.LightGray : Brushes.White;             
                Value = value;
            }
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
