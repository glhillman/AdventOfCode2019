using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day22
{
    public class Command
    {
        public enum CmdEnum
        {
            NewStack,
            DealIncrement,
            CutN
        };

        public Command(string cmdString)
        {
            int index;
            
            if (cmdString.IndexOf("stack") > 0)
            {
                Cmd = CmdEnum.NewStack;
                Value = 0;
            }
            else if ((index = cmdString.IndexOf("increment")) > 0)
            {
                Cmd = CmdEnum.DealIncrement;
                Value = long.Parse(cmdString.Substring(index + "increment ".Length));
            }
            else if (cmdString.StartsWith("cut"))
            {
                Cmd = CmdEnum.CutN;
                Value = long.Parse(cmdString.Substring("cut ".Length));
            }
            else
            {
                throw new ArgumentException("Unexpected Command");
            }
        }

        public CmdEnum Cmd { get; private set; }
        public long Value { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Cmd, Value);
        }
    }
}
