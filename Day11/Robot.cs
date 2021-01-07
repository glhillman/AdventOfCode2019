using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Channels;
using AoCIntCode;

namespace Day11
{
    public class Robot
    {
        public Robot(IntCode intCode, int[,] grid, int startColor)
        {
            MaxX = int.MinValue;
            MaxY = int.MinValue;
            MinX = int.MaxValue;
            MinY = int.MaxValue;

            Intcode = intCode;
            Grid = grid;

            XSize = grid.GetLength(0);
            YSize = grid.GetLength(1);

            X = XSize / 2;
            Y = X;
            Grid[X, Y] = startColor;
            Arrow = new ArrowInfo(0, 1); // pointing up

            ChangeGrid = new bool[XSize, YSize];
            for (int i = 0; i < XSize; i++)
                for (int j = 0; j < YSize; j++)
                    ChangeGrid[i, j] = false;
        }

        public void Run()
        {
            int count = 0;
            Intcode.WriteInputChannel(Grid[X, Y]);
            while (Intcode.Finished == false)
            {
                int value = (int)Intcode.ReadOutputChannel();
                int direction = (int)Intcode.ReadOutputChannel();
                if (++count % 1000 == 0)
                {
                    // the Sleep call at the bottom prevents channel crashes, but causes the code to run slowly
                    // this just keeps me informed that the system is still running...
                    Console.WriteLine("{0} Value: {1}, Direction: {2}, X,Y: {3},{4}", count, value, direction, X, Y);
                }
                Grid[X, Y] = value;
                ChangeGrid[X, Y] = true;
                Arrow.Turn(direction);
                X += Arrow.X;
                Y += Arrow.Y;

                MaxX = Math.Max(MaxX, X);
                MaxY = Math.Max(MaxY, Y);
                MinX = Math.Min(MinX, X);
                MinY = Math.Min(MinY, Y);

                Intcode.WriteInputChannel(Grid[X, Y]);
                System.Threading.Thread.Sleep(1);
            }

            int rslt = ChangeCount;
        }

        public int ChangeCount
        {
            get
            {
                int count = 0;

                for (int i = 0; i < XSize; i++)
                {
                    for (int j = 0; j < YSize; j++)
                    {
                        count += ChangeGrid[i, j] ? 1 : 0;
                    }
                }

                return count;
            }
        }
        public int[,] Grid { get; set; }
        private bool[,] ChangeGrid { get; set; }
        public int NChanges { get; private set; }
        private int X { get; set; }
        private int Y { get; set; }
        public int MaxX { get; set; }
        public int MinX { get; set; }
        public int MaxY { get; set; }
        public int MinY { get; set; }
        private int XSize { get; set; }
        private int YSize { get; set; }
        private ArrowInfo Arrow { get; set; }
        private IntCode Intcode { get; set; }
    }
}
