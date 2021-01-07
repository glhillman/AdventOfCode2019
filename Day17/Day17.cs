using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Channels;
using AoCIntCode;

namespace Day17
{
    public class Day17
    {
        private long[] _code;
        private char[,] _grid;
        private int _width;
        private int _height;
        private List<long> _intCodeOutput;
        private Queue<long> _intCodeInput;
        private int _startX;
        private int _startY;
        private char _currentDirection = ' ';

        private string _mainRoutine = string.Empty;
        private string _functionA = string.Empty;
        private string _functionB = string.Empty;
        private string _functionC = string.Empty;
        public Day17()
        {
            LoadData();
        }

        public void Part1()
        {
            if (_code != null)
            {
                _intCodeOutput = new List<long>();
                IntCode intCode = new IntCode("Day17", _code, IntCodeInput, IntCodeOutput);
                intCode.RunIntCode();

                _width = _intCodeOutput.IndexOf('\x0a');
                _height = _intCodeOutput.Count(b => b == '\x0a') - 1;

                _grid = new char[_width, _height];
            }

            int xx = 0;
            int yy = 0;

            foreach (char c in _intCodeOutput)
            {
                if (c == '\x0a')
                {
                    xx = 0;
                    yy++;
                }
                else
                {
                    if (c == '^' || c == 'v' || c == '<' || c == '>')
                    {
                        _currentDirection = c;
                        _startX = xx;
                        _startY = yy;
                    }
                    _grid[xx++, yy] = c;
                }
            }

            // find intersections
            int alignmentParametersSum = 0;

            for (int y = 1; y < _height-1; y++)
            {
                for (int x = 1; x < _width-1; x++)
                {
                    if (_grid[x, y] == '#')
                    {
                        if (_grid[x - 1, y] == '#' &&
                            _grid[x + 1, y] == '#' &&
                            _grid[x, y - 1] == '#' &&
                            _grid[x, y + 1] == '#')
                        {
                            // intersection is true.
                            alignmentParametersSum += x * y;
                        }
                    }
                }
            }

            Console.WriteLine("Part1: {0}", alignmentParametersSum);
        }

        public void Part2()
        {
            _intCodeOutput.Clear();

            FindFunctions();
            _intCodeInput = new Queue<long>();

            QueueInput(_mainRoutine);
            QueueInput(_functionA);
            QueueInput(_functionB);
            QueueInput(_functionC);
            _intCodeInput.Enqueue('n');
            _intCodeInput.Enqueue(10);

            _code[0] = 2;
            IntCode intCode = new IntCode("Day17", _code, IntCodeInput, IntCodeOutput);
            intCode.RunIntCode();

            long rslt = _intCodeOutput[_intCodeOutput.Count - 1];

            Console.WriteLine("Part2: {0}", rslt);
        }

        public void QueueInput(string input)
        {
            foreach (long ascii in input)
            {
                _intCodeInput.Enqueue(ascii);
            }
            _intCodeInput.Enqueue(10);
        }


