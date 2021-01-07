using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day10
{
    public class AnchorPointWithHits
    {
        public AnchorPointWithHits(Point anchorPoint)
        {
            AnchorPoint = anchorPoint;
            Hits = new List<Angle>();
            NVisible = 0;
        }

        public Point AnchorPoint { get; private set; }
        public List<Angle> Hits { get; private set; }

        public void AddHit(Angle angle)
        {
            Hits.Add(angle);
        }

        public void CalculateVisible()
        {
            if (Hits.Count > 0)
            {
                Hits.Sort(CompareAngles);

                int visible = 1;
                Hits[0].Unique = true;
                double currentSlope = Hits[0].Slope;
                double currentRay = Hits[0].RayLength;
                for (int i = 1; i < Hits.Count; i++)
                {
                    Hits[i].Unique = false;
                    if (EssentiallyEqual(Hits[i].Slope, currentSlope, 10) == false)
                    {
                        visible++;
                        Hits[i].Unique = true;
                        currentSlope = Hits[i].Slope;
                        currentRay = Hits[i].RayLength;
                    }
                }

                NVisible = visible;
            }
            else
            {
                NVisible = 0;
            }
        }

        public Angle BlowEmUp(int nthToReturn)
        {
            Angle angleToReturn = null;
 
            int i = 0;
            foreach (Angle angle in Hits)
            {
                if (angle.Unique)
                {
                    if (++i == nthToReturn)
                    {
                        angleToReturn = angle;
                        break;
                    }
                }
            }

            return angleToReturn;
        }

        private bool EssentiallyEqual(double value1, double value2, int units)
        {
            long lValue1 = BitConverter.DoubleToInt64Bits(value1);
            long lValue2 = BitConverter.DoubleToInt64Bits(value2);

            // If the signs are different, return false except for +0 and -0.
            if ((lValue1 >> 63) != (lValue2 >> 63))
            {
                if (value1 == value2)
                    return true;

                return false;
            }

            long diff = Math.Abs(lValue1 - lValue2);

            if (diff <= (long)units)
                return true;

            return false;
        }

        public int NVisible { get; private set; }

        private int CompareAngles(Angle x, Angle y)
        {
            int retval = 0;

            if (x.Quadrant != y.Quadrant)
            {
                retval = x.Quadrant - y.Quadrant;
            }
            else if (EssentiallyEqual(x.Slope, y.Slope, 10) == false)
            {
                retval = x.Slope > y.Slope ? 1 : -1;                
            }
            else
            {
                retval = x.RayLength > y.RayLength ? 1 : -1;
            }

            return retval;
        }
    }
}
