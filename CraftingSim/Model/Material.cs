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
    }
}