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
                var possibleSpotSetsDic = sourceSet.PossibleSpots.Where(kvp => kvp.Value.Count() <= 3 && kvp.Value.Count() >= 2);
                foreach (var possibleSpots in possibleSpotSetsDic)
                {
                    var overlappingSets = sets.Except(sourceSet).Where(s => s.IsSubset(possibleSpots.Value));
                    foreach (var overlappingSet in overlappingSets)
                    {
                        var value = possibleSpots.Key;
                        var affectedCells = overlappingSet.Cells.Except(sourceSet.Cells);

                        var technique = CreatePossibleSpotOverlapTechnique(value, possibleSpots.Value, sourceSet, affectedCells.Indexes());
          
                        foreach (var cell in affectedCells)
                        {
                            cell.EliminatePossibleValue(possibleSpots.Key, technique);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates a <see cref="PossibleSpotOverlapTechnique"/>.
        /// </summary>
        /// <param name="value">Value that must go in the overlapping cells.</param>
        /// <param name="indexes">Indexes of the overlapping cells.</param>
        /// <param name="setindexes">Indexes of the cells in the sets that overlap.</param>
        /// <param name="affectedIndexes">Indexes of cells that had values eliminated by this technique.</param>
        private static PossibleSpotOverlapTechnique CreatePossibleSpotOverlapTechnique(int value, IEnumerable<Cell> cells, Set set, IEnumerable<int> affectedIndexes)
        {
            var techniques = set.FindMinTechniques(cells, value);
            return new PossibleSpotOverlapTechnique()
            {
                Complexity = DEFAULT_COMPLEXITY,
                IndexValueMap = set.Indexes().ToDictionary(i => i, i => cells.Indexes().Contains(i) ? value.ToEnumerable() : Enumerable.Empty<int>()),
                AffectedIndexes = affectedIndexes,
                Details = techniques,
            };
        }
    }
}
