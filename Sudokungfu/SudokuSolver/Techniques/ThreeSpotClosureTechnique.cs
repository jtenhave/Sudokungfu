using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Techniques
{
    using Extensions;
    using Model;
    using Sets;

    /// <summary>
    /// Class that represents the 'Three Spot Closure' technique.
    /// </summary>
    public class ThreeSpotClosureTechnique : TechniqueBase
    {
        private const int COMPLEXITY = 3;
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

        private ThreeSpotClosureTechnique(KeyValuePair<int, IEnumerable<Cell>> valueA, KeyValuePair<int, IEnumerable<Cell>> valueB, KeyValuePair<int, IEnumerable<Cell>> valueC, Set set) : base (COMPLEXITY)
        {
            _techniques = set.FindMinTechniques(valueA.Value, valueA.Key)
                .Concat(set.FindMinTechniques(valueB.Value, valueB.Key))
                .Concat(set.FindMinTechniques(valueC.Value, valueC.Key));

            var values = new KeyValuePair<int, IEnumerable<Cell>>[] { valueA, valueB, valueC };
            foreach (var index in set.Indexes)
            {
                _indexValueMap[index] = values.Where(v => v.Value.Indexes().Contains(index)).Select(v => v.Key);
            }

            var affectedIndexes = valueA.Value.Indexes()
                .Concat(valueB.Value.Indexes())
                .Concat(valueC.Value.Indexes())
                .Distinct();

            _affectedIndexes.AddRange(affectedIndexes);
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
                                var valueA = new KeyValuePair<int, IEnumerable<Cell>>(possibleSpotsA.Key, possibleSpotsA.Value);
                                var valueB = new KeyValuePair<int, IEnumerable<Cell>>(possibleSpotsB.Key, possibleSpotsB.Value);
                                var valueC = new KeyValuePair<int, IEnumerable<Cell>>(possibleSpotsC.Key, possibleSpotsC.Value);

                                var technique = new ThreeSpotClosureTechnique(valueA, valueB, valueC, set);
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
    } 
}
