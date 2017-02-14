using System.Collections.Generic;

namespace Sudokungfu.SudokuSolver.Techniques
{
    /// <summary>
    /// Static class for accessing the list of advanced techniques
    /// </summary>
    public static class AdvancedTechniqueFactory
    {
        private static List<AdvancedTechnique> _techniques;

        /// <summary>
        /// Instances of <see cref="AdvancedTechnique"/> that have been registered with the factory.
        /// </summary>
        public static IEnumerable<AdvancedTechnique> Techniques
        {
            get
            {
                return _techniques;   
            }
        }

        static AdvancedTechniqueFactory()
        {
            RegisterTechniques();
        }

        private static void RegisterTechniques()
        {
            _techniques = new List<AdvancedTechnique>();

            // Register advanced techniques here
            _techniques.Add(new PossibleValueSpotOverlapTechnique());
        }
    }
}
