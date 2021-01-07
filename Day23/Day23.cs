using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCIntCode;

namespace Day23
{
    public class Day23
    {
        private NetNode[] _netNodes;
        private long[] _code;
        private int _nNodes = 50;
        private NAT _NAT;

        public Day23()
        {
            LoadData();
            _netNodes = new NetNode[_nNodes];
        }

        public void Part1()
        {
            long rslt = ProcessPackets(false);

            Console.WriteLine("Part1: {0}", rslt);
        }

        public void Part2()
        {
            long rslt = ProcessPackets(true);

            Console.WriteLine("Part2: {0}", rslt);
        }

        private long ProcessPackets(bool useNAT)
        {
            for (long i = 0; i < _nNodes; i++)
            {
                IntCode intCode = new IntCode(i.ToString(), _code, null, null);
                NetNode node = new NetNode(intCode, i);
                _netNodes[i] = node;
            }
            _NAT = new NAT(_nNodes, _netNodes[0]);

            long rslt = 0;

            while (rslt == 0)
            {
                for (int i = 0; i < _nNodes; i++)
                {
                    long id;
                    long x;
                    long y;
                    bool activeStatus;

                    _netNodes[i].Intcode.Step();
                    if ((activeStatus = _netNodes[i].GetOutputPacket(out id, out x, out y)) == true)
                    {
                        if (id == 255)
                        {
                            if (useNAT)
                            {
                                _NAT.SetXY(x, y);
                            }
                            else
                            {
                                rslt = y;
                                i = _nNodes; // terminate loop
                            }
                        }
                        else
                        {
                            _netNodes[id].QueueInput(x);
                            _netNodes[id].QueueInput(y);
                        }
                    }
                    if (useNAT)
                    {
                        long targetY = _NAT.SetActiveStatus(i, activeStatus);
                        if (targetY > long.MinValue)
                        {
                            rslt = targetY;
                        }
                    }
                }
            }

            return rslt;
        }

        private void LoadData()
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\input.txt";

            if (File.Exists(inputFile))
            {
                string line;
                StreamReader file = new StreamReader(inputFile);
                line = file.ReadLine();
                string[] splits = line.Split(',');
                List<long> temp = new List<long>();
                foreach (string s in splits)
                {
                    temp.Add(long.Parse(s));
                }

                _code = temp.ToArray();

                file.Close();
            }
        }
    }
}
