using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Techniques
{
    using Extensions;
    using Sets;

    /// <summary>
    /// Class that represents the 'Two Spot Closure' technique.
    /// </summary>
    public class TwoSpotClosureTechnique : BasicTechnique
    {
        private const int DEFAULT_COMPLEXITY = 2;

        private TwoSpotClosureTechnique()
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
                var possibleSpotDic = set.PossibleSpots.Where(s => s.Value.Count() == 2).ToList();
                foreach (var possibleSpotsA in possibleSpotDic)
                {
                    foreach (var possibleSpotsB in possibleSpotDic.Except(possibleSpotsA))
                    {
                        var twoValueSpotsUnion = possibleSpotsA.Value.Union(possibleSpotsB.Value);
                        if (twoValueSpotsUnion.Count() == 2)
                        {
                            var valueA = possibleSpotsA.Key;
                            var valueB = possibleSpotsB.Key;

                            var technique = CreateTwoSpotClosureTechnique(valueA, valueB, twoValueSpotsUnion, set, twoValueSpotsUnion.Indexes());

                            foreach (var cell in twoValueSpotsUnion)
                            {
                                foreach (var value in cell.PossibleValues.Except(possibleSpotsA.Key, possibleSpotsB.Key).ToList())
                                {
                                    cell.EliminatePossibleValue(value, technique);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates a <see cref="TwoSpotClosureTechnique"/>.
        /// </summary>
        /// <param name="valueA">Value in the closure.</param>
        /// <param name="valueB">Value in the closure.</param>
        /// <param name="indexes">Indexes of the closure.</param>
        /// <param name="setIndexes">Indexes of the cells in the set with a closure.</param>
        /// <param name="affectedIndexes">Indexes of cells that had values eliminated by this technique.</param>
        private static TwoSpotClosureTechnique CreateTwoSpotClosureTechnique(int valueA, int valueB, IEnumerable<Cell> cells, Set set, IEnumerable<int> affectedIndexes)
        {
            var techniquesA = set.FindMinTechniques(cells, valueA);
            var techniquesB = set.FindMinTechniques(cells, valueB);
            var values = new int[] { valueA, valueB };
            return new TwoSpotClosureTechnique()
            {
                Complexity = DEFAULT_COMPLEXITY,
                IndexValueMap = set.Indexes().ToDictionary(i => i, i => cells.Indexes().Contains(i) ? values : Enumerable.Empty<int>()),
                AffectedIndexes = affectedIndexes,
                Details = techniquesA.Concat(techniquesB)
            };
        }
    }
}
