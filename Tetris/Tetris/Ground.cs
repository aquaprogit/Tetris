using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Tetris
{
    internal class Ground
    {
        private List<Coordinate> coordinates = new List<Coordinate>();
        public void AddCoordinates(params Coordinate[] elems)
        {
            coordinates.AddRange(elems);
            coordinates = coordinates.Distinct().ToList();
            RemoveFilledRows();
        }

        public List<Coordinate> GetCoordinates()
        {
            return coordinates;
        }
        private void RemoveFilledRows()
        {
            coordinates = coordinates.OrderBy(x => x.X).ToList().OrderBy(x => x.Y).ToList();
            var rowsToRemove = coordinates.GroupBy(coord => coord.Y)
                                          .Select(group => new { Index = group.Key, Count = group.Count() })
                                          .Where(row => row.Count == 11)
                                          .OrderBy(group => group.Index);
            List<int> indexes = rowsToRemove.Select(row => row.Index).ToList();
            int rowsCount = indexes.Count();
            if (rowsCount > 0)
            {
                int toppestIndexToRemove = indexes.First();
                coordinates.RemoveAll(coord => indexes.Any(c => c == coord.Y));
                coordinates.Where(coord => coord.Y < toppestIndexToRemove).ToList().ForEach(coord => coord.Y += rowsCount);
            }
        }

    }
}
