using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Techniques
{
    using Extensions;
    using Model;
    using Sets;

    /// <summary>
    /// Class that represents the 'Two Spot Closure' technique.
    /// </summary>
    public class TwoSpotClosureTechnique : TechniqueBase
    {
        private const int COMPLEXITY = 2;
        private readonly IEnumerable<ISudokuModel> _techniques;

        /// <summary>
        /// Details that make up this technique.
        /// </summary>
        public override IEnumerable<ISudokuModel> Details
        {
            get
            {
                return _techniques;
            }
        }

        /// <summary>
        /// Creates a new <see cref="TwoSpotClosureTechnique"/>.
        /// </summary>
        /// <param name="valueA">First value.</param>
        /// <param name="valueB">Second value.</param>
        /// <param name="cells">Cells where the values go.</param>
        /// <param name="set">Set that contains the cells.</param>
        private TwoSpotClosureTechnique(int valueA, int valueB, IEnumerable<Cell> cells, Set set) : base (COMPLEXITY)
        {
            _techniques = set.FindMinTechniques(cells, valueA)
                .Concat(set.FindMinTechniques(cells, valueB));

            foreach (var index in set.Indexes)
            {
                _indexValueMap[index] = Enumerable.Empty<int>();
            }

            var values = new int[] { valueA, valueB };
            foreach (var index in cells.Indexes())
            {
                _indexValueMap[index] = values;
            }

            _affectedIndexes.AddRange(cells.Indexes());
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

                            var technique = new TwoSpotClosureTechnique(valueA, valueB, twoValueSpotsUnion, set);

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
    }
}
