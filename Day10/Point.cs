using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day10
{
    public class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool IsEquivalent(Point other)
        {
            return other.X == X && other.Y == Y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public override string ToString()
        {
            return string.Format("x,y:{0},{1}", X, Y);
        }
    }
}
