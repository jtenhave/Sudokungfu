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
        /// The techniques used to find the value.
        /// </summary>
        public IEnumerable<IEliminationTechnique> Techniques { get; private set; }

        public FoundValue(int index, int value)
        {
            Index = index;
            Value = value;
            Indexes = new List<int>();
            Techniques = new List<IEliminationTechnique>();
        }

        public FoundValue(int index, int value, IEnumerable<IEliminationTechnique> initalTechniques)
            : this(index, value)
        {
            Index = index;
            Value = value;
            Techniques = FindMinTechniques(initalTechniques);
        }

        public FoundValue(Cell cell, int value) 
            : this(cell.Index, value, Constants.ALL_VALUES.Except(value).SelectMany(v => cell.EliminationTechniques[v]))
        {
            Indexes = new List<int> { cell.Index };
        }

        public FoundValue(Cell cell, int value, Set set) 
            : this(cell.Index, value, set.Cells.Except(cell).SelectMany(c => c.EliminationTechniques[value]))
        {
            Indexes = set.Cells.Select(c => c.Index);
        }

        private IEnumerable<IEliminationTechnique> FindMinTechniques(IEnumerable<IEliminationTechnique> techniques)
        {
            var finalTechniques = new List<IEliminationTechnique>();
           /* var indexesRemaining = new List<int>(UsedIndexes);
            indexesRemaining.Remove(Index);

            // Add the techniques for the cells that are occupied.
            var occupiedTechniques = techniques.Where(t => t.Complexity == 0);
            finalTechniques.AddRange(occupiedTechniques);
            indexesRemaining.RemoveAll(i => occupiedTechniques.SelectMany(t => t.Indexes).Contains(i));

            // Get the list of techniques that definately needed
            var singleTechniqueIndexes = indexesRemaining.Where(i => techniques.Where(t => t.Indexes.Contains(i)).Count() == 1);
            var requiredTechniques = techniques.Where(t => t.Indexes.Intersect(singleTechniqueIndexes).Any());
            finalTechniques.AddRange(requiredTechniques);
            indexesRemaining.RemoveAll(i => requiredTechniques.Any(t => t.Indexes.Contains(i)));*/

            // Get the remaining techniques required.

            return finalTechniques;
        }
    }
}
