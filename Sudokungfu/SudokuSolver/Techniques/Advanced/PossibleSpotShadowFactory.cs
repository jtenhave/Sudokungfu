using System.Collections.Generic;
using System.Linq;

namespace Sudokungfu.SudokuSolver.Techniques.Advanced
{
    using Extensions;
    using Sets;

    /// <summary>
    /// Advanced technique factory for the Possible Spot Shadow technique.
    /// </summary>
    public class PossibleSpotShadowFactory : AdvancedTechniqueFactoryBase
    {
        public const int COMPLEXITY = 4;

        /// <summary>
        /// Creates a new <see cref="PossibleSpotShadowFactory"/>.
        /// </summary>
        /// <param name="cells">Cells in the Sudoku.</param>
        /// <param name="sets">Sets in the Sudoku.</param>
        public PossibleSpotShadowFactory(IEnumerable<Cell> cells, IEnumerable<Set> sets) :base(cells, sets) 
        {

        }

        protected override IEnumerable<Technique> ApplyInternal()
        {
            var techniques = new List<Technique>();
            foreach (var sourceSet in _sets)
            {
                var possibleSpotSetsDic = sourceSet.PossibleSpots.Where(kvp => kvp.Value.Count() == 2);
                foreach (var possibleSpots in possibleSpotSetsDic)
                {
                    var value = possibleSpots.Key;
                    var cellsA = possibleSpots.Value;

                    foreach (var shadowSet in _sets.Except(sourceSet))
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
                            affectedCells = _cells.Where(c => rows.Contains(Row(c)))
                                .Except(sourceSet.Cells)
                                .Except(shadowSet.Cells);
                        }

                        if (columns.Distinct().Count() == 2)
                        {
                            affectedCells = _cells.Where(c => columns.Contains(Column(c)))
                                .Except(sourceSet.Cells)
                                .Except(shadowSet.Cells);
                        }

                        if (affectedCells != null && !techniques.Any(t => t.AffectedCells.SetEqual(affectedCells)))
                        {
                            var technique = new Technique();
                            technique.Techniques.AddRange(sourceSet.FindMinTechniques(cellsA, value));
                            technique.Techniques.AddRange(shadowSet.FindMinTechniques(cellsB, value));
                            technique.Values.Add(value);
                            technique.AffectedCells.AddRange(affectedCells);
                            sourceSet.Cells.Concat(shadowSet.Cells).ForEach(c => technique.CellValueMap[c] = Enumerable.Empty<int>());
                            cellsA.Concat(cellsB).ForEach(c => technique.CellValueMap[c] = value.ToEnumerable());
                            technique.Complexity = COMPLEXITY;
                            techniques.Add(technique);

                            yield return technique;
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
