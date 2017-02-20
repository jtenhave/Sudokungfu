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
                            var indexes = twoValueSpotsUnion.Indexes();
                            var setIndexes = set.Indexes();

                            var technique = CreateTwoSpotClosureTechnique(valueA, valueB, indexes, setIndexes);

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

        public static TwoSpotClosureTechnique CreateTwoSpotClosureTechnique(int valueA, int valueB, IEnumerable<int> indexes, IEnumerable<int> setIndexes)
        {
            var values = new int[] { valueA, valueB };
            return new TwoSpotClosureTechnique()
            {
                Complexity = DEFAULT_COMPLEXITY,
                IndexValueMap = setIndexes.ToDictionary(i => i, i => indexes.Contains(i) ?  values : Enumerable.Empty<int>()),
                UsesFoundValues = false
            };
        }
    }
}
