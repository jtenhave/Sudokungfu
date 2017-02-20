using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test
{
    using Sudokungfu.SudokuSolver.Techniques;

    internal class TestTechnique : ITechnique
    {
        public int Complexity { get; set; }

        public IDictionary<int, IEnumerable<int>> IndexValueMap { get; set; }

        public bool UsesFoundValues { get; set; }

        public static ITechnique CreateTestTechnique(int complexity, params int[] indexes)
        {
            return new TestTechnique()
            {
                Complexity = complexity,
                IndexValueMap = indexes.ToDictionary(i => i, i => Enumerable.Empty<int>())
            };
        }
    }
}
