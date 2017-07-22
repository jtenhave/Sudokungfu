using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Sets
{
    using Extensions;
    using Model;
    using Techniques;

    /// <summary>
    /// Class that represents a set of nine values in a Sudoku.
    /// </summary>
    public abstract class Set
    {
        protected int _index;

        /// <summary>
        /// Cells in this set.
        /// </summary>
        public IEnumerable<Cell> Cells { get; private set; }

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
        /// Creates a new <see cref="Set"/>.
        /// </summary>
        /// <param name="grid">The Sudoku grid to create the set from.</param>
        /// <param name="index">The index of the set.</param>
        public Set(IEnumerable<Cell> grid, int index)
        {
            _index = index;
            Cells = grid.Where(IsCellInSet).ToList();
            Cells.ForEach(c => c.Sets.Add(this));
        }

        /// <summary>
        /// Type of the set.
        /// </summary>
        public abstract string Type { get; }

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

        /// <summary>
        /// Calculates the techniques used to find a value in this set.
        /// </summary>
        /// <param name="cells">Cells that techniques should be calculated for.</param>
        /// <param name="value">Value to calculate techniques for.</param>
        public IEnumerable<ISudokuModel> FindMinTechniques(IEnumerable<Cell> cells, int value)
        {
            var finalTechniques = new List<Technique>();

            var techniques = Cells
                .Except(cells)
                .Where(c => c.Techniques.ContainsKey(value))
                .SelectMany(c => c.Techniques[value])
                .ToList();

            var uncoveredCells = Cells
                .Except(cells)
                .Except(finalTechniques.SelectMany(t => t.AffectedCells));

            var sortedTechniques = techniques
                .Where(t => t.AffectedCells.Intersect(uncoveredCells).Any())
                .OrderBy(t => t.Complexity)
                .ThenByDescending(t => t
                    .AffectedCells
                    .Intersect(uncoveredCells)
                    .Except(techniques
                        .Except(t)
                        .SelectMany(u => u
                            .AffectedCells
                            .Intersect(uncoveredCells)))
                    .Count())
                .ThenByDescending(t => t
                    .AffectedCells
                    .Intersect(uncoveredCells)
                    .Count());

            // Apply techniques until all indexes have been covered.
            while (uncoveredCells.Any() && sortedTechniques.Any())
            {
                finalTechniques.Add(sortedTechniques.First());
            }

            return finalTechniques;
        }
    }
}
