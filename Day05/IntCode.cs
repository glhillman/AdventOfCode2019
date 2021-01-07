using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day05
{
    public class IntCode
    {

        public int[] RunIntCode(int[] inCode)
        {
            int[] code = new int[inCode.Length];
            inCode.CopyTo(code, 0);

            int index = 0;
            bool validInput = true;

            while (code[index] != 99 && validInput)
            {
                int instruction = code[index];
                int opcode;
                int mode1;
                int mode2;
                int mode3;

                opcode = instruction % 100;
                instruction /= 100;
                mode1 = instruction % 10;
                instruction /= 10;
                mode2 = instruction % 10;
                instruction /= 10;
                mode3 = instruction % 10;

                switch (opcode)
                {
                    case 1:
                        {
                            int op1 = code[index + 1];
                            int op2 = code[index + 2];

                            if (mode1 == 0)
                            {
                                op1 = code[op1];
                            }
 
                            if (mode2 == 0)
                            {
                                op2 = code[op2];
                            }

                            int sum = op1 + op2;

                            if (mode3 == 0)
                            {
                                code[code[index + 3]] = sum;
                            }
                            else
                            {
                                throw new ArgumentException("unexpected output argument");
                            }
                            index += 4;
                        }
                        break;
                    case 2:
                        {
                            int op1 = code[index + 1];
                            int op2 = code[index + 2];

                            if (mode1 == 0)
                            {
                                op1 = code[op1];
                            }

                            if (mode2 == 0)
                            {
                                op2 = code[op2];
                            }

                            int prod = op1 * op2;

                            if (mode3 == 0)
                            {
                                code[code[index + 3]] = prod;
                            }
                            else
                            {
                                throw new ArgumentException("unexpected output argument");
                            }
                            
                            index += 4;
                        }
                        break;
                    case 3:
                        // save input to position
                        {
                            int input = GetIntCodeInput();

                            int saveLocation = code[index + 1]; // save locations are ALWAYS indirect


                            code[saveLocation] = input;

                            index += 2;
                        }
                        break;
                    case 4:
                        // output input
                        {
                            int output = code[index + 1];
                            if (mode1 == 0)
                            {
                                output = code[output];
                            }

                            IntCodeOutput(output);

                            index += 2;
                        }
                        break;
                    case 5:
                        // jump if true
                        {
                            int value = code[index + 1];
                            if (mode1 == 0)
                            {
                                value = code[value];
                            }
                            int newIndex = code[index + 2];
                            if (mode2 == 0)
                            {
                                newIndex = code[newIndex];
                            }

                            if (value != 0)
                            {
                                index = newIndex;
                            }
                            else
                            {
                                index += 3;
                            }
                        }
                        break;
                    case 6:
                        // jump if false
                        {
                            int value = code[index + 1];
                            if (mode1 == 0)
                            {
                                value = code[value];
                            }
                            int newIndex = code[index + 2];
                            if (mode2 == 0)
                            {
                                newIndex = code[newIndex];
                            }

                            if (value == 0)
                            {
                                index = newIndex;
                            }
                            else
                            {
                                index += 3;
                            }
                        }
                        break;
                    case 7:
                        // less than
                        {
                            int value1 = code[index + 1];
                            if (mode1 == 0)
                            {
                                value1 = code[value1];
                            }
                            int value2 = code[index + 2];
                            if (mode2 == 0)
                            {
                                value2 = code[value2];
                            }
                            int writeIndex = code[index + 3]; // save location is always indirect

                            int valueToWrite = value1 < value2 ? 1 : 0;

                            code[writeIndex] = valueToWrite;

                            index += 4;
                        }
                        break;
                    case 8:
                        // equals
                        {
                            int value1 = code[index + 1];
                            if (mode1 == 0)
                            {
                                value1 = code[value1];
                            }
                            int value2 = code[index + 2];
                            if (mode2 == 0)
                            {
                                value2 = code[value2];
                            }
                            int writeIndex = code[index + 3];

                            int valueToWrite = value1 == value2 ? 1 : 0;

                            code[writeIndex] = valueToWrite;

                            index += 4;
                        }
                        break;
                    default:
                        Console.WriteLine("Unexpected operand: {0} at index {1}", code[index], index);
                        validInput = false;
                        break;
                }
            }

            return code;
        }

        public int GetIntCodeInput()
        {
            int input = 0;
            Console.Write("Enter IntCode Integer: ");
            string strInput = Console.ReadLine();
            if (!int.TryParse(strInput, out input))
            {
                // Parsing failed
                Console.WriteLine("IntCode Input failed!");
            }

            return input;
        }

        public void IntCodeOutput(int output)
        {
            Console.WriteLine("IntCode Output: {0}", output);
        }
    }
}
