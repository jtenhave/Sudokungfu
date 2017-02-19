using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.Techniques
{
    using Sudokungfu.Extensions;
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

            EliminatePossibleValues(testTechnique, cells, testValue, 1, 9, 10, 11, 12, 13, 14);

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

            EliminatePossibleValues(testTechnique, cells, testValue, 9, 10, 11, 12, 13, 14);

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

            EliminatePossibleValues(testTechnique, cells, testValue, 1, 10, 11, 12, 13, 14);

            var possibleValueSpotOverlapTechnique = new PossibleSpotOverlapTechnique();
            possibleValueSpotOverlapTechnique.Apply(cells, new List<Set>() { box, row });

            var expectedCells = new List<Cell>() { cells[0], cells[2], cells[3], cells[4], cells[5], cells[6], cells[7], cells[8] };

            Assert.IsTrue(row.PossibleSpots[testValue].SequenceEqual(expectedCells));
        }

        [TestMethod]
        public void TestTechniqueNotAppliedToSourceSet()
        {
            var testValue = 8;

            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(2), new Cell(3), new Cell(4),
                new Cell(5), new Cell(6), new Cell(7), new Cell(8), new Cell(9), new Cell(10), new Cell(11),
                new Cell(18), new Cell(19),new Cell(20)
            };

            var box = new Box(cells, 0);
            var row = new Row(cells, 0);

            var expectedTechnique = new TestEliminationTechnique(3);

            EliminatePossibleValues(expectedTechnique, cells, testValue, 1, 9, 10, 11, 12, 13, 14);

            var possibleValueSpotOverlapTechnique = new PossibleSpotOverlapTechnique();
            possibleValueSpotOverlapTechnique.Apply(cells, new List<Set>() { box, row });


            var actualTechnique = cells[1].EliminationTechniques[testValue].FirstOrDefault();
            Assert.AreEqual(expectedTechnique, actualTechnique);
        }

        [TestMethod]
        public void TestOverlapTechniqueDetails()
        {
            var testValue = 8;

            var cells = new List<Cell>() { new Cell(0), new Cell(1), new Cell(2), new Cell(3), new Cell(4),
                new Cell(5), new Cell(6), new Cell(7), new Cell(8), new Cell(9), new Cell(10), new Cell(11),
                new Cell(18), new Cell(19),new Cell(20)
            };

            var box = new Box(cells, 0);
            var row = new Row(cells, 0);

            var expectedTechnique = new TestEliminationTechnique(3);

            EliminatePossibleValues(expectedTechnique, cells, testValue, 1, 9, 10, 11, 12, 13, 14);

            var possibleValueSpotOverlapTechnique = new PossibleSpotOverlapTechnique();
            possibleValueSpotOverlapTechnique.Apply(cells, new List<Set>() { box, row });

            var actualTechnique = cells[4].EliminationTechniques[testValue].FirstOrDefault();
            Assert.IsNotNull(actualTechnique);
            Assert.IsTrue(actualTechnique.Indexes.SetEqual(cells.Select(c => c.Index)));
            Assert.IsTrue(actualTechnique.ValueMap.ContainsKey(testValue));
            Assert.AreEqual(2, actualTechnique.ValueMap[testValue].Count());
            Assert.IsFalse(actualTechnique.ValueMap[testValue].Except(0, 2).Any());
        }

        private void EliminatePossibleValues(IEliminationTechnique technique, List<Cell> cells, int value, params int[] indexes)
        {
            foreach (var i in indexes)
            {
                cells[i].EliminatePossibleValue(value, technique);
            }
        }
    }
}
