using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Techniques
{
    using Extensions;
    using Model;
    using Sets;

    /// <summary>
    /// Class that represents the 'Possible Spot Shadow' technique.
    /// </summary>
    public class PossibleSpotShadowTechnique : TechniqueBase
    {
        private const int COMPLEXITY = 4;
        private readonly IEnumerable<ISudokuModel> _techniques;

        /// <summary>
        /// Details that make up this technique.
        /// </summary>
        public override IEnumerable<ISudokuModel> Details
        {
            get
            {
                return _techniques;
            }
        }

        /// <summary>
        /// Creates a new <see cref="PossibleSpotShadowTechnique"/>.
        /// </summary>
        /// <param name="value">Possible value.</param>
        /// <param name="cellsA">First set of possible spots.</param>
        /// <param name="cellsB">Second set of possible spots.</param>
        /// <param name="setA">First set with possible spots.</param>
        /// <param name="setB">Second set with possible spots.</param>
        /// <param name="affectedCells">Cells affected by the technique.</param>
        private PossibleSpotShadowTechnique(int value, IEnumerable<Cell> cellsA, IEnumerable<Cell> cellsB, Set setA, Set setB, IEnumerable<Cell> affectedCells) : base(COMPLEXITY)
        {
            _techniques = setA.FindMinTechniques(cellsA, value)
                .Concat(setB.FindMinTechniques(cellsB, value));

            foreach (var index in setA.Indexes.Concat(setB.Indexes))
            {
                _indexValueMap[index] = Enumerable.Empty<int>();
            }

            foreach (var index in cellsA.Concat(cellsB).Indexes())
            {
                _indexValueMap[index] = value.ToEnumerable();
            }

            _affectedIndexes.AddRange(affectedCells.Indexes());
        }

        /// <summary>
        /// Applies the technique to the Sudoku.
        /// </summary>
        /// <param name="cells">Cells in the Sudoku.</param>
        /// <param name="sets">Sets of cells in the Sudoku.</param>
        public static void Apply(IEnumerable<Cell> cells, IEnumerable<Set> sets)
        {
            var techniques = new List<PossibleSpotShadowTechnique>();
            foreach (var sourceSet in sets)
            {
                var possibleSpotSetsDic = sourceSet.PossibleSpots.Where(kvp => kvp.Value.Count() == 2);
                foreach (var possibleSpots in possibleSpotSetsDic)
                {
                    var value = possibleSpots.Key;
                    var cellsA = possibleSpots.Value;
                       
                    foreach (var shadowSet in sets.Except(sourceSet))
                    {
                        if (!shadowSet.PossibleSpots.ContainsKey(value))
                        {
                            continue;
                        }

                        var cellsB = shadowSet.PossibleSpots[value];
                        var allCells = cellsA.Concat(cellsB);
                        if (allCells.Distinct().Count() != 4)
                        {
                            continue;
                        }

                        var rows = new List<int>();
                        var columns = new List<int>();
                        foreach (var cell in allCells)
                        {
                            rows.Add(Row(cell));
                            columns.Add(Column(cell));
                        }

                        IEnumerable<Cell> affectedCells = null;
                        if (rows.Distinct().Count() == 2)
                        {
                            affectedCells = cells.Where(c => rows.Contains(Row(c)))
                                .Except(sourceSet.Cells)
                                .Except(shadowSet.Cells);
                        }

                        if (columns.Distinct().Count() == 2)
                        {
                            affectedCells = cells.Where(c => columns.Contains(Column(c)))
                                .Except(sourceSet.Cells)
                                .Except(shadowSet.Cells);
                        }

                        if (affectedCells != null && !techniques.Any(t => t.AffectedIndexes.SetEqual(affectedCells.Indexes())))
                        {
                            var technique = new PossibleSpotShadowTechnique(value, cellsA, cellsB, sourceSet, shadowSet, affectedCells);
                            techniques.Add(technique);

                            foreach (var cell in affectedCells)
                            {
                                cell.EliminatePossibleValue(value, technique);
                            }
                        }
                    }
                }
            }
        }

        private static int Row(Cell cell)
        {
            return cell.Index / Constants.SET_SIZE;
        }

        private static int Column(Cell cell)
        {
            return cell.Index % Constants.SET_SIZE;
        }
    }
}
