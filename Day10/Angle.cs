using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day10
{
    public class Angle
    {
        public Angle(Point anchor, Point target)
        {
            Unique = false;
            Anchor = anchor;
            Target = target;

            StepY = target.Y - anchor.Y;
            StepX = target.X - anchor.X;
            if (StepX != 0.0)
            {
                Slope = StepY / StepX;
                if (StepY == 0)
                {
                    Quadrant = StepX < 0 ? 3 : 7; // straight left or right
                }
                else if (StepX > 0)
                {
                    if (StepY > 0)
                    {
                        Quadrant = 4;
                    }
                    else
                    {
                        Quadrant = 2;
                    }
                }
                else // StepX < 0
                {
                    if (StepY > 0)
                    {
                        Quadrant = 6;
                    }
                    else
                    {
                        Quadrant = 8;
                    }
                }
            }
            else
            {
                Slope = double.MaxValue; // treated as Infinity for comparison purposes
                Quadrant = StepY < 0 ? 1 : 5; // straight up or down
            }
            RayLength = Math.Sqrt(StepX * StepX + StepY * StepY);
        }

        public bool Unique { get; set; } // for debugging
        public Point Anchor { get; private set; }
        public Point Target { get; private set; }
        public double Slope { get; private set; }
        public int Quadrant { get; private set; }
        public double StepX { get; private set; }
        public double StepY { get; private set; }
        public double RayLength { get; private set; }

        public override string ToString()
        {
            string slope = (Slope.CompareTo(double.MaxValue) == 0) ? "INF" : Slope.ToString();
            return string.Format("Quadrant: {0}, Slope: {1} From {2} to {3}, RayLength: {4}, {5}", Quadrant, slope, Anchor, Target, RayLength, Unique ? "Unique" : "");
        }
    }
}
