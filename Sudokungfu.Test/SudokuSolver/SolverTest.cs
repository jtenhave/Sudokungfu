using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver
{
    using Sudokungfu.SudokuSolver;

    /// <summary>
    /// Test class for <see cref="Solver"/>
    /// </summary>
    [TestClass]
    public class SolverTest
    {
        /// <summary>
        /// Test solving with too few values.
        /// </summary>
        [TestMethod]
        public void TestTooFewValues()
        {
            var values = new int[74];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = 4;
            }

            var result = Solver.Solve(values);
            Assert.AreEqual(SudokuResultType.ERROR, result.Type);
        }

        /// <summary>
        /// Test solving with invalid values.
        /// </summary>
        [TestMethod]
        public void TestInvalidValues()
        {
            var values = new int[Constants.CELL_COUNT];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = 4;
            }

            values[45] = 10;

            var result = Solver.Solve(values);
            Assert.AreEqual(SudokuResultType.ERROR, result.Type);
        }

        /// <summary>
        /// Test solving a basic Sudoku (only basic rules needed).
        /// </summary>
        [TestMethod]
        public void TestSolveBasicSudoku()
        {
            var values = new int[] { 0,9,1,0,7,0,0,0,0,0,7,0,9,8,5,1,2,0,8,0,0,0,0,4,0,9,7,7,0,0,8,0,6,4,0,0,0,4,0,0,0,0,0,6,9,6,1,0,0,0,7,0,0,0,0,0,0,3,0,9,2,7,8,9,0,7,2,0,0,0,1,0,0,8,2,0,0,1,0,3,6 };
            var result = Solver.Solve(values);

            Assert.AreEqual(SudokuResultType.SUCCESS, result.Type);
            Assert.AreEqual(Constants.CELL_COUNT, result.FoundValues.Count());
        }
    }
}
