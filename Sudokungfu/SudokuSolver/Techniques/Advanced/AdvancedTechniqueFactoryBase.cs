using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Techniques.Advanced
{
    using Extensions;
    using Sets;

    /// <summary>
    /// Base class for advanced technique factories. An advanced technique factory applies an 
    /// advanced technique to the Sudoku.
    /// </summary>
    public abstract class AdvancedTechniqueFactoryBase
    {
        protected IEnumerable<Cell> _cells;
        protected IEnumerable<Set> _sets;

        private List<Technique> _techniques;

        /// <summary>
        /// Creates a new <see cref="AdvancedTechniqueFactoryBase"/>.
        /// </summary>
        /// <param name="cells">Cells in the Sudoku.</param>
        /// <param name="sets">Sets in the Sudoku.</param>
        public AdvancedTechniqueFactoryBase(IEnumerable<Cell> cells, IEnumerable<Set> sets)
        {
            _cells = cells;
            _sets = sets;
            _techniques = new List<Technique>();
        }

        /// <summary>
        /// Applies the advanced technique to the Sudoku.
        /// </summary>
        /// <returns>Returns true if at least one technique was successfully applied.</returns>
        public bool Apply()
        {
            var initialTechCount = _techniques.Count;

            int roundTechCount;
            do
            {
                roundTechCount = _techniques.Count;
                var techniques = ApplyInternal();

                foreach (var technique in techniques)
                {
                    if (_techniques.Any(t => TechniqueEquals(t, technique)))
                    {
                        continue;
                    }

                    _techniques.Add(technique);

                    foreach (var cell in technique.AffectedCells)
                    {
                        foreach (var value in technique.Values)
                        {
                            cell.ApplyTechnique(value, technique);
                        }
                    }
                }
            }
            while (_techniques.Count - roundTechCount > 0);

            return _techniques.Count - initialTechCount > 0;
        }

        protected abstract IEnumerable<Technique> ApplyInternal();

        private bool TechniqueEquals(Technique a, Technique b)
        {
            return a.AffectedCells.SetEqual(b.AffectedCells) &&
                a.Values.SetEqual(b.Values) &&
                a.Complexity == b.Complexity;
        }
    }
}
