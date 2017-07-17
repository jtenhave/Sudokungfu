using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Techniques.Advanced
{
    using Extensions;
    using Sets;

    /// <summary>
    /// Advanced technique factory for the Two Spot Closure technique.
    /// </summary>
    public class TwoSpotClosureFactory : AdvancedTechniqueFactoryBase
    {
        public const int COMPLEXITY = 4;

        /// <summary>
        /// Creates a new <see cref="TwoSpotClosureFactory"/>.
        /// </summary>
        /// <param name="cells">Cells in the Sudoku.</param>
        /// <param name="sets">Sets in the Sudoku.</param>
        public TwoSpotClosureFactory(IEnumerable<Cell> cells, IEnumerable<Set> sets) : base(cells, sets)
        {
        }

        protected override IEnumerable<Technique> ApplyInternal()
        {
            foreach (var set in _sets)
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
                            int[] values = { valueA, valueB };

                            var affectedCells = twoValueSpotsUnion;
                            var technique = new Technique();
                            technique.Techniques.AddRange(set.FindMinTechniques(affectedCells, valueA));
                            technique.Techniques.AddRange(set.FindMinTechniques(affectedCells, valueB));
                            technique.Values.AddRange(Constants.ALL_VALUES.Except(values));
                            technique.AffectedCells.AddRange(affectedCells);
                            set.Cells.ForEach(c => technique.CellValueMap[c] = Enumerable.Empty<int>());
                            affectedCells.ForEach(c => technique.CellValueMap[c] = values);
                            technique.Complexity = COMPLEXITY;
                            technique.Description = "Two Spot Closure technique.";

                            yield return technique;
                        }
                    }
                }
            }
        }
    }
}
