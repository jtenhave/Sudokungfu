using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.Techniques
{
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;
    using Sudokungfu.SudokuSolver.Techniques;

    /// <summary>
    /// Test class for <see cref="ThreeSpotClosureTechnique"/>.
    /// </summary>
    [TestClass]
    public class ThreeSpotClosureTechniqueTest : BaseTest
    {
        /*[TestMethod]
        public void TestThreeValueClosureA()
        {
            var testValueA = 3;
            var testValueB = 4;
            var testValueC = 5;
            var cells = GetAllCells();

            var box = new Box(cells, 0);

            var testTechnique = new TestTechnique();
            for (int i = 3; i < cells.Count; i++)
            {
                cells[i].EliminatePossibleValue(testValueA, testTechnique);
                cells[i].EliminatePossibleValue(testValueB, testTechnique);
                cells[i].EliminatePossibleValue(testValueC, testTechnique);
            }

            ThreeSpotClosureTechnique.Apply(cells, new Set[] { box });

            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(3, cells[i].PossibleValues.Count());
                Assert.IsTrue(cells[i].PossibleValues.SequenceEqual(new List<int>() { testValueA, testValueB, testValueC }));
            }
        }

        [TestMethod]
        public void TestThreeValueClosureB()
        {
            var testValueA = 3;
            var testValueB = 4;
            var testValueC = 5;
            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(2),
                new Cell(9), new Cell(10), new Cell(11),
                new Cell(18), new Cell(19),new Cell(20) };

            var box = new Box(cells, 0);

            var testTechnique = new TestTechnique(0);
            cells[2].EliminatePossibleValue(testValueA, testTechnique);
            cells[0].EliminatePossibleValue(testValueB, testTechnique);
            cells[1].EliminatePossibleValue(testValueC, testTechnique);

            for (int i = 3; i < cells.Count; i++)
            {
                cells[i].EliminatePossibleValue(testValueA, testTechnique);
                cells[i].EliminatePossibleValue(testValueB, testTechnique);
                cells[i].EliminatePossibleValue(testValueC, testTechnique);
            }

            ThreeSpotClosureTechnique.Apply(cells, new Set[] { box });

            Assert.AreEqual(2, cells[0].PossibleValues.Count());
            Assert.AreEqual(2, cells[1].PossibleValues.Count());
            Assert.AreEqual(2, cells[2].PossibleValues.Count());

            Assert.IsTrue(cells[0].PossibleValues.SequenceEqual(new List<int>() { testValueA, testValueC }));
            Assert.IsTrue(cells[1].PossibleValues.SequenceEqual(new List<int>() { testValueA, testValueB }));
            Assert.IsTrue(cells[2].PossibleValues.SequenceEqual(new List<int>() { testValueB, testValueC }));
        }*/
    }
}
