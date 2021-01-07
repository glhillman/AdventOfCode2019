using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day22
{
    public class Card
    {
        public Card(long cardNum)
        {
            CardNum = cardNum;
            Prev = this;
            Next = this;
        }

        public override string ToString()
        {
            return string.Format("Card: {0} Prev: {1}, Next: {2}", CardNum, Prev.CardNum, Next.CardNum);
        }

        public long CardNum;
        public Card Prev;
        public Card Next;
    }
}
