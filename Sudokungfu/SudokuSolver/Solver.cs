using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver
{
    using Extensions;
    using Model;
    using Sets;
    using System.Threading.Tasks;
    using Techniques;

    /// <summary>
    /// Class for solving Sudokus.
    /// </summary>
    public class Solver
    {
        private List<Cell> _cells;
        private List<Set> _sets;
        private List<FoundValue> _foundValues;

        /// <summary>
        /// Create a new <see cref="Solver"/>
        /// </summary>
        private Solver()
        {
            _cells = new List<Cell>();
            _sets = new List<Set>();
            _foundValues = new List<FoundValue>();

            for (int i = 0; i < Constants.CELL_COUNT; i++)
            {
                _cells.Add(new Cell(i));
            }

            for (int i = 0; i < Constants.SET_SIZE; i++)
            {
                _sets.Add(new Row(_cells, i));
                _sets.Add(new Column(_cells, i));
                _sets.Add(new Box(_cells, i));
            }
        }

        /// <summary>
        /// Solves a Sudoku.
        /// </summary>
        /// <param name="values">The intial values in the Sudoku.</param>
        /// <returns>The result.</returns>
        public static async Task<IEnumerable<ISudokuModel>> Solve(IEnumerable<int> values)
        {
            if (values.Count() != Constants.CELL_COUNT || values.Any(v => !v.IsSudokuValue()))
            {
                throw new ArgumentException("values: Must contains 81 valid values.");
            }

            var sudokuSolver = new Solver();
            return sudokuSolver.SolveInternal(values);
        }

        /// <summary>
        /// Solves a Sudoku.
        /// </summary>
        /// <param name="values">The intial values in the Sudoku.</param>
        /// <returns>The result.</returns>
        private IEnumerable<ISudokuModel> SolveInternal(IEnumerable<int> values)
        {
            // Insert the intial values.
            foreach (var cell in _cells)
            {
                if (values.ElementAt(cell.Index) != 0)
                {
                    var foundValue = FoundValue.CreateGivenValue(cell.Index, values.ElementAt(cell.Index));
                    InsertValue(foundValue);
                }
            }

            // The main loop for finding values in the Sudoku.
            while (_foundValues.Count < Constants.CELL_COUNT)
            {
                AdvancedTechniqueManager.ApplyAdvancedTechniques(_cells, _sets);

                var foundValue = FindValue();
                if (foundValue == null)
                {
                    return null;
                }
                else
                {
                    InsertValue(foundValue);
                }
            }

            return _foundValues;
        }

        /// <summary>
        /// Check all sets to see if a value has been found.
        /// </summary>
        /// <returns>The found value.</returns>
        private FoundValue FindValue()
        {
            var foundValuesFromSets = _sets
                   .SelectMany(s => s
                       .PossibleSpots
                       .Where(v => v.Value.Count() == 1)
                       .Select(v => FoundValue.CreateFoundInSetValue(v.Value.First(), v.Key, s)))
                    .OrderBy(v => v.TechniqueComplexity)
                    .ThenBy(v => v.TechniqueCount);

            var foundOnlyPossibleValues = _cells
                .Where(c => c.PossibleValues.Count() == 1)
                .Select(c => FoundValue.CreateOnlyPossiblValue(c, c.PossibleValues.First()));

            var foundValues = foundValuesFromSets.Zipper(foundOnlyPossibleValues);
            return foundValues.FirstOrDefault();
        }

        /// <summary>
        /// Inserts a value into a cell.
        /// </summary>
        /// <param name="value">Found value to insert into the Sudoku</param>
        private void InsertValue(FoundValue value)
        {
            _cells[value.Index].InsertValue(value.Value);
            _foundValues.Add(value);
        }
    }
}
