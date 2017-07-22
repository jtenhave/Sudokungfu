using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace Sudokungfu.View
{
    /// <summary>
    /// Class for the cursor converter that decides what cursor to display.
    /// </summary>
    public class CursorConverter : IValueConverter
    {
        /// <summary>
        /// Converts a bool to a WPF cursor.
        /// </summary>
        /// <param name="value">Whether a Sudoku is being solved.</param>
        /// <returns>Cursor to display.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isSolving = value as bool?;
            return isSolving.Value ? Cursors.Wait : Cursors.Arrow;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
