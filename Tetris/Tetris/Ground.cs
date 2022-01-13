using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class Ground
    {
        public List<Coordinate> Coordinates { get; private set; } = new List<Coordinate>();
        public void AddCoordinates(params Coordinate[] elems)
        {
            Coordinates.AddRange(elems);
        }
    }
}
