using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCIntCode;

namespace Day13
{
    class Day13
    {

        public Day13()
        {
            LoadData();
        }

        public void Part1()
        {
            IntCode intCode = new IntCode("Game", Code);
            intCode.RunIntCode();
            Output = intCode.ReadFinalResults();
            GetDimensions();

            Console.WriteLine("Part1: {0}", NBlocks);
        }

        public void Part2()
        {
            Code[0] = 2; // set to free play
            IntCode intCode = new IntCode("Game", Code);
            OutputState = 0;

            InputQueue = new Queue<long>();
            OutputQueue = new Queue<long>();
            intCode = new IntCode("Game", Code, InputDelegate, OutputDelegate);
            intCode.RunIntCode();

            Console.WriteLine("Part2: {0}", Score);
        }

        private long InputDelegate()
        {
            long value = 0;

            GamePlayHasStarted = true;
            if (InputQueue.Count > 0)
                value = InputQueue.Dequeue();
            
            return value;
        }

        private void OutputDelegate(long value)
        {
            OutputQueue.Enqueue(value);

            switch (OutputState)
            {
                case 0:
                    XPos = value;
                    OutputState++;
                    break;
                case 1:
                    YPos = value;
                    OutputState++;
                    break;
                case 2:
                    TileID = value;
                    if (GamePlayHasStarted)
                    {
                        ProcessGameInstruction();
                    }
                    OutputState = 0;
                    break;
                default:
                    throw new ArgumentException("OutputState wonky");
            }
        }

        private void ProcessGameInstruction()
        {
            if (XPos < 0 && YPos == 0)
            {
                Score = TileID;
                Console.WriteLine("Score = {0}", Score);
            }
            else
            {
                switch (TileID)
                {
                    case 3: // paddle position
                        PaddleX = XPos;
                        break;
                    case 4: // ball position
                        BallX = XPos;
                        if (PaddleX == BallX)
                        {
                            InputQueue.Enqueue(0); // leave paddle horizontal
                        }
                        else if (PaddleX > BallX)
                        {
                            InputQueue.Enqueue(-1); // move left
                        }
                        else
                        {
                            InputQueue.Enqueue(1); // move right
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private Queue<long> InputQueue;
        private Queue<long> OutputQueue;
        private int OutputState;
        private long XPos = 0;
        private long YPos = 0;
        private long PaddleX = 0;
        private long BallX = 0;
        private long TileID = 0;
        private long Score = 0;
        private bool GamePlayHasStarted = false;

        private void GetDimensions()
        {
            MaxX = long.MinValue;
            MaxY = long.MinValue;
            NBlocks = 0;

            for (int i = 0; i < Output.Count; i += 3)
            {
                MaxX = Math.Max(MaxX, Output[i]);
                MaxY = Math.Max(MaxY, Output[i + 1]);
                NBlocks += Output[i + 2] == 2 ? 1 : 0;
            }
        }

        private long[] Code { get; set; }
        private List<long> Output;
        private long MaxX;
        private long MaxY;
        private int NBlocks;
        private void LoadData()
        {

            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\input.txt";

            if (File.Exists(inputFile))
            {

                string line;
                StreamReader file = new StreamReader(inputFile);
                while ((line = file.ReadLine()) != null)
                {
                    string[] strInstructions = line.Split(',');
                    Code = new long[strInstructions.Length];
                    for (int i = 0; i < Code.Length; i++)
                    {
                        Code[i] = long.Parse(strInstructions[i]);
                    }
                }

                file.Close();
            }

        }

    }
}
