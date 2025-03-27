using System;
using System.Collections.Generic;
using System.Linq;
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

    // The exact ingredients of the order. The order may be slightly customized by the customer to exclude certain ingredients. 
    [field: SerializeField]
    public Dictionary<IngredientData, List<Property>> SelectedIngredients { get; set; } = new Dictionary<IngredientData, List<Property>>(); 

    // Whether the order is served or not
    [field: SerializeField]
    public bool Served { get; set; }

    [field: SerializeField]
    public Station Station { get; set; }

    [field: SerializeField]
    public int StationIdx { get; set; }

    [field: SerializeField]
    public CookingUIEventChannel cookingUIEventChannel { get; set; }

    public Order(Customer Customer, RecipeData recipe, CookingUIEventChannel cookingUIEventChannel)
    {
        this.Customer = Customer;
        this.Recipe = recipe;
        this.cookingUIEventChannel = cookingUIEventChannel;
        Reinitialize();
    }

    private void Reinitialize()
    {
        Served = false;
        StationIdx = 0;
        Station = Recipe.StationSequence[0].Create(Recipe.InitialStockSequence[StationIdx].InitialStock, cookingUIEventChannel);
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
    // CorrectStockSequence -> The correct stocks of ingredients & properties for each station; CorrectPropertiesPerIngredient -> Each ingredient has a list of properties that it needs to have by the end (cut, etc.)
    // Properties -> the list of properties of one ingredient
    /// <summary>
    /// Determines whether the current state of the order matches the expected recipe's requirements, including
    /// ingredients and their associated properties.
    /// </summary>
    /// <returns>
    /// True if the order is correct and meets the recipe's final requirements, including correct ingredients
    /// and their properties; otherwise, false.
    /// </returns>
    public bool IsCorrect()
    {
        // TODO: Hopefully this works.
        if (SelectedIngredients.Count != Recipe.CorrectStockSequence[StationIdx].CorrectIngredients.Count)
            return false;
        
        for (int i = 0; i < SelectedIngredients.Count; i++)
            if (!AreListsEqual(SelectedIngredients[Recipe.CorrectStockSequence[StationIdx].CorrectIngredients[i]], Recipe.CorrectStockSequence[StationIdx].CorrectPropertiesPerIngredient[i].Properties))
                return false;
        
        return true;
    }

    bool AreListsEqual(List<Property> list1, List<Property> list2)
    {
        if (list1.Count != list2.Count)
            return false;

        list1.Sort();
        list2.Sort();

        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i] != list2[i])
                return false;
        }

        return true;
    }
}
