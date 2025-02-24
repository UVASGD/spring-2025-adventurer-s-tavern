using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "BiomeData", menuName = "ScriptableObjects/BiomeData")]

public class BiomeData : ScriptableObject
{
    // Name of biome to be displayed in the shop, etc.
    [field: SerializeField]
    public string Name { get; set; }

    // Description of biome to be displayed in the shop, etc.
    [field: SerializeField]
    public string Description { get; set; }

    // How much the spell to get to this biome will cost in the shop
    [field:SerializeField]
    public int Price { get; set; }

    // The number of the biome within the order that it appears. E.g. starting biome = 1, next biome = 2, etc.
    [field: SerializeField]
    public int StageNumber { get; set; }

    // The recipe(s) that will show up to be learned by the player through certain Star NPCs
    [field: SerializeField]
    public RecipeData[] RecipesAvailable { get; set; }

    // If star NPCs are getting cut, instead of recipesavailable, make a dictionary of <recipedata, int> to act as a list of recipes that appear in the shop in that biome, along with their costs

    // List of all ingredients to become available in the shop upon entering this biome
    [field: SerializeField]
    public IngredientData[] IngredientsAvailable { get; set; }

    // List of all possible customers that can randomly appear in while in this biome
    [field: SerializeField]
    public CustomerData[] CustomersAvailable { get; set; }


    // Extra variable(s) (not on the trello board):
    // List of all Equipment/Actions that become availible in the shop once the biome is entered
    [field: SerializeField]
    public ActionData[] EquipmentAvailable { get; set; } // Change ActionData to Equipment when the two are merged


    // Extra possible mechanics if we have time:

    // Maximum number of cucstomers that can appear in any given day in this biome
    [field: SerializeField]
    public int MaxCustomers { get; set; }

    // Minimum number of customers that can appear in any given day in this biome
    [field: SerializeField]
    public int MinCustomers { get; set; }

    // Required amount of customers served to unlock the next biome in the shop
    // Could also be the amount required to unlock THIS biome in the shop, depending on implementation
    [field: SerializeField]
    public int RequiredCustomers { get; set; }

    // Same as RequiredCustomers, just with amount of days completed (assuming a day system will be implemented, if not this is useless)
    [field: SerializeField]
    public int RequiredDays { get; set; }

    // Same as RequiredCustomers, jsut with total money earned all time/in this biome, would need a total money earned variable in PlayerManager to be implemented
    [field: SerializeField]
    public int RequiredMoney { get; set; }

    // Any (or none) of the requirements can be implemented as is necesary/possible
}
