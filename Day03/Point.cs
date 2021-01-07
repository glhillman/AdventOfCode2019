using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day03
{
    public class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point(Point previousPoint)
        {
            X = previousPoint.X;
            Y = previousPoint.Y;
        }

        public Point OffsetPoint(char direction, int offset)
        {
            Point newPoint = null;

            switch (direction)
            {
                case 'U':
                    newPoint = new Point(this.X, this.Y + offset);
                    break;
                case 'D':
                    newPoint = new Point(this.X, this.Y - offset);
                    break;
                case 'R':
                    newPoint = new Point(this.X + offset, this.Y);
                    break;
                case 'L':
                    newPoint = new Point(this.X - offset, this.Y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unexpected offset");
            }

            return newPoint;
        }

        public List<Point> PointLine(string offset)
        {
            int value = int.Parse(offset.Substring(1));
            char direction = offset[0];
            List<Point> pointLine = new List<Point>();

            for (int i = 1; i <= value; i++)
            {
                pointLine.Add(OffsetPoint(direction, i));
            }

            return pointLine;
        }


        public int X { get; private set; }
        public int Y { get; private set; }

        public override string ToString()
        {
            return "X: " + this.X + " Y: " + this.Y;
        }
    }


    public class PointComparer : IEqualityComparer<Point>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(Point x, Point y)
        {
            bool isEqual = false;

            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y))
            {
                isEqual = true;
            }

            //Check whether any of the compared objects is null.
            if (isEqual == false && !Object.ReferenceEquals(x, null) && !Object.ReferenceEquals(y, null))
            {
                isEqual = x.X == y.X && x.Y == y.Y;
            }

            return isEqual;
        }

        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(Point point)
        {
            int hashCode = 0;

            //Check whether the object is null
            if (!Object.ReferenceEquals(point, null))
            {
                hashCode = point.X.GetHashCode() ^ point.Y.GetHashCode();
            }

            return hashCode;
        }
    }

}
