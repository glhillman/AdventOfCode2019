using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day03
{
    public class Day03
    {
        List<Point> wire1 = new List<Point>();
        List<Point> wire2 = new List<Point>();
        PointComparer pointComparer = new PointComparer();

        public void Puzzel1()
        {
            List<Point> duplicates = wire1.Intersect(wire2, new PointComparer()).ToList();

            int minDistance = duplicates.Min(p => Math.Abs(p.X) + Math.Abs(p.Y));

            Console.WriteLine("Closest Manhattan distance = {0}", minDistance);
        }

        public void Puzzel2()
        {
            List<Point> duplicates = wire1.Intersect(wire2, new PointComparer()).ToList();
            int minimumSteps = int.MaxValue;

            foreach (Point targetPoint in duplicates)
            {
                int steps1 = TraversePath(wire1, targetPoint);
                int steps2 = TraversePath(wire2, targetPoint);

                minimumSteps = Math.Min(steps1 + steps2, minimumSteps);
            }

            Console.WriteLine("Minimum steps = {0}", minimumSteps);
        }

        private int TraversePath(List<Point> wire, Point targetPoint)
        {
            int steps = StepsBetweenPoints(new Point(0,0), wire[0]);
            int index = 0;

            while (index < wire.Count - 1)
            {
                steps += StepsBetweenPoints(wire[index], wire[index + 1]);
                if (pointComparer.Equals(wire[index + 1], targetPoint))
                {
                    break;
                }
                else
                {
                    index++;
                }
            }

            return steps;
        }

        private int StepsBetweenPoints(Point start, Point end)
        {
            int xSteps = Math.Abs(start.X - end.X);
            int ySteps = Math.Abs(start.Y - end.Y);

            return xSteps + ySteps;
        }

        public bool InitializeWires()
        {
            bool loadOK = false;

            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\Day03WireInput.txt";

            if (File.Exists(inputFile))
            {
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(inputFile);
                if ((line = file.ReadLine()) != null)
                {
                    LoadWire(wire1, line);
                    if ((line = file.ReadLine()) != null)
                    {
                        LoadWire(wire2, line);
                        loadOK = true;
                    }
                }

                file.Close();
            }

            return loadOK;
        }

        private void LoadWire(List<Point> wire, string pointString)
        {
            string[] points = pointString.Split(',');
            Point previousPoint = new Point(0, 0);

            foreach (string point in points)
            {
                List<Point> pointLine = previousPoint.PointLine(point);
                wire.AddRange(pointLine);
                previousPoint = new Point(pointLine.Last());
            }
            wire.Add(previousPoint);
        }

    }
}
