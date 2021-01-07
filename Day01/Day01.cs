using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day01_2019
{
    class Day01
    {
        static void Main(string[] args)
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\Day02Input.txt";

            if (File.Exists(inputFile))
            {
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(inputFile);
                int totalFuel_1 = 0;
                int totalFuel_2 = 0;
                int fuel = 0;

                while ((line = file.ReadLine()) != null)
                {
                    int mass = int.Parse(line);
                    fuel = CalculateFuel(mass);
                    totalFuel_1 += fuel;
                    while (fuel > 0)
                    {
                        totalFuel_2 += fuel;
                        fuel = CalculateFuel(fuel);
                    }
                }
                file.Close();

                Console.WriteLine("Total fuel 1 = {0}", totalFuel_1);
                Console.WriteLine("Total fuel 2 = {0}", totalFuel_2);
            }
        }

        static int CalculateFuel(int mass)
        {
            int fuel = mass / 3 - 2;

            return fuel;
        }
    }
}
