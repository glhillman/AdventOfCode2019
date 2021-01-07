using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCIntCode;

namespace Day19
{
    class Day19
    {
        long[] _code;
        Queue<long> _intCodeInput;
        Queue<long> _intCodeOutput;
        IntCode _intCode;

       public Day19()
        {
            LoadData();
            _intCodeInput = new Queue<long>();
            _intCodeOutput = new Queue<long>();

            _intCode = new IntCode("Day19", _code, IntCodeInput, IntCodeOutput);
        }

        public void Part1()
        {
            long sum = 0;

            for (long y = 0; y < 50; y++)
            {
                for (long x = 0; x < 50; x++)
                {
                    sum += RetrieveValue(x, y);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Part1: {0}", sum);
        }

        public void Part2()
        {
            bool found = false;

            long targetSize = 100;

            long high = 1000;
            long low = targetSize;
            long rslt = 0;

            int xOffset = 1;

            while (!found)
            {
                long mid = (high - low) / 2 + low;
                long y = mid;
                long x = y + xOffset;

                while (RetrieveValue(x, y) == 0)
                {
                    xOffset++;
                    x++;
                }
                if (RetrieveValue(x, y) == 1 && RetrieveValue(x, y + 1) == 0) // sanity check - should be zero below us
                {
                    long yy = y - (targetSize - 1);
                    long xx = x + (targetSize - 1);
                    if (RetrieveValue(xx, yy) == 1)
                    {
                        if (RetrieveValue(xx, yy - 1) == 0 && RetrieveValue(xx+1, yy) == 0)
                        {
                            found = true;
                            rslt = x * 10000 + yy;
                        }
                        else
                        {
                            // too far down - y is too great
                            high = mid;
                            xOffset /= 3;
                        }
                    }
                    else
                    {
                        // not deep enough - y is too small
                        low = mid;
                    }
                }
            }

            Console.WriteLine("Part2: {0}", rslt);
        }

        private long RetrieveValue(long x, long y)
        {
            _intCode.ResetCode();
            _intCodeInput.Enqueue(x);
            _intCodeInput.Enqueue(y);
            _intCode.RunIntCode();
            return _intCodeOutput.Dequeue();
        }

        private long IntCodeInput()
        {
            return _intCodeInput.Dequeue();
        }

        private void IntCodeOutput(long value)
        {
            _intCodeOutput.Enqueue(value);
        }

        private void LoadData()
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\input.txt";

            if (File.Exists(inputFile))
            {
                string line;
                StreamReader file = new StreamReader(inputFile);
                List<long> list = new List<long>();
                line = file.ReadLine();
                file.Close();

                string[] values = line.Split(',');
                foreach (string value in values)
                {
                    list.Add(long.Parse(value));
                }
                _code = list.ToArray();
            }
        }

    }
}
