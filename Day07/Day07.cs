using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Day07
{
    public class Day07
    {
        private int[] _code;

        public Day07()
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\CodeData.txt";

            if (File.Exists(inputFile))
            {
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(inputFile);
                if ((line = file.ReadLine()) != null)
                {
                    string[] ints = line.Split(',');
                    _code = new int[ints.Count()];
                    for (int i = 0; i < ints.Count(); i++)
                    {
                        _code[i] = int.Parse(ints[i]);
                    }
                }

                file.Close();
            }

        }

        public void Puzzle1()
        {
            int calculated = 0;
            int[] phases = { 0, 1, 2, 3, 4 };
            int highest = int.MinValue;
            IntCode intCode = new IntCode("Oink", _code);

            Permutation perm = new Permutation(phases);

            phases = perm.NextPermutation();
            while (phases != null)
            {
                for (int phaseIndex = 0; phaseIndex < phases.Length; phaseIndex++)
                {
                    intCode.WriteInputChannel(phases[phaseIndex]);
                    intCode.WriteInputChannel(calculated);
                    intCode.RunIntCode();
                    calculated = intCode.ReadOutputChannel();
                }

                highest = Math.Max(highest, calculated);
                calculated = 0;
                phases = perm.NextPermutation();
            }

            Console.WriteLine("Puzzle1 Highest Signal: {0}", highest);
        }

        public async void Puzzle2()
        {
            int[] inputs = new int[2];
            int[] phases = { 5, 6, 7, 8, 9 };
            int highest = int.MinValue;

            IntCode intCodeA = new IntCode("IntCodeA", _code);
            IntCode intCodeB = new IntCode("IntCodeB", _code, intCodeA.OutputChannel);
            IntCode intCodeC = new IntCode("IntCodeC", _code, intCodeB.OutputChannel);
            IntCode intCodeD = new IntCode("IntCodeD", _code, intCodeC.OutputChannel);
            IntCode intCodeE = new IntCode("IntCodeE", _code, intCodeD.OutputChannel, intCodeA.InputChannel);

            // loop this around
            intCodeA.InputChannel = intCodeE.OutputChannel;

            Permutation perms = new Permutation(phases);

            IntCode[] intCodes = { intCodeA, intCodeB, intCodeC, intCodeD, intCodeE };
            
            int[] nextPerm;

            while ((nextPerm = perms.NextPermutation()) != null)
            {
                for (int i = 0; i < nextPerm.Length; i++)
                {
                    intCodes[i].WriteInputChannel(nextPerm[i]);
                }
                intCodeA.WriteInputChannel(0);

                List<Task> tasks = new List<Task>();

                // this block doesn't like to be put in a loop - channel sync crashes
                tasks.Add(Task.Run(() => { intCodeA.RunIntCode(); }));
                tasks.Add(Task.Run(() => { intCodeB.RunIntCode(); }));
                tasks.Add(Task.Run(() => { intCodeC.RunIntCode(); }));
                tasks.Add(Task.Run(() => { intCodeD.RunIntCode(); }));
                tasks.Add(Task.Run(() => { intCodeE.RunIntCode(); }));

                Task.WaitAll(tasks.ToArray());

                int rslt = intCodeE.ReadOutputChannel();

                highest = Math.Max(rslt, highest);
            }

            Console.WriteLine("Puzzle1 Highest Signal: {0}", highest);
        }
    }
}
