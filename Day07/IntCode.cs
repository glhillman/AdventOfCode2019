using System;
using System.Threading.Channels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day07
{
    public class IntCode
    {
        private const int ADD = 1;
        private const int MULT = 2;
        private const int SAVE_TO_POSITION = 3;
        private const int OUTPUT = 4;
        private const int JUMP_IF_TRUE = 5;
        private const int JUMP_IF_FALSE = 6;
        private const int LESS_THAN = 7;
        private const int EQUALS = 8;

        private int[] _code = null;

        public IntCode(string instanceName, int[] code, Channel<int> inputChannel = null, Channel<int> outputChannel = null)
        {
            InstanceName = instanceName;

            _code = new int[code.Length];
            code.CopyTo(_code, 0);

            // create the channels if they were not specified
            if (inputChannel == null)
            {
                InputChannel = Channel.CreateUnbounded<int>(new UnboundedChannelOptions()
                {
                    SingleWriter = true,
                    SingleReader = true
                });
            }
            else
            {
                InputChannel = inputChannel;
            }

            if (outputChannel == null)
            {
                OutputChannel = Channel.CreateUnbounded<int>(new UnboundedChannelOptions()
                {
                    SingleWriter = true,
                    SingleReader = true
                });
            }
            else
            {
                OutputChannel = outputChannel;
            }
        }

        public string InstanceName { get; private set; }

        public Channel<int> InputChannel { get; set; }
        public Channel<int> OutputChannel { get; set; }

        // external use - feed the IntCode externally
        public async void WriteInputChannel(int value)
        {
            await InputChannel.Writer.WriteAsync(value);
        }

        // external use - retrieve results
        public int ReadOutputChannel()
        {
            int value = ReadOutputChannelInternal().Result;

            return value;
        }

        private async ValueTask<int> ReadOutputChannelInternal()
        {
            int value = await OutputChannel.Reader.ReadAsync();

            return value;
        }

        public int[] RunIntCode()
        {
            int index = 0;
            bool validInput = true;

//            finalOutput = 0; // must initialize an out argument

            while (_code[index] != 99 && validInput)
            {
                int instruction = _code[index];
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
                    case ADD:
                        {
                            int op1 = _code[index + 1];
                            int op2 = _code[index + 2];

                            if (mode1 == 0)
                            {
                                op1 = _code[op1];
                            }

                            if (mode2 == 0)
                            {
                                op2 = _code[op2];
                            }

                            int sum = op1 + op2;

                            if (mode3 == 0)
                            {
                                _code[_code[index + 3]] = sum;
                            }
                            else
                            {
                                throw new ArgumentException("unexpected output argument");
                            }
                            index += 4;
                        }
                        break;
                    case MULT:
                        {
                            int op1 = _code[index + 1];
                            int op2 = _code[index + 2];

                            if (mode1 == 0)
                            {
                                op1 = _code[op1];
                            }

                            if (mode2 == 0)
                            {
                                op2 = _code[op2];
                            }

                            int prod = op1 * op2;

                            if (mode3 == 0)
                            {
                                _code[_code[index + 3]] = prod;
                            }
                            else
                            {
                                throw new ArgumentException("unexpected output argument");
                            }

                            index += 4;
                        }
                        break;
                    case SAVE_TO_POSITION:
                        {
                            int input;
                            int saveLocation = _code[index + 1]; // save locations are ALWAYS indirect

                            input = ReadChannelInput().Result;

                            //    // Console input
                            //    input = GetIntCodeInput();

                            _code[saveLocation] = input;

                            index += 2;
                        }
                        break;
                    case OUTPUT:
                        {
                            int output = _code[index + 1];
                            if (mode1 == 0)
                            {
                                output = _code[output];
                            }

                            WriteChannelOutput(output);
                            //IntCodeOutput(output);

                            index += 2;
                        }
                        break;
                    case JUMP_IF_TRUE:
                        // jump if true
                        {
                            int value = _code[index + 1];
                            if (mode1 == 0)
                            {
                                value = _code[value];
                            }
                            int newIndex = _code[index + 2];
                            if (mode2 == 0)
                            {
                                newIndex = _code[newIndex];
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
                    case JUMP_IF_FALSE:
                        {
                            int value = _code[index + 1];
                            if (mode1 == 0)
                            {
                                value = _code[value];
                            }
                            int newIndex = _code[index + 2];
                            if (mode2 == 0)
                            {
                                newIndex = _code[newIndex];
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
                    case LESS_THAN:
                        {
                            int value1 = _code[index + 1];
                            if (mode1 == 0)
                            {
                                value1 = _code[value1];
                            }
                            int value2 = _code[index + 2];
                            if (mode2 == 0)
                            {
                                value2 = _code[value2];
                            }
                            int writeIndex = _code[index + 3]; // save location is always indirect

                            int valueToWrite = value1 < value2 ? 1 : 0;

                            _code[writeIndex] = valueToWrite;

                            index += 4;
                        }
                        break;
                    case EQUALS:
                        {
                            int value1 = _code[index + 1];
                            if (mode1 == 0)
                            {
                                value1 = _code[value1];
                            }
                            int value2 = _code[index + 2];
                            if (mode2 == 0)
                            {
                                value2 = _code[value2];
                            }
                            int writeIndex = _code[index + 3];

                            int valueToWrite = value1 == value2 ? 1 : 0;

                            _code[writeIndex] = valueToWrite;

                            index += 4;
                        }
                        break;
                    default:
                        Console.WriteLine("Unexpected operand: {0} at index {1}", _code[index], index);
                        validInput = false;
                        break;
                }
            }

            return _code;
        }

        public async void WriteChannelOutput(int value)
        {
            await OutputChannel.Writer.WriteAsync(value);
        }

        public async ValueTask<int> ReadChannelInput()
        {
           int rslt = await InputChannel.Reader.ReadAsync();

           return rslt;
       }
        
        private int GetIntCodeInput()
        {
            int input;

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

        public override string ToString()
        {
            return InstanceName;
        }
    }
}
