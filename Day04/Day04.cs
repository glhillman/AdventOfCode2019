using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day04
{
    public class Day04
    {
        public void Puzzle1()
        {
            int validCount = 0;

            for (int pwd = 246515; pwd <= 739105; pwd++)
            {
                string pwdString = pwd.ToString();

                if (DigitsAscending(pwdString) && DigitsRepeat(pwdString))
                {
                    validCount++;
                }
            }

            Console.WriteLine("Puzzle1: {0}", validCount);
        }

        public void Puzzle2()
        {
            int validCount = 0;

            for (int pwd = 246515; pwd <= 739105; pwd++)
            {
                string pwdString = pwd.ToString();

                if (DigitsAscending(pwdString) && ContainsADoubleDigit(pwdString))
                {
                    validCount++;
                }
            }

            Console.WriteLine("Puzzle2: {0}", validCount);
        }

        private bool DigitsAscending(string digits)
        {
            bool digitsAscend = true;

            for (int i = 0; i < digits.Length-1 && digitsAscend; i++)
            {
                if (digits[i+1] < digits[i])
                {
                    digitsAscend = false;
                }
            }

            return digitsAscend;
        }

        private bool DigitsRepeat(string digits)
        {
            bool digitsRepeat = false;

            for (int i = 0; i < digits.Length-1 && !digitsRepeat; i++)
            {
                if (digits[i] == digits[i+1])
                {
                    digitsRepeat = true;
                }
            }

            return digitsRepeat;
        }

        private bool ContainsADoubleDigit(string digits)
        {
            bool doubleDigits = false;

            // digits string is 6 digits long
            // this function is called only after we have determined the digits ascend
            // Check for leading double
            if (digits[0] == digits[1] && digits[0] != digits[2])
            {
                doubleDigits = true;
            }
            else if (digits[4] == digits[5] && digits[3] != digits[4]) // check tail
            {
                doubleDigits = true;
            }
            // check for pairs between digits[1]..digits[4]
            // only three possibilities 1-2, 2-3, 3-4
            else
            {
                for (int i = 1; i <= 3 && !doubleDigits; i++)
                {
                    if (digits[i] == digits[i+1] && digits[i] != digits[i-1] && digits[i] != digits[i+2])
                    {
                        doubleDigits = true;
                    }
                }
            }

            return doubleDigits;
        }
    }
}
