using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CraftingSim.Model
{
    public class Recipe : IRecipe, IComparable<Recipe>
    {
        public string Name { get; }
        public double SuccessRate { get; }
        public IReadOnlyDictionary<IMaterial, int> RequiredMaterials { get; }
    }
}