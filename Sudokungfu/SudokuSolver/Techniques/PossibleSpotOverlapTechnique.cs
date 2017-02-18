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
            foreach (var set in sets)
            {
                var possibleValueSpotSets = set.PossibleSpots.Where(kvp => kvp.Value.Count() <= 3);
                foreach (var possibleValueSpot in possibleValueSpotSets)
                {
                    var overlappingSets = sets.Except(set).Where(s => possibleValueSpot.Value.All(p => s.Cells.Contains(p)));
                    foreach (var overlappingSet in overlappingSets)
                    {
                        var cellsToEliminate = overlappingSet.Cells.Except(possibleValueSpot.Value);
                        var technique = new PossibleSpotOverlapTechnique()
                        {
                            Indexes = cellsToEliminate.Select(c => c.Index),
                            ValueMap = new Dictionary<int, IEnumerable<int>>()
                            {
                                [possibleValueSpot.Key] = possibleValueSpot.Value.Select(c => c.Index)
                            }
                        };

                        foreach (var cell in cellsToEliminate)
                        {
                            cell.EliminatePossibleValue(possibleValueSpot.Key, technique);
                        }
                    }
                }
            }
        }
    }
}
