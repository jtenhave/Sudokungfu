using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace Sudokungfu.ViewModel
{
    using Model;
    using Sudokungfu.SudokuSolver;

    /// <summary>
    /// Class that represents event args for a click event on a <see cref="Cell"/>.
    /// </summary>
    public class ClickedEventArgs: EventArgs 
    {
        /// <summary>
        /// Model that will be displayed as a result of the click event.
        /// </summary>
        public ISudokuModel ClickedModel { get; private set; }

        /// <summary>
        /// Creates a new <see cref="ClickedEventArgs"/>.
        /// </summary>
        /// <param name="model"></param>
        public ClickedEventArgs(ISudokuModel model)
        {
            ClickedModel = model;
        }
    }

    /// <summary>
    /// Class for the view model of a cell in the Sudoku grid.
    /// </summary>
    /// <remarks>
    /// Cells are indexed horizontally (e.g. The first row in the grid is indexed 0-8).
    /// </remarks>
    public class CellViewModel : INotifyPropertyChanged
    {
        public const int ONE_VALUE_SIZE_DEFAULT = 36;
        public const int TWO_VALUE_SIZE_DEFAULT = 26;
        public const int THREE_VALUE_SIZE_DEFAULT = 16;

        private string _value;
        private Brush _background;
        private int _fontSize;

        private DelegateCommand _clickCommand;
        private FoundValue _foundValue;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<ClickedEventArgs> CellClicked;

        /// <summary>
        /// Gets the click command.
        /// </summary>
        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand;
            }
        }

        /// <summary>
        /// Model that will be shown when the cell is clicked.
        /// </summary>
        public ISudokuModel ClickableModel { get; set; }

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
                if (value.Length > 1)
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
                else if (string.IsNullOrWhiteSpace(value) || int.TryParse(value, out i))
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
        /// Found value that belongs in the cell.
        /// </summary>
        public FoundValue FoundValue
        {
            get
            {
                return _foundValue;
            }
            set
            {
                _foundValue = value;
                SetDefaultCellProperties();

                if (_foundValue != null)
                {
                    Value = _foundValue.Value.ToString();
                    if (_foundValue.Details.Any())
                    {
                        ClickableModel = _foundValue.ClickableModel;
                    }
                    else
                    {
                        Background = Brushes.LightGray;
                    }
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="CellViewModel"/>.
        /// </summary>
        /// <param name="index">Index of the cell.</param>
        public CellViewModel(int index)
        {
            Index = index;
            _clickCommand = DelegateCommand.Create(OnCellClicked);

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
            ClickableModel = null;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnCellClicked()
        {
            if (ClickableModel != null)
            {
                CellClicked?.Invoke(this, new ClickedEventArgs(ClickableModel));
            }
        }
    }
}
