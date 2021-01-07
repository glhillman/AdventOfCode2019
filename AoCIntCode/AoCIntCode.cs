//#define UseChannels

using System;
#if UseChannels
using System.Threading.Channels;
#endif
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AoCIntCode
{

    public delegate void IntCodeOutput(long value);
    public delegate long IntCodeInput();

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

#if UseChannels

        // Channel i/o
        public IntCode(string instanceName, long[] code, Channel<long> inputChannel = null, Channel<long> outputChannel = null)
        {
            SetCode(instanceName, code);

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
#endif

        // delegate i/o
        public IntCode(string instanceName, long[] code, IntCodeInput inputDelegate, IntCodeOutput outputDelegate)
        {
            SetCode(instanceName, code);

            InputDelegate = inputDelegate;
            OutputDelegate = outputDelegate;
        }

        private void SetCode(string instanceName, long[] code)
        {
            InstanceName = instanceName;

            Code = new long[code.Length * 10]; // extra space at end
            code.CopyTo(Code, 0);

            // save a copy for reset
            _originalCode = new long[code.Length];
            code.CopyTo(_originalCode, 0);
        }

        public void ResetCode()
        {
            for (int i = 0; i < Code.Length; i++)
            {
                Code[i] = 0;
            }
            _originalCode.CopyTo(Code, 0);
        }

        public void SetNewInputDelegate(IntCodeInput inputDelegate)
        {
            InputDelegate = inputDelegate;
        }

        public void SetNewOutputDelegate(IntCodeOutput outputDelegate)
        {
            OutputDelegate = outputDelegate;
        }

        public string InstanceName { get; private set; }

#if UseChannels
        public Channel<long> InputChannel { get; set; }
        public Channel<long> OutputChannel { get; set; }
#endif
        public IntCodeInput InputDelegate { get; private set; }
        public IntCodeOutput OutputDelegate { get; private set; }
        private long[] Code { get; set; }
        private long[] _originalCode { get; set; }

        // external use - feed the IntCode externally
#if UseChannels
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

            if (OutputDelegate == null)
            {
                long value;
                while (OutputChannel.Reader.TryRead(out value))
                {
                    values.Add(value);
                }
            }
            return values;
        }
#endif
        long _codeIndex = 0;
        bool _validInput = true;
        long _relativeBase = 0;

        public long[] RunIntCode()
        {
            _codeIndex = 0;
            _validInput = true;
            _relativeBase = 0;

            Finished = false;

            while (Code[_codeIndex] != 99 && _validInput)
            {
                Step();
            }

#if UseChannels
            if (OutputChannel != null)
            {
                OutputChannel.Writer.Complete();
            }
#endif

            Finished = true;

            return Code;
        }

        public void Step()
        {
            long instruction = Code[_codeIndex];
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
                        long op1 = Code[_codeIndex + 1];
                        long op2 = Code[_codeIndex + 2];
                        long writeIndex = Code[_codeIndex + 3];

                        if (mode1 == POSITION_MODE)
                        {
                            op1 = Code[op1];
                        }
                        else if (mode1 == RELATIVE_MODE)
                        {
                            op1 = Code[_relativeBase + op1];
                        }

                        if (mode2 == POSITION_MODE)
                        {
                            op2 = Code[op2];
                        }
                        else if (mode2 == RELATIVE_MODE)
                        {
                            op2 = Code[_relativeBase + op2];
                        }

                        long sum = op1 + op2;

                        // check for relative
                        if (mode3 == POSITION_MODE)
                        {
                            Code[writeIndex] = sum;
                        }
                        else if (mode3 == RELATIVE_MODE)
                        {
                            Code[_relativeBase + writeIndex] = sum;
                        }
                        else
                        {
                            throw new ArgumentException("unexpected output argument");
                        }
                        _codeIndex += 4;
                    }
                    break;
                case MULT:
                    {
                        long op1 = Code[_codeIndex + 1];
                        long op2 = Code[_codeIndex + 2];
                        long writeIndex = Code[_codeIndex + 3];

                        if (mode1 == POSITION_MODE)
                        {
                            op1 = Code[op1];
                        }
                        else if (mode1 == RELATIVE_MODE)
                        {
                            op1 = Code[_relativeBase + op1];
                        }

                        if (mode2 == POSITION_MODE)
                        {
                            op2 = Code[op2];
                        }
                        else if (mode2 == RELATIVE_MODE)
                        {
                            op2 = Code[_relativeBase + op2];
                        }

                        long prod = op1 * op2;

                        if (mode3 == POSITION_MODE)
                        {
                            Code[writeIndex] = prod;
                        }
                        else if (mode3 == RELATIVE_MODE)
                        {
                            Code[_relativeBase + writeIndex] = prod;
                        }
                        else
                        {
                            throw new ArgumentException("unexpected output argument");
                        }

                        _codeIndex += 4;
                    }
                    break;
                case SAVE_TO_POSITION:
                    {
                        long input = 0;
                        long saveLocation = Code[_codeIndex + 1];

                        if (InputDelegate != null)
                        {
                            input = InputDelegate();
                        }
#if UseChannels
                        else
                        {
                            input = ReadChannelInput().Result;
                        }
#endif
                        if (mode1 == RELATIVE_MODE)
                        {
                            Code[_relativeBase + saveLocation] = input;
                        }
                        else
                        {
                            Code[saveLocation] = input;
                        }

                        _codeIndex += 2;
                    }
                    break;
                case OUTPUT:
                    {
                        long output = Code[_codeIndex + 1];
                        if (mode1 == POSITION_MODE)
                        {
                            output = Code[output];
                        }
                        else if (mode1 == RELATIVE_MODE)
                        {
                            output = Code[_relativeBase + output];
                        }

                        if (OutputDelegate != null)
                        {
                            OutputDelegate(output);
                        }
#if UseChannels
                        else
                        {
                            WriteChannelOutput(output);
                        }
#endif
                        _codeIndex += 2;
                    }
                    break;
                case JUMP_IF_TRUE:
                    // jump if true
                    {
                        long value = Code[_codeIndex + 1];
                        if (mode1 == POSITION_MODE)
                        {
                            value = Code[value];
                        }
                        else if (mode1 == RELATIVE_MODE)
                        {
                            value = Code[_relativeBase + value];
                        }

                        long newIndex = Code[_codeIndex + 2];
                        if (mode2 == POSITION_MODE)
                        {
                            newIndex = Code[newIndex];
                        }
                        else if (mode2 == RELATIVE_MODE)
                        {
                            newIndex = Code[_relativeBase + newIndex];
                        }

                        if (value != 0)
                        {
                            _codeIndex = newIndex;
                        }
                        else
                        {
                            _codeIndex += 3;
                        }
                    }
                    break;
                case JUMP_IF_FALSE:
                    {
                        long value = Code[_codeIndex + 1];
                        if (mode1 == POSITION_MODE)
                        {
                            value = Code[value];
                        }
                        else if (mode1 == RELATIVE_MODE)
                        {
                            value = Code[_relativeBase + value];
                        }

                        long newIndex = Code[_codeIndex + 2];
                        if (mode2 == POSITION_MODE)
                        {
                            newIndex = Code[newIndex];
                        }
                        else if (mode2 == RELATIVE_MODE)
                        {
                            newIndex = Code[_relativeBase + newIndex];
                        }

                        if (value == 0)
                        {
                            _codeIndex = newIndex;
                        }
                        else
                        {
                            _codeIndex += 3;
                        }
                    }
                    break;
                case LESS_THAN:
                    {
                        long value1 = Code[_codeIndex + 1];
                        if (mode1 == POSITION_MODE)
                        {
                            value1 = Code[value1];
                        }
                        else if (mode1 == RELATIVE_MODE)
                        {
                            value1 = Code[_relativeBase + value1];
                        }

                        long value2 = Code[_codeIndex + 2];
                        if (mode2 == POSITION_MODE)
                        {
                            value2 = Code[value2];
                        }
                        else if (mode2 == RELATIVE_MODE)
                        {
                            value2 = Code[_relativeBase + value2];
                        }

                        long writeIndex = Code[_codeIndex + 3];

                        long valueToWrite = value1 < value2 ? 1 : 0;

                        if (mode3 == POSITION_MODE)
                        {
                            Code[writeIndex] = valueToWrite;
                        }
                        else if (mode3 == RELATIVE_MODE)
                        {
                            Code[_relativeBase + writeIndex] = valueToWrite;
                        }
                        else
                        {
                            throw new ArgumentException("Unexpected mode in LESS_THAN");
                        }

                        _codeIndex += 4;
                    }
                    break;
                case EQUALS:
                    {
                        long value1 = Code[_codeIndex + 1];
                        if (mode1 == POSITION_MODE)
                        {
                            value1 = Code[value1];
                        }
                        else if (mode1 == RELATIVE_MODE)
                        {
                            value1 = Code[_relativeBase + value1];
                        }

                        long value2 = Code[_codeIndex + 2];
                        if (mode2 == POSITION_MODE)
                        {
                            value2 = Code[value2];
                        }
                        else if (mode2 == RELATIVE_MODE)
                        {
                            value2 = Code[_relativeBase + value2];
                        }

                        long writeIndex = Code[_codeIndex + 3];

                        long valueToWrite = value1 == value2 ? 1 : 0;

                        if (mode3 == POSITION_MODE)
                        {
                            Code[writeIndex] = valueToWrite;
                        }
                        else if (mode3 == RELATIVE_MODE)
                        {
                            Code[_relativeBase + writeIndex] = valueToWrite;
                        }
                        else
                        {
                            throw new ArgumentException("Unexpected mode in EQUALS");
                        }

                        _codeIndex += 4;
                    }
                    break;
                case ADJUST_RELATIVE_BASE:
                    long offset = Code[_codeIndex + 1];
                    if (mode1 == POSITION_MODE)
                    {
                        offset = Code[offset];
                    }
                    else if (mode1 == RELATIVE_MODE)
                    {
                        offset = Code[offset + _relativeBase];
                    }

                    _relativeBase += offset;

                    _codeIndex += 2;

                    break;
                default:
                    Console.WriteLine("Unexpected operand: {0} at index {1}", Code[_codeIndex], _codeIndex);
                    _validInput = false;
                    break;
            }
        }

        public bool Finished { get; private set; }

        public override string ToString()
        {
            return InstanceName;
        }
    }
}
