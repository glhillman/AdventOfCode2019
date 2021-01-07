using System;
using System.Collections.Generic;
using System.IO;
using AoCIntCode;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    public class Day11
    {
        public const int MAXLENGTH = 5000;
        public Day11()
        {
            LoadData();
            Grid = new int[MAXLENGTH, MAXLENGTH];
        }

        public void Part1()
        {
            InitGrid();
            // start in the middle
            IntCode intCode = new IntCode("Day11", Instructions);
            Robot robot = new Robot(intCode, Grid, 0);

            List<Task> tasks = new List<Task>();

            // this block doesn't like to be put in a loop - channel sync crashes
            tasks.Add(Task.Run(() => { intCode.RunIntCode(); }));
            tasks.Add(Task.Run(() => { robot.Run(); }));

            Task.WaitAll(tasks.ToArray());

            int rslt = robot.ChangeCount;

            Console.WriteLine("Part1 {0}", rslt);
        }

        public void Part2()
        {
            InitGrid();
            // start in the middle
            IntCode intCode = new IntCode("Day11", Instructions);
            Robot robot = new Robot(intCode, Grid, 1);

            List<Task> tasks = new List<Task>();

            // this block doesn't like to be put in a loop - channel sync crashes
            tasks.Add(Task.Run(() => { intCode.RunIntCode(); }));
            tasks.Add(Task.Run(() => { robot.Run(); }));

            Task.WaitAll(tasks.ToArray());

            for (int y = robot.MaxY; y >= robot.MinY; y--)
            {
                for (int x = robot.MinX; x <= robot.MaxX; x++)
                {
                    Console.Write("{0}", (Grid[x,y] == 1) ? '#' : ' ');
                }
                Console.WriteLine();
            }
            int rslt = robot.ChangeCount;

            Console.WriteLine("Part1 {0}", rslt);

        }

        private long[] Instructions { get; set; }

        private int[,] Grid { get; set; }
        private void InitGrid()
        {
            for (int i = 0; i < MAXLENGTH; i++)
                for (int j = 0; j < MAXLENGTH; j++)
                    Grid[i, j] = 0;
        }

        private void LoadData()
        {

            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\input.txt";

            if (File.Exists(inputFile))
            {
                string line;
                StreamReader file = new StreamReader(inputFile);
                line = file.ReadLine();
                string[] values = line.Split(',');
                Instructions = new long[values.Length];

                for (int i = 0; i < values.Length; i++)
                {
                    Instructions[i] = long.Parse(values[i]);
                }

                file.Close();
            }

        }

    }
}
