using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day06
{
    public class Day06
    {
        private List<Moon> _moons;

        public Day06()
        {
            ReadOrbitData();
            LinkOrbitData();

        }

        public void Puzzle1()
        {
            int totalOrbits = 0;

            var terminals = _moons.Where(m => m.OrbitedByCount == 0);
            
            foreach (Moon moon in _moons)
            {
                //  travserse back to orgin, counting steps;
                int steps = string.IsNullOrEmpty(moon.OrbitsName) ? 0 : 1;
                Moon tempMoon = moon;
                while (tempMoon.Orbits != null)
                {
                    tempMoon = tempMoon.Orbits;
                    steps++;
                }
                totalOrbits += steps;
            }

            Console.WriteLine("Puzzle1 Total orbits = {0}", totalOrbits);
        }

        public void Puzzle2()
        {
            int steps = FindShortRoute();

            Console.WriteLine("Puzzle2 Total steps = {0}", steps);
        }

        
        public int FindShortRoute()
        {
            List<string> youRoute = new List<string>();
            List<string> sanRoute = new List<string>();

            Moon you = _moons.FirstOrDefault(m => m.Name == "YOU");
            Moon san = _moons.FirstOrDefault(m => m.Name == "SAN");

            int steps = 0;
            string common = null;

            Moon curr = you.Orbits;
            while (curr.Orbits != null)
            {
                youRoute.Add(curr.OrbitsName);
                curr = curr.Orbits;
            }

            curr = san.Orbits;
            while (curr.Orbits != null)
            {
                if (youRoute.FirstOrDefault(m => m == curr.OrbitsName) != null)
                {
                    steps++;
                    common = curr.OrbitsName;
                    break;
                }
                else
                {
                    curr = curr.Orbits;
                    steps++;
                }
            }

            // backtrack
            curr = you.Orbits;
            while (curr.Name != common)
            {
                steps++;
                curr = curr.Orbits;
            }

            return steps;

        }


        private void LinkOrbitData()
        {
            // first, set the 'orbits' field for each entry
            foreach (Moon moon in _moons)
            {
                Moon orbits = _moons.FirstOrDefault(m => m.Name == moon.OrbitsName);
                if (orbits != null)
                {
                    moon.Orbits = orbits;
                }
            }

            // COM won't have been created - do it now;
            Moon com = _moons.FirstOrDefault(m => m.Name == "COM");
            if (com == null)
            {
                _moons.Add(new Moon("COM", string.Empty)); // doesn't orbit anything
            }

            //now spin through, linking up the orbitedBy list
            foreach (Moon moon in _moons)
            {
                var tempMoons = _moons.Where(m => m.OrbitsName == moon.Name);
                if (tempMoons != null && moon.Orbits != null)
                {
                    foreach (Moon tempMoon in tempMoons)
                    {
                        moon.AddOrbitedBy(tempMoon);
                    }
                }
            }

        }
        
        private void ReadOrbitData()
        {
            _moons = new List<Moon>();

            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\OrbitData.txt";

            if (File.Exists(inputFile))
            {
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(inputFile);

                while ((line = file.ReadLine()) != null)
                {
                    string[] parts = line.Split(')');

                    _moons.Add(new Moon(parts[1], parts[0]));
                }

                file.Close();
            }
        }
    }
}
