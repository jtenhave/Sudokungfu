using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver
{
    using Extensions;
    using Sets;

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
        public static SolveResult Solve(int[] values)
        {
            var sudokuSolver = new Solver();
            return sudokuSolver.SolveInternal(values);
        }

        /// <summary>
        /// Solves a Sudoku.
        /// </summary>
        /// <param name="values">The intial values in the Sudoku.</param>
        /// <returns>The result.</returns>
        private SolveResult SolveInternal(int[] values)
        {
            try
            {
                if (values.Length != Constants.CELL_COUNT || values.Any(v => !v.IsSudokuValue()))
                {
                    throw new ArgumentException("values: Must contains 81 valid values.");
                }

                // Insert the intial values.
                foreach (var cell in _cells)
                {
                    if (values[cell.Index] != 0)
                    {
                        var foundValue = InsertValue(cell, values[cell.Index]);
                        foundValue.Method = FoundValueMethod.CreateGivenMethod();
                        _foundValues.Add(foundValue);
                    }
                }

                // The main loop for finding values in the Sudoku.
                while (_foundValues.Count < Constants.CELL_COUNT)
                {
                    var foundValue = FindValue();
                    if (foundValue == null)
                    {
                        return new SolveResult()
                        {
                            Type = SudokuResultType.INVALID
                        };
                    }
                    else
                    {
                        _foundValues.Add(foundValue);
                    }
                }

                return new SolveResult()
                {
                    Type = SudokuResultType.SUCCESS,
                    FoundValues = _foundValues
                };

            }
            catch (Exception ex)
            {
                return new SolveResult()
                {
                    Type = SudokuResultType.ERROR,
                    Error = $"{ex.GetType().ToString()}:{ex.Message}"
                };
            }
        }

        /// <summary>
        /// Check all sets to see if a value has been found.
        /// </summary>
        /// <returns>The found value.</returns>
        private FoundValue FindValue()
        {
            foreach (var set in _sets)
            {
                var foundValues = set.GetValuePossibleSpots().Where(v => v.Value.Count() == 1);
                if (foundValues.Any())
                {
                    var cell = foundValues.First().Value.First();
                    var value = foundValues.First().Key;
                    return InsertValue(cell, value);
                }
            }

            return null;
        }

        /// <summary>
        /// Inserts a value into a cell.
        /// </summary>
        /// <param name="cell">The cell to insert in.</param>
        /// <param name="value">The value to insert.</param>
        /// <returns>The found value.</returns>
        private FoundValue InsertValue(Cell cell, int value)
        {
            cell.InsertValue(value);
            return new FoundValue()
            {
                Index = cell.Index,
                Value = value
            };
        }
    }
}
