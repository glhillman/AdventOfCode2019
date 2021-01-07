using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day10
{
    public class Day10
    {
        char[,] _data;

        public Day10()
        {
            LoadData();
            HashPoints = new List<Point>();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (_data[x, y] == '#')
                    {
                        HashPoints.Add(new Point(x, y));
                    }
                }
            }

            AnchorsWithHits = new List<AnchorPointWithHits>();

            foreach (Point anchor in HashPoints)
            {
                AnchorPointWithHits anchorPoint = new AnchorPointWithHits(anchor);

                foreach (Point target in HashPoints)
                {
                    if (anchor.IsEquivalent(target) == false)
                    {
                        anchorPoint.AddHit(new Angle(anchor, target));        
                    }
                }

                AnchorsWithHits.Add(anchorPoint);
            }
        }

        public void Part1()
        {
            foreach (AnchorPointWithHits anchor in AnchorsWithHits)
            {
                anchor.CalculateVisible();
            }

            int max = AnchorsWithHits.Max(a => a.NVisible);

            Console.WriteLine("Part1: {0}", max);
        }

        public void Part2()
        {
            foreach (AnchorPointWithHits anchor in AnchorsWithHits)
            {
                anchor.CalculateVisible();
            }

            int max = AnchorsWithHits.Max(a => a.NVisible);
            AnchorPointWithHits best = AnchorsWithHits.FirstOrDefault(a => a.NVisible == max);
            Angle nth = best.BlowEmUp(200);

            int rslt = nth.Target.X * 100 + nth.Target.Y;

            Console.WriteLine("Part1: {0}", rslt);
        }

        private int Width { get; set; }
        private int Height { get; set; }

        private List<Point> HashPoints { get; set; }
        private List<AnchorPointWithHits> AnchorsWithHits { get; set; }

        private void LoadData()
        {

            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\input.txt";

            if (File.Exists(inputFile))
            {
                string line;
                Height = 0;
                // two passes through data to figure out the size - first test, second load
                StreamReader file = new StreamReader(inputFile);
                while ((line = file.ReadLine()) != null)
                {
                    Width = line.Length; // redundant, but toss-up with test for already done
                    Height++;
                }
                file.Close();

                _data = new char[Width, Height]; 
                file = new StreamReader(inputFile);
                int y = 0;
                while ((line = file.ReadLine()) != null)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        _data[x, y] = line[x];
                    }
                    y++;
                }
                file.Close();
            }
        }
    }
}
