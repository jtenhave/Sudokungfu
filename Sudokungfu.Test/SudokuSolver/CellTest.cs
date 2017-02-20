using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver
{
    using Sudokungfu.Extensions;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;

    /// <summary>
    /// Test class for <see cref="Cell"/>
    /// </summary>
    [TestClass]
    public class CellTest : BaseTest
    {
        [TestMethod]
        public void TestCellInitialize()
        {
            var expectedIndex = 45;
            var expectedValues = Constants.ALL_VALUES;

            var cell = new Cell(expectedIndex);

            Assert.AreEqual(expectedIndex, cell.Index);
            Assert.IsTrue(cell.PossibleValues.SetEqual(expectedValues));
            Assert.AreEqual(expectedValues.Count(), cell.EliminationTechniques.Count);

            foreach (var value in expectedValues)
            {
                Assert.IsTrue(cell.EliminationTechniques.ContainsKey(value));
                Assert.IsFalse(cell.EliminationTechniques[value].Any());
            }
        }

        [TestMethod]
        public void TestEliminatePossibleValue()
        {
            var testValue = 3;
            var expectedValues = Constants.ALL_VALUES.Except(testValue);

            var cell = new Cell(0);
            cell.EliminatePossibleValue(testValue, new TestTechnique());

            Assert.IsTrue(expectedValues.SetEqual(cell.PossibleValues));
        }

        [TestMethod]
        public void TestTechniqueAdded()
        {
            var testValue = 3;
            var expectedTechnique = new TestTechnique()
            {
                Complexity = 3,
                IndexValueMap = new Dictionary<int, IEnumerable<int>>()
                {
                    [testValue] = 0.ToEnumerable()
                }
            };

            var cell = new Cell(0);
            cell.EliminatePossibleValue(testValue, expectedTechnique);

            Assert.AreEqual(1, cell.EliminationTechniques[testValue].Count());
            AssertITechniqueEqual(expectedTechnique, cell.EliminationTechniques[testValue].First());
        }

        [TestMethod]
        public void TestHigherComplexityEliminationTechniqueIgnored()
        {
            var testValue = 3;
            var expectedTechniqueA = new TestTechnique()
            {
                Complexity = 0
            };

            var expectedTechniqueB = new TestTechnique()
            {
                Complexity = 1
            };

            var cell = new Cell(0);
            cell.EliminatePossibleValue(testValue, expectedTechniqueA);
            cell.EliminatePossibleValue(testValue, expectedTechniqueB);

            Assert.AreEqual(1, cell.EliminationTechniques[testValue].Count());
            AssertITechniqueEqual(expectedTechniqueA, cell.EliminationTechniques[testValue].First());
        }

        [TestMethod]
        public void TestLowerComplexityEliminationTechniqueOverwrites()
        {
            var testValue = 3;
            var expectedTechniqueA = new TestTechnique()
            {
                Complexity = 0
            };

            var expectedTechniqueB = new TestTechnique()
            {
                Complexity = 1
            };

            var cell = new Cell(0);
            cell.EliminatePossibleValue(testValue, expectedTechniqueB);
            cell.EliminatePossibleValue(testValue, expectedTechniqueA);

            Assert.AreEqual(1, cell.EliminationTechniques[testValue].Count());
            AssertITechniqueEqual(expectedTechniqueA, cell.EliminationTechniques[testValue].First());
        }

        [TestMethod]
        public void TestEqualComplexityEliminationTechniqueIsAdded()
        {
            var testValue = 3;
            var expectedTechniqueA = new TestTechnique()
            {
                Complexity = 0
            };

            var expectedTechniqueB = new TestTechnique()
            {
                Complexity = 0
            };

            var cell = new Cell(0);
            cell.EliminatePossibleValue(testValue, expectedTechniqueA);
            cell.EliminatePossibleValue(testValue, expectedTechniqueB);

            Assert.AreEqual(2, cell.EliminationTechniques[testValue].Count());
            AssertITechniqueEqual(expectedTechniqueA, cell.EliminationTechniques[testValue].First());
            AssertITechniqueEqual(expectedTechniqueB, cell.EliminationTechniques[testValue].Last());
        }

        [TestMethod]
        public void TestEliminationTechniqueNotAddedAfterCellFilled()
        {
            var testValue = 3;
            var expectedTechniqueA = new TestTechnique()
            {
                Complexity = 1
            };

            var expectedTechniqueB = new TestTechnique()
            {
                Complexity = 0
            };
         
            var cell = new Cell(0);
            foreach (var value in Constants.ALL_VALUES)
            {
                cell.EliminatePossibleValue(value, expectedTechniqueA);
            }

            cell.EliminatePossibleValue(testValue, expectedTechniqueB);

            Assert.AreEqual(1, cell.EliminationTechniques[testValue].Count());
            AssertITechniqueEqual(expectedTechniqueA, cell.EliminationTechniques[testValue].First());
        }

        [TestMethod]
        public void TestInsertValueEliminatesAllOtherValues()
        {
            var testValue = 3;
            var testIndex = 23;

            var cell = new Cell(testIndex);
            cell.InsertValue(testValue);

            var expectedTechnique = new TestTechnique()
            {
                Complexity = 0,
                IndexValueMap = new Dictionary<int, IEnumerable<int>>()
                {
                    [testIndex] = testValue.ToEnumerable()
                },
                UsesFoundValues = true
            };

            Assert.AreEqual(0, cell.EliminationTechniques[testValue].Count());

            foreach (var value in Constants.ALL_VALUES.Except(testValue))
            {
                Assert.AreEqual(1, cell.EliminationTechniques[value].Count());
                AssertITechniqueEqual(expectedTechnique, cell.EliminationTechniques[value].First());
            } 
        }

        [TestMethod]
        public void TestInsertValuePreventsAddingEliminationTechnique()
        {
            var testValue = 3;
            var testIndex = 23;
            var testInsertedValue = 5;

            var cell = new Cell(testIndex);
            cell.InsertValue(testInsertedValue);

            var expectedTechnique = new TestTechnique()
            {
                Complexity = 0,
                IndexValueMap = new Dictionary<int, IEnumerable<int>>()
                {
                    [testIndex] = testInsertedValue.ToEnumerable()
                },
                UsesFoundValues = true
            };

            var testTechnique = new TestTechnique()
            {
                Complexity = 3,
                IndexValueMap = new Dictionary<int, IEnumerable<int>>()
                {
                    [45] = 56.ToEnumerable()
                },
                UsesFoundValues = false
            };

            cell.EliminatePossibleValue(testValue, testTechnique);

            Assert.AreEqual(1, cell.EliminationTechniques[testValue].Count());
            AssertITechniqueEqual(expectedTechnique, cell.EliminationTechniques[testValue].First());
        }

        [TestMethod]
        public void TestInsertValueEliminatesAllOtherValuesInMemberSets()
        {
            var testValue = 4;
            var expectedValues = Constants.ALL_VALUES.Except(testValue);
            var cells = GetAllCells();

            var row = new Row(cells, 0);
            var col = new Column(cells, 0);
            var box = new Box(cells, 0);

            var testCell = cells.First();
            testCell.InsertValue(testValue);

            Assert.IsFalse(row.Cells.First().PossibleValues.Any());
            foreach (var cell in row.Cells.Except(testCell))
            {
                Assert.IsTrue(expectedValues.SetEqual(cell.PossibleValues));
            }

            foreach (var cell in row.Cells.Except(testCell))
            {
                Assert.IsTrue(expectedValues.SetEqual(cell.PossibleValues));
            }

            foreach (var cell in box.Cells.Except(testCell))
            {
                Assert.IsTrue(expectedValues.SetEqual(cell.PossibleValues));
            }
        }
    }
}
