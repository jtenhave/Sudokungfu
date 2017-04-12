using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Sets
{
    using Extensions;
    using Techniques;

    /// <summary>
    /// Class that represents a set of nine values in a Sudoku.
    /// </summary>
    public abstract class Set
    {
        /// <summary>
        /// The cells in this set.
        /// </summary>
        public IEnumerable<Cell> Cells { get; private set; }

        /// <summary>
        /// The index of the set.
        /// </summary>
        protected int Index { get; private set; }

        /// <summary>
        /// Spots where all the unfound values in the set can go.
        /// </summary>
        public IDictionary<int, IEnumerable<Cell>> PossibleSpots
        {
            get
            {
                return Cells
                    .SelectMany(c => c.PossibleValues)
                    .Distinct()
                    .ToDictionary(i => i, i => Cells.Where(c => c.PossibleValues.Contains(i)));
            }
        }

        /// <summary>
        /// Creates a new <see cref="Set"/>
        /// </summary>
        /// <param name="grid">The Sudoku grid to create the set from.</param>
        /// <param name="index">The index of the set.</param>
        public Set(IEnumerable<Cell> grid, int index)
        {
            Index = index;
            Cells = grid.Where(IsCellInSet).ToList();
            Cells.ForEach(c => c.Sets.Add(this));
        }

        /// <summary>
        /// Returns true if a cell is in this set.
        /// </summary>
        protected abstract bool IsCellInSet(Cell cell);

        /// <summary>
        /// Returns true if a sequence of cells is a subset of this set.
        /// </summary>
        /// <param name="cells">Cells to compare to this set.</param>
        public bool IsSubset(IEnumerable<Cell> cells)
        {
            return cells.All(c => Cells.Contains(c));
        }

        public IEnumerable<ITechnique> FindMinTechniques(IEnumerable<Cell> cells, int value)
        {
            var finalTechniques = new List<ITechnique>();

            var techniques = Cells
                .Except(cells)
                .SelectMany(c => c.EliminationTechniques[value])
                .ToList();

            var uncoveredIndexes = this.Indexes()
                .Except(cells.Indexes())
                .Except(finalTechniques.SelectMany(t => t.AffectedIndexes));

            var sortedTechniques = techniques
                .Where(t => t.AffectedIndexes.Intersect(uncoveredIndexes).Any())
                .OrderBy(t => t.Complexity)
                .ThenByDescending(t => t
                    .AffectedIndexes
                    .Intersect(uncoveredIndexes)
                    .Except(techniques
                        .Except(t)
                        .SelectMany(u => u
                            .AffectedIndexes
                            .Intersect(uncoveredIndexes)))
                    .Count())
                .ThenByDescending(t => t
                    .AffectedIndexes
                    .Intersect(uncoveredIndexes)
                    .Count());

            // Apply techniques until all indexes have been covered.
            while (uncoveredIndexes.Any() && sortedTechniques.Any())
            {
                finalTechniques.Add(sortedTechniques.First());
            }

            return finalTechniques;
        }
    }
}
