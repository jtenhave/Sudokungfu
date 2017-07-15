using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver
{
    using Model;
    using Sudokungfu.Extensions;
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;
    using Sudokungfu.SudokuSolver.Techniques;

    /// <summary>
    /// Test class for <see cref="Cell"/>.
    /// </summary>
    [TestClass]
    public class CellTest : BaseTest
    {
        [TestMethod]
        public void TestInitialPossibleValues()
        {
            var expectedValues = Constants.ALL_VALUES;

            var cell = new Cell(45);
            
            Assert.IsTrue(cell.PossibleValues.SetEqual(expectedValues));
        }

        [TestMethod]
        public void TestApplyTechnique()
        {
            var expectedValue = 3;
            var expectedPossibleValues = Constants.ALL_VALUES.Except(expectedValue);
            var expectedTechnique = new Technique();
            expectedTechnique.Values.Add(expectedValue);

            var cell = new Cell(0);
            cell.ApplyTechnique(expectedTechnique);

            Assert.IsTrue(expectedPossibleValues.SetEqual(cell.PossibleValues));
            Assert.IsTrue(cell.Techniques.ContainsKey(expectedValue));
            Assert.AreEqual(expectedTechnique, cell.Techniques[expectedValue].FirstOrDefault());
        }

        [TestMethod]
        public void TestAppliedTechniquesRemovesExistingHigherComplexity()
        {
            var expectedValue = 3;
            var expectedTechniqueA = new Technique()
            {
                Complexity = 1
            };
            expectedTechniqueA.Values.Add(expectedValue);

            var expectedTechniqueB = new Technique()
            {
                Complexity = 0
            };
            expectedTechniqueB.Values.Add(expectedValue);

            var cell = new Cell(0);
            cell.ApplyTechnique(expectedTechniqueA);
            Assert.IsTrue(cell.Techniques.ContainsKey(expectedValue));
            Assert.AreEqual(1, cell.Techniques[expectedValue].Count());
            Assert.AreEqual(expectedTechniqueA, cell.Techniques[expectedValue].First());

            cell.ApplyTechnique(expectedTechniqueB);
            Assert.AreEqual(1, cell.Techniques[expectedValue].Count());
            Assert.AreEqual(expectedTechniqueB, cell.Techniques[expectedValue].First());
        }

        [TestMethod]
        public void TestAppliedTechniquesIgnoresHigherComplexity()
        {
            var expectedValue = 3;
            var expectedTechniqueA = new Technique()
            {
                Complexity = 1
            };
            expectedTechniqueA.Values.Add(expectedValue);

            var expectedTechniqueB = new Technique()
            {
                Complexity = 0
            };
            expectedTechniqueB.Values.Add(expectedValue);

            var cell = new Cell(0);
            cell.ApplyTechnique(expectedTechniqueB);
            cell.ApplyTechnique(expectedTechniqueA);
            Assert.IsTrue(cell.Techniques.ContainsKey(expectedValue));
            Assert.AreEqual(1, cell.Techniques[expectedValue].Count());
            Assert.AreEqual(expectedTechniqueB, cell.Techniques[expectedValue].First());
        }

        [TestMethod]
        public void TestAppliedTechniquesAddsSamesComplexity()
        {
            var expectedValue = 3;
            var expectedTechniqueA = new Technique()
            {
                Complexity = 0
            };
            expectedTechniqueA.Values.Add(expectedValue);

            var expectedTechniqueB = new Technique()
            {
                Complexity = 0
            };
            expectedTechniqueB.Values.Add(expectedValue);

            var cell = new Cell(0);
            cell.ApplyTechnique(expectedTechniqueB);
            cell.ApplyTechnique(expectedTechniqueA);
            Assert.IsTrue(cell.Techniques.ContainsKey(expectedValue));
            Assert.AreEqual(2, cell.Techniques[expectedValue].Count());
            Assert.AreEqual(expectedTechniqueB, cell.Techniques[expectedValue].First());
            Assert.AreEqual(expectedTechniqueA, cell.Techniques[expectedValue].Last());
        }

        [TestMethod]
        public void TestResetAppliedTechniques()
        {
            var expectedValue = 3;
            var expectedTechnique = new Technique()
            {
                Complexity = 0
            };
            expectedTechnique.Values.Add(expectedValue);

            var cell = new Cell(0);
            cell.ApplyTechnique(expectedTechnique);

            Assert.IsFalse(cell.PossibleValues.Contains(expectedValue));
            Assert.IsTrue(cell.Techniques.ContainsKey(expectedValue));

            cell.ResetAppliedTechniques();

            Assert.IsTrue(cell.PossibleValues.Contains(expectedValue));
            Assert.IsFalse(cell.Techniques.ContainsKey(expectedValue));
        }

        [TestMethod]
        public void TestInsertResetsAppliedTechniques()
        {
            var expectedValue = 3;
            var expectedTechnique = new Technique()
            {
                Complexity = 0
            };
            expectedTechnique.Values.Add(expectedValue);

            var cell = new Cell(0);
            cell.ApplyTechnique(expectedTechnique);
            cell.InsertValue(new TestFoundValue(0, 5));
            
            Assert.IsFalse(cell.Techniques[expectedValue].Contains(expectedTechnique));
        }

        [TestMethod]
        public void TestInsertEliminatesAllOtherValues()
        {
            var expectedValue = 3;
            var expectedIndex = 23;
            var expectedFoundValue = new TestFoundValue(expectedIndex, expectedValue);

            var cell = new Cell(expectedIndex);
            cell.InsertValue(expectedFoundValue);

            ISudokuModel technique = null;
            foreach (var value in Constants.ALL_VALUES.Except(expectedValue))
            {
                Assert.AreEqual(1, cell.Techniques[value].Count());
                technique = technique ?? cell.Techniques[value].First();
                Assert.AreSame(technique, cell.Techniques[value].First());
            } 
        }

        [TestMethod]
        public void TestInsertEliminatesOtherValuesInMemberSets()
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
                cell.ResetAppliedTechniques();

                Assert.IsTrue(expectedValues.SetEqual(cell.PossibleValues));
                rowTechnique = rowTechnique ?? cell.Techniques[expectedValue].First();
                Assert.IsTrue(cell.Techniques[expectedValue].Contains(rowTechnique));
            }

            ISudokuModel colTechnique = null;
            foreach (var cell in col.Cells.Except(expectedCell).Reverse())
            {
                cell.ResetAppliedTechniques();

                Assert.IsTrue(expectedValues.SetEqual(cell.PossibleValues));
                colTechnique = colTechnique ?? cell.Techniques[expectedValue].First();
                Assert.IsTrue(cell.Techniques[expectedValue].Contains(colTechnique));
            }

            ISudokuModel boxTechnique = null;
            foreach (var cell in box.Cells.Except(expectedCell).Reverse())
            {
                cell.ResetAppliedTechniques();

                Assert.IsTrue(expectedValues.SetEqual(cell.PossibleValues));
                boxTechnique = boxTechnique ?? cell.Techniques[expectedValue].First();
                Assert.IsTrue(cell.Techniques[expectedValue].Contains(boxTechnique));
            }
        }

        [TestMethod]
        public void TestOccupiedClickableModel()
        {
            var expectedIndex = 0;
            var expectedClickableModel = new TestFoundValue(expectedIndex, 4);
            var cell = new Cell(expectedIndex);
            cell.InsertValue(expectedClickableModel);

            var technique = cell.Techniques[5].First();
            Assert.AreSame(expectedClickableModel, technique.ClickableModel);
        }

        [TestMethod]
        public void TestOccupiedIndexValueMap()
        {
            var expectedIndex = 34;
            var expectedValue = 4;

            var cell = new Cell(expectedIndex);
            cell.InsertValue(new TestFoundValue(expectedIndex, expectedValue));

            var technique = cell.Techniques[5].First();
            Assert.AreEqual(1, technique.IndexValueMap.Count());
            Assert.IsTrue(technique.IndexValueMap.ContainsKey(expectedIndex));
            Assert.AreEqual(1, technique.IndexValueMap[expectedIndex].Count());
            Assert.AreEqual(expectedValue, technique.IndexValueMap[expectedIndex].First());
        }

        [TestMethod]
        public void TestOccupiedAffectedIndexes()
        {
            var expectedIndex = 34;

            var cell = new Cell(expectedIndex);
            cell.InsertValue(new TestFoundValue(expectedIndex, 4));

            var technique = cell.Techniques[5].First();
            Assert.AreEqual(1, technique.AffectedIndexes.Count());
            Assert.IsTrue(technique.AffectedIndexes.Contains(expectedIndex));
        }

        [TestMethod]
        public void TestSetClickableModel()
        {
            var expectedValue = 4;
            var cells = GetAllCells();
            var box = new Box(cells, 0);
            var cell = cells.First();
            cell.Sets.Add(box);

            var expectedClickableModel = new TestFoundValue(cell.Index, expectedValue);
            cell.InsertValue(expectedClickableModel);

            var memberCell = cells.ElementAt(1);
            memberCell.ResetAppliedTechniques();
            var technique = memberCell.Techniques[expectedValue].First();

            Assert.AreSame(expectedClickableModel, technique.ClickableModel);
        }

        [TestMethod]
        public void TestSetCellValueMap()
        {

            var expectedValue = 4;
            var cells = GetAllCells();
            var box = new Box(cells, 0);
            var cell = cells.First();
            cell.Sets.Add(box);

            var expectedClickableModel = new TestFoundValue(cell.Index, expectedValue);
            cell.InsertValue(expectedClickableModel);

            var memberCell = cells.ElementAt(1);
            memberCell.ResetAppliedTechniques();
            var technique = memberCell.Techniques[expectedValue].First();

            AssertSetEqual(box.Cells, technique.CellValueMap.Keys);
            foreach (var key in technique.CellValueMap.Keys.Except(cell))
            {
                Assert.AreEqual(0, technique.CellValueMap[key].Count());
            }
            Assert.AreEqual(1, technique.CellValueMap[cell].Count());
            Assert.AreEqual(expectedValue, technique.CellValueMap[cell].First());
        }
    }
}
