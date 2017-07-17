using System.Collections.Generic;

namespace Sudokungfu.Model
{
    /// <summary>
    /// Interface that describes how the details of a found value or technique should be displayed.
    /// </summary>
    public interface ISudokuModel
    {
        /// <summary>
        /// Indexes of the cells that should be displayed and the values that go in them.
        /// </summary>
        IDictionary<int, IEnumerable<int>> IndexValueMap { get; }

        /// <summary>
        /// Details that make up this model.
        /// </summary>
        IEnumerable<ISudokuModel> Details { get; }

        /// <summary>
        /// Indexes that should be displayed, but are not part of technique details.
        /// </summary>
        IEnumerable<int> AffectedIndexes { get; }

        /// <summary>
        /// Complexity of the model.
        /// </summary>
        int Complexity { get; }

        /// <summary>
        /// Model that will be displayed when this model is clicked.
        /// </summary>
        ISudokuModel ClickableModel { get; }

        /// <summary>
        /// Description of the model.
        /// </summary>
        string Description { get; set; }
    }
}
