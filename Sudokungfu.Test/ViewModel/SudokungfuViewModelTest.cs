using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Sudokungfu.Test.ViewModel
{
    using Sudokungfu.ViewModel;

    /// <summary>
    /// Test class for <see cref="SudokungfuViewModel"/>.
    /// </summary>
    [TestClass]
    public class SudokungfuViewModelTest
    {
        private readonly List<string> EASY_VALID = new List<string>()
        {
            "", "9", "1", "", "7", "", "", "", "",
            "", "7", "", "9", "8", "5", "1", "2", "",
            "8", "", "", "", "", "4", "", "9", "7",
            "7", "", "", "8", "", "6", "4", "", "",
            "", "4", "", "", "", "", "", "6", "9",
            "6", "1", "", "", "", "7", "", "", "",
            "", "", "", "3", "", "9", "2", "7", "8",
            "9", "", "7", "2", "", "", "", "1", "",
            "", "8", "2", "", "", "1", "", "3", "6"
        };

        private readonly List<string> EASY_INVALID = new List<string>()
        {
            "", "9", "1", "", "7", "", "", "", "9",
            "", "7", "", "9", "8", "5", "1", "2", "",
            "8", "", "", "", "", "4", "", "9", "7",
            "7", "", "", "8", "", "6", "4", "", "",
            "", "4", "", "", "", "", "", "6", "9",
            "6", "1", "", "", "", "7", "", "", "",
            "", "", "", "3", "", "9", "2", "7", "8",
            "9", "", "7", "2", "", "", "", "1", "",
            "", "8", "2", "", "", "1", "", "3", "6"
        };

        private readonly List<string> HARD_VALID = new List<string>()
        {
            "", "", "", "4", "9", "", "", "", "",
            "", "", "", "8", "", "1", "6", "", "",
            "", "1", "", "", "", "3", "", "", "9",
            "", "", "", "", "", "", "", "", "8",
            "", "7", "1", "", "", "9", "4", "", "",
            "6", "", "", "", "", "", "", "7", "5",
            "", "", "3", "", "", "", "5", "", "",
            "", "6", "", "3", "7", "2", "", "", "",
            "", "", "7", "5", "", "", "8", "", "",
        };


        [TestMethod]
        public void TestDefaultCommandStatus()
        {
            var model = new SudokungfuViewModel(null, null, null);

            Assert.IsFalse(model.BackCommand.CanExecute(null));
            Assert.IsTrue(model.ClearCommand.CanExecute(null));
            Assert.IsFalse(model.CloseCommand.CanExecute(null));
            Assert.IsTrue(model.EnterCommand.CanExecute(null));
            Assert.IsFalse(model.NextCommand.CanExecute(null));
            Assert.IsFalse(model.PreviousCommand.CanExecute(null));
            Assert.IsFalse(model.SolveCommand.CanExecute(null));
        }

        [TestMethod]
        public async Task TestEnterCommandSetsSolving()
        {
            var solving = false;
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_INVALID, model);

            Assert.IsFalse(model.IsSolving);
            model.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "IsSolving")
                {
                    solving = true;
                }
            };

            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            Assert.IsTrue(solving);
            Assert.IsFalse(model.IsSolving);
        }

        [TestMethod]
        public async Task TestEnterCommandCallsInvalidForInvalid()
        {
            var invalid = false;
            var model = new SudokungfuViewModel(null, () => invalid = true, null);
            SetSudoku(EASY_INVALID, model);

            await (model.EnterCommand as DelegateCommand).ExecuteAsync();

            Assert.IsTrue(invalid);
        }

        [TestMethod]
        public async Task TestEnterCommandSetsEnterCommandStatusInvalid()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_INVALID, model);

            await AssertCommandStatusSequence(model.EnterCommand, 
                () => (model.EnterCommand as DelegateCommand).ExecuteAsync(), 
                false, true);
        }

        [TestMethod]
        public async Task TestEnterCommandSetsEnterCommandStatusValid()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);

            await AssertCommandStatusSequence(model.EnterCommand,
                () => (model.EnterCommand as DelegateCommand).ExecuteAsync(),
                false);
        }

        [TestMethod]
        public async Task TestEnterCommandSetsClearCommandStatusInvalid()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_INVALID, model);

            await AssertCommandStatusSequence(model.ClearCommand,
                () => (model.EnterCommand as DelegateCommand).ExecuteAsync(),
                false, true);
        }

        [TestMethod]
        public async Task TestEnterCommandSetsClearCommandStatusValid()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);

            await AssertCommandStatusSequence(model.ClearCommand,
                () => (model.EnterCommand as DelegateCommand).ExecuteAsync(),
                false, true);
        }

        [TestMethod]
        public async Task TestEnterCommandSetsNextCommandStatusInvalid()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_INVALID, model);

            await AssertCommandStatusSequence(model.NextCommand,
                () => (model.EnterCommand as DelegateCommand).ExecuteAsync());
        }

        [TestMethod]
        public async Task TestEnterCommandSetsNextCommandStatusValid()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);

            await AssertCommandStatusSequence(model.NextCommand,
                () => (model.EnterCommand as DelegateCommand).ExecuteAsync(),
                true);
        }

        [TestMethod]
        public async Task TestEnterCommandSetsSolveCommandStatusInvalid()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_INVALID, model);

            await AssertCommandStatusSequence(model.SolveCommand,
                () => (model.EnterCommand as DelegateCommand).ExecuteAsync());
        }

        [TestMethod]
        public async Task TestEnterCommandSetsSolveCommandStatusValid()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);

            await AssertCommandStatusSequence(model.SolveCommand,
                () => (model.EnterCommand as DelegateCommand).ExecuteAsync(),
                true);
        }

        [TestMethod]
        public async Task TestNextCommandDisplaysNextValue()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            var valueCount = model.Cells.Count(c => c.ValueAsInt != 0);
            for (int i = 1; i <= Constants.CELL_COUNT - valueCount; i++)
            {
                await (model.NextCommand as DelegateCommand).ExecuteAsync();
                Assert.AreEqual(valueCount + i, model.Cells.Count(c => c.ValueAsInt != 0));
            }

            Assert.AreEqual(Constants.CELL_COUNT, model.Cells.Count(c => c.ValueAsInt != 0));
        }

        [TestMethod]
        public async Task TestNextCommandDisabledWhenNoValuesLeft()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            var valueCount = model.Cells.Count(c => c.ValueAsInt != 0);
            for (int i = 0; i < Constants.CELL_COUNT - valueCount; i++)
            {
                await (model.NextCommand as DelegateCommand).ExecuteAsync();
            }

            Assert.IsFalse(model.NextCommand.CanExecute(null));
            Assert.IsFalse(model.SolveCommand.CanExecute(null));
        }

        [TestMethod]
        public async Task TestNextCommandEnablesPreviousCommand()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();

            Assert.IsFalse(model.PreviousCommand.CanExecute(null));
            await (model.NextCommand as DelegateCommand).ExecuteAsync();
            Assert.IsTrue(model.PreviousCommand.CanExecute(null));
        }

        [TestMethod]
        public async Task TestSolveCommandDisplaysAllValues()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.SolveCommand as DelegateCommand).ExecuteAsync();

            Assert.AreEqual(Constants.CELL_COUNT, model.Cells.Count(c => c.ValueAsInt != 0));
        }

        [TestMethod]
        public async Task TestSolveCommandDisablesSolveCommand()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.SolveCommand as DelegateCommand).ExecuteAsync();

            Assert.IsFalse(model.NextCommand.CanExecute(null));
            Assert.IsFalse(model.SolveCommand.CanExecute(null));
            Assert.IsTrue(model.PreviousCommand.CanExecute(null));
        }

        [TestMethod]
        public async Task TestPreviousCommandHidesLastValue()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            var valueCount = model.Cells.Count(c => c.ValueAsInt != 0);
            await (model.SolveCommand as DelegateCommand).ExecuteAsync();
            for (int i = 1; i <= Constants.CELL_COUNT - valueCount; i++)
            {
                await (model.PreviousCommand as DelegateCommand).ExecuteAsync();
                Assert.AreEqual(Constants.CELL_COUNT - i, model.Cells.Count(c => c.ValueAsInt != 0));
            }

            Assert.AreEqual(valueCount, model.Cells.Count(c => c.ValueAsInt != 0));
        }

        [TestMethod]
        public async Task TestPreviousCommandDisabledWhenNoValuesLeft()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            var valueCount = model.Cells.Count(c => c.ValueAsInt != 0);
            await (model.SolveCommand as DelegateCommand).ExecuteAsync();
            for (int i = 1; i <= Constants.CELL_COUNT - valueCount; i++)
            {
                await (model.PreviousCommand as DelegateCommand).ExecuteAsync();
                Assert.AreEqual(Constants.CELL_COUNT - i, model.Cells.Count(c => c.ValueAsInt != 0));
            }

            Assert.IsFalse(model.PreviousCommand.CanExecute(null));
        }

        [TestMethod]
        public async Task TestPreviousEnablesNextCommand()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.SolveCommand as DelegateCommand).ExecuteAsync();
            await (model.PreviousCommand as DelegateCommand).ExecuteAsync();

            Assert.IsTrue(model.NextCommand.CanExecute(null));
            Assert.IsTrue(model.SolveCommand.CanExecute(null));
        }

        [TestMethod]
        public async Task TestClearCommandClearsOnConfirm()
        {
            var model = new SudokungfuViewModel(() => true, null, null);
            SetSudoku(EASY_VALID, model);

            await (model.ClearCommand as DelegateCommand).ExecuteAsync();

            Assert.AreEqual(0, model.Cells.Count(c => c.ValueAsInt != 0));
        }

        [TestMethod]
        public async Task TestClearCommandDoesNothinOnCancel()
        {
            var model = new SudokungfuViewModel(() => false, null, null);
            SetSudoku(EASY_VALID, model);

            await (model.ClearCommand as DelegateCommand).ExecuteAsync();

            for (int i = 0; i < Constants.CELL_COUNT; i++)
            {
                Assert.AreEqual(EASY_VALID[i], model.Cells[i].Value);
            }
        }

        [TestMethod]
        public async Task TestClearCommandEnablesEnterCommand()
        {
            var model = new SudokungfuViewModel(() => true, null, null);
            SetSudoku(EASY_VALID, model);

            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.ClearCommand as DelegateCommand).ExecuteAsync();

            Assert.IsFalse(model.SolveCommand.CanExecute(null));
            Assert.IsFalse(model.NextCommand.CanExecute(null));
            Assert.IsFalse(model.PreviousCommand.CanExecute(null));
            Assert.IsTrue(model.EnterCommand.CanExecute(null));
        }

        [TestMethod]
        public async Task TestClickCommandDisplaysDetails()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.NextCommand as DelegateCommand).ExecuteAsync();

            await (model.Cells[42].ClickCommand as DelegateCommand).ExecuteAsync();

            Assert.AreEqual(7, model.Cells[42].ValueAsInt);
            Assert.AreEqual(Brushes.LightGreen, model.Cells[42].Background);
        }

        [TestMethod]
        public async Task TestClickCommandEnablesDetailsCommands()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.NextCommand as DelegateCommand).ExecuteAsync();

            await (model.Cells[42].ClickCommand as DelegateCommand).ExecuteAsync();

            Assert.IsTrue(model.BackCommand.CanExecute(null));
            Assert.IsTrue(model.CloseCommand.CanExecute(null));
        }

        [TestMethod]
        public async Task TestClickCommandDisplaysNextDetailsSet()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.SolveCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[40].ClickCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[35].ClickCommand as DelegateCommand).ExecuteAsync();

            Assert.AreEqual(1, model.Cells[35].ValueAsInt);
            Assert.AreEqual(Brushes.LightGreen, model.Cells[35].Background);
        }

        [TestMethod]
        public async Task TestCloseCommandClosesDetails()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.NextCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[42].ClickCommand as DelegateCommand).ExecuteAsync();
            await (model.CloseCommand as DelegateCommand).ExecuteAsync();

            for (int i = 0; i < Constants.CELL_COUNT; i++)
            {
                if (i == 42)
                {
                    Assert.AreEqual("7", model.Cells[i].Value);
                }
                else
                {
                    Assert.AreEqual(EASY_VALID[i], model.Cells[i].Value);
                }
            }
        }

        [TestMethod]
        public async Task TestCloseCommandEnablesCommands()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.NextCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[42].ClickCommand as DelegateCommand).ExecuteAsync();
            await (model.CloseCommand as DelegateCommand).ExecuteAsync();

            Assert.IsTrue(model.NextCommand.CanExecute(null));
            Assert.IsTrue(model.PreviousCommand.CanExecute(null));
            Assert.IsTrue(model.SolveCommand.CanExecute(null));
            Assert.IsFalse(model.BackCommand.CanExecute(null));
            Assert.IsFalse(model.CloseCommand.CanExecute(null));
        }

        [TestMethod]
        public async Task TestBackCommandClosesDetails()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.NextCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[42].ClickCommand as DelegateCommand).ExecuteAsync();
            await (model.BackCommand as DelegateCommand).ExecuteAsync();

            for (int i = 0; i < Constants.CELL_COUNT; i++)
            {
                if (i == 42)
                {
                    Assert.AreEqual("7", model.Cells[i].Value);
                }
                else
                {
                    Assert.AreEqual(EASY_VALID[i], model.Cells[i].Value);
                }
            }
        }

        [TestMethod]
        public async Task TestBackCommandEnablesCommands()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.NextCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[42].ClickCommand as DelegateCommand).ExecuteAsync();
            await (model.BackCommand as DelegateCommand).ExecuteAsync();

            Assert.IsTrue(model.NextCommand.CanExecute(null));
            Assert.IsTrue(model.PreviousCommand.CanExecute(null));
            Assert.IsTrue(model.SolveCommand.CanExecute(null));
            Assert.IsFalse(model.BackCommand.CanExecute(null));
            Assert.IsFalse(model.CloseCommand.CanExecute(null));
        }

        [TestMethod]
        public async Task TestBackDisplaysPreviousTechnique()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.SolveCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[40].ClickCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[35].ClickCommand as DelegateCommand).ExecuteAsync();
            await (model.BackCommand as DelegateCommand).ExecuteAsync();

            Assert.AreEqual(1, model.Cells[40].ValueAsInt);
            Assert.AreEqual(Brushes.LightGreen, model.Cells[40].Background);
        }

        [TestMethod]
        public async Task TestDetailsValueColour()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.SolveCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[42].ClickCommand as DelegateCommand).ExecuteAsync();

            Assert.AreEqual(7, model.Cells[42].ValueAsInt);
            Assert.AreEqual(Brushes.LightGreen, model.Cells[42].Background);
        }

        [TestMethod]
        public async Task TestDetailsSetColour()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.SolveCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[42].ClickCommand as DelegateCommand).ExecuteAsync();

            for (int i = 36; i <= 44; i++)
            {
                if (model.Cells[i].ValueAsInt == 0)
                {
                    Assert.AreEqual(Brushes.Salmon, model.Cells[i].Background);
                }
            }
        }

        [TestMethod]
        public async Task TestDetailsTechniqueValues()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.SolveCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[42].ClickCommand as DelegateCommand).ExecuteAsync();

            Assert.AreEqual(4, model.Cells[37].ValueAsInt);
            Assert.AreEqual(6, model.Cells[43].ValueAsInt);
            Assert.AreEqual(9, model.Cells[44].ValueAsInt);
            Assert.AreEqual(7, model.Cells[50].ValueAsInt);
            Assert.AreEqual(7, model.Cells[27].ValueAsInt);
        }

        [TestMethod]
        public async Task TestDetailsTechniqueValuesWithNoDetailsColor()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.SolveCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[42].ClickCommand as DelegateCommand).ExecuteAsync();

            Assert.AreEqual(Brushes.DarkSalmon, model.Cells[37].Background);
            Assert.AreEqual(Brushes.DarkSalmon, model.Cells[43].Background);
            Assert.AreEqual(Brushes.DarkSalmon, model.Cells[44].Background);
            Assert.AreEqual(Brushes.DarkSalmon, model.Cells[50].Background);
            Assert.AreEqual(Brushes.DarkSalmon, model.Cells[27].Background);
        }

        [TestMethod]
        public async Task TestDetailsTechniqueValuesInSetWithDetailsColor()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.SolveCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[51].ClickCommand as DelegateCommand).ExecuteAsync();

            Assert.IsTrue(model.Cells[35].ValueAsInt > 0);
            Assert.AreEqual(Brushes.Salmon, model.Cells[35].Background);
            Assert.IsTrue(model.Cells[42].ValueAsInt > 0);
            Assert.AreEqual(Brushes.Salmon, model.Cells[42].Background);
            Assert.IsTrue(model.Cells[53].ValueAsInt > 0);
            Assert.AreEqual(Brushes.Salmon, model.Cells[53].Background);
        }

        [TestMethod]
        public async Task TestDetailsTechniqueValuesOutSideSetWithDetailsColor()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.SolveCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[19].ClickCommand as DelegateCommand).ExecuteAsync();

            Assert.IsTrue(model.Cells[47].ValueAsInt > 0);
            Assert.AreEqual(Brushes.LightSalmon, model.Cells[47].Background);
            Assert.IsTrue(model.Cells[72].ValueAsInt > 0);
            Assert.AreEqual(Brushes.LightSalmon, model.Cells[72].Background);
        }

        [TestMethod]
        public async Task TestDetailsTechniqueValueOverlapSetColour()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(EASY_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.SolveCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[42].ClickCommand as DelegateCommand).ExecuteAsync();

            Assert.AreEqual(Brushes.LightSalmon, model.Cells[30].Background);
            Assert.AreEqual(Brushes.LightSalmon, model.Cells[31].Background);
            Assert.AreEqual(Brushes.LightSalmon, model.Cells[32].Background);
            Assert.AreEqual(Brushes.LightSalmon, model.Cells[48].Background);
            Assert.AreEqual(Brushes.LightSalmon, model.Cells[49].Background);
        }

        [TestMethod]
        public async Task TestDetailsTechniqueMultipleValues()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(HARD_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.SolveCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[2].ClickCommand as DelegateCommand).ExecuteAsync();

            Assert.AreEqual("57", model.Cells[5].Value);
            Assert.AreEqual(CellViewModel.TWO_VALUE_SIZE_DEFAULT, model.Cells[5].FontSize);
            Assert.AreEqual("57", model.Cells[32].Value);
            Assert.AreEqual(CellViewModel.TWO_VALUE_SIZE_DEFAULT, model.Cells[32].FontSize);
        }

        [TestMethod]
        public async Task TestDetailsMultipleValues()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(HARD_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.SolveCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[2].ClickCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[5].ClickCommand as DelegateCommand).ExecuteAsync();

            Assert.AreEqual("57", model.Cells[5].Value);
            Assert.AreEqual(CellViewModel.TWO_VALUE_SIZE_DEFAULT, model.Cells[5].FontSize);
            Assert.AreEqual(Brushes.LightGreen, model.Cells[5].Background);
            Assert.AreEqual("57", model.Cells[32].Value);
            Assert.AreEqual(CellViewModel.TWO_VALUE_SIZE_DEFAULT, model.Cells[32].FontSize);
            Assert.AreEqual(Brushes.LightGreen, model.Cells[32].Background);
        }

        [TestMethod]
        public async Task TestDetailsTechniqueSingleValueMultipleCell()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(HARD_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.SolveCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[6].ClickCommand as DelegateCommand).ExecuteAsync();

            Assert.AreEqual(3, model.Cells[43].ValueAsInt);
            Assert.AreEqual(CellViewModel.TWO_VALUE_SIZE_DEFAULT, model.Cells[43].FontSize);
            Assert.AreEqual(3, model.Cells[44].ValueAsInt);
            Assert.AreEqual(CellViewModel.TWO_VALUE_SIZE_DEFAULT, model.Cells[44].FontSize);
        }

        [TestMethod]
        public async Task TestDetailsSingleValueMultipleSquares()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(HARD_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.SolveCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[6].ClickCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[44].ClickCommand as DelegateCommand).ExecuteAsync();

            Assert.AreEqual(3, model.Cells[43].ValueAsInt);
            Assert.AreEqual(CellViewModel.TWO_VALUE_SIZE_DEFAULT, model.Cells[43].FontSize);
            Assert.AreEqual(Brushes.LightGreen, model.Cells[43].Background);
            Assert.AreEqual(3, model.Cells[44].ValueAsInt);
            Assert.AreEqual(CellViewModel.TWO_VALUE_SIZE_DEFAULT, model.Cells[44].FontSize);
            Assert.AreEqual(Brushes.LightGreen, model.Cells[44].Background);
        }

        [TestMethod]
        public async Task TestDetailsTechniqueAffectedCells()
        {
            var model = new SudokungfuViewModel(null, () => { }, null);
            SetSudoku(HARD_VALID, model);
            await (model.EnterCommand as DelegateCommand).ExecuteAsync();
            await (model.SolveCommand as DelegateCommand).ExecuteAsync();
            await (model.Cells[6].ClickCommand as DelegateCommand).ExecuteAsync();

            Assert.AreEqual(Brushes.LightSalmon, model.Cells[35].Background);
            Assert.AreEqual(Brushes.LightSalmon, model.Cells[34].Background);
            Assert.AreEqual(Brushes.LightSalmon, model.Cells[53].Background);
            Assert.AreEqual(Brushes.LightSalmon, model.Cells[52].Background);
        }

        private void SetSudoku(List<string> values, SudokungfuViewModel model)
        {
            for (int i = 0; i < Constants.CELL_COUNT; i++)
            {
                model.Cells[i].Value = values[i];
            }
        }

        private async Task AssertCommandStatusSequence(ICommand command, Func<Task> action, params bool[] expectedValues)
        {
            var results = new List<bool>();
            command.CanExecuteChanged += (sender, args) =>
            {
                results.Add(command.CanExecute(null));
            };

            await action();

            Assert.AreEqual(expectedValues.Length, results.Count);
            for (int i = 0; i < expectedValues.Length; i++)
            {
                Assert.AreEqual(expectedValues[i], results[i]);
            }
        }
    }
}
