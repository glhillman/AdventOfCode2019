using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day06
{
    public class Moon
    {
        public List<Moon> _orbitedBy;
        
        public Moon(string name, string orbitsName)
        {
            Name = name;
            OrbitsName = orbitsName;
            Orbits = null;
            _orbitedBy = new List<Moon>();
        }

        public void AddOrbitedBy(Moon orbitedBy)
        {
            _orbitedBy.Add(orbitedBy);
        }

        public int OrbitedByCount
        {
            get
            {
                return _orbitedBy.Count;
            }
        }
        

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Name + " Orbits " + OrbitsName);

            if (_orbitedBy.Count > 0)
            {
                sb.Append(" Orbited by: ");
                foreach (Moon moon in _orbitedBy)
                {
                    sb.Append(moon.Name + ",");
                }
            }

            return sb.ToString();
        }

        public string Name { get; private set; }
        public string OrbitsName { get; private set; }
        public Moon Orbits { get; set; }
    }
}
