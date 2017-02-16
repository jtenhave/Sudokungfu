using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Sudokungfu.Test.SudokuSolver.Techniques
{
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;
    using Sudokungfu.SudokuSolver.Techniques;
    using System.Linq;

    /// <summary>
    /// Test class for <see cref="TwoSpotClosureTechnique"/>.
    /// </summary>
    [TestClass]
    public class TwoSpotClosureTechniqueTest
    {
        [TestMethod]
        public void TestTwoSpotClosure()
        {
            var testValueA = 3;
            var testValueB = 4;
            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(2),
                new Cell(9), new Cell(10), new Cell(11),
                new Cell(18), new Cell(19),new Cell(20) };

            var box = new Box(cells, 0);

            var testTechnique = new TestEliminationTechnique(0);
            for (int i = 2; i < cells.Count; i++)
            {
                cells[i].EliminatePossibleValue(testValueA, testTechnique);
                cells[i].EliminatePossibleValue(testValueB, testTechnique);
            }

            var closureTechnique = new TwoSpotClosureTechnique();
            closureTechnique.Apply(cells, new List<Set> { box });

            for (int i = 0; i < 2; i++)
            {
                Assert.AreEqual(2, cells[i].PossibleValues.Count());
                Assert.IsTrue(cells[i].PossibleValues.SequenceEqual(new List<int>() { testValueA, testValueB }));
            }
        }

        [TestMethod]
        public void TestTwoSpotClosureRequiresTwoValues()
        {
            var testValueA = 3;
            var testValueB = 4;
            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(2),
                new Cell(9), new Cell(10), new Cell(11),
                new Cell(18), new Cell(19),new Cell(20) };

            var box = new Box(cells, 0);

            var testTechnique = new TestEliminationTechnique(0);
            cells[1].EliminatePossibleValue(testValueA, testTechnique);
            for (int i = 2; i < cells.Count; i++)
            {
                cells[i].EliminatePossibleValue(testValueA, testTechnique);
                cells[i].EliminatePossibleValue(testValueB, testTechnique);
            }

            var closureTechnique = new TwoSpotClosureTechnique();
            closureTechnique.Apply(cells, new List<Set> { box });

            Assert.AreEqual(9, cells[0].PossibleValues.Count());
            Assert.AreEqual(8, cells[1].PossibleValues.Count());
        }
    }
}
