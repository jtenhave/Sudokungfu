﻿using System;
using System.ComponentModel;
using System.Windows;
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
        private const int FONT_SIZE_DEFAULT = 36;

        private string _value;
        private Brush _background;
        private FontStyle _fontStyle;
        private int _fontSize;

        public event PropertyChangedEventHandler PropertyChanged;

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
                if (string.IsNullOrWhiteSpace(value) || (int.TryParse(value, out i) && i.IsSudokuValue()))
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
        public CellViewModel(int index)
        {
            if (!index.IsSudokuIndex())
            {
                throw new ArgumentOutOfRangeException("index must be between 0 and 80");
            }

            Index = index;

            SetDefaultCellProperties();
        }

        public void SetDefaultCellProperties()
        {
            Value = string.Empty;
            FontSize = FONT_SIZE_DEFAULT;
            Background = Brushes.White;
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