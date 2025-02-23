using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeData", menuName = "ScriptableObjects/RecipeData")]
public class RecipeData : ScriptableObject 
{

    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public string Description { get; set; }

    [field: SerializeField]
    public int Price { get; set; }

    [field: SerializeField]
    public Sprite Icon { get; set; }

    [field: SerializeField]
    public Dictionary<IngredientData, List<Property>> SelectedIngredients { get; set; } = new Dictionary<IngredientData, List<Property>>();

    [field: SerializeField]
    public StationData[] StationSequence { get; set; }

    [field: SerializeField]
    public InitialStockPerStation[] InitialStockSequence { get; set; }
    // TODO: Add Biome, Station details, maybe NPC
    // Does the Recipe need to know which NPC is associated with it?

    public struct InitialStockPerStation
    {
        public List<IngredientData> InitialStock;
    }
}
