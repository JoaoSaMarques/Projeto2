using System;

namespace CraftingSim.Model
{
    public class Material : IMaterial, IEquatable<IMaterial>
    {
        public int Id { get; }
        public string Name { get; }

        public Material(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IMaterial);
        }

        public bool Equals(IMaterial other)
        {
            if (other == null)
                return false;

            return this.Id == other.Id || string.Equals(this.Name, other.Name,
            StringComparison.OrdinalIgnoreCase);
        }

        // Recipes são ordenadas pelo nome (case insensitive).
        public override int GetHashCode()
        {
            int hashId = Id.GetHashCode();
            int hashName = Name?.ToLowerInvariant().GetHashCode() ?? 0;
            return hashId ^ hashName;
        }
    }
}
