using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    public class Pattern
    {
        public Pattern(int[] pattern, int inputLength)
        {
            _pattern = new int[pattern.Length];
            pattern.CopyTo(_pattern, 0);
            _inputLength = inputLength;
            ReSetCycle();
        }

        public int NextValue
        {
            get
            {
                int value = _pattern[_patternIndex];
                _cycleCount++;
                _inputIndex++;

                if (_inputIndex == _inputLength)
                {
                    _cycleSize++;
                    _cycleCount = 1;
                    _inputIndex = 0;
                    _patternIndex = 0;

                }
                if (_cycleCount > _cycleSize)
                {
                    _cycleCount = 0;
                    _patternIndex++;
                    if (_patternIndex >= _pattern.Length)
                    {
                        _patternIndex = 0;
                    }
                }

                return value;
            }
        }

        public void ReSetCycle()
        {
            _cycleSize = 0;
            _cycleCount = 0;
            _patternIndex = 1;
            _inputIndex = 0;
        }

        private int[] _pattern;
        private int _inputLength;
        private int _inputIndex;
        private int _patternIndex;
        private int _cycleCount;
        private int _cycleSize;

    }
}
