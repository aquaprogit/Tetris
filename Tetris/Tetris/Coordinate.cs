using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinate((int x, int y) coords)
        {
            X = coords.x;
            Y = coords.y;
        }
        public Coordinate Moved((int x, int y) offset)
        {
            return new Coordinate((X + offset.x, Y + offset.y));
        }
        public override bool Equals(object obj)
        {
            return obj is Coordinate coordinate && coordinate.X == X && coordinate.Y == Y;
        }
        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
