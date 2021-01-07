using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day02
{
    class Day02
    {
        static void Main(string[] args)
        {
            int[] code =
            {
                1,0,0,3,1,1,2,3,1,3,4,3,1,5,0,3,2,1,10,19,1,6,19,23,2,23,6,27,2,6,27,31,2,13,31,35,1,10,35,
                39,2,39,13,43,1,43,13,47,1,6,47,51,1,10,51,55,2,55,6,59,1,5,59,63,2,9,63,67,1,6,67,71,2,9,
                71,75,1,6,75,79,2,79,13,83,1,83,10,87,1,13,87,91,1,91,10,95,2,9,95,99,1,5,99,103,2,10,103,
                107,1,107,2,111,1,111,5,0,99,2,14,0,0
            };


            int result = RunIntCode(code, 12, 2);
            
            Console.WriteLine("Part 1 result: {0}", result);

            int noun = 0;
            int verb = 0;

            result = 0;

            for (noun = 0; noun <= 99 && result == 0; noun++)
            {
                for (verb = 0; verb <= 99 && result == 0; verb++)
                {
                    int value = RunIntCode(code, noun, verb);
                    if (value == 19690720)
                    {
                        result = 100 * noun + verb;
                    }
                }
            }

            Console.WriteLine("Part 2 result: {0}", result);
        }

        static int RunIntCode(int[] inCode, int init1, int init2)
        {
            int[] code = new int[inCode.Length];
            inCode.CopyTo(code, 0);

            code[1] = init1;
            code[2] = init2;

            int index = 0;
            bool validInput = true;

            while (code[index] != 99 && validInput)
            {
                switch (code[index])
                {
                    case 1:
                        {
                            int op1 = code[index + 1];
                            int op2 = code[index + 2];

                            int sum = code[op1] + code[op2];
                            code[code[index + 3]] = sum;
                            index += 4;
                        }
                        break;
                    case 2:
                        {
                            int op1 = code[index + 1];
                            int op2 = code[index + 2];

                            int prod = code[op1] * code[op2];
                            code[code[index + 3]] = prod;
                            index += 4;
                        }
                        break;
                    default:
                        Console.WriteLine("Unexpected operand: {0} at index {1}", code[index], index);
                        validInput = false;
                        break;
                }
            }
            return code[0];
        }
    }
}
