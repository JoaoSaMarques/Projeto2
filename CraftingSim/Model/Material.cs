using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CraftingSim.Model
{
    public class Material : IMaterial, IEquatable<Material>
    {
        public int Id { get; }
        public string Name { get; }
    }
}