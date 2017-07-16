using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudokungfu.ViewModel
{
    using Model;

    public interface ISudokungfuViewModel
    {
        int ShownValues { get; }

        void CellClicked(ISudokuModel model);
    }
}
