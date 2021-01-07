using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    public class Day16
    {
        private int[] _input;
        private int[] _basePattern = { 0, 1, 0, -1 };
        public Day16()
        {
            LoadData();
        }

        public void Part1()
        {
            Pattern pattern = new Pattern(_basePattern, _input.Length);

            int[] input = new int[_input.Length];
            int[] output = new int[_input.Length];

            _input.CopyTo(input, 0);

            for (int x = 0; x < 100; x++)
            {
                for (int i = 0; i < output.Length; i++)
                {
                    int sum = 0;
                    for (int j = 0; j < input.Length; j++)
                    {
                        sum += input[j] * pattern.NextValue;
                    }
                    output[i] = Math.Abs(sum) % 10;
                }
                pattern.ReSetCycle();
                output.CopyTo(input, 0);
            }


            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                sb.Append(output[i].ToString());
            }
            string rslt = sb.ToString();

            Console.WriteLine("Part1: {0}", rslt);
        }

        public void Part2()
        {
            int repeatValue = 10000;
            int[] input = new int[_input.Length * repeatValue];
            int[] output = new int[_input.Length * repeatValue];

            for (int i = 0; i < repeatValue; i++)
            {
                _input.CopyTo(input, i * _input.Length);
            }

            int offset = input[0];
            for (int i = 1; i < 7; i++)
            {
                offset = offset * 10 + input[i];
            }

            for (int x = 0; x < 100; x++)
            {
                int runningSum = input[input.Length - 1];
                for (int i = input.Length-2; i >= offset-1; i--)
                {
                    output[i + 1] = runningSum;
                    runningSum = (runningSum + input[i]) % 10;
                }
                output.CopyTo(input, 0);
            }

            StringBuilder sb = new StringBuilder();
            for (int i = offset; i < offset + 8; i++)
            {
                sb.Append(output[i].ToString());
            }
            
            string rslt = sb.ToString();

            Console.WriteLine("Part2: {0}", rslt);
        }

        private void LoadData()
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\input.txt";

            if (File.Exists(inputFile))
            {
                string line;
                StreamReader file = new StreamReader(inputFile);
                line = file.ReadLine();
                _input = new int[line.Length];
                for (int i = 0; i < line.Length; i++)
                {
                    _input[i] = int.Parse(line[i].ToString());
                }
                file.Close();
            }
        }

    }
}
