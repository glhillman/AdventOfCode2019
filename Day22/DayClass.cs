using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DayClass
{
    internal class Day22
    {
        List<Command> _commands = new();
        List<string> _strCommands = new();

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
            BigInteger size = 119315717514047;
            BigInteger iter = 101741582076661;
            BigInteger position = 2020;
            BigInteger offset_diff = 0;
            BigInteger increment_mul = 1;

            foreach (string line in _strCommands)
            {
                RunP2(ref increment_mul, ref offset_diff, size, line);
            }

            (BigInteger increment, BigInteger offset) = getseq(iter, increment_mul, offset_diff, size);

            var card = get(offset, increment, 2020, size);

            Console.WriteLine("Part2: {0}", card);
            Console.WriteLine("Part 2 was beyond my math - I read a number of descriptions, copied this one, and don't understand it.");
        }

        private void RunP2(ref BigInteger inc_mul, ref BigInteger offset_diff, BigInteger size, string line)
        {
            if (line.StartsWith("cut"))
            {
                offset_diff += Int32.Parse(line.Split(" ").Last()) * inc_mul;
            }
            else if (line == "deal into new stack")
            {
                inc_mul *= -1;
                offset_diff += inc_mul;
            }
            else
            {
                BigInteger num = Int32.Parse(line.Split(" ").Last());

                inc_mul *= BigInteger.ModPow(num, size - 2, size);
            }

            inc_mul = (inc_mul % size + size) % size;
            offset_diff = (offset_diff % size + size) % size;
        }

        private BigInteger get(BigInteger offset, BigInteger increment, BigInteger i, BigInteger size)
        {
            return (offset + i * increment) % size;
        }

        private (BigInteger increment, BigInteger offset) getseq(BigInteger iterations, BigInteger inc_mul, BigInteger offset_diff, BigInteger size)
        {
            var increment = BigInteger.ModPow(inc_mul, iterations, size);

            var offset = offset_diff * (1 - increment) * BigInteger.ModPow((1 - inc_mul) % size, size - 2, size);

            offset %= size;

            return (increment, offset);
        }

        private void LoadData()
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

            if (File.Exists(inputFile))
            {
                string? line;
                StreamReader file = new StreamReader(inputFile);
                while ((line = file.ReadLine()) != null)
                {
                    _commands.Add(new Command(line));
                    _strCommands.Add(line);
                }

                file.Close();
            }
        }

    }
}