        private void FindFunctions()
        { 
            List<string> path = FindPath();

            StringBuilder sb = new StringBuilder();

            for (int s = 0; s < path.Count; s++)
            {
                sb.Append(path[s++]);
                sb.Append(path[s]);
                if (s < path.Count - 1)
                {
                    sb.Append(" ");
                }
            }

            string master = sb.ToString().Trim();
            bool found = false;

            int index = 0;
            while (!found && index < master.Length - 1)
            {
                // incrementally build a candidate for A by appending move instructions from the front
                while (++index < master.Length - 1 && master[index] != ' ') ;
                string tempA = master;
                _functionA = tempA.Substring(0, index);
                // replace all the substring occurances of the candidate with "A"
                tempA = tempA.Replace(_functionA, "A");
                // split the resulting string on the 'A' character, yielding a list of strings that don't contain A
                string[] splitA = tempA.Split('A');
                // now try some variations of B
                foreach (string candidateB in splitA)
                {
                    if (candidateB.Trim() != "")
                    {
                        // repeat the above process to find a B candidate, looking at each of the entries in splitA
                        int index2 = 0;
                        while (!found && index2 < candidateB.Length - 2)
                        {
                            while (++index2 < candidateB.Length && candidateB[index2] != ' ') ;
                            _functionB = candidateB.Substring(0, index2).Trim();
                            string tempB = candidateB.Replace(_functionB, "B");
                            string[] splitB = tempB.Split('B');
                            // now try the C candidates
                            foreach (string candidateC in splitB)
                            {
                                // finally, repeat the process to isolate a C candidate
                                if (candidateC.Trim() != "")
                                {
                                    int index3 = 0;
                                    while (!found && index3 < candidateC.Length - 2)
                                    {
                                        while (++index3 < candidateC.Length && candidateC[index3] != ' ') ;
                                        _functionC = candidateC.Substring(0, index3).Trim();
                                        // replace all the occurances of B & C sequences with their respective letter (A was already applied above)
                                        _mainRoutine = tempA.Replace(_functionB, "B").Replace(_functionC, "C");
                                        // for an easy check, now replace every occurance of "A", "B", and "C" with nothing
                                        string empty = _mainRoutine.Replace("A", "").Replace("B", "").Replace("C", "").Trim();
                                        // if the string is empty, we have our three functions & the main routine!
                                        if (empty.Length == 0)
                                        {
                                            found = true;  
                                        }
                                    }
                                }
                                if (found)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    if (found)
                    {
                        break;
                    }
                }
            }
            _mainRoutine = _mainRoutine.Replace(" ", ",");
            _functionA = _functionA.Replace("L", "L,").Replace("R", "R,").Replace(" ", ",");
            _functionB = _functionB.Replace("L", "L,").Replace("R", "R,").Replace(" ", ",");
            _functionC = _functionC.Replace("L", "L,").Replace("R", "R,").Replace(" ", ",");

        }

        // wordy, but I don't want to mess with it. It is a one-off...
        private List<string> FindPath()
        {
            List<string> path = new List<string>();
            bool finished = false;
            int x = _startX;
            int y = _startY;
            int xIncrement = 0;
            int yIncrement = 0;

            while (!finished)
            {
                switch (_currentDirection)
                {
                    case '^':
                        if (x > 0 && _grid[x - 1, y] == '#')
                        {
                            path.Add("L");
                            _currentDirection = '<';
                            xIncrement = -1;
                            yIncrement = 0;
                        }
                        else if (x < _width - 1 && _grid[x + 1, y] == '#')
                        {
                            path.Add("R");
                            _currentDirection = '>';
                            xIncrement = 1;
                            yIncrement = 0;
                        }
                        else
                        {
                            finished = true;
                        }
                        break;
                    case 'v':
                        if (x > 0 && _grid[x - 1, y] == '#')
                        {
                            path.Add("R");
                            _currentDirection = '<';
                            xIncrement = -1;
                            yIncrement = 0;
                        }
                        else if (x < _width - 1 && _grid[x + 1, y] == '#')
                        {
                            path.Add("L");
                            _currentDirection = '>';
                            xIncrement = 1;
                            yIncrement = 0;
                        }
                        else
                        {
                            finished = true;
                        }
                        break;
                    case '>':
                        if (y > 0 && _grid[x, y - 1] == '#')
                        {
                            path.Add("L");
                            _currentDirection = '^';
                            xIncrement = 0;
                            yIncrement = -1;
                        }
                        else if (y < _height - 1 && _grid[x, y + 1] == '#')
                        {
                            path.Add("R");
                            _currentDirection = 'v';
                            xIncrement = 0;
                            yIncrement = 1;
                        }
                        else
                        {
                            finished = true;
                        }
                        break;
                    case '<':
                        if (y > 0 && _grid[x, y - 1] == '#')
                        {
                            path.Add("R");
                            _currentDirection = '^';
                            xIncrement = 0;
                            yIncrement = -1;
                        }
                        else if (y < _height - 1 && _grid[x, y + 1] == '#')
                        {
                            path.Add("L");
                            _currentDirection = 'v';
                            xIncrement = 0;
                            yIncrement = 1;
                        }
                        else
                        {
                            finished = true;
                        }
                        break;
                    default:
                        break;
                }

                // now move in the right direction
                if (!finished)
                {
                    int moves = 0;

                    int yTemp = y + yIncrement;
                    int xTemp = x + xIncrement;
                    while (xTemp >= 0 && xTemp < _width && yTemp >= 0 && yTemp < _height && _grid[xTemp, yTemp] == '#')
                    {
                        if (_grid[xTemp, yTemp] == '#')
                        {
                            moves++;
                            x = xTemp;
                            y = yTemp;
                            yTemp = y + yIncrement;
                            xTemp = x + xIncrement;
                        }
                    }

                    path.Add(moves.ToString());
                }
            }

            return path;
        }


        private void DumpGrid()
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    Console.Write(_grid[x, y]);
                }
                Console.WriteLine();
            }
        }

        private long IntCodeInput()
        {
            return _intCodeInput.Dequeue();
        }

        private void IntCodeOutput(long value)
        {
            _intCodeOutput.Add(value);
        }

        private void LoadData()
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\input.txt";

            if (File.Exists(inputFile))
            {
                string line;
                StreamReader file = new StreamReader(inputFile);
                line = file.ReadLine();
                string[] instructions = line.Split(',');
                _code = new long[instructions.Length];
                for (int i = 0; i < instructions.Length; i++)
                {
                    _code[i] = long.Parse(instructions[i]);
                }
                file.Close();
            }
        }
    }
}
