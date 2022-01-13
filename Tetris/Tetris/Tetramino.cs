using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
    internal class Tetramino
    {
        private List<Coordinate> coordinates = new List<Coordinate>();

        public List<Coordinate> AllCoordinates {
            get {
                return new List<Coordinate>(coordinates) {
                    Center
                };
            }
            set => coordinates = value;
        }
        public Coordinate Center { get; set; }
        public List<Coordinate> Lowers => AllCoordinates.GroupBy(c => c.X)
                                                        .Select(group => new Coordinate((group.Key, group.Max(c => c.Y))))
                                                        .ToList();

        public Coordinate Lowest => Lowers.OrderByDescending(c => c.Y).First();

        public MovableYState MovableOnY {
            get {
                return Lowers.All(l => !Settings.Ground.Coordinates.Contains(l) && l.Y + 1 < Settings.Height)
                    ? MovableYState.GroundUnreached
                    : MovableYState.GroundReached;
            }
        }
        public MovableXState MovableOnX {
            get {
                if (AllCoordinates.Any(coord => Settings.Ground.Coordinates.Contains(new Coordinate((coord.X + 1, coord.Y))) || coord.X + 1 >= Settings.Width))
                    return MovableXState.RightObstacle;
                else if (AllCoordinates.Any(coord => Settings.Ground.Coordinates.Contains(new Coordinate((coord.X - 1, coord.Y))) || coord.X - 1 < 0))
                    return MovableXState.LeftObstacle;
                else
                    return MovableXState.NoObstacle;
            }
        }

        public Tetramino((int x, int y) centerCoords, TetraminoForm form)
        {
            Center = new Coordinate(centerCoords);
            coordinates = GetCoordsFromCenter(centerCoords, form);
        }
        private List<Coordinate> GetCoordsFromCenter((int x, int y) centerCoords, TetraminoForm form)
        {
            Coordinate center = new Coordinate(centerCoords);
            List<Coordinate> result = new List<Coordinate>();
            switch (form)
            {
                case TetraminoForm.SnakeL:
                    result.Add(center.Moved((-1, +0)));
                    result.Add(center.Moved((-1, -1)));
                    result.Add(center.Moved((+0, +1)));
                    break;
                case TetraminoForm.SnakeR:
                    result.Add(center.Moved((+0, +1)));
                    result.Add(center.Moved((+1, +0)));
                    result.Add(center.Moved((+1, -1)));
                    break;
                case TetraminoForm.Line:
                    result.Add(center.Moved((+0, -1)));
                    result.Add(center.Moved((+0, +1)));
                    result.Add(center.Moved((+0, +2)));
                    break;
                case TetraminoForm.J:
                    result.Add(center.Moved((-1, +0)));
                    result.Add(center.Moved((+0, -1)));
                    result.Add(center.Moved((+0, -2)));
                    break;
                case TetraminoForm.L:
                    result.Add(center.Moved((+1, +0)));
                    result.Add(center.Moved((+0, -1)));
                    result.Add(center.Moved((+0, -2)));
                    break;
                case TetraminoForm.Square:
                    result.Add(center.Moved((-1, +0)));
                    result.Add(center.Moved((+0, +1)));
                    result.Add(center.Moved((-1, +1)));
                    break;
            }
            return result;

        }
        public void Rotate()
        {
            List<Coordinate> updatedCoords = new List<Coordinate>();
            List<Coordinate> withoutCenter = AllCoordinates.Except(new List<Coordinate>() { Center }).ToList();
            updatedCoords.AddRange(withoutCenter);
            foreach (Coordinate item in updatedCoords)
            {
                int x = Center.X - item.Y + Center.Y;
                int y = Center.Y + item.X - Center.X;
                item.X = x;
                item.Y = y;
            }
            if (!updatedCoords.Any(c => Settings.Ground.Coordinates.Contains(c) || c.Y >= Settings.Height || c.X < 0 || c.X >= Settings.Width))
                AllCoordinates = updatedCoords;
        }
        public void Move(Direction dir = Direction.Down)
        {
            if (dir == Direction.Down && MovableOnY == MovableYState.GroundUnreached)
                AllCoordinates.ForEach(coord => coord.Y++);
            else if (dir == Direction.Left && MovableOnX != MovableXState.LeftObstacle)
                AllCoordinates.ForEach(coord => coord.X--);
            else if (dir == Direction.Right && MovableOnX != MovableXState.RightObstacle)
                AllCoordinates.ForEach(coord => coord.X++);
        }
    }
    enum MovableXState
    {
        LeftObstacle,
        RightObstacle,
        NoObstacle
    }
    enum MovableYState
    {
        GroundReached,
        GroundUnreached
    }
    enum Direction
    {
        Down,
        Left,
        Right
    }
    enum TetraminoForm
    {
        SnakeL,
        SnakeR,
        Line,
        J,
        L,
        Square
    }
}