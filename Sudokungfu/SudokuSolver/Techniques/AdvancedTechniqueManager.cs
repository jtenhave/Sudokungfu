using System.Collections.Generic;

namespace Sudokungfu.SudokuSolver.Techniques
{
    using Advanced;
    using Sets;
    using SudokuSolver;

    /// <summary>
    /// Class for applying advanced techniques to the Sudoku.
    /// </summary>
    public class AdvancedTechniqueManager
    {
        private List<AdvancedTechniqueFactoryBase> _techniques;
        private int _currentIndex;

        /// <summary>
        /// Creates a new <see cref="AdvancedTechniqueManager"/>.
        /// </summary>
        /// <param name="cells">Cells in the Sudoku.</param>
        /// <param name="sets">Sets in the Sudoku.</param>
        public AdvancedTechniqueManager(IEnumerable<Cell> cells, IEnumerable<Set> sets)
        {
            _techniques = new List<AdvancedTechniqueFactoryBase>();
            _techniques.Add(new TwoSpotOverlapFactory(cells, sets));
            _techniques.Add(new ThreeSpotOverlapFactory(cells, sets));
            _techniques.Add(new TwoSpotClosureFactory(cells, sets));
            _techniques.Add(new ThreeSpotClosureFactory(cells, sets));
            _techniques.Add(new PossibleSpotShadowFactory(cells, sets));
            _currentIndex = 0;
        }

        /// <summary>
        /// Whether another advanced technique can be applied.
        /// </summary>
        public bool HasNext()
        {
            return _currentIndex < _techniques.Count;
        }

        /// <summary>
        /// Applies the next advanced technique.
        /// </summary>
        public void ApplyNext()
        {
            var endIndex = _currentIndex + 1;
            for (; _currentIndex < endIndex;)
            {
                var hasNewTechs = _techniques[_currentIndex].Apply();
                if (hasNewTechs && _currentIndex > 0)
                {
                    _currentIndex = 0;
                }
                else
                {
                    _currentIndex++;
                }
            }

            _currentIndex = endIndex;
        }
    }
}
