using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Techniques
{
    using Extensions;
    using Model;
    using Sets;

    /// <summary>
    /// Class that represents the 'Possible Spot Overlap' technique.
    /// </summary>
    public class PossibleSpotOverlapTechnique : TechniqueBase
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
        /// Creates a new <see cref="PossibleSpotOverlapTechnique"/>.
        /// </summary>
        /// <param name="value">Possible value.</param>
        /// <param name="cells">Cells that are overlapping.</param>
        /// <param name="set">Set with the possible value.</param>
        /// <param name="affectedCells">Cells affected by the technique.</param>
        private PossibleSpotOverlapTechnique(int value, IEnumerable<Cell> cells, Set set, IEnumerable<Cell> affectedCells) : base(COMPLEXITY)
        {
            _techniques = set.FindMinTechniques(cells, value);

            foreach (var index in set.Indexes)
            {
                _indexValueMap[index] = Enumerable.Empty<int>();
            }

            foreach (var index in cells.Indexes())
            {
                _indexValueMap[index] = value.ToEnumerable();
            }          

            _affectedIndexes.AddRange(affectedCells.Indexes());
        }

        /// <summary>
        /// Applies the technique to the Sudoku.
        /// </summary>
        /// <param name="cells">Cells in the Sudoku.</param>
        /// <param name="sets">Sets of cells in the Sudoku.</param>
        public static void Apply(IEnumerable<Cell> cells, IEnumerable<Set> sets)
        {
            foreach (var sourceSet in sets)
            {
                var possibleSpotSetsDic = sourceSet.PossibleSpots.Where(kvp => kvp.Value.Count() <= 3 && kvp.Value.Count() >= 2);
                foreach (var possibleSpots in possibleSpotSetsDic)
                {
                    var overlappingSets = sets.Except(sourceSet).Where(s => s.IsSubset(possibleSpots.Value));
                    foreach (var overlappingSet in overlappingSets)
                    {
                        var value = possibleSpots.Key;
                        var affectedCells = overlappingSet.Cells.Except(sourceSet.Cells);

                        var technique = new PossibleSpotOverlapTechnique(value, possibleSpots.Value, sourceSet, affectedCells);
          
                        foreach (var cell in affectedCells)
                        {
                            cell.EliminatePossibleValue(possibleSpots.Key, technique);
                        }
                    }
                }
            }
        }      
    }
}
