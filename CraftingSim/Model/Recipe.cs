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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="successRate"></param>
        /// <param name="requiredMaterials"></param>
        public Recipe(string name, double successRate, Dictionary<IMaterial,
        int> requiredMaterials)
        {
            Name = name;
            SuccessRate = successRate;
            RequiredMaterials = new Dictionary<IMaterial, int>(requiredMaterials);
        }

        public int CompareTo(Recipe other)
        {
            if (other == null)
                return 1;

            return string.Compare(this.Name, other.Name,
            StringComparison.OrdinalIgnoreCase);
        }
    }
}