using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeData", menuName = "ScriptableObjects/RecipeData")]
public class RecipeData : ScriptableObject 
{
    /// <summary>
    /// Represents the name of a recipe.
    /// </summary>
    [field: SerializeField]
    public string Name { get; set; }

    /// <summary>
    /// Represents the description of a recipe, providing details or additional context about it.
    /// </summary>
    [field: SerializeField]
    public string Description { get; set; }

    /// <summary>
    /// Represents the price of the recipe.
    /// </summary>
    [field: SerializeField]
    public int Price { get; set; }

    /// <summary>
    /// Represents the visual icon associated with a recipe.
    /// </summary>
    [field: SerializeField]
    public Sprite Icon { get; set; }

    /// <summary>
    /// Defines the sequence of stations that are required to complete a recipe.
    /// </summary>
    /// <remarks>
    /// Each station in the sequence is represented by an instance of the <c>StationData</c> class.
    /// This property determines the specific order in which stations are utilized during the recipe preparation process.
    /// </remarks>
    [field: SerializeField]
    public StationData[] StationSequence { get; set; }

    /// <summary>
    /// Represents the initial stock configuration for each station in the recipe workflow.
    /// </summary>
    /// <remarks>
    /// This property defines a sequence of initial stock setups, where each entry corresponds to the starting
    /// ingredients or items available at a specific station in the production process. Each element in the list
    /// is an instance of <c>InitialStockPerStation</c>, detailing the initial state for that station.
    /// </remarks>
    [field: SerializeField]
    public List<InitialStockPerStation> InitialStockSequence { get; set; } = new List<InitialStockPerStation>();
    // TODO: Add Biome, Station details, maybe NPC
    // Does the Recipe need to know which NPC is associated with it?

    /// <summary>
    /// Represents the sequence of correct stock for each station in a recipe.
    /// </summary>
    /// <remarks>
    /// This property contains a list of <c>CorrectStockPerStation</c> instances, where each instance defines the correct stock ingredients
    /// and their properties required for a specific station in the recipe preparation process.
    /// </remarks>
    [field: SerializeField]
    public List<CorrectStockPerStation> CorrectStockSequence { get; set; } = new List<CorrectStockPerStation>();

    /// <summary>
    /// Represents the initial stock of ingredients available at a specific station
    /// during the recipe preparation process.
    /// </summary>
    /// <remarks>
    /// This class maintains a list of ingredients that are initially stocked
    /// at a particular station, providing the required resources for the
    /// first steps of the recipe execution.
    /// </remarks>
    [Serializable]
    public class InitialStockPerStation
    {
        public List<IngredientData> InitialStock;
    }

    /// <summary>
    /// Defines a collection of specific properties required for an ingredient
    /// to meet its criteria in a recipe preparation process.
    /// </summary>
    /// <remarks>
    /// This class associates an ingredient with a set of properties
    /// (such as "Cut", "Cooked", etc.) to ensure that it satisfies
    /// the requirements of a recipe at a particular point in the cooking sequence.
    /// </remarks>
    [Serializable]
    public class PropertiesPerIngredient
    {
        public List<Property> Properties;
    }

    /// <summary>
    /// Represents the correct stock and properties required at a specific station
    /// for preparing a recipe in a cooking sequence.
    /// </summary>
    /// <remarks>
    /// This class is used to define the ingredients and their required properties
    /// that must be present at any given station to ensure the preparation process
    /// adheres to the recipe specifications.
    /// </remarks>
    /// <example>
    /// Correct stock might include specific ingredients like vegetables,
    /// and properties such as "Cut" or "Cooked", depending on the station's requirements.
    /// </example>
    [Serializable]
    public class CorrectStockPerStation
    {
        public List<IngredientData> CorrectIngredients; 
        public List<PropertiesPerIngredient> CorrectPropertiesPerIngredient; // NOTE:  These two lists SHOULD BE THE SAME LENGTH
    }
    // recipe.CorrectStockSequence[^1].CorrectIngredients -> list of all ingredients in the recipe
    // recipe.CorrectStockSequence[^1].CorrectPropertiesPerIngredient[0->n].Properties -> all the properties that each ingredient(0 to n) needs to have by the end of the order; Sort of a 3D array.
    // CorrectStockSequence -> The correct stocks of ingredients & properties for each station; CorrectPropertiesPerIngredient -> Each ingredient has a list of properties that it needs to have by the end (cut, etc.)
    // Properties -> the list of properties of one ingredient
}
