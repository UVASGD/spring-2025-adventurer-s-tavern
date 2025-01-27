using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "BiomeData", menuName = "ScriptableObjects/BiomeData")]

public class BiomeData : ScriptableObject
{
    // How much the spell to get to this biome will cost in the shop
    [field:SerializeField]
    public int CostOfEntry { get; set; }

    // The number of the biome within the order that it appears. E.g. starting biome = 1, next biome = 2, etc.
    [field: SerializeField]
    public int StageNumber { get; set; }

    // The recipe(s) that will show up to be learned by the player through certain Star NPCs
    [field: SerializeField]
    public RecipeData[] RecipesAvailable { get; set; }

    // If star NPCs are getting cut, instead of recipesavailable, make a dictionary of <recipedata, int> to act as a list of recipes that appear in the shop in that biome, along with their costs
}
