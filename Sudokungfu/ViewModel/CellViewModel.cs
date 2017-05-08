using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Sudokungfu.ViewModel
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
        private const int ONE_VALUE_SIZE_DEFAULT = 36;
        private const int TWO_VALUE_SIZE_DEFAULT = 26;
        private const int THREE_VALUE_SIZE_DEFAULT = 16;

        private string _value;
        private Brush _background;
        private FontStyle _fontStyle;
        private int _fontSize;

        private Action<int> _clicked;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the click command.
        /// </summary>
        public ICommand ClickCommand
        {
            get
            {
                return DelegateCommand.Create(() =>
                {
                    _clicked(Index);
                });
            }
        }

        /// <summary>
        /// Gets the index of the cell. 
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
                int i = 0;
                if (string.IsNullOrWhiteSpace(value) || int.TryParse(value, out i))
                {
                    _value = i == 0 ? string.Empty : value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        /// <summary>
        /// Gets the value of the cell as an int.
        /// </summary>
        public int ValueAsInt
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Value))
                {
                    return 0;
                }

                return int.Parse(Value);
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
        /// Gets the font style of the cell.
        /// </summary>
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

        /// <summary>
        /// Gets the font size of cell.
        /// </summary>
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
        /// <param name="index">Index of the cell.</param>
        public CellViewModel(int index, Action<int> clicked)
        {
            Index = index;
            _clicked = clicked;

            SetDefaultCellProperties();
        }

        /// <summary>
        /// Sets default properties for the cell.
        /// </summary>
        public void SetDefaultCellProperties()
        {
            Value = string.Empty;
            FontSize = ONE_VALUE_SIZE_DEFAULT;
            Background = Brushes.White;
        }

        /// <summary>
        /// Sets the value to display in the cell. Sets the font size accordingly.
        /// </summary>
        /// <param name="values">Values that will go in the cell.</param>
        public void SetCellValues(IEnumerable<int> values)
        {
            _value = string.Join("", values);
            OnPropertyChanged(nameof(Value));
            switch (values.Count())
            {
                case 3:
                    FontSize = THREE_VALUE_SIZE_DEFAULT;
                    break;
                case 2:
                    FontSize = TWO_VALUE_SIZE_DEFAULT;
                    break;
                default:
                    FontSize = ONE_VALUE_SIZE_DEFAULT;
                    break;
            }
        }

        /// <summary>
        /// Notifies listeners of the PropertyChanged event that a property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
