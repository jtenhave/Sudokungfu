using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Techniques.Advanced
{
    using Extensions;
    using Sets;

    /// <summary>
    /// Advanced technique factory for the Two Spot Overlap technique.
    /// </summary>
    public class TwoSpotOverlapFactory : AdvancedTechniqueFactoryBase
    {
        public const int COMPLEXITY = 2;

        protected virtual int OverlapSize
        {
            get
            {
                return 2;
            }
        }

        protected virtual int Complexity
        {
            get
            {
                return COMPLEXITY;
            }
        }

        /// <summary>
        /// Creates a new <see cref="TwoSpotOverlapFactory"/>.
        /// </summary>
        /// <param name="cells">Cells in the Sudoku.</param>
        /// <param name="sets">Sets in the Sudoku.</param>
        public TwoSpotOverlapFactory(IEnumerable<Cell> cells, IEnumerable<Set> sets) : base(cells, sets)
        {
        }

        protected override IEnumerable<Technique> ApplyInternal()
        {
            foreach (var sourceSet in _sets)
            {
                var possibleSpotSetsDic = sourceSet.PossibleSpots.Where(kvp => kvp.Value.Count() == OverlapSize);
                foreach (var possibleSpots in possibleSpotSetsDic)
                {
                    var overlappingSets = _sets.Except(sourceSet).Where(s => s.IsSubset(possibleSpots.Value));
                    foreach (var overlappingSet in overlappingSets)
                    {
                        var affectedCells = overlappingSet.Cells.Except(sourceSet.Cells);
                        var value = possibleSpots.Key;
                        var cells = possibleSpots.Value;
                        var technique = new Technique();
                        technique.Techniques.AddRange(sourceSet.FindMinTechniques(cells, value));
                        technique.Values.Add(value);
                        technique.AffectedCells.AddRange(affectedCells);
                        sourceSet.Cells.ForEach(c => technique.CellValueMap[c] = Enumerable.Empty<int>());
                        cells.ForEach(c => technique.CellValueMap[c] = value.ToEnumerable());
                        technique.Complexity = Complexity;

                        yield return technique;
                    }
                }
            }
        }
    }
}
