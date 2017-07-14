using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Techniques.Advanced
{
    using Extensions;
    using Sets;

    /// <summary>
    /// Advanced technique factory for the Three Spot Closure technique.
    /// </summary>
    public class ThreeSpotClosureFactory : AdvancedTechniqueFactoryBase
    {
        public const int COMPLEXITY = 5;

        /// <summary>
        /// Creates a new <see cref="ThreeSpotClosureFactory"/>.
        /// </summary>
        /// <param name="cells">Cells in the Sudoku.</param>
        /// <param name="sets">Sets in the Sudoku.</param>
        public ThreeSpotClosureFactory(IEnumerable<Cell> cells, IEnumerable<Set> sets) : base(cells, sets)
        {

        }

        protected override IEnumerable<Technique> ApplyInternal()
        {
            foreach (var set in _sets)
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

                                var values = new KeyValuePair<int, IEnumerable<Cell>>[] { valueA, valueB, valueC };

                                var affectedCells = threeValueSpotsUnion;
                                var technique = new Technique();
                                technique.Techniques.AddRange(set.FindMinTechniques(affectedCells, valueA.Key));
                                technique.Techniques.AddRange(set.FindMinTechniques(affectedCells, valueB.Key));
                                technique.Techniques.AddRange(set.FindMinTechniques(affectedCells, valueC.Key));
                                technique.Values.AddRange(Constants.ALL_VALUES.Except(valueA.Key, valueB.Key, valueC.Key));
                                technique.AffectedCells.AddRange(affectedCells);
                                set.Cells.ForEach(c => technique.CellValueMap[c] = Enumerable.Empty<int>());
                                affectedCells.ForEach(c => technique.CellValueMap[c] = values.Where(v => v.Value.Indexes().Contains(c.Index)).Select(v => v.Key));
                                technique.Complexity = COMPLEXITY;

                                yield return technique;
                            }
                        }
                    }
                }
            }
        }
    }
}
