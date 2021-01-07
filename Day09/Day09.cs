using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day09
{
    public class Day09
    {
        private long[] _code;

        public Day09()
        {
            ReadInstructions();
        }

        public void Part1()
        {
            IntCode intCode = new IntCode("Oink", _code);
            intCode.WriteInputChannel(1);
            intCode.RunIntCode();

            List<long> finalResults = intCode.ReadFinalResults();

            Console.WriteLine("Part 1:");
            foreach (long value in finalResults)
                Console.WriteLine("{0}", value);
        }

        public void Part2()
        {
            IntCode intCode = new IntCode("Oink", _code);
            intCode.WriteInputChannel(2);
            intCode.RunIntCode();

            List<long> finalResults = intCode.ReadFinalResults();

            Console.WriteLine("Part 2:");
            foreach (long value in finalResults)
                Console.WriteLine("{0}", value);
        }

        private void ReadInstructions()
        {

            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\Input.txt";

            if (File.Exists(inputFile))
            {
                string line;
                StreamReader file = new StreamReader(inputFile);
                while ((line = file.ReadLine()) != null)
                {
                    string[] ints = line.Split(',');
                    _code = new long[ints.Count()];
                    for (int i = 0; i < ints.Count(); i++)
                    {
                        _code[i] = long.Parse(ints[i]);
                    }
                }

                file.Close();
            }
        }
    }
}
