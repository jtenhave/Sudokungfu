using System;
using System.Globalization;
using System.Windows.Data;

namespace Sudokungfu
{
    using Extensions;

    /// <summary>
    /// Class for the Sudoku grid <see cref="CellViewModel"/> border converter.
    /// </summary>
    /// <remarks>
    /// This border converter makes the cell grid appear like a Sudoku grid.
    /// </remarks>
    public class CellBorderConverter : IValueConverter
    {
        public const int NO_WIDTH = 0;
        public const int THIN_WIDTH = 1;
        public const int THICK_WIDTH = 4;

        /// <summary>
        /// Converts a <see cref="CellViewModel"/> index to a WPF border string.
        /// </summary>
        /// <param name="value">The cell index.</param>
        /// <returns>The cell border string.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cellIndex = value as int?;
            if (!cellIndex.HasValue)
            {
                throw new ArgumentNullException("value must be an int");
            }

            if (!cellIndex.Value.IsSudokuIndex())
            {
                throw new ArgumentOutOfRangeException("value must be between 0 and 80");
            }

            var mod = cellIndex % Constants.SET_SIZE;
            var div = cellIndex / Constants.SET_SIZE;

            var left = mod == 0 ? THICK_WIDTH : NO_WIDTH;
            var top = div == 0 ? THICK_WIDTH : NO_WIDTH;
            var right = mod == 2 || mod == 5 || mod == 8 ? THICK_WIDTH : THIN_WIDTH;
            var bottom = div == 2 || div == 5 || div == 8 ? THICK_WIDTH : THIN_WIDTH;

            return $"{left} {top} {right} {bottom}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
