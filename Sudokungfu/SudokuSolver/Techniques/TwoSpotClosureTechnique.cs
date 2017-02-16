using Sudokungfu.SudokuSolver.Sets;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Techniques
{
    using Extensions;

    /// <summary>
    /// Class that represents the 'Three Spot Closure' technique.
    /// </summary>
    public class TwoSpotClosureTechnique : AdvancedTechnique
    {
        /// <summary>
        /// Creates a new <see cref="TwoSpotClosureTechnique"/>.
        /// </summary>
        public TwoSpotClosureTechnique()
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
                var possibleSpotDic = set.GetValuePossibleSpots().Where(s => s.Value.Count() == 2).ToList();
                foreach (var possibleSpotsA in possibleSpotDic)
                {
                    foreach (var possibleSpotsB in possibleSpotDic.Except(possibleSpotsA))
                    {
                        var twoValueSpotsUnion = possibleSpotsA.Value.Union(possibleSpotsB.Value);
                        if (twoValueSpotsUnion.Count() == 2)
                        {
                            var twoSpotTechnique = new TwoSpotClosureTechnique()
                            {
                                Indexes = twoValueSpotsUnion.Select(c => c.Index),
                                ValueMap = new Dictionary<int, IEnumerable<int>>()
                                {
                                    [possibleSpotsA.Key] = possibleSpotsA.Value.Select(c => c.Index),
                                    [possibleSpotsB.Key] = possibleSpotsB.Value.Select(c => c.Index)
                                }
                            };

                            foreach (var cell in twoValueSpotsUnion)
                            {
                                foreach (var value in cell.PossibleValues.Except(possibleSpotsA.Key, possibleSpotsB.Key).ToList())
                                {
                                    cell.EliminatePossibleValue(value, twoSpotTechnique);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
