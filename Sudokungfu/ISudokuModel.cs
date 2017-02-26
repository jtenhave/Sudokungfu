using System.Collections.Generic;
using System.ComponentModel;

namespace Sudokungfu
{
    /// <summary>
    /// Interface that describes how the details of a found value or technique should be displayed.
    /// </summary>
    public interface ISudokuModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Indexes of the cells that should be displayed and the values that go in them.
        /// </summary>
        IDictionary<int, IEnumerable<int>> IndexValueMap { get; }

        /// <summary>
        /// Sub-details that make up these details;
        /// </summary>
        IEnumerable<ISudokuModel> Details { get; }

        /// <summary>
        /// Indexes that should be displayed, but are not part of technique details.
        /// </summary>
        IEnumerable<int> AffectedIndexes { get; }

        /// <summary>
        /// Whether the model is accepting input at this time.
        /// </summary>
        bool IsInputEnabled { get; }

        /// <summary>
        /// Whether the model is solving.
        /// </summary>
        bool IsSolving { get; }
    }
}
