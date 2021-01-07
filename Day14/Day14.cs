using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day14
{
    public class Day14
    {

        public Dictionary<string, Chemical> ChemicalMap;

        public Day14()
        {
            ChemicalMap = new Dictionary<string, Chemical>();
            LoadData();
        }

        public void Part1()
        {
            long oreRequired = OreRequired(1);
            
            Console.WriteLine("Part1: {0}", oreRequired);
        }

        public void Part2()
        {
            long trillion = 1_000_000_000_000;

            long oreRequired = OreRequired(1);
            long low = trillion / oreRequired;
            long high = low * 2;
            long mid = (high - low) / 2 + low;

            // use a binary reduction to hit & miss
            while (mid < high && mid > low)
            {
                oreRequired = OreRequired(mid);
                double factor = trillion / (double)oreRequired;
                if (factor > 1)
                {
                    low = mid;
                    mid = (high - mid) / 2 + mid;
                }
                else
                {
                    high = mid;
                    mid = (mid - low) / 2 + low;
                }
            }

            Console.WriteLine("Part2: {0}", mid);

        }

        // full disclaimer - My original solution was recursive. It made perfect sense to me, and was extremely fast to find 1 unit of fuel.
        // It was extremely slow to find multiple units of fuel as it completely recalculated for each fuel unit. I could never figure out
        // how to make it adjust for multiples.
        // I found a python solution that was iterative, and it made sense to me. This solution is based on that example.
        private long OreRequired(long fuelUnits)
        {
            Dictionary<string, long> chemNeeds = new Dictionary<string, long>();
            Dictionary<string, long> chemHave = new Dictionary<string, long>();

            chemNeeds["FUEL"] = fuelUnits;
            long ore = 0;

            while (chemNeeds.Count > 0)
            {
                string chem = chemNeeds.Keys.ToArray()[0];
                if (chemHave.ContainsKey(chem) == false)
                {
                    chemHave[chem] = 0;
                }
                if (chemNeeds[chem] <= chemHave[chem])
                {
                    chemHave[chem] -= chemNeeds[chem];
                    chemNeeds.Remove(chem);
                    continue;
                }

                long numNeeded = chemNeeds[chem] - chemHave[chem];
                chemHave.Remove(chem);
                chemNeeds.Remove(chem);
                long numProduced = ChemicalMap[chem].Out;

                long numReactions;
                if (Math.Floor((double)numNeeded / numProduced) * numProduced == numNeeded)
                {
                    numReactions = (long)Math.Floor((double)numNeeded / numProduced);
                }
                else
                {
                    numReactions = (long)Math.Floor((double)numNeeded / numProduced) + 1;
                }

                if (chemHave.ContainsKey(chem) == false)
                {
                    chemHave[chem] = 0;
                }
                chemHave[chem] += (numReactions * numProduced) - numNeeded;
                foreach (Ingredient ingredient in ChemicalMap[chem].In)
                {
                    if (ingredient.Name == "ORE")
                    {
                        ore += ingredient.Required * numReactions;
                    }
                    else
                    {
                        if (chemNeeds.ContainsKey(ingredient.Name) == false)
                        {
                            chemNeeds[ingredient.Name] = 0;
                        }
                        chemNeeds[ingredient.Name] += ingredient.Required * numReactions;
                    }
                }
            }

            return ore;
        }

        private Chemical ParseChemical(string str)
        {
            int index = str.IndexOf("=>");
            int required;
            string name;

            ParseQuantityAndName(str.Substring(index + 3), out name, out required);
            Chemical chemical = new Chemical(name, required);

            str = str.Substring(0, index-1);
            string[] pairs = str.Split(',');
            
            foreach (string pair in pairs)
            {
                ParseQuantityAndName(pair, out name, out required);
                Ingredient ingredient = new Ingredient(name, required);
                chemical.AddIngredient(ingredient);
            }
            return chemical;
        }

        private void ParseQuantityAndName(string pair, out string name, out int required)
        {
            string[] values = pair.Trim().Split(' ');
            required = int.Parse(values[0]);
            name = values[1];
        }

        private void LoadData()
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\input.txt";

            if (File.Exists(inputFile))
            {
                string line;
                StreamReader file = new StreamReader(inputFile);
                while ((line = file.ReadLine()) != null)
                {
                    Chemical chemical = ParseChemical(line);
                    ChemicalMap[chemical.Name] = chemical;
                }

                file.Close();
            }

        }
    }
}
