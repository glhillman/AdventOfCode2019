using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCIntCode;

namespace Day21
{
    class Day21
    {
        long[] _code;
        Queue<long> _intCodeInput;
        Queue<long> _intCodeOutput;
        IntCode _intCode;

        public Day21()
        {
            LoadData();
            _intCodeInput = new Queue<long>();
            _intCodeOutput = new Queue<long>();

            _intCode = new IntCode("Day21", _code, IntCodeInput, IntCodeOutput);
        }

        public void Part1()
        {
            string instructions = "NOT A J\n" + // jump if next step empty
                                  "NOT C T\n" + // is C (3 steps away) empty?
                                  "AND D T\n" + // are 3 AND 4 steps away empty
                                  "OR T J\n" +  // jump if A and C and D are empty
                                  "WALK\n";

            //                    ###.#..###
            //                       ABCD

            InstructionsToInput(instructions);

            _intCode.RunIntCode();

            long rslt = DumpOutput(false);

            Console.WriteLine("Part1: {0}", rslt);
        }

        public void Part2()
        {
            // a jump is always 4 spaces - make sure there are places to land!        
            string instructions = "NOT B J\n" + // jump if nothing at B (2 away)
                                  "NOT C T\n" + // nothing at C (3 away)?
                                  "OR T J\n" +  // jump if nothing at B OR C
                                  "AND D J\n" + // jump if OK to land at D
                                  "AND H J\n" + // jump if OK to land at H (4 away from D)
                                  "NOT A T\n" + // Nothing at A (1 away)?
                                  "OR T J\n" +  // If JMP is True from previous logic, OR A is empty space, JMP
                                  "RUN\n";
                      
            //                      "###.#..###"
            //                        ABCDEFGHI                                 
            
            InstructionsToInput(instructions);

            _intCode.ResetCode();
            _intCode.RunIntCode();

            long rslt = DumpOutput(false);

            Console.WriteLine("Part2: {0}", rslt);
        }

        private long IntCodeInput()
        {
            return _intCodeInput.Dequeue();
        }

        private void IntCodeOutput(long value)
        {
            _intCodeOutput.Enqueue(value);
        }

        private long DumpOutput(bool displayOutput)
        {
            long rslt = -1;

            foreach (long c in _intCodeOutput)
            {
                if (c > 255)
                {
                    // Success ! 
                    rslt = c;
                }
                else if (displayOutput)
                {
                    if (c == 10)
                    {
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.Write((char)c);
                    }
                }
            }

            return rslt;
        }
        private void InstructionsToInput(string instructions)
        {
            for (int i = 0; i < instructions.Length; i++)
            {
                _intCodeInput.Enqueue(instructions[i]);
            }
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
