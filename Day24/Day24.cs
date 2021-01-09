using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day24
{
    public class Day24
    {
        const int SIZE = 5;

        private char[,] _grid1 = new char[SIZE, SIZE];
        private char[,] _grid2 = new char[SIZE, SIZE];

        public Day24()
        {
        }

        public void Part1()
        {
            LoadData();

            List<char[,]> grids = new List<char[,]>();
            grids.Add(_grid1);
            grids.Add(_grid2);

            int srcIndex = 0;
            int dstIndex = 1;

            int gridNum = 0;
            HashSet<int> gridNums = new HashSet<int>();

            while (gridNums.Contains(gridNum) == false)
            {
                gridNums.Add(gridNum); // assume 0, first state, will not occur

                gridNum = ApplyRules(grids[srcIndex], grids[dstIndex]);

                srcIndex = srcIndex == 1 ? 0 : 1;
                dstIndex = dstIndex == 1 ? 0 : 1;
            }

            long rslt = BioDiversityRating(gridNum);

            Console.WriteLine("Part1: {0}", rslt);
        }

        public void Part2()
        {
            // other than LoadData & this method, all of part2 logic is in the RecursiveGrid class
            LoadData();

            // start with the loaded grid, an inner, and an outer grid
            // kind of a clutzy startup, but heck
            RecursiveGrid innerinner = new RecursiveGrid(1, RecursiveGrid.InitializeGrid(new char[5, 5]));
            RecursiveGrid rgridInner = new RecursiveGrid(0, _grid1);
            rgridInner.InnerGrid = innerinner;
            innerinner.OuterGrid = rgridInner;
            RecursiveGrid rgrid = new RecursiveGrid(-1, rgridInner);
            RecursiveGrid tmp;

            for (int i = 0; i < 200; i++)
            {
                bool needOuter = rgrid.ApplyRules();
                tmp = rgrid;
                while (tmp != null)
                {
                    tmp.SwitchSrcAndDst();
                    tmp = tmp.InnerGrid;
                }
                if (needOuter)
                {
                    rgrid = new RecursiveGrid(rgrid.Depth - 1, rgrid);
                }
            }

            int bugCount = 0;
            tmp = rgrid;
            while (tmp != null)
            {
                bugCount += tmp.CountBugs();
                tmp = tmp.InnerGrid;
            }

            Console.WriteLine("Part2: {0}", bugCount);
        }

        private int ApplyRules(char[,] src, char[,] dst)
        {
            for (int y = 0; y < SIZE; y++)
            {
                for (int x = 0; x < SIZE; x++)
                {
                    int adjacentCount = 0;
                    // count adjacent bugs
                    adjacentCount += (x > 0 && src[x - 1, y] == '#') ? 1 : 0;
                    adjacentCount += (x < SIZE - 1 && src[x + 1, y] == '#') ? 1 : 0;
                    adjacentCount += (y > 0 && src[x, y - 1] == '#') ? 1 : 0;
                    adjacentCount += (y < SIZE - 1 && src[x, y + 1] == '#') ? 1 : 0;

                    char newState = src[x, y];

                    if (src[x, y] == '#')
                    {
                        if (adjacentCount != 1)
                        {
                            newState = '.';
                        }
                    }
                    else
                    {
                        if (adjacentCount == 1 || adjacentCount == 2)
                        {
                            newState = '#';
                        }
                    }

                    dst[x, y] = newState;
                }
            }

            return GridToNum(dst);
        }

        private int GridToNum(char[,] grid)
        {
            // Create a number using the hashmarks as bits. 
            // Each row is a 5 bit number, shifting left by 5 bits as rows increase

            int num = 0;
            for (int y = 0; y < SIZE; y++)
            {
                for (int x = 0; x < SIZE; x++)
                {
                    num <<= 1;
                    num += grid[x, y] == '#' ? 1 : 0;
                }
            }

            return num;
        }

        private char[,] NumToGrid(int num)
        {
            char[,] grid = new char[SIZE, SIZE];

            for (int y = SIZE-1; y >= 0; y--)
            {
                for (int x = SIZE-1; x >= 0; x--)
                {
                    grid[x, y] = (num & 1) > 0 ? '#' : '.';
                    num >>= 1;
                }
            }

            return grid;
        }

        private int BioDiversityRating(int gridNum)
        {
            char[,] grid = NumToGrid(gridNum);
            int rating = 0;
            int value = 1;

            for (int y = 0; y < SIZE; y++)
            {
                for (int x = 0; x < SIZE; x++)
                {
                    rating += grid[x, y] == '#' ? value : 0;
                    value <<= 1;
                }
            }

            return rating;
        }

        private void DumpGrid(string label, char[,] grid)
        {
            Console.WriteLine(label);

            int size = grid.GetLength(0);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Console.Write(grid[x, y]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void LoadData()
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\input.txt";

            if (File.Exists(inputFile))
            {
                string line;
                StreamReader file = new StreamReader(inputFile);
                int y = 0;
                while ((line = file.ReadLine()) != null)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        _grid1[x, y] = line[x];
                    }
                    y++;
                }

                file.Close();
            }
        }

    }
}
