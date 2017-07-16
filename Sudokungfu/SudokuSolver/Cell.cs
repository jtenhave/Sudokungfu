using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver
{
    using Extensions;
    using Sets;
    using Techniques;

    /// <summary>
    /// Class that represents a cell in the Sudoku grid.
    /// </summary>
    public class Cell
    {
        private class TechniqueList : List<Technique>
        {
            private int _complexity;

            public TechniqueList() : base()
            {
                _complexity = int.MaxValue;
            }

            public void AddTechnique(Technique technique)
            {
                if (technique.Complexity > _complexity)
                {
                    return;
                }

                if (technique.Complexity < _complexity)
                {
                    Clear();       
                    _complexity = technique.Complexity;
                }

                Add(technique);
            }
        }

        private int OCCUPIED_COMPLEXITY = 0;

        private int SET_COMPLEXITY = 1;

        private Dictionary<int, TechniqueList> _committedTechniques;

        private Dictionary<int, TechniqueList> _appliedTechniques;

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
                return Constants.ALL_VALUES.Except(_appliedTechniques.Keys);
            }
        }

        /// <summary>
        /// Techniques used to eliminate values from this cell.
        /// </summary>
        public IDictionary<int, IEnumerable<Technique>> Techniques
        {
            get
            {
                return _appliedTechniques.ToDictionary(k => k.Key, k => k.Value.OrderBy(t => t.Complexity).AsEnumerable());
            }
        }

        /// <summary>
        /// Creates a new <see cref="Cell"/>.
        /// </summary>
        /// <param name="index">The index of the cell.</param>
        public Cell(int index)
        {
            _committedTechniques = new Dictionary<int, TechniqueList>();
            _appliedTechniques = new Dictionary<int, TechniqueList>();

            Index = index;
            Sets = new List<Set>();
        }

        /// <summary>
        /// Applies a technique to the cell.
        /// </summary>
        /// <param name="technique">Technique to apply.</param>
        public void ApplyTechnique(Technique technique)
        {
            foreach (var value in technique.Values)
            {
                if (!_appliedTechniques.ContainsKey(value))
                {
                    _appliedTechniques[value] = new TechniqueList();
                }

                _appliedTechniques[value].AddTechnique(technique);
            }            
        }

        /// <summary>
        /// Resets the applied techniques.
        /// </summary>
        public void ResetAppliedTechniques()
        {            
            _appliedTechniques = new Dictionary<int, TechniqueList>(_committedTechniques);
        }

        /// <summary>
        /// Inserts a value into this cell.
        /// </summary>
        /// <param name="value">Value to insert.</param>
        public void InsertValue(FoundValue value)
        {
            var occupiedTechnique = CreateOccupiedTechnique(value);
            CommitTechnique(occupiedTechnique);

            foreach (var set in Sets)
            {
                var setTechnique = CreateSetTechnique(value, set);
                set.Cells.Except(this).ForEach(c => c.CommitTechnique(setTechnique));
            }

            _committedTechniques[value.Value] = new TechniqueList();
            ResetAppliedTechniques();
        }

        private void CommitTechnique(Technique technique)
        {
            foreach (var value in technique.Values)
            {
                if (!_committedTechniques.ContainsKey(value))
                {
                    _committedTechniques[value] = new TechniqueList();
                }

                _committedTechniques[value].AddTechnique(technique);

            }
        }

        private Technique CreateOccupiedTechnique(FoundValue foundValue)
        {
            var technique = new Technique();
            technique.Complexity = OCCUPIED_COMPLEXITY;
            technique.ClickableModel = foundValue;
            technique.AffectedCells.Add(this);
            technique.CellValueMap[this] = foundValue.Value.ToEnumerable();
            technique.Values.AddRange(Constants.ALL_VALUES.Except(foundValue.Value));

            return technique;
        }

        private Technique CreateSetTechnique(FoundValue foundValue, Set set)
        {
            var technique = new Technique();
            technique.Complexity = SET_COMPLEXITY;
            technique.ClickableModel = foundValue;
            technique.AffectedCells.AddRange(set.Cells.Except(this));
            technique.CellValueMap[this] = foundValue.Value.ToEnumerable();
            technique.AffectedCells.ForEach(c => technique.CellValueMap[c] = Enumerable.Empty<int>());
            technique.Values.Add(foundValue.Value);

            return technique;
        }
    }
}
