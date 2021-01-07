using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCIntCode;

namespace Day23
{
    public class NetNode
    {
        public NetNode(IntCode intCode, long id)
        {
            Id = id;
            Intcode = intCode;
            Intcode.SetNewInputDelegate(Input);
            Intcode.SetNewOutputDelegate(Output);
            _intCodeInput = new Queue<long>();
            _intCodeOutput = new Queue<long>();
            QueueInput(Id);
        }

        public IntCode Intcode { get; private set; }
        public long Id { get; private set; }
        public void QueueInput(long value)
        {
            _intCodeInput.Enqueue(value);
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        private long Input()
        {
            long value;

            if (_intCodeInput.Count > 0)
            {
                value = _intCodeInput.Dequeue();
            }
            else
            {
                value = -1;
            }

            return value;
        }

        private void Output(long value)
        {
            _intCodeOutput.Enqueue(value);
        }
        
        public bool GetOutputPacket(out long id, out long x, out long y)
        {
            bool packetReturned = false;
            id = -1;
            x = -1;
            y = -1;

            if (_intCodeOutput.Count >= 3)
            {
                id = _intCodeOutput.Dequeue();
                x = _intCodeOutput.Dequeue();
                y = _intCodeOutput.Dequeue();
                packetReturned = true;
            }

            return packetReturned;
        }

        private Queue<long> _intCodeInput;
        private Queue<long> _intCodeOutput;
    }
}
