using Sudokungfu.SudokuSolver.Sets;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Techniques
{
    using Extensions;

    /// <summary>
    /// Class that represents the 'Possible Spot Overlap' technique.
    /// </summary>
    public class PossibleSpotOverlapTechnique : AdvancedTechnique
    {
        public PossibleSpotOverlapTechnique()
        {
            Complexity = 2;
        }

        /// <summary>
        /// Applies the technique to the Sudoku.
        /// </summary>
        /// <param name="cells">Cells in the Sudoku.</param>
        /// <param name="sets">Sets of cells in the Sudoku.</param>
        public override void Apply(IEnumerable<Cell> cells, IEnumerable<Set> sets)
        {
            foreach (var sourceSet in sets)
            {
                var possibleSpotSetsDic = sourceSet.PossibleSpots.Where(kvp => kvp.Value.Count() <= 3);
                foreach (var possibleSpots in possibleSpotSetsDic)
                {
                    var overlappingSets = sets.Except(sourceSet).Where(s => s.IsSubset(possibleSpots.Value));
                    foreach (var overlappingSet in overlappingSets)
                    {
                        var cellsToEliminate = overlappingSet.Cells.Except(sourceSet.Cells);
                        var technique = new PossibleSpotOverlapTechnique()
                        {
                            Indexes = sourceSet.Cells.Union(overlappingSet.Cells).Select(c => c.Index),
                            ValueMap = new Dictionary<int, IEnumerable<int>>()
                            {
                                [possibleSpots.Key] = possibleSpots.Value.Select(c => c.Index)
                            }
                        };

                        foreach (var cell in cellsToEliminate)
                        {
                            cell.EliminatePossibleValue(possibleSpots.Key, technique);
                        }
                    }
                }
            }
        }
    }
}
