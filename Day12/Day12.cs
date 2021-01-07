using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    public class Day12
    {

        public Day12()
        {
            LoadData();
        }

        public void Part1()
        {
            LoadData();

            for (int x = 0; x < 1000; x++)
            {
                for (int i = 0; i < Moons.Count; i++)
                {
                    for (int j = i+1; j < Moons.Count; j++)
                    {
                        Moons[i].ApplyGravity(Moons[j]);
                    }
                }

                for (int i = 0; i < Moons.Count; i++)
                {
                    Moons[i].ApplyVelocity();
                }
            }

            int sum = 0;
            for (int i = 0; i < Moons.Count; i++)
            {
                sum += Moons[i].TotalEnergy;
            }

            Console.WriteLine("Part1: {0}", sum);
        }

        public void Part2()
        {
            LoadData();

            do
            {
                for (int i = 0; i < Moons.Count; i++)
                {
                    for (int j = i + 1; j < Moons.Count; j++)
                    {
                        Moons[i].ApplyGravity(Moons[j]);
                    }
                }

                for (int i = 0; i < Moons.Count; i++)
                {
                    Moons[i].ApplyVelocity();
                }

                Cycle++;
            } while (CyclesFound() == false);

            long[] cycles = new long[3] { XCycle+1, YCycle+1, ZCycle+1 };

            long rslt = LeastCommonMultipleMany(cycles);

            Console.WriteLine("Part2: {0}", rslt);
        }

        public bool CyclesFound()
        {
            bool xCycleMatch = true;
            bool yCycleMatch = true;
            bool zCycleMatch = true;

            int i;
            for (i = 0; i < Moons.Count - 1; i++)
            {
                xCycleMatch = xCycleMatch && Moons[i].AnchorPosition.X == Moons[i].Position.X;
                yCycleMatch = yCycleMatch && Moons[i].AnchorPosition.Y == Moons[i].Position.Y;
                zCycleMatch = zCycleMatch && Moons[i].AnchorPosition.Z == Moons[i].Position.Z;
            }

            if (xCycleMatch && XCycle == 0)
            {
                XCycle =Cycle;
            }
            if (yCycleMatch && YCycle == 0)
            {
                YCycle = Cycle;
            }
            if (zCycleMatch && ZCycle == 0)
            {
                ZCycle = Cycle;
            }

            return XCycle > 0 && YCycle > 0 && ZCycle > 0;
        }


        static long LeastCommonMultipleMany(long[] numbers)
        {
            return numbers.Aggregate(LeastCommonMultiple);
        }
        static long LeastCommonMultiple(long a, long b)
        {
            return Math.Abs(a * b) / GreatestCommonDivisor(a, b);
        }
        static long GreatestCommonDivisor(long a, long b)
        {
            return b == 0 ? a : GreatestCommonDivisor(b, a % b);
        }
        private List<Moon> Moons { get; set; }
        private long XCycle = 0;
        private long YCycle = 0;
        private long ZCycle = 0;
        private long Cycle = 0;

        private void LoadData()
        {

            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\input.txt";

            if (File.Exists(inputFile))
            {
                Moons = new List<Moon>();

                string line;
                char name = 'A';

                StreamReader file = new StreamReader(inputFile);
                while ((line = file.ReadLine()) != null)
                {
                    line = line.Replace("<", "");
                    line = line.Replace(">", "");
                    string[] values = line.Split(',');
                    int x = int.Parse(values[0].Trim().Substring(2));
                    int y = int.Parse(values[1].Trim().Substring(2));
                    int z = int.Parse(values[2].Trim().Substring(2));

                    Moons.Add(new Moon(name.ToString(), new Coordinates(x, y, z)));
                    name++;
                }

                file.Close();
            }

        }

    }
}
