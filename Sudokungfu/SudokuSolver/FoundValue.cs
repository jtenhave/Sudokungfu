using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Sudokungfu.SudokuSolver
{
    using Extensions;
    using Model;
    using Sets;
    using Techniques;

    /// <summary>
    /// Class that represents a found value in the Sudoku.
    /// </summary>
    public class FoundValue : ISudokuModel
    {
        private IEnumerable<ITechnique> _techniques;

        #region ISudokuModel

        /// <summary>
        /// Not used by <see cref="FoundValue"/>.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Indexes of the cells that should be displayed and the values that go in them.
        /// </summary>
        public IDictionary<int, IEnumerable<int>> IndexValueMap { get; private set; }

        /// <summary>
        /// Techniques that make up this model.
        /// </summary>
        public IEnumerable<ISudokuModel> Details
        {
            get
            {
                return _techniques;
            }
        }

        /// <summary>
        /// Not used by <see cref="FoundValue"/>.
        /// </summary>
        public IEnumerable<int> AffectedIndexes
        {
            get
            {
                return Enumerable.Empty<int>();
            }
        }

        /// <summary>
        /// Not used by <see cref="FoundValue"/>.
        /// </summary>
        public bool IsInputEnabled
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Not used by <see cref="FoundValue"/>.
        /// </summary>
        public bool IsSolving
        {
            get
            {
                return false;
            }
        }

        #endregion

        /// <summary>
        /// Index of the value that was found.
        /// </summary>
        public int Index
        {
            get
            {
                return IndexValueMap.First(k => k.Value.Any()).Key;
            }
        }

        /// <summary>
        /// Value that was found.
        /// </summary>
        public int Value
        {
            get
            {
                return IndexValueMap.SelectMany(k => k.Value).First();
            }
        }

        /// <summary>
        /// Techniques used to find the value.
        /// </summary>
        public IEnumerable<ITechnique> Techniques
        {
            get
            {
                return _techniques.OrderBy(t => t.Complexity);
            }
        }

        /// <summary>
        /// Number of techniques used to find the value.
        /// </summary>
        public int TechniqueCount
        {
            get
            {
                return Techniques?.Count() ?? 0;
            }
        }

        /// <summary>
        /// Complexity of the techniques used to find the value.
        /// </summary>
        public int TechniqueComplexity
        {
            get
            {
                return Techniques?.LastOrDefault()?.Complexity ?? int.MaxValue;
            }
        }

        private FoundValue()
        {
        
        }

        /// <summary>
        /// Creates a <see cref="FoundValue"/> that represents a given value.
        /// </summary>
        /// <param name="index">Index of the given value.</param>
        /// <param name="value">Value that was given.</param>
        public static FoundValue CreateGivenValue(int index, int value)
        {
            return new FoundValue()
            {
                _techniques = Enumerable.Empty<ITechnique>(),
                IndexValueMap = index.ToDictionary(value)
            };
        }

        /// <summary>
        /// Creates a <see cref="FoundValue"/> that a value found in a set.
        /// </summary>
        /// <param name="cell">Cell where the value was found.</param>
        /// <param name="value">Value that was found.</param>
        /// <param name="set">Set where the value was found.</param>
        public static FoundValue CreateFoundInSetValue(Cell cell, int value, Set set)
        {
            return new FoundValue()
            {
                _techniques = FindMinTechniques(cell, value, set),
                IndexValueMap = set.Cells.Indexes().ToDictionary(cell.Index, value)
            };
        }

        /// <summary>
        /// Creates a <see cref="FoundValue"/> that was found by eliminating all other possible values for a cell.
        /// </summary>
        /// <param name="cell">Cell where the value was found.</param>
        /// <param name="value">Value that was found.</param>
        public static FoundValue CreateOnlyPossiblValue(Cell cell, int value)
        {
            return new FoundValue()
            {
                _techniques = FindMinTechniques(cell, value),
                IndexValueMap = cell.Index.ToDictionary(value)
            };
        }

        private static IEnumerable<ITechnique> FindMinTechniques(Cell cell, int value, Set set)
        {
            var finalTechniques = new List<ITechnique>();

            var techniques = set
                .Cells
                .Except(cell)
                .SelectMany(c => c.EliminationTechniques[value])
                .ToList();

            var uncoveredIndexes = set
                .Indexes()
                .Except(cell.Index)
                .Except(finalTechniques.SelectMany(t => t.AffectedIndexes));

            var sortedTechniques = techniques
                .Where(t => t.AffectedIndexes.Intersect(uncoveredIndexes).Any())
                .OrderBy(t => t.Complexity)
                .ThenByDescending(t => t
                    .AffectedIndexes
                    .Except(techniques.Except(t).SelectMany(u => u.AffectedIndexes))
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

        private static IEnumerable<ITechnique> FindMinTechniques(Cell cell, int value)
        {
            return Constants.ALL_VALUES.Except(value)
                .Select(v => cell
                    .EliminationTechniques[v]
                    .FirstOrDefault())
                .Where(t => t != null);
        }
    }
}
