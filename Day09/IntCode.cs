﻿using System;
using System.Threading.Channels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day09
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
        private const int ADJUST_RELATIVE_BASE = 9;

        private const int POSITION_MODE = 0;
        private const int IMMEDIATE_MODE = 1;
        private const int RELATIVE_MODE = 2;


        private long[] _code = null;

        public IntCode(string instanceName, long[] code, Channel<long> inputChannel = null, Channel<long> outputChannel = null)
        {
            InstanceName = instanceName;

            _code = new long[code.Length * 10]; // extra space at end
            code.CopyTo(_code, 0);

            // create the channels if they were not specified
            if (inputChannel == null)
            {
                InputChannel = Channel.CreateUnbounded<long>(new UnboundedChannelOptions()
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
                OutputChannel = Channel.CreateUnbounded<long>(new UnboundedChannelOptions()
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

        public Channel<long> InputChannel { get; set; }
        public Channel<long> OutputChannel { get; set; }

        // external use - feed the IntCode externally
        public async void WriteInputChannel(long value)
        {
            await InputChannel.Writer.WriteAsync(value);
        }

        // external use - retrieve results
        public long ReadOutputChannel()
        {
            long value = ReadOutputChannelInternal().Result;

            return value;
        }

        private async ValueTask<long> ReadOutputChannelInternal()
        {
            long value = await OutputChannel.Reader.ReadAsync();

            return value;
        }

        public long[] RunIntCode()
        {
            long index = 0;
            bool validInput = true;
            long relativeBase = 0;

            while (_code[index] != 99 && validInput)
            {
                long instruction = _code[index];
                long opcode;
                long mode1;
                long mode2;
                long mode3;

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
                            long op1 = _code[index + 1];
                            long op2 = _code[index + 2];
                            long writeIndex = _code[index + 3];

                            if (mode1 == POSITION_MODE)
                            {
                                op1 = _code[op1];
                            }
                            else if (mode1 == RELATIVE_MODE)
                            {
                                op1 = _code[relativeBase + op1];
                            }

                            if (mode2 == POSITION_MODE)
                            {
                                op2 = _code[op2];
                            }
                            else if (mode2 == RELATIVE_MODE)
                            {
                                op2 = _code[relativeBase + op2];
                            }

                            long sum = op1 + op2;

                            // check for relative
                            if (mode3 == POSITION_MODE)
                            {
                                _code[writeIndex] = sum;
                            }
                            else if (mode3 == RELATIVE_MODE)
                            {
                                _code[relativeBase + writeIndex] = sum;
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
                            long op1 = _code[index + 1];
                            long op2 = _code[index + 2];
                            long writeIndex = _code[index + 3];

                            if (mode1 == POSITION_MODE)
                            {
                                op1 = _code[op1];
                            }
                            else if (mode1 == RELATIVE_MODE)
                            {
                                op1 = _code[relativeBase + op1];
                            }

                            if (mode2 == POSITION_MODE)
                            {
                                op2 = _code[op2];
                            }
                            else if (mode2 == RELATIVE_MODE)
                            {
                                op2 = _code[relativeBase + op2];
                            }

                            long prod = op1 * op2;

                            if (mode3 == POSITION_MODE)
                            {
                                _code[writeIndex] = prod;
                            }
                            else if (mode3 == RELATIVE_MODE)
                            {
                                _code[relativeBase + writeIndex] = prod;
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
                            long input;
                            long saveLocation = _code[index + 1];

                            input = ReadChannelInput().Result;

                            if (mode1 == RELATIVE_MODE)
                            {
                                _code[relativeBase + saveLocation] = input;
                            }
                            else
                            {
                                _code[saveLocation] = input;
                            }

                            index += 2;
                        }
                        break;
                    case OUTPUT:
                        {
                            long output = _code[index + 1];
                            if (mode1 == POSITION_MODE)
                            {
                                output = _code[output];
                            }
                            else if (mode1 == RELATIVE_MODE)
                            {
                                output = _code[relativeBase + output];
                            }
                            else
                            {
                                throw new ArgumentException("Unexpected mode argument in OUTPUT");
                            }

                            WriteChannelOutput(output);

                            index += 2;
                        }
                        break;
                    case JUMP_IF_TRUE:
                        // jump if true
                        {
                            long value = _code[index + 1];
                            if (mode1 == POSITION_MODE)
                            {
                                value = _code[value];
                            }
                            else if (mode1 == RELATIVE_MODE)
                            {
                                value = _code[relativeBase + value];
                            }

                            long newIndex = _code[index + 2];
                            if (mode2 == POSITION_MODE)
                            {
                                newIndex = _code[newIndex];
                            }
                            else if (mode2 == RELATIVE_MODE)
                            {
                                newIndex = _code[relativeBase + newIndex];
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
                            long value = _code[index + 1];
                            if (mode1 == POSITION_MODE)
                            {
                                value = _code[value];
                            }
                            else if (mode1 == RELATIVE_MODE)
                            {
                                value = _code[relativeBase + value];
                            }

                            long newIndex = _code[index + 2];
                            if (mode2 == POSITION_MODE)
                            {
                                newIndex = _code[newIndex];
                            }
                            else if (mode2 == RELATIVE_MODE)
                            {
                                newIndex = _code[relativeBase + newIndex];
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
                            long value1 = _code[index + 1];
                            if (mode1 == POSITION_MODE)
                            {
                                value1 = _code[value1];
                            }
                            else if (mode1 == RELATIVE_MODE)
                            {
                                value1 = _code[relativeBase + value1];
                            }

                            long value2 = _code[index + 2];
                            if (mode2 == POSITION_MODE)
                            {
                                value2 = _code[value2];
                            }
                            else if (mode2 == RELATIVE_MODE)
                            {
                                value2 = _code[relativeBase + value2];
                            }

                            long writeIndex = _code[index + 3];

                            long valueToWrite = value1 < value2 ? 1 : 0;

                            if (mode3 == POSITION_MODE)
                            {
                                _code[writeIndex] = valueToWrite;
                            }
                            else if (mode3 == RELATIVE_MODE)
                            {
                                _code[relativeBase + writeIndex] = valueToWrite;
                            }
                            else
                            {
                                throw new ArgumentException("Unexpected mode in LESS_THAN");
                            }

                            index += 4;
                        }
                        break;
                    case EQUALS:
                        {
                            long value1 = _code[index + 1];
                            if (mode1 == POSITION_MODE)
                            {
                                value1 = _code[value1];
                            }
                            else if (mode1 == RELATIVE_MODE)
                            {
                                value1 = _code[relativeBase + value1];
                            }

                            long value2 = _code[index + 2];
                            if (mode2 == POSITION_MODE)
                            {
                                value2 = _code[value2];
                            }
                            else if (mode2 == RELATIVE_MODE)
                            {
                                value2 = _code[relativeBase + value2];
                            }

                            long writeIndex = _code[index + 3];

                            long valueToWrite = value1 == value2 ? 1 : 0;

                            if (mode3 == POSITION_MODE)
                            {
                                _code[writeIndex] = valueToWrite;
                            }
                            else if (mode3 == RELATIVE_MODE)
                            {
                                _code[relativeBase + writeIndex] = valueToWrite;
                            }
                            else
                            {
                                throw new ArgumentException("Unexpected mode in EQUALS");
                            }

                            index += 4;
                        }
                        break;
                    case ADJUST_RELATIVE_BASE:
                        long offset = _code[index + 1];
                        if (mode1 == POSITION_MODE)
                        {
                            offset = _code[offset];
                        }
                        else if (mode1 == RELATIVE_MODE)
                        {
                            offset = _code[offset + relativeBase];
                        }

                        relativeBase += offset;

                        index += 2;

                        break;
                    default:
                        Console.WriteLine("Unexpected operand: {0} at index {1}", _code[index], index);
                        validInput = false;
                        break;
                }
            }

            OutputChannel.Writer.Complete();

            return _code;
        }

        public async void WriteChannelOutput(long value)
        {
            await OutputChannel.Writer.WriteAsync(value);
        }

        public async ValueTask<long> ReadChannelInput()
        {
            long rslt = await InputChannel.Reader.ReadAsync();

           return rslt;
       }
        
       public List<long> ReadFinalResults()
        {
            List<long> values = new List<long>();
            long value;
            while (OutputChannel.Reader.TryRead(out value))
            {
                values.Add(value);
            }

            return values;
        }

        public override string ToString()
        {
            return InstanceName;
        }
    }
}
