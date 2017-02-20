using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Sudokungfu.Test
{
    using Sudokungfu.Extensions;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Techniques;


    public class BaseTest
    {
        public static IEnumerable<Cell> GetAllCells()
        {
            var cells = new List<Cell>();
            for (int i = 0; i < Constants.CELL_COUNT; i++)
            {
                cells.Add(new Cell(i));
            }

            return cells;
        }

        public static void AssertITechniqueEqual(ITechnique expected, ITechnique actual)
        {
            Assert.AreEqual(expected.Complexity, actual.Complexity);
            Assert.AreEqual(expected.UsesFoundValues, actual.UsesFoundValues);

            if (expected.IndexValueMap == null)
            {
                Assert.IsNull(actual.IndexValueMap);
            }
            else
            {
                Assert.IsNotNull(actual.IndexValueMap);
                Assert.IsNotNull(actual.IndexValueMap.Values);

                Assert.IsTrue(actual.IndexValueMap.Keys.SetEqual(expected.IndexValueMap.Keys));

                foreach (var index in expected.IndexValueMap.Keys)
                {
                    Assert.IsTrue(actual.IndexValueMap[index].SetEqual(expected.IndexValueMap[index]));
                }
            }
        }

        public static void EliminatePossibleValues(ITechnique technique, List<Cell> cells, int value, params int[] indexes)
        {
            foreach (var i in indexes)
            {
                cells[i].EliminatePossibleValue(value, technique);
            }
        }
    }

    
}
