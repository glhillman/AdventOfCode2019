using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day22
{
    class Day22
    {
        List<Command> _commands = new List<Command>();

        public Day22()
        {
            LoadData();
        }

        public void Part1()
        {
            Deck deck = new Deck(10007);

            for (int i = 0; i < 10007; i++)
            {
                deck.AddCard(new Card(i));
            }

            foreach (Command cmd in _commands)
            {
                switch (cmd.Cmd)
                {
                    case Command.CmdEnum.NewStack:
                        deck.NewStack();
                        break;
                    case Command.CmdEnum.DealIncrement:
                        deck.DealWithIncrement(cmd.Value);
                        break;
                    case Command.CmdEnum.CutN:
                        deck.CutN(cmd.Value);
                        break;
                    default:
                        throw new ArgumentException("Unexpected Command");
                }
            }

            long rslt = deck.PositionOfCard(2019);

            Console.WriteLine("Part1: {0}", rslt);
        }

        public void Part2()
        {

            Console.WriteLine("Part2: {0}", "Ridiculous Math - beats me!");
        }

        private void LoadData()
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\input.txt";

            if (File.Exists(inputFile))
            {
                string line;
                StreamReader file = new StreamReader(inputFile);
                while ((line = file.ReadLine()) != null)
                {
                    _commands.Add(new Command(line));
                }

                file.Close();
            }
        }

    }
}
