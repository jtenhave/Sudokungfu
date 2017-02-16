using Sudokungfu.SudokuSolver.Sets;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Techniques
{
    using Extensions;

    /// <summary>
    /// Class that represents the 'Three Spot Closure' technique.
    /// </summary>
    public class ThreeSpotClosureTechnique : AdvancedTechnique
    {
        /// <summary>
        /// Creates a new <see cref="ThreeSpotClosureTechnique"/>.
        /// </summary>
        public ThreeSpotClosureTechnique()
        {
            Complexity = 3;
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
                var possibleSpotSets = set.GetValuePossibleSpots().Where(s => s.Value.Count() == 2 || s.Value.Count() == 3);
                foreach (var possibleSpotsA in possibleSpotSets)
                {
                    foreach (var possibleSpotsB in possibleSpotSets.Except(possibleSpotsA))
                    {
                        foreach (var possibleSpotsC in possibleSpotSets.Except(possibleSpotsA, possibleSpotsB))
                        {
                            var threeValueSpotsUnion = possibleSpotsA.Value.Union(possibleSpotsB.Value).Union(possibleSpotsC.Value);
                            if (threeValueSpotsUnion.Count() == 3)
                            {
                                var threeSpotTechnique = new ThreeSpotClosureTechnique()
                                {
                                    Indexes = threeValueSpotsUnion.Select(c => c.Index),
                                    ValueMap = new Dictionary<int, IEnumerable<int>>()
                                    {
                                        [possibleSpotsA.Key] = possibleSpotsA.Value.Select(c => c.Index),
                                        [possibleSpotsB.Key] = possibleSpotsB.Value.Select(c => c.Index),
                                        [possibleSpotsC.Key] = possibleSpotsC.Value.Select(c => c.Index),
                                    }
                                };

                                foreach (var cell in threeValueSpotsUnion)
                                {
                                    foreach (var value in cell.PossibleValues.Except(possibleSpotsA.Key, possibleSpotsB.Key, possibleSpotsC.Key).ToList())
                                    {
                                        cell.EliminatePossibleValue(value, threeSpotTechnique);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
