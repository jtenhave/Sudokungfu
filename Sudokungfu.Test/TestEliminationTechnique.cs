using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test
{
    using Sudokungfu.SudokuSolver;

    internal class TestEliminationTechnique : IEliminationTechnique
    {
        private int _complexity;

        public int Complexity
        {
            get
            {
                return _complexity;
            }
        }

        public IEnumerable<int> Indexes
        {
            get
            {
                return Enumerable.Empty<int>();
            }
        }

        public IDictionary<int, IEnumerable<int>> ValueMap
        {
            get
            {
                return new Dictionary<int, IEnumerable<int>>();
            }
        }

        public TestEliminationTechnique(int complexity)
        {
            _complexity = complexity;
        }
    }
}
