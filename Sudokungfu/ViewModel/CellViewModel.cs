using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace Sudokungfu.ViewModel
{
    using Model;

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
        private int _fontSize;

        private List<ISudokuModel> _clickableModels;
        private DelegateCommand _clickCommand;
        private ISudokungfuViewModel _viewModel;

        public event PropertyChangedEventHandler PropertyChanged;

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
        public CellViewModel(int index, ISudokungfuViewModel viewModel)
        {
            Index = index;
            _clickCommand = DelegateCommand.Create(ClickAction);
            _clickableModels = new List<ISudokuModel>();
            _viewModel = viewModel;

            SetDefaultCellProperties();
        }

        public void SetCellProperties(IEnumerable<ISudokuModel> sudoku)
        {
            SetDefaultCellProperties();

            if (sudoku == null)
            {
                return;
            }

            var cellModel = sudoku.First(m => m.IndexValueMap.ContainsKey(Index) && m.IndexValueMap[Index].Any());
            if (sudoku.ToList().IndexOf(cellModel) >= _viewModel.ShownValues)
            {
                return;
            }

            SetCellValues(cellModel.IndexValueMap[Index]);
            if (cellModel.Details.Any())
            {
                _clickableModels.Add(cellModel.ClickableModel);
            }
            else
            {
                Background = Brushes.LightGray;
            }
        }

        public void SetCellProperties(ISudokuModel model)
        {
            SetDefaultCellProperties();

            var affectingTechniques = model.Details.Where(t => t.IndexValueMap.ContainsKey(Index) || t.AffectedIndexes.Contains(Index));
            var techniques = model.Details
                .Where(t => t.IndexValueMap.ContainsKey(Index))
                .Where(t => t.IndexValueMap[Index].Any()).Distinct();

            if (model.IndexValueMap.ContainsKey(Index))
            {
                if (model.IndexValueMap[Index].Any())
                {
                    var values = model.IndexValueMap[Index];
                    var size = ONE_VALUE_SIZE_DEFAULT;
                    if (values.Count() == 2)
                    {
                        size = TWO_VALUE_SIZE_DEFAULT;
                    }
                    else if (values.Count() == 3)
                    {
                        size = THREE_VALUE_SIZE_DEFAULT;
                    }

                    SetCellValues(model.IndexValueMap[Index], size);
                    Background = Brushes.LightGreen;
                }
                else if (techniques.Any())
                {
                    SetClickableTechniques(techniques);
                    Background = _clickableModels.Any() ? Brushes.Salmon : Brushes.DarkSalmon;
                }
                else
                {
                    Background = Brushes.Salmon;
                }
            }
            else if (techniques.Any())
            {
                SetClickableTechniques(techniques);
                Background = _clickableModels.Any() ? Brushes.LightSalmon : Brushes.DarkSalmon;
            }

            else if (affectingTechniques.Any())
            {
                Background = Brushes.LightSalmon;
            }
            else
            {
                Background = Brushes.DarkGray;
            }
        }

        private void SetClickableTechniques(IEnumerable<ISudokuModel> techniques)
        {
            var size = ONE_VALUE_SIZE_DEFAULT;
            var values = new List<int>();

            foreach (var technique in techniques)
            {
                var techValues = technique.IndexValueMap[Index];
                if ((techValues.Count() == 3 || technique.IndexValueMap.Count(kvp => kvp.Value.Any()) > 2) && size > THREE_VALUE_SIZE_DEFAULT)
                {
                    size = THREE_VALUE_SIZE_DEFAULT;
                }
                else if ((techValues.Count() == 2 || technique.IndexValueMap.Count(kvp => kvp.Value.Any()) == 2) && size > TWO_VALUE_SIZE_DEFAULT)
                {
                    size = TWO_VALUE_SIZE_DEFAULT;
                }
            }

             SetCellValues(techniques.SelectMany(t => t.IndexValueMap[Index]), size);
            _clickableModels.AddRange(techniques.Select(t => t.ClickableModel).Where(t => t.Details.Any()));
        }

        /// <summary>
        /// Sets default properties for the cell.
        /// </summary>
        private void SetDefaultCellProperties()
        {
            Value = string.Empty;
            FontSize = ONE_VALUE_SIZE_DEFAULT;
            Background = Brushes.White;
            _clickableModels.Clear();
        }

        /// <summary>
        /// Action to perform when the cell is clicked.
        /// </summary>
        private void ClickAction()
        {
            if (_clickableModels.Any())
            {
                _viewModel.CellClicked(_clickableModels.First());
            }
        }

        /// <summary>
        /// Sets the value to display in the cell. Sets the font size accordingly.
        /// </summary>
        /// <param name="values">Values that will go in the cell.</param>
        private void SetCellValues(IEnumerable<int> values, int size = ONE_VALUE_SIZE_DEFAULT)
        {
            _value = string.Join("", values);
            OnPropertyChanged(nameof(Value));
            FontSize = size;
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
