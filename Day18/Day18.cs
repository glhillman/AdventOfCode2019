using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    public class Day18
    {
        Dictionary<char, (int x, int y)> _doors;
        Dictionary<char, (int x, int y)> _keys;
        char[,] _grid;
        int _startX;
        int _startY;

        public Day18()
        {
            _doors = new Dictionary<char, (int x, int y)>();
            _keys = new Dictionary<char, (int x, int y)>();
            LoadData();
        }

        public void Part1()
        {

            long rslt = 0;

            Console.WriteLine("Part1: {0}", rslt);
        }

        public void Part2()
        {

            long rslt = 0;

            Console.WriteLine("Part2: {0}", rslt);
        }

        private void LoadData()
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\test.txt";

            if (File.Exists(inputFile))
            {
                string line;
                int lineLength = 0;

                StreamReader file = new StreamReader(inputFile);
                List<string> rows = new List<string>();
                while ((line = file.ReadLine()) != null)
                {
                    lineLength = line.Length; // redundant. all same. faster than check to see if already set...
                    rows.Add(line);
                }

                file.Close();

                _grid = new char[lineLength, rows.Count];

                for (int y = 0; y < rows.Count; y++)
                {
                    string row = rows[y];
                    for (int x = 0; x < row.Length; x++)
                    {
                        char c = row[x];
                        _grid[x, y] = c;
                        if (char.IsLetter(c))
                        {
                            if (char.IsUpper(c))
                            {
                                _doors[c] = (x, y);
                            }
                            else
                            {
                                _keys[c] = (x, y);
                            }
                        }
                        else if (c == '@')
                        {
                            _startX = x;
                            _startY = y;
                         }
                     }
                }
            }
        }
    }
}
