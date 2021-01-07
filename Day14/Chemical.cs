using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day14
{
    public class Ingredient
    {
        public Ingredient(string name, long required)
        {
            Name = name;
            Required = required;
        }

        public string Name; // { get; private set; }
        public long Required; // { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Name, Required);
        }
    }
    
    public class Chemical
    {
        public Chemical(string name, long nOut)
        {
            Name = name;
            Out = nOut;
            In = new List<Ingredient>();
        }

        public void AddIngredient(Ingredient ingredient)
        {
            In.Add(ingredient);
        }

        public override string ToString()
        {
            return string.Format("{0} Out: {1}", Name, Out);
        }

        public string Name; // { get; private set; }
        public long Out; // { get; private set; }
        public List<Ingredient> In; // { get; private set; }
    }
}
