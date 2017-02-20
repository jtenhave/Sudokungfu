using Sudokungfu.SudokuSolver.Sets;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Techniques
{
    using Extensions;

    /// <summary>
    /// Class that represents the 'Possible Spot Overlap' technique.
    /// </summary>
    public class PossibleSpotOverlapTechnique : BasicTechnique
    {
        private const int DEFAULT_COMPLEXITY = 2;

        private PossibleSpotOverlapTechnique()
        {
            
        }

        /// <summary>
        /// Applies the technique to the Sudoku.
        /// </summary>
        /// <param name="cells">Cells in the Sudoku.</param>
        /// <param name="sets">Sets of cells in the Sudoku.</param>
        public static void Apply(IEnumerable<Cell> cells, IEnumerable<Set> sets)
        {
            foreach (var sourceSet in sets)
            {
                var possibleSpotSetsDic = sourceSet.PossibleSpots.Where(kvp => kvp.Value.Count() <= 3);
                foreach (var possibleSpots in possibleSpotSetsDic)
                {
                    var overlappingSets = sets.Except(sourceSet).Where(s => s.IsSubset(possibleSpots.Value));
                    foreach (var overlappingSet in overlappingSets)
                    {
                        var value = possibleSpots.Key;
                        var indexes = possibleSpots.Value.Indexes();
                        var setIndexes = sourceSet.Cells.Union(overlappingSet.Cells).Indexes();

                        var technique = CreatePossibleSpotOverlapTechnique(value, indexes, setIndexes);
          
                        foreach (var cell in overlappingSet.Cells.Except(sourceSet.Cells))
                        {
                            cell.EliminatePossibleValue(possibleSpots.Key, technique);
                        }
                    }
                }
            }
        }

        public static PossibleSpotOverlapTechnique CreatePossibleSpotOverlapTechnique(int value, IEnumerable<int> indexes, IEnumerable<int> setindexes)
        {
            return new PossibleSpotOverlapTechnique()
            {
                Complexity = DEFAULT_COMPLEXITY,
                IndexValueMap = setindexes.ToDictionary(i => i, i => indexes.Contains(i) ? value.ToEnumerable() : Enumerable.Empty<int>()),
                UsesFoundValues = false
            };
        }
    }
}
