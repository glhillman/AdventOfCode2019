using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day23
{
    public class NAT
    {
        private NetNode _netNode0;
        private int[] _activeNetNodes;
        private long _currX;
        private long _currY;
        private long _deliveredY;
        
        public NAT(int nNodes, NetNode node0)
        {
            _netNode0 = node0;
            _activeNetNodes = new int[nNodes];
            _currX = 0;
            _currY = 0;
            _deliveredY = 0;

            for (int i = 0; i < nNodes; i++)
            {
                _activeNetNodes[i] = 0;
            }
        }

        public void SetXY(long x, long y)
        {
            _currX = x;
            _currY = y;
        }

        public long SetActiveStatus(long id, bool status)
        {
            long targetY = long.MinValue;

            if (status == true)
                _activeNetNodes[id] = 0;
            else
                _activeNetNodes[id]++;

            int active = _activeNetNodes.Count(n => n > 675); // arbitrary number to ensure idle state
            if (active == _activeNetNodes.Length)
            {
                _netNode0.QueueInput(_currX);
                _netNode0.QueueInput(_currY);
                if (_currY == _deliveredY)
                {
                    targetY = _currY; // setting target will terminate this process
                }
                else
                {
                    _deliveredY = _currY;
                    for (int i = 0; i < _activeNetNodes.Length; i++)
                    {
                        _activeNetNodes[i] = 0;
                    }
                }
            }

            return targetY;
        }
    }
}
