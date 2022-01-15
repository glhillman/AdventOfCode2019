using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCIntCode;

namespace Day15
{
    public class Day15
    {
        private const int SIZE = 100;
        
        private const long North = 1;
        private const long South = 2;
        private const long West = 3;
        private const long East = 4;

        private const long HitWall = 0;
        private const long Moved = 1;
        private const long OxySensor = 2;

        private const int Left = 1;
        private const int Right = 2;

        private List<long> Code;
        private Queue<long> InputQueue;
        private Queue<long> OutputQueue;
        private IntCode _intCode;

        private long _currDirection;
        private int _currX;
        private int _currY;
        private int _anchorX;
        private int _anchorY;
        private int _oxyX = -1;
        private int _oxyY = -1;
        private char[,] _grid;
        private Dictionary<(int x, int y), char> _dgrid;
        private int _steps = 0;

        private int _minX = int.MaxValue;
        private int _maxX = int.MinValue;
        private int _minY = int.MaxValue;
        private int _maxY = int.MinValue;


        public Day15()
        {
            LoadData();
            _grid = new char[SIZE, SIZE];
            _dgrid = new Dictionary<(int x, int y), char>();
            for (int x = 0; x < SIZE; x++)
                for (int y = 0; y < SIZE; y++)
                    _grid[x, y] = ' ';
            // start out in the middle of the grid
            _currX = SIZE / 2;
            _currY = _currX;
            _currDirection = South;

            InputQueue = new Queue<long>();
            OutputQueue = new Queue<long>();
        }

        public void Part1()
        {
            _intCode = new IntCode("Day15", Code.ToArray(), IntCodeInput, SouthUntilWallOutput);
            
            InputQueue.Enqueue(_currDirection);

            _intCode.RunIntCode();

            Console.WriteLine("Part1: {0}", _steps);
        }

        public void Part2()
        {

            long rslt = 0;

            Console.WriteLine("Part2: {0}", rslt);
        }

        private void SouthUntilWallOutput(long value)
        {
            if (value == Moved)
            {
                _currY++;
            }
            else if (value == HitWall)
            {
                _grid[_currX, _currY + 1] = '#';
                _dgrid[(_currX, _currY + 1)] = '#';
                Turn(Left);
                _anchorX = _currX;
                _anchorY = _currY;
                _intCode.SetNewOutputDelegate(TraverseOutput);
            }
            InputQueue.Enqueue(_currDirection);
        }

        private long IntCodeInput()
        {
            return InputQueue.Dequeue();
        }

        private void TraverseOutput(long value)
        {
            if (value != 99)
            {
                TraverseNextStep(value);
                DumpGrid(value.ToString());
            }
            else
            {
                DumpGrid("Finished");
            }
        }

        private void Turn(int direction)
        {
            if (direction == Left)
            {
                switch (_currDirection)
                {
                    case North:
                        _currDirection = West;
                        break;
                    case South:
                        _currDirection = East;
                        break;
                    case East:
                        _currDirection = North;
                        break;
                    case West:
                        _currDirection = South;
                        break;
                    default:
                        throw new ArgumentException("Unexpected Direction");
                }
            }
            else // right
            {
                switch (_currDirection)
                {
                    case North:
                        _currDirection = East;
                        break;
                    case South:
                        _currDirection = West;
                        break;
                    case East:
                        _currDirection = South;
                        break;
                    case West:
                        _currDirection = North;
                        break;
                    default:
                        throw new ArgumentException("Unexpected Direction");
                }
            }
        }
        private void TraverseNextStep(long currResult)
        {
            // Right-turn traversal follows wall, marking out its shape

            switch (currResult)
            {
                case HitWall:
                    // Mark wall
                    switch (_currDirection)
                    {
                        case North:
                            _grid[_currX, _currY - 1] = '#';
                            _dgrid[(_currX, _currY - 1)] = '#';
                            break;
                        case South:
                            _grid[_currX, _currY + 1] = '#';
                            _dgrid[(_currX, _currY + 1)] = '#';
                            break;
                        case East:
                            _grid[_currX + 1, _currY] = '#';
                            _dgrid[(_currX + 1, _currY)] = '#';
                            break;
                        case West:
                            _grid[_currX - 1, _currY] = '#';
                            _dgrid[(_currX - 1, _currY)] = '#';
                            break;
                    }
                    Turn(Left);
                    AdjustLimits();
                    InputQueue.Enqueue(_currDirection);
                    break;
                case Moved:
                case OxySensor:
                    _steps++;
                    switch (_currDirection)
                    {
                        case North:
                            _currY--;
                            break;
                        case South:
                            _currY++;
                            break;
                        case East:
                            _currX++;
                            break;
                        case West:
                            _currX--;
                            break;
                    }
                    if (currResult == OxySensor)
                    {
                        _oxyX = _currX;
                        _oxyY = _currY;
                        //DumpGrid(string.Format("Found OxySensor at {0},{1}", _currX, _currY));
                    }
                    AdjustLimits();
                    if (WallToRight() == false)
                    {
                        Turn(Right);
                    }
                    InputQueue.Enqueue(_currDirection);
                    break;
                default:
                    break;
            }
        }

        private bool WallToRight()
        {
            int x = _currX;
            int y = _currY;

            switch (_currDirection)
            {
                case North:
                    x++; // look East
                    break;
                case South:
                    x--; // look West
                    break;
                case East:
                    y++; // look South
                    break;
                case West:
                    y--; // look North
                    break;
            }
            char value;
            return _dgrid.TryGetValue((x, y), out value) ? value == '#' : false;
            //return (_grid[x, y] == '#');
        }

        int _dumpNum = 0;
        private void AdjustLimits()
        {
            _minX = Math.Min(_minX, _currX);
            _maxX = Math.Max(_maxX, _currX);
            _minY = Math.Min(_minY, _currY);
            _maxY = Math.Max(_maxY, _currY);
            //DumpGrid(string.Format("Dump {0}", _dumpNum++));
        }

        private void DumpGrid(string title)
        {
            Console.WriteLine(title);
            for (int y = _minY-1; y <= _maxY + 1; y++)
            {
                for (int x = _minX-1; x <= _maxX + 1; x++)
                {
                    char value;
                    if (_dgrid.TryGetValue((x,y), out value) == false)
                    {
                        value = ' ';
                    }
                    if (x == _anchorX && y == _anchorY)
                    {
                        Console.Write('X');
                    }
                    else if ((x == _minX - 1 || x == _maxX + 1) && value != '#') //_grid[x, y] != '#') 
                    {
                        Console.Write('|');
                    }
                    else if ((y == _minY-1 || y == _maxY+1) && value != '#') //_grid[x,y] != '#')
                    {
                        Console.Write('-');
                    } 
                    else if (x == _oxyX && y == _oxyY)
                    {
                        Console.Write('0');
                    }
                    else if (x == _currX && y == _currY)
                    {
                        Console.Write('D');
                    }
                    else
                    {
                        Console.Write(value); // _grid[x, y]);
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        private void LoadData()
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\input.txt";

            if (File.Exists(inputFile))
            {
                Code = new List<long>();
                string line;
                StreamReader file = new StreamReader(inputFile);
                line = file.ReadLine();
                string[] instructions = line.Split(',');
                foreach (string instruction in instructions)
                {
                    Code.Add(long.Parse(instruction));
                }

                file.Close();
            }
        }

    }
}
