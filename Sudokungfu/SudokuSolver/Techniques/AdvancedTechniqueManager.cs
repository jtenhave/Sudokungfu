using System;
using System.Collections.Generic;

namespace Sudokungfu.SudokuSolver.Techniques
{
    using Sets;
    using SudokuSolver;

    /// <summary>
    /// Static class for accessing the list of advanced techniques
    /// </summary>
    public static class AdvancedTechniqueManager
    {
        private static List<Action<IEnumerable<Cell>, IEnumerable<Set>>> _techniques;

        static AdvancedTechniqueManager()
        {
            RegisterTechniques();
        }

        private static void RegisterTechniques()
        {
            _techniques = new List<Action<IEnumerable<Cell>, IEnumerable<Set>>>();

            // Register advanced techniques in order of complexity
            _techniques.Add(PossibleSpotOverlapTechnique.Apply);
            _techniques.Add(TwoSpotClosureTechnique.Apply);
            _techniques.Add(ThreeSpotClosureTechnique.Apply);
        }

        public static void ApplyAdvancedTechniques(IEnumerable<Cell> cells, IEnumerable<Set> sets)
        {
            _techniques.ForEach(t => t(cells, sets));
        }
    }
}
