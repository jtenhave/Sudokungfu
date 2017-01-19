
namespace Sudokungfu.Extensions
{
    public static class IntExtensions
    {
        public static bool IsSudokuIndex(this int value)
        {
            return value >= 0 && value < Constants.CELL_COUNT;
        }
    }
}
