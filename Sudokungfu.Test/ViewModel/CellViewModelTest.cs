using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Media;

namespace Sudokungfu.Test.ViewModel
{
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Techniques;
    using Sudokungfu.ViewModel;

    /// <summary>
    /// Test class for <see cref="CellViewModel"/>.
    /// </summary>
    [TestClass]
    public class CellViewModelTest
    {
        [TestMethod]
        public void TestValueMustBeInt()
        {
            var expectedValue = "6";

            var model = new CellViewModel(0);
            model.Value = "a";
            model.Value = "h";
            model.Value = expectedValue;
            model.Value = "b";
            model.Value = "n";

            Assert.AreEqual(expectedValue, model.Value);
        }

        [TestMethod]
        public void TestValueCanBeCleared()
        {
            var expectedValue = string.Empty;

            var model = new CellViewModel(0);
            model.Value = "5";
            model.Value = expectedValue;

            Assert.AreEqual(expectedValue, model.Value);
        }

        [TestMethod]
        public void TestSetFoundValueSetsPropertiesForNoDetails()
        {
            var expectedValue = 4;
            var foundValue = new FoundValue(new Cell(0), expectedValue);

            var model = new CellViewModel(0);
            model.FoundValue = foundValue;

            Assert.AreEqual(Brushes.LightGray, model.Background);
            Assert.AreEqual(expectedValue, model.ValueAsInt);
            Assert.IsNull(model.ClickableModel);
        }

        [TestMethod]
        public void TestSetFoundValueSetsPropertiesForDetails()
        {
            var expectedValue = 4;
            var technique = new Technique();
            var foundValue = new FoundValue(new Cell(0), expectedValue);
            foundValue.Techniques.Add(technique);

            var model = new CellViewModel(0);
            model.FoundValue = foundValue;

            Assert.AreEqual(Brushes.White, model.Background);
            Assert.AreEqual(expectedValue, model.ValueAsInt);
            Assert.AreEqual(foundValue, model.ClickableModel);
        }
    }
}
