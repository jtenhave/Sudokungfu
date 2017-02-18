using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.Techniques
{
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;
    using Sudokungfu.SudokuSolver.Techniques;

    /// <summary>
    /// Test class for <see cref="PossibleSpotOverlapTechnique"/>.
    /// </summary>
    [TestClass]
    public class PossibleSpotOverlapTechniqueTest
    {
        [TestMethod]
        public void TestTwoOverlappingCells()
        {
            var testValue = 8;

            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(2), new Cell(3), new Cell(4),
                new Cell(5), new Cell(6), new Cell(7), new Cell(8), new Cell(9), new Cell(10), new Cell(11),
                new Cell(18), new Cell(19),new Cell(20)
            };

            var box = new Box(cells, 0);
            var row = new Row(cells, 0);

            var testTechnique = new TestEliminationTechnique(1);

            cells[1].EliminatePossibleValue(testValue, testTechnique);
            cells[9].EliminatePossibleValue(testValue, testTechnique);
            cells[10].EliminatePossibleValue(testValue, testTechnique);
            cells[11].EliminatePossibleValue(testValue, testTechnique);
            cells[12].EliminatePossibleValue(testValue, testTechnique);
            cells[13].EliminatePossibleValue(testValue, testTechnique);
            cells[14].EliminatePossibleValue(testValue, testTechnique);

            var possibleValueSpotOverlapTechnique = new PossibleSpotOverlapTechnique();
            possibleValueSpotOverlapTechnique.Apply(cells, new List<Set>() { box, row });

            var expectedCells = new List<Cell>() { cells[0], cells[2] };

            Assert.IsTrue(row.PossibleSpots[testValue].SequenceEqual(expectedCells));
        }

        [TestMethod]
        public void TestThreeOverlappingCells()
        {
            var testValue = 8;

            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(2), new Cell(3), new Cell(4),
                new Cell(5), new Cell(6), new Cell(7), new Cell(8), new Cell(9), new Cell(10), new Cell(11),
                new Cell(18), new Cell(19),new Cell(20)
            };

            var box = new Box(cells, 0);
            var row = new Row(cells, 0);

            var testTechnique = new TestEliminationTechnique(1);

            cells[9].EliminatePossibleValue(testValue, testTechnique);
            cells[10].EliminatePossibleValue(testValue, testTechnique);
            cells[11].EliminatePossibleValue(testValue, testTechnique);
            cells[12].EliminatePossibleValue(testValue, testTechnique);
            cells[13].EliminatePossibleValue(testValue, testTechnique);
            cells[14].EliminatePossibleValue(testValue, testTechnique);

            var possibleValueSpotOverlapTechnique = new PossibleSpotOverlapTechnique();
            possibleValueSpotOverlapTechnique.Apply(cells, new List<Set>() { box, row });

            var expectedCells = new List<Cell>() { cells[0], cells[1], cells[2] };

            Assert.IsTrue(row.PossibleSpots[testValue].SequenceEqual(expectedCells));
        }

        [TestMethod]
        public void TestTwoOverlappingCellsWithOneNonOverlapping()
        {
            var testValue = 8;

            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(2), new Cell(3), new Cell(4),
                new Cell(5), new Cell(6), new Cell(7), new Cell(8), new Cell(9), new Cell(10), new Cell(11),
                new Cell(18), new Cell(19),new Cell(20)
            };

            var box = new Box(cells, 0);
            var row = new Row(cells, 0);

            var testTechnique = new TestEliminationTechnique(1);

            cells[1].EliminatePossibleValue(testValue, testTechnique);
            cells[10].EliminatePossibleValue(testValue, testTechnique);
            cells[11].EliminatePossibleValue(testValue, testTechnique);
            cells[12].EliminatePossibleValue(testValue, testTechnique);
            cells[13].EliminatePossibleValue(testValue, testTechnique);
            cells[14].EliminatePossibleValue(testValue, testTechnique);

            var possibleValueSpotOverlapTechnique = new PossibleSpotOverlapTechnique();
            possibleValueSpotOverlapTechnique.Apply(cells, new List<Set>() { box, row });

            var expectedCells = new List<Cell>() { cells[0], cells[2], cells[3], cells[4], cells[5], cells[6], cells[7], cells[8] };

            Assert.IsTrue(row.PossibleSpots[testValue].SequenceEqual(expectedCells));
        }
    }
}
