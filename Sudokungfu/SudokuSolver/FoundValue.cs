using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver
{
    using Extensions;
    using Sets;

    /// <summary>
    /// Class that represents a found value in the Sudoku.
    /// </summary>
    public class FoundValue
    {
        private IEnumerable<IEliminationTechnique> _techniques;

        /// <summary>
        /// Index of the value that was found.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Value that was found.
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Indexes of cells that should be shown when details for this found value are displayed.
        /// </summary>
        public IEnumerable<int> Indexes { get; private set; }

        /// <summary>
        /// Techniques used to find the value.
        /// </summary>
        public IEnumerable<IEliminationTechnique> Techniques
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
                Index = index,
                Value = value,
                Indexes = index.ToEnumerable(),
                _techniques = new List<IEliminationTechnique>()
            };
        }

        /// <summary>
        /// Creates a <see cref="FoundValue"/> that a value found in a set.
        /// </summary>
        /// <param name="cell">Cell where the value was found.</param>
        /// <param name="value">Value that was found.</param>
        /// <param name="set">The set where the value was found.</param>
        public static FoundValue CreateFoundInSetValue(Cell cell, int value, Set set)
        {
            var setIndexes = set.Cells.Select(c => c.Index);
            return new FoundValue()
            {
                Index = cell.Index,
                Value = value,
                Indexes = setIndexes,
                _techniques = FindMinSetValueTechniques(set.Cells.Except(cell).SelectMany(c => c.EliminationTechniques[value]), setIndexes, cell.Index)
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
                Index = cell.Index,
                Value = value,
                Indexes = cell.Index.ToEnumerable(),
                _techniques = Constants.ALL_VALUES.Except(value).Select(v => cell.EliminationTechniques[v].First())
            };
        }

        private static IEnumerable<IEliminationTechnique> FindMinSetValueTechniques(IEnumerable<IEliminationTechnique> techniques, IEnumerable<int> indexes, int index)
        {
            var finalTechniques = new List<IEliminationTechnique>();
            var unusedTechniques = techniques.Except(finalTechniques);

            var eliminatedIndexesWithoutTechnique = new List<int>(indexes);
            eliminatedIndexesWithoutTechnique.Remove(index);

            // Add the techniques for the cells that are occupied.
            var occupiedTechniques = techniques.Where(t => t.Complexity == 0);
            finalTechniques.AddRange(occupiedTechniques);
            eliminatedIndexesWithoutTechnique.RemoveAll(i => occupiedTechniques.SelectMany(t => t.Indexes).Contains(i));

            // Get the list of techniques that are definately needed
            var requiredTechniques = unusedTechniques
                .Where(t => t
                    .Indexes
                    .Intersect(eliminatedIndexesWithoutTechnique)
                    .Except(unusedTechniques
                        .Except(t)
                        .SelectMany(u => u.Indexes))
                    .Any())
                .ToList();

            finalTechniques.AddRange(requiredTechniques);
            eliminatedIndexesWithoutTechnique.RemoveAll(i => requiredTechniques.SelectMany(t => t.Indexes).Contains(i));

            // Apply leftover techniques until all indexes have a technique.
            while (eliminatedIndexesWithoutTechnique.Any())
            {
                var leftoverTechnique = unusedTechniques
                    .OrderByDescending(t => t.Indexes.Intersect(eliminatedIndexesWithoutTechnique).Count())
                    .ThenBy(t => t.Complexity)
                    .FirstOrDefault();

                if (leftoverTechnique != null)
                {
                    finalTechniques.Add(leftoverTechnique);
                    eliminatedIndexesWithoutTechnique.RemoveAll(i => leftoverTechnique.Indexes.Contains(i));
                }
                else
                {
                    break;
                }
            }

            return finalTechniques;
        }
    }
}
