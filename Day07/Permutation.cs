using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day07
{
    public class Permutation
    {
        List<int[]> _permutations;
        int _permutationIndex = 0;

        public Permutation(int[] values)
        {
            _permutations = new List<int[]>();
            CalculatePermutations(values, 0, values.Length - 1);
        }

        public int[] NextPermutation()
        {
            return _permutationIndex < _permutations.Count ? _permutations[_permutationIndex++] : null;
        }

        private void CalculatePermutations(int[] list, int start, int end)
        {
            int i;
            if (start == end)
            {
                int[] permutation = new int[list.Length];
                list.CopyTo(permutation, 0);
                _permutations.Add(permutation);
            }
            else
            {
                for (i = start; i <= end; i++)
                {
                    IntSwap(ref list[start], ref list[i]);
                    CalculatePermutations(list, start + 1, end);
                    IntSwap(ref list[start], ref list[i]);
                }
            }
        }

        private void IntSwap(ref int int1, ref int int2)
        {
            int tmp = int1;
            int1 = int2;
            int2 = tmp;
        }

    }
}
