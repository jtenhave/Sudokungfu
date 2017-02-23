using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver
{
    using Techniques;
    using Sets;

    public static class SolverExtensions
    {
        public static IEnumerable<int> Indexes(this ITechnique technique)
        {
            return technique.IndexValueMap.Keys;
        }

        public static IEnumerable<int> Indexes(this Set set)
        {
            return set.Cells.Indexes();
        }

        public static IEnumerable<int> Indexes(this IEnumerable<Cell> cells)
        {
            return cells.Select(c => c.Index);
        }

        public static IEnumerable<int> Indexes(this IEnumerable<ITechnique> techniques)
        {
            return techniques.SelectMany(t => t.IndexValueMap.Keys).Distinct();
        }
    }
}
