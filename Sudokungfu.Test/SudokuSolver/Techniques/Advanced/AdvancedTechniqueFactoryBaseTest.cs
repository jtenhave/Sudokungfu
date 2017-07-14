using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.Test.SudokuSolver.Techniques.Advanced
{
    using Sudokungfu.SudokuSolver;
    using Sudokungfu.SudokuSolver.Sets;
    using Sudokungfu.SudokuSolver.Techniques;
    using Sudokungfu.SudokuSolver.Techniques.Advanced;

    public class TestAdvancedTechniqueFactory : AdvancedTechniqueFactoryBase
    {
        private Func<IEnumerable<Technique>> _applyInternal;

        public TestAdvancedTechniqueFactory(IEnumerable<Cell> cells, IEnumerable<Set> sets) : base(cells, sets)
        {
        }

        public void SetApplyInternal(Func<IEnumerable<Technique>> applyInternal)
        {
            _applyInternal = applyInternal;

        }

        protected override IEnumerable<Technique> ApplyInternal()
        {
            return _applyInternal();
        }
    }

    /// <summary>
    /// Test class for <see cref="AdvancedTechniqueFactoryBase"/>.
    /// </summary>
    [TestClass]
    public class AdvancedTechniqueFactoryBaseTest: BaseTest
    {
        [TestMethod]
        public void TestReturnsFalseIfNothingApplied()
        {
            var factory = new TestAdvancedTechniqueFactory(null, null);
            factory.SetApplyInternal(() => {
                return Enumerable.Empty<Technique>();
            });

            Assert.IsFalse(factory.Apply());
        }

        [TestMethod]
        public void TestReturnTrueIfAtLeastOneApplied()
        {
            var factory = new TestAdvancedTechniqueFactory(null, null);
            var run = false;
            factory.SetApplyInternal(() => {
                if (!run)
                {
                    run = true;
                    return new List<Technique>() { new Technique() };
                }

                return Enumerable.Empty<Technique>();
            });

            Assert.IsTrue(factory.Apply());
        }

        [TestMethod]
        public void TestAppliesTechniques()
        {
            var factory = new TestAdvancedTechniqueFactory(null, null);
            var run = false;
            var cell = new Cell(0);
            var value = 1;
            var technique = new Technique();
            technique.AffectedCells.Add(cell);
            technique.Values.Add(value);
            technique.Complexity = 0;

            factory.SetApplyInternal(() => {
                if (!run)
                {
                    run = true;
                    return new List<Technique>() { technique };
                }

                return Enumerable.Empty<Technique>();
            });

            factory.Apply();

            Assert.IsFalse(cell.PossibleValues.Contains(value));
        }

        [TestMethod]
        public void TestAppliesTechniquesOnlyOnce()
        {
            var factory = new TestAdvancedTechniqueFactory(null, null);
            var cell = new Cell(0);
            var value = 1;
            var technique = new Technique();
            technique.AffectedCells.Add(cell);
            technique.Values.Add(value);
            technique.Complexity = 0;

            factory.SetApplyInternal(() => {
                return new List<Technique>() { technique };
            });

            Assert.IsTrue(factory.Apply());
        }
    }
}
