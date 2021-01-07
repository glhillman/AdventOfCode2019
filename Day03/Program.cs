using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day03
{
     class Program
    {
        static void Main(string[] args)
        {
            Day03 day3 = new Day03();

            day3.InitializeWires();
            
            day3.Puzzel1();

            day3.Puzzel2();
        }
    }
}
