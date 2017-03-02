using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Sudokungfu.Test
{
    using Sudokungfu.SudokuSolver.Techniques;

    internal class TestTechnique : ITechnique
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IDictionary<int, IEnumerable<int>> IndexValueMap { get; set; }

        public IEnumerable<ISudokuModel> Details { get; set; }

        public IEnumerable<int> AffectedIndexes { get; set; }

        public bool IsInputEnabled { get; set; }

        public bool IsSolving { get; set; }

        public int Complexity { get; set; }

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
