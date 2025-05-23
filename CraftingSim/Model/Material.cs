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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public Material(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Material);
        }

        public bool Equals(Material other)
        {
            if (other == null)
                return false;

            return this.Id == other.Id || string.Equals(this.Name, other.Name,
            StringComparison.OrdinalIgnoreCase);
        }

        public bool Equals(IMaterial other)
        {
            throw new NotImplementedException();
        }
    }
}