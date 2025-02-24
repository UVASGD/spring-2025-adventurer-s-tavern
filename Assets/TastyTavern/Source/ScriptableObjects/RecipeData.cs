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
    public List<IngredientData> Ingredients { get; set; } = new List<IngredientData>();

    [field: SerializeField]
    public List<PropertiesPerIngredient> Properties { get; set; } = new List<PropertiesPerIngredient>();

    [field: SerializeField]
    public StationData[] StationSequence { get; set; }

    [field: SerializeField]
    public List<InitialStockPerStation> InitialStockSequence { get; set; } = new List<InitialStockPerStation>();
    // TODO: Add Biome, Station details, maybe NPC
    // Does the Recipe need to know which NPC is associated with it?

    [Serializable]
    public class InitialStockPerStation
    {
        public List<IngredientData> InitialStock;
    }

    [Serializable]
    public class PropertiesPerIngredient
    {
        public List<Property> Properties;
    }
}
