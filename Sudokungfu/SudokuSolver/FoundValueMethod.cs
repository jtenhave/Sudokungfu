
namespace Sudokungfu.SudokuSolver
{
    /// <summary>
    /// Enum that represents the type of method.
    /// </summary>
    public enum FoundValueMethodType
    {
        GIVEN,
        SET,
        SINGLE
    }

    /// <summary>
    /// Class that represents a method of finding a value in the Sudoku.
    /// </summary>
    public class FoundValueMethod
    {
        /// <summary>
        /// The type of the method.
        /// </summary>
        public FoundValueMethodType Type { get; private set; }


        /// <summary>
        /// Creates a new <see cref="FoundValueMethod"/>.
        /// </summary>
        private FoundValueMethod()
        {

        }

        /// <summary>
        /// Creates a new <see cref="FoundValueMethod"/> for value that was give.
        /// </summary>
        public static FoundValueMethod CreateGivenMethod()
        {
            return new FoundValueMethod()
            {
                Type = FoundValueMethodType.GIVEN
            };
        }
    }
}
