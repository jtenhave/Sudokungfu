using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver
{
    using Model;
    using Sudokungfu.Extensions;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;

    /// <summary>
    /// Test class for <see cref="Cell"/>.
    /// </summary>
    [TestClass]
    public class CellTest : BaseTest
    {
        [TestMethod]
        public void TestInitializePossibleValues()
        {
            var expectedValues = Constants.ALL_VALUES;

            var cell = new Cell(45);
            
            Assert.IsTrue(cell.PossibleValues.SetEqual(expectedValues));
        }

        [TestMethod]
        public void TestInitializeEliminationTechniques()
        {
            var expectedValues = Constants.ALL_VALUES;

            var cell = new Cell(45);

            AssertSetEqual(expectedValues, cell.EliminationTechniques.Keys);
            foreach (var value in expectedValues)
            {
                Assert.IsFalse(cell.EliminationTechniques[value].Any());
            }
        }

        [TestMethod]
        public void TestEliminatePossibleValue()
        {
            var expectedValue = 3;
            var expectedPossibleValues = Constants.ALL_VALUES.Except(expectedValue);
            var expectedTechnique = new TestModel();

            var cell = new Cell(0);
            cell.EliminatePossibleValue(expectedValue, expectedTechnique);

            Assert.IsTrue(expectedPossibleValues.SetEqual(cell.PossibleValues));
            foreach (var value in expectedPossibleValues)
            {
                Assert.IsFalse(cell.EliminationTechniques[value].Any());
            }

            Assert.AreEqual(1, cell.EliminationTechniques[expectedValue].Count());
            Assert.AreEqual(expectedTechnique, cell.EliminationTechniques[expectedValue].First());

        }

        [TestMethod]
        public void TestHigherComplexityEliminationTechniqueIgnored()
        {
            var expectedValue = 3;
            var expectedTechniqueA = new TestModel()
            {
                Complexity = 0
            };

            var expectedTechniqueB = new TestModel()
            {
                Complexity = 1
            };

            var cell = new Cell(0);
            cell.EliminatePossibleValue(expectedValue, expectedTechniqueA);
            cell.EliminatePossibleValue(expectedValue, expectedTechniqueB);

            Assert.AreEqual(1, cell.EliminationTechniques[expectedValue].Count());
            Assert.AreEqual(expectedTechniqueA, cell.EliminationTechniques[expectedValue].First());
        }

        [TestMethod]
        public void TestLowerComplexityEliminationTechniqueOverwrites()
        {
            var expectedValue = 3;
            var expectedTechniqueA = new TestModel()
            {
                Complexity = 0
            };

            var expectedTechniqueB = new TestModel()
            {
                Complexity = 1
            };

            var cell = new Cell(0);
            cell.EliminatePossibleValue(expectedValue, expectedTechniqueB);
            cell.EliminatePossibleValue(expectedValue, expectedTechniqueA);

            Assert.AreEqual(1, cell.EliminationTechniques[expectedValue].Count());
            Assert.AreEqual(expectedTechniqueA, cell.EliminationTechniques[expectedValue].First());
        }

        [TestMethod]
        public void TestEqualComplexityEliminationTechniqueIsAdded()
        {
            var expectedValue = 3;
            var expectedTechniqueA = new TestModel()
            {
                Complexity = 0
            };

            var expectedTechniqueB = new TestModel()
            {
                Complexity = 0
            };

            var cell = new Cell(0);
            cell.EliminatePossibleValue(expectedValue, expectedTechniqueA);
            cell.EliminatePossibleValue(expectedValue, expectedTechniqueB);

            Assert.AreEqual(2, cell.EliminationTechniques[expectedValue].Count());
            Assert.AreEqual(expectedTechniqueA, cell.EliminationTechniques[expectedValue].First());
            Assert.AreEqual(expectedTechniqueB, cell.EliminationTechniques[expectedValue].Last());
        }

        [TestMethod]
        public void TestEliminationTechniqueNotAddedAfterCellFilled()
        {
            var expectedValue = 3;
            var expectedTechniqueA = new TestModel()
            {
                Complexity = 1
            };

            var expectedTechniqueB = new TestModel()
            {
                Complexity = 0
            };
         
            var cell = new Cell(0);
            cell.EliminatePossibleValue(expectedValue, expectedTechniqueA);
            cell.InsertValue(new TestFoundValue(0, expectedValue));

            cell.EliminatePossibleValue(expectedValue, expectedTechniqueB);

            Assert.AreEqual(1, cell.EliminationTechniques[expectedValue].Count());
            Assert.AreEqual(expectedTechniqueA, cell.EliminationTechniques[expectedValue].First());
        }

        [TestMethod]
        public void TestInsertValueEliminatesAllOtherValues()
        {
            var expectedValue = 3;
            var expectedIndex = 23;
            var expectedFoundValue = new TestFoundValue(expectedIndex, expectedValue);

            var cell = new Cell(expectedIndex);
            cell.InsertValue(expectedFoundValue);

            ISudokuModel technique = null;
            foreach (var value in Constants.ALL_VALUES.Except(expectedValue))
            {
                Assert.AreEqual(1, cell.EliminationTechniques[value].Count());
                technique = technique ?? cell.EliminationTechniques[value].First();
                Assert.AreSame(technique, cell.EliminationTechniques[value].First());
            } 
        }

        [TestMethod]
        public void TestInsertValueEliminatesAllOtherValuesInMemberSets()
        {
            var expectedValue = 4;
            var expectedValues = Constants.ALL_VALUES.Except(expectedValue);
            var cells = GetAllCells();
            var row = new Row(cells, 0);
            var col = new Column(cells, 0);
            var box = new Box(cells, 0);

            var expectedCell = cells.First();
            expectedCell.InsertValue(new TestFoundValue(expectedCell.Index, expectedValue));

            ISudokuModel rowTechnique = null;
            foreach (var cell in row.Cells.Except(expectedCell).Reverse())
            {
                Assert.IsTrue(expectedValues.SetEqual(cell.PossibleValues));
                rowTechnique = rowTechnique ?? cell.EliminationTechniques[expectedValue].First();
                Assert.IsTrue(cell.EliminationTechniques[expectedValue].Contains(rowTechnique));
            }

            ISudokuModel colTechnique = null;
            foreach (var cell in col.Cells.Except(expectedCell).Reverse())
            {
                Assert.IsTrue(expectedValues.SetEqual(cell.PossibleValues));
                colTechnique = colTechnique ?? cell.EliminationTechniques[expectedValue].First();
                Assert.IsTrue(cell.EliminationTechniques[expectedValue].Contains(colTechnique));
            }

            ISudokuModel boxTechnique = null;
            foreach (var cell in box.Cells.Except(expectedCell).Reverse())
            {
                Assert.IsTrue(expectedValues.SetEqual(cell.PossibleValues));
                boxTechnique = boxTechnique ?? cell.EliminationTechniques[expectedValue].First();
                Assert.IsTrue(cell.EliminationTechniques[expectedValue].Contains(boxTechnique));
            }
        }

        [TestMethod]
        public void TestFindMinTechniquesEmpty()
        {
            var cell = new Cell(0);

            Assert.IsFalse(cell.FindMinTechniques(4).Any());
        }

        [TestMethod]
        public void TestFindMinTechniques()
        {
            var expectedValue = 4;
            var cell = new Cell(0);
            var expectedTechniques = new List<TestModel>();

            foreach (var value in Constants.ALL_VALUES.Except(expectedValue))
            {
                var technique = new TestModel();
                expectedTechniques.Add(technique);
                cell.EliminatePossibleValue(value, technique);
            }


            AssertSetEqual(expectedTechniques, cell.FindMinTechniques(expectedValue));
        }
    }
}
