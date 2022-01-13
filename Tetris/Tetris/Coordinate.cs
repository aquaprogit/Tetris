using System;

namespace Tetris
{
    internal class Coordinate : IEquatable<Coordinate>
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
        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }
        public override string ToString()
        {
            return $"({X}, {Y})";
        }
        public bool Equals(Coordinate other)
        {
            return other != null &&
                   X == other.X &&
                   Y == other.Y;
        }
        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}
