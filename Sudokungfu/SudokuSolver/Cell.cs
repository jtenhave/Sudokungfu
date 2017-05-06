using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver
{
    using Extensions;
    using FoundValues;
    using Model;
    using Sets;
    using Techniques;

    /// <summary>
    /// Class that represents a cell in the Sudoku grid.
    /// </summary>
    public class Cell
    {
        private HashSet<int> _possibleValues;

        private Dictionary<int, List<ISudokuModel>> _eliminationTechniques;

        /// <summary>
        /// Index of the cell.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Sets that this cell belongs to.
        /// </summary>
        public List<Set> Sets { get; private set; }

        /// <summary>
        /// Values that can possibly go in this cell.
        /// </summary>
        public IEnumerable<int> PossibleValues
        {
            get
            {
                return _possibleValues;
            }
        }

        /// <summary>
        /// Techniques used to eliminate values from this cell.
        /// </summary>
        public IDictionary<int, IEnumerable<ISudokuModel>> EliminationTechniques
        {
            get
            {
                return _eliminationTechniques.ToDictionary(k => k.Key, k => k.Value.AsEnumerable());
            }
        }

        /// <summary>
        /// Creates a new <see cref="Cell"/>.
        /// </summary>
        /// <param name="index">The index of the cell.</param>
        public Cell(int index)
        {
            _possibleValues = new HashSet<int>(Constants.ALL_VALUES);
            _eliminationTechniques = _possibleValues.ToDictionary(v => v, v => new List<ISudokuModel>());

            Index = index;
            Sets = new List<Set>();
        }

        /// <summary>
        /// Eliminates a possible value from this cell.
        /// </summary>
        /// <param name="value">Value to eliminate.</param>
        /// <param name="technique">Technique used to eliminate the value.</param>
        public void EliminatePossibleValue(int value, ISudokuModel technique)
        {
            if (_possibleValues.Any())
            {
                _possibleValues.Remove(value);

                var currentDifficulty = _eliminationTechniques[value].FirstOrDefault()?.Complexity ?? technique.Complexity;
                if (technique.Complexity < currentDifficulty)
                {
                    _eliminationTechniques[value].Clear();
                }
                else if (technique.Complexity > currentDifficulty)
                {
                    return;
                }

                _eliminationTechniques[value].Add(technique);
            }
        }

        /// <summary>
        /// Inserts a value into this cell.
        /// </summary>
        /// <param name="value">Value to insert.</param>
        public void InsertValue(FoundValueBase value)
        {
            var occupiedTechnique = new OccupiedTechnique(value);
            foreach(var v in Constants.ALL_VALUES.Except(value.Value))
            {
                EliminatePossibleValue(v, occupiedTechnique);
            }

            _possibleValues.Remove(value.Value);

            foreach (var set in Sets)
            {
                var setTechnique = new SetTechnique(value, set);
                set.Cells.Except(this).ForEach(c => c.EliminatePossibleValue(value.Value, setTechnique));
            }
        }

        /// <summary>
        /// Calculates the techniques used to find a value in this cell.
        /// </summary>
        /// <param name="value">Value to calculate techniques for.</param>
        public IEnumerable<ISudokuModel> FindMinTechniques(int value)
        {
            return Constants.ALL_VALUES.Except(value)
                .Select(v => EliminationTechniques[v].FirstOrDefault())
                .Where(v => v != null)
                .ToList();
        }
    }
}
