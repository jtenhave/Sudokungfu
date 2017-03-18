using System.Collections.Generic;

namespace Sudokungfu.SudokuSolver.Techniques
{
    using Extensions;
    using Model;

    /// <summary>
    /// Class that represents the basic techniques for eliminating a possile value from a Sudoku cell.
    /// </summary>
    public class BasicTechnique : ITechnique
    {
        private ISudokuModel _clickableModel;

        #region ISudokuModel

        /// <summary>
        /// Indexes of the cells that are part of the technique and the values that go in them.
        /// </summary>
        public IDictionary<int, IEnumerable<int>> IndexValueMap { get; protected set; }

        /// <summary>
        /// Techniques used to form this technique.
        /// </summary>
        public IEnumerable<ISudokuModel> Details { get; protected set; }

        /// <summary>
        /// Indexes of cells that had values eliminanated by this technique.
        /// </summary>
        public IEnumerable<int> AffectedIndexes { get; protected set; }

        /// <summary>
        /// Not used by <see cref="BasicTechnique"/>.
        /// </summary>
        public ISudokuModel ClickableModel
        {
            get
            {
                if (_clickableModel == this)
                {
                    return new BasicTechnique()
                    {
                        IndexValueMap = IndexValueMap,
                        Complexity = Complexity,
                        AffectedIndexes = AffectedIndexes
                    };
                }

                return _clickableModel;
            }
            protected set
            {
                _clickableModel = value;
            }
        }
        
        #endregion

        #region ITechnique

        /// <summary>
        /// Complexity of the technique. 
        /// </summary>
        /// <remarks>
        /// Basic techniques will have a lower complexity and advanced techniques will have a higher complexity.
        /// </remarks>
        public int Complexity { get; protected set; }

        #endregion

        protected BasicTechnique()
        {   
                    
        }

        /// <summary>
        /// Creates a <see cref="BasicTechnique"/> that represents a possible value being eliminated from a cell
        /// because that cell already contains a value.
        /// </summary>
        /// <param name="value">Found value that creates this occupied technique.</param>
        public static BasicTechnique CreateOccupiedTechnique(FoundValue value)
        {
            return new BasicTechnique()
            {
                Complexity = 0,
                IndexValueMap = value.Index.ToDictionary(value.Value),
                AffectedIndexes = value.Index.ToEnumerable(),
                ClickableModel = value
            };
        }

        /// <summary>
        /// Creates a <see cref="BasicTechnique"/> that represents a possible value being eliminated from a cell
        /// because one of that cells member sets already contains the value.
        /// </summary>
        /// <param name="value">Value that already exists in one of the member sets.</param>
        /// <param name="setIndexes">Indxes of the member set cells.</param>

        public static BasicTechnique CreateSetTechnique(FoundValue value, IEnumerable<int> setIndexes)
        {
            var technique = new BasicTechnique()
            {
                Complexity = 1,
                IndexValueMap = setIndexes.ToDictionary(value.Index, value.Value),
                AffectedIndexes = setIndexes.Except(value.Index),
                ClickableModel = value
            };

            return technique;
        }
    }
}
