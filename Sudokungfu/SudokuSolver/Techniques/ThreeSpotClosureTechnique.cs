using Sudokungfu.SudokuSolver.Sets;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Sudokungfu.SudokuSolver.Techniques
{
    using Extensions;
    
    /// <summary>
    /// Class that represents the 'Three Spot Closure' technique.
    /// </summary>
    public class ThreeSpotClosureTechnique : BasicTechnique
    {
        private const int TWO_VALUE_FONT_SIZE = 20;
        private const int THREE_VALUE_FONT_SIZE = 15;
        private const int DEFAULT_COMPLEXITY = 3;

        private ThreeSpotClosureTechnique()
        {

        }

        /// <summary>
        /// Applies the technique to the Sudoku.
        /// </summary>
        /// <param name="cells">Cells in the Sudoku.</param>
        /// <param name="sets">Sets of cells in the Sudoku.</param>
        public static void Apply(IEnumerable<Cell> cells, IEnumerable<Set> sets)
        {
            foreach (var set in sets)
            {
                var possibleSpotSets = set.PossibleSpots.Where(s => s.Value.Count() == 2 || s.Value.Count() == 3);
                foreach (var possibleSpotsA in possibleSpotSets)
                {
                    foreach (var possibleSpotsB in possibleSpotSets.Except(possibleSpotsA))
                    {
                        foreach (var possibleSpotsC in possibleSpotSets.Except(possibleSpotsA, possibleSpotsB))
                        {
                            var threeValueSpotsUnion = possibleSpotsA.Value.Union(possibleSpotsB.Value).Union(possibleSpotsC.Value);
                            if (threeValueSpotsUnion.Count() == 3)
                            {
                                var valueA = new KeyValuePair<int, IEnumerable<int>>(possibleSpotsA.Key, possibleSpotsA.Value.Indexes());
                                var valueB = new KeyValuePair<int, IEnumerable<int>>(possibleSpotsB.Key, possibleSpotsB.Value.Indexes());
                                var valueC = new KeyValuePair<int, IEnumerable<int>>(possibleSpotsC.Key, possibleSpotsC.Value.Indexes());

                                var technique = CreateThreeSpotClosureTechnique(valueA, valueB, valueC, set.Indexes());
                                foreach (var cell in threeValueSpotsUnion)
                                {
                                    foreach (var value in cell.PossibleValues.Except(possibleSpotsA.Key, possibleSpotsB.Key, possibleSpotsC.Key).ToList())
                                    {
                                        cell.EliminatePossibleValue(value, technique);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static ThreeSpotClosureTechnique CreateThreeSpotClosureTechnique(KeyValuePair<int, IEnumerable<int>> valueA, KeyValuePair<int, IEnumerable<int>> valueB, KeyValuePair<int, IEnumerable<int>> valueC, IEnumerable<int> setIndexes)
        {
            var values = new KeyValuePair<int, IEnumerable<int>>[] { valueA, valueB, valueB };
            return new ThreeSpotClosureTechnique()
            {
                Complexity = DEFAULT_COMPLEXITY,
                IndexValueMap = setIndexes.ToDictionary(i => i, i => values.Where(v => v.Value.Contains(i)).Select(v => v.Key)),
                UsesFoundValues = false
            };
        }
    }
}
