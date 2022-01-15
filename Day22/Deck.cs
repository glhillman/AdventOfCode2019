using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayClass
{
    public class Deck
    {
        private Card[]  _cardIndex;
        private long _deckSize;
        private Card _topCard;
        private Card[] _scratchArray;
        public List<Card> TempList;


        public Deck(long deckSize)
        {
            _deckSize = deckSize;
            _cardIndex = new Card[deckSize];
            _topCard = null;
            _scratchArray = new Card[deckSize];
        }

        public void AddCard(Card card)
        {
            if (_topCard == null)
            {
                _topCard = card;
            }
            else
            {
                Card bottomCard = _topCard.Prev;
                card.Prev = bottomCard;
                bottomCard.Next = card;
                card.Next = _topCard;
                _topCard.Prev = card;
            }

            _cardIndex[card.CardNum] = card; // redundant after first time
        }

        public void NewStack()
        {
            Card newTop = _topCard.Prev;

            foreach (Card card in _cardIndex)
            {
                Card temp = card.Next;
                card.Next = card.Prev;
                card.Prev = temp;
            }

            _topCard = newTop;
        }

        public void DealWithIncrement(long increment)
        {
            Card card = _topCard;
            long index = 0;

            for (long i = 0; i < _deckSize; i++)
            {
                _scratchArray[index] = card;
                index = (index + increment) % _deckSize;
                card = card.Next;
            }
            // rewire cards
            _topCard = _scratchArray[0];
            _topCard.Prev = _topCard;
            _topCard.Next = _topCard;
            for (long i = 1; i < _scratchArray.Length; i++)
            {
                AddCard(_scratchArray[i]);
            }
        }

        public long PositionOfCard(long n)
        {
            long pos = 0;
            Card card = _topCard;
            while (card.CardNum != n)
            {
                card = card.Next;
                pos++;
            }

            return pos;
        }

        public void CutN(long n)
        {
            if (n > 0)
            {
                CutPositiveN(n);
            }
            else if (n < 0)
            {
                CutNegativeN(n);
            }
            // if n == 0 do nothing
        }

        private void CutPositiveN(long n)
        {
            // optimization - divide n / 2 & locate cut point from shortest direction
            for (long i = 0; i < n; i++)
            {
                _topCard = _topCard.Next;
            }
        }

        private void CutNegativeN(long n)
        {
            long absN = -n;

            // optimization - divide n / 2 & locate cut point from shortest direction
            for (long i = 0; i < absN; i++)
            {
                _topCard = _topCard.Prev;
            }
        }


    }
}
