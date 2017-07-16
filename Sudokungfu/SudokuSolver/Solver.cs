using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sudokungfu.SudokuSolver
{
    using Extensions;
    using Model;
    using Sets;
    using Techniques;

    /// <summary>
    /// Class for solving Sudokus.
    /// </summary>
    public class Solver
    {
        private List<Cell> _cells;
        private List<Set> _sets;
        private List<ISudokuModel> _foundValues;

        /// <summary>
        /// Create a new <see cref="Solver"/>
        /// </summary>
        private Solver()
        {
            _cells = new List<Cell>();
            _sets = new List<Set>();
            _foundValues = new List<ISudokuModel>();

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
        public static async Task<List<ISudokuModel>> Solve(IEnumerable<int> values)
        {
            return await Task.Run(() =>
            {
                var sudokuSolver = new Solver();
                return sudokuSolver.SolveInternal(values);
            });
        }

        /// <summary>
        /// Solves a Sudoku.
        /// </summary>
        /// <param name="values">The intial values in the Sudoku.</param>
        /// <returns>The result.</returns>
        private List<ISudokuModel> SolveInternal(IEnumerable<int> values)
        {
            // Insert the intial values.
            foreach (var cell in _cells)
            {
                if (values.ElementAt(cell.Index) != 0)
                {
                    var foundValue = CreateGivenValue(cell, values.ElementAt(cell.Index));
                    InsertValue(foundValue);
                }
            }

            _cells.ForEach(c => c.ResetAppliedTechniques());

            // The main loop for finding values in the Sudoku.
            while (_foundValues.Count < Constants.CELL_COUNT)
            {
                var manager = new AdvancedTechniqueManager(_cells, _sets);
                while (true)
                {
                    var foundValue = FindValue();
                    if (foundValue != null)
                    {
                        InsertValue(foundValue);
                        _cells.ForEach(c => c.ResetAppliedTechniques());
                        break;
                    }
                    else if (!manager.HasNext())
                    {
                        return null;
                    }
                    else
                    {
                        manager.ApplyNext();
                    }
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
            IEnumerable<FoundValue> foundValuesFromSets = _sets
                   .SelectMany(s => s
                       .PossibleSpots
                       .Where(v => v.Value.Count() == 1)
                       .Select(v => CreateFoundInSetValue(v.Value.First(), v.Key, s)));

            IEnumerable<FoundValue> foundOnlyPossibleValues = _cells
                .Where(c => c.PossibleValues.Count() == 1)
                .Select(c => CreateOnlyPossibleValue(c, c.PossibleValues.First()));

            var foundValues = foundValuesFromSets
                .Concat(foundOnlyPossibleValues)
                .OrderBy(v => v.Complexity)
                .ThenBy(v => v.Details.Count());

            return foundValues.FirstOrDefault();
        }

        /// <summary>
        /// Inserts a value into a cell.
        /// </summary>
        /// <param name="value">Found value to insert into the Sudoku</param>
        private void InsertValue(FoundValue value)
        {
            value.Cell.InsertValue(value);
            _foundValues.Add(value);
        }

        private FoundValue CreateGivenValue(Cell cell, int value)
        {
            var foundValue = new FoundValue(cell, value);
            foundValue.CellValueMap.Add(cell, value.ToEnumerable());
            return foundValue;
        }

        private FoundValue CreateFoundInSetValue(Cell cell, int value, Set set)
        {
            var foundValue = new FoundValue(cell, value);
            var techniques = set.FindMinTechniques(cell.ToEnumerable(), value);
            foundValue.Techniques.AddRange(techniques);

            foreach (var c in set.Cells)
            {
                foundValue.CellValueMap[c] = Enumerable.Empty<int>();
            }

            foundValue.CellValueMap[cell] = value.ToEnumerable();



            return foundValue;
        }

        private FoundValue CreateOnlyPossibleValue(Cell cell, int value)
        {
            var foundValue = new FoundValue(cell, value);

            foundValue.Techniques.AddRange(Constants.ALL_VALUES.Except(value)
                .Where(v => cell.Techniques.ContainsKey(v))
                .Select(v => cell.Techniques[v].First()));

            foundValue.CellValueMap[cell] = value.ToEnumerable();
            foundValue.Complexity = foundValue.Complexity + 1;

            return foundValue;
        }
    }
}
