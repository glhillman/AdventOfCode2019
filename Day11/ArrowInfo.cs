using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    public class ArrowInfo
    {
        public ArrowInfo(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Turn(int direction)
        {
            switch (direction)
            {
                case 0: // turn left
                    if (X == 0)
                    {
                        if (Y == 1)
                        {
                            X = -1;
                        }
                        else
                        {
                            X = 1;
                        }
                        Y = 0;
                    }
                    else
                    { 
                        if (X == 1)
                        {
                            Y = 1;
                        }
                        else
                        {
                            Y = -1;
                        }
                        X = 0;
                    }
                    break;
                case 1: // turn right;
                    if (X == 0)
                    {
                        if (Y == 1)
                        {
                            X = 1;
                        }
                        else
                        {
                            X = -1;
                        }
                        Y = 0;
                    }
                    else
                    {
                        if (X == 1)
                        {
                            Y = -1;
                        }
                        else
                        {
                            Y = 1;
                        }
                        X = 0;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unexpected turn direction");
            }
        }
        public int X { get; private set; }
        public int Y { get; private set; }

    }
}
