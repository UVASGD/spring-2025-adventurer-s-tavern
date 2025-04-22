using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class Order
{
    // [field: SerializeField]
    // public int OrderSlot { get; set; }

    // the customer that ordered the order
    [field: SerializeField]
    public Customer Customer { get; set; }

    // The recipe of the order
    [field: SerializeField]
    public RecipeData Recipe { get; set; }

    // The exact ingredients of the order with no properties. Properties list inside it will be updated as the order progresses. 
    [field: SerializeField]
    public Dictionary<IngredientData, List<Property>> CurrentIngredients { get; set; } = new(); 

    // Whether the order is served or not
    [field: SerializeField]
    public bool Served { get; set; }

    [field: SerializeField]
    public Station Station { get; set; }

    [field: SerializeField]
    public int StationIdx { get; set; }

    [field: SerializeField]
    public CookingUIEventChannel cookingUIEventChannel { get; set; }

    public Order(Customer customer, RecipeData recipe, CookingUIEventChannel cookingUIEventChannel)
    {
        this.Customer = customer;
        this.Recipe = recipe;
        this.cookingUIEventChannel = cookingUIEventChannel;
        Reinitialize();
    }

    private void Reinitialize()
    {
        Served = false;
        StationIdx = 0;
        Station = Recipe.StationSequence[0]
            .Create(Recipe.InitialStockSequence[StationIdx].InitialStock, cookingUIEventChannel);
        var correctIngredients = Recipe.CorrectStockSequence[^1].CorrectIngredients;
        CurrentIngredients.Clear();
        // resetting currentingredients to have blank, unprocessed ingredients
        foreach (var ingredientdata in correctIngredients)
        {
            CurrentIngredients.Add(ingredientdata, new List<Property>());
        }
        
    }

    // Order manager triggers station change
    // for now assuming button is not present at last station
    public void ChangeStation(){
        StationIdx++;
        Station.ProgressStation(Recipe.StationSequence[StationIdx],Recipe.InitialStockSequence[StationIdx].InitialStock);
    }

    public void ResetStation()
    {
        Reinitialize();
    }
    
    // recipe.CorrectStockSequence[^1].CorrectIngredients -> list of all ingredients in the recipe
    // recipe.CorrectStockSequence[^1].CorrectPropertiesPerIngredient[0->n].Properties -> all the properties that each ingredient(0 to n) needs to have by the end of the order; Sort of a 3D array.
    // CorrectStockSequence -> The correct stocks of ingredients & properties for each station;
    // CorrectPropertiesPerIngredient (A list of property lists) -> Each ingredient has a list of properties that it needs to have by the end (cut, etc.)
    // Properties -> the list of properties of one ingredient
    /// <summary>
    /// Determines whether the current state of the order matches the expected recipe's requirements, including
    /// ingredients and their associated properties.
    /// </summary>
    /// <returns>
    /// True if the order is correct and meets the recipe's final requirements, including correct ingredients
    /// and their properties; otherwise, false.
    /// </returns>
    public int IsCorrect()
    {
        int totalPenalty = 0;
        var expectedIngredients = Recipe.CorrectStockSequence[StationIdx].CorrectIngredients;
        var expectedPropertiesPerIngredient = Recipe.CorrectStockSequence[StationIdx].CorrectPropertiesPerIngredient;

        var missingIngredientsCount = expectedIngredients.Count - CurrentIngredients.Count;
        if (missingIngredientsCount > 0)
        {
            // High penalty for missing ingredients
            totalPenalty += missingIngredientsCount * 10;
            Debug.Log("Missing Ingredient!");
        }
        else if (missingIngredientsCount < 0)
        {
            // Lower penalty for extra ingredients
            totalPenalty += Math.Abs(missingIngredientsCount) * 5;
            Debug.Log("Extra Ingredient!");
        }

        // Iterate over each expected ingredient
        for (int i = 0; i < expectedIngredients.Count; i++)
        {
            IngredientData expectedIngredient = expectedIngredients[i];
            List<Property> idealProperties = expectedPropertiesPerIngredient[i].Properties;

            // Ensure that the current ingredients dictionary has this ingredient.
            // If not, add full penalty for missing properties.
            if (!CurrentIngredients.TryGetValue(expectedIngredient, out List<Property> currentProperties))
            {
                // Penalize for missing the entire ingredient's properties.
                totalPenalty += idealProperties.Count * 2; // or another weight as appropriate
                Debug.Log("Missing Property!");
                continue;
            }

            // Compare the ideal vs. current properties using a set based approach
            var idealSet = new HashSet<Property>(idealProperties);
            var currentSet = new HashSet<Property>(currentProperties);
            
            // Count missing properties (expected but not present)
            int missingProperties = idealSet.Except(currentSet).Count();
            // Count extra properties (present but not expected)
            int extraProperties = currentSet.Except(idealSet).Count();
            
            totalPenalty += (missingProperties * 2) + (extraProperties * 1);
        }

        return totalPenalty;
    }

    
    
}
