using System;
using System.Collections.Generic;


namespace CraftingSim.Model
{
    /// <summary>
    /// Implementation of ICrafter. 
    /// </summary>
    public class Crafter : ICrafter
    {
        private readonly Inventory inventory;
        private readonly List<IRecipe> recipeList;

        public Crafter(Inventory inventory)
        {
            this.inventory = inventory;
            recipeList = new List<IRecipe>();
        }

        /// <summary>
        /// returns a read only list of loaded recipes.
        /// </summary>
        public IEnumerable<IRecipe> RecipeList => recipeList;

        /// <summary>
        /// Loads recipes from the files.
        /// Must parse the name, success rate, required materials and
        /// necessary quantities.
        /// </summary>
        /// <param name="recipeFiles">Array of file paths</param>
        public void LoadRecipesFromFile(string[] recipeFiles)
        {
            recipeList.Clear();

            foreach (string filePath in recipeFiles)
            {
                if (!System.IO.File.Exists(filePath))
                    continue;

                string[] lines = System.IO.File.ReadAllLines(filePath);
                string name = null;
                double successRate = 0.0;
                Dictionary<IMaterial, int> requiredMaterials = new Dictionary<IMaterial, int>();

                bool materialsSection = false;

                // This will trim everything to be short
                foreach (string line in lines)
                {
                    string trimmed = line.Trim();

                    if (string.IsNullOrEmpty(trimmed))
                        continue;

                    if (trimmed.StartsWith("Name:", StringComparison.OrdinalIgnoreCase))
                    {
                        name = trimmed.Substring(5).Trim();
                    }
                    else if (trimmed.StartsWith("SuccessRate:", StringComparison.OrdinalIgnoreCase))
                    {
                        double.TryParse(trimmed.Substring(12).Trim(), out successRate);
                    }
                    else if (trimmed.StartsWith("Materials:", StringComparison.OrdinalIgnoreCase))
                    {
                        materialsSection = true;
                    }
                    else if (materialsSection)
                    {
                        // Expecting lines like "<material name>: <quantity>"
                        string[] parts = trimmed.Split(':');
                        if (parts.Length == 2)
                        {
                            string materialName = parts[0].Trim();
                            int quantity = 0;
                            int.TryParse(parts[1].Trim(), out quantity);

                            // Find material in inventory by name
                            IMaterial material = null;
                            foreach (var m in inventory.Materials)
                            {
                                if (string.Equals(m.Name, materialName, StringComparison.OrdinalIgnoreCase))
                                {
                                    material = m;
                                    break;
                                }
                            }

                            if (material != null && quantity > 0)
                            {
                                requiredMaterials[material] = quantity;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(name))
                {
                    IRecipe recipe = new Recipe(name, successRate, requiredMaterials);
                    recipeList.Add(recipe);
                }
            }

            // Sort the recipe list by name (case insensitive)
            recipeList.Sort((r1, r2) => string.Compare(r1.Name, r2.Name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Attempts to craft an item from a given recipe. Consumes inventory 
        /// materials and returns the result message.
        /// </summary>
        /// <param name="recipeName">Name of the recipe to craft</param>
        /// <returns>A message indicating success, failure, or error</returns>
        public string CraftItem(string recipeName)
        {
            IRecipe selected = null;

            for (int i = 0; i < recipeList.Count; i++)
            {
                if (recipeList[i].Name.Equals(recipeName,
                        StringComparison.OrdinalIgnoreCase))
                {
                    selected = recipeList[i];
                    break;
                }
            }
            
            if (selected == null)
                return "Recipe not found.";

            foreach (KeyValuePair<IMaterial, int> required in selected.RequiredMaterials)
            {
                IMaterial material = required.Key;
                int need = required.Value;
                int have = inventory.GetQuantity(material);

                if (have < need)
                {
                    if (have == 0)
                    {
                        return "Missing material: " + material.Name;
                    }
                    return "Not enough " + material.Name +
                           " (need " + need +
                           ", have " + have + ")";
                }
            }

            foreach (KeyValuePair<IMaterial, int> required in selected.RequiredMaterials)
                if (!inventory.RemoveMaterial(required.Key, required.Value))
                    return "Not enough materials";

            Random rng = new Random();
            if (rng.NextDouble() < selected.SuccessRate)
                return "Crafting '" + selected.Name + "' succeeded!";
            else
                return "Crafting '" + selected.Name + "' failed. Materials lost.";

        }
    }
}